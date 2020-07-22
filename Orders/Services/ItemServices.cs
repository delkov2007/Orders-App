using GemBox.Spreadsheet;
using Orders.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Orders.Services
{
    public class ItemServices : IItemServices
    {
        public  List<ItemModel> ReadFromXml(string path = @"C:\Users\HP\source\repos\Orders\Orders\wwwroot\xml\autogentestbase.xml")
        {
            var reader = new StreamReader(path);
            string dataSource = reader.ReadToEnd();
            reader.Close();

            return dataSource.DeserializeToObject<List<ItemModel>>().ToList();
        }
        public void WriteToXml(List<ItemModel> list, string path = @"C:\Users\HP\source\repos\Orders\Orders\wwwroot\xml\autogentestbase.xml")
        {
            var updatedDataSource = list.SerializeToXml<List<ItemModel>>();
            
            if (File.Exists(path))
            {
                File.WriteAllText(path, updatedDataSource);
           
            }

        }
        public void EditOrAddItem(ItemModel model)
        {
            List<ItemModel> itemsList = ReadFromXml();
            

            if(itemsList.Where(el => el.ID == model.ID).ToList().Count==0)
            {
                itemsList.Append(new ItemModel
                {
                    ID = model.ID,
                    ProductCode = model.ProductCode,
                    Name = model.Name,
                    Quantity = model.Quantity,
                    Price = model.Price,
                    Image = model.Image

                });
            }
            else
            {
                var index = itemsList.FindIndex(e => e.ID == model.ID);
                itemsList[index] = new ItemModel
                {
                    ID = model.ID,
                    ProductCode = model.ProductCode,
                    Name = model.Name,
                    Quantity = model.Quantity,
                    Price = model.Price,
                    Image = model.Image

                };
            }

            WriteToXml(itemsList);

        }
        public ItemModel ItemInfo(string id)
        {
            IEnumerable<ItemModel> itemList = ReadFromXml();
            ItemModel model;
            if (string.IsNullOrEmpty(id))
            {
                model = new ItemModel
                {
                    ID = Math.Abs(DateTime.Now.GetHashCode()).ToString()
                };
            }   

            model = itemList.Where(item => item.ID.Equals(id)).ToList().FirstOrDefault();

            return model;
        }
        public void DeleteItem(string id)
        {
            List<ItemModel> itemsList = ReadFromXml().ToList();

            var index = itemsList.FindIndex(e => e.ID == id);

            itemsList.RemoveAt(index);

            WriteToXml(itemsList);

        }
        public PagedFilteredSortedResult<ItemModel> ForgePageSortFilterResult(string sortBy = "id", string sortDir = "asc", int currentPage = 1, int itemsPerPage = 5, string filterValue = "", string currentLine=null)
        {
            IEnumerable<ItemModel> data = ReadFromXml();

            if (string.IsNullOrEmpty(filterValue))
            {
                filterValue = "";
            }
            else
            {
                filterValue = filterValue.ToLower();
                currentPage = 1;

                data = data.Where(x => x.ID.ToString().ToLower().Contains(filterValue)
                    || x.Name.ToLower().Contains(filterValue)
                    || x.ProductCode.ToString().ToLower().Contains(filterValue)
                    || x.Price.ToLower().Contains(filterValue)
                    || x.Quantity.ToString().ToLower().Contains(filterValue));
            }
            
            Func<ItemModel, object> order = x => x.ID;
            
            switch (sortBy.ToLower())
            {
                case "id":
                    order = x => long.Parse(x.ID);
                    break;

                case "name":
                    order = x => x.Name;
                    break;

                case "quantity":
                    order = x => x.Quantity;
                    break;

                case "price":
                    order = x => double.Parse(x.Price, CultureInfo.InvariantCulture);
                    break;

                case "productcode":
                    order = x => x.ProductCode;
                    break;
            }

            
            data = (sortDir == "asc")
                ? data.OrderBy(order)
                : data.OrderByDescending(order);

            PagedFilteredSortedResult<ItemModel> model = new PagedFilteredSortedResult<ItemModel>()
            {
                SortBy = sortBy,
                SortDir = sortDir,
                ItemsPerPage = itemsPerPage,
                CurrentPage = currentPage,
                Items = data,
                FilterValue = filterValue
            };

            model.Count = model.Items.ToList().Count;
            model.TotalPages = (int)Math.Ceiling(decimal.Divide(model.Count, itemsPerPage));
            model.CurrentPage = model.CurrentPage > model.TotalPages ? model.TotalPages : currentPage;
            model.Items = data.Skip((model.CurrentPage - 1) * itemsPerPage).Take(itemsPerPage).ToList();
            model.CurrentElementIndex = currentLine;
            
            
            return model;
        }
        public string ExportToXLS(PagedFilteredSortedResult<ItemModel> model)
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            var exportFile = new ExcelFile();
            var workSheet = exportFile.Worksheets.Add("Exported data");
            var dataTable = new DataTable();
            var objModel = model.Items.ToList()[0];
            

            

            foreach (var prop in objModel.GetType().GetProperties())
            {
                dataTable.Columns.Add(prop.Name);
            }


            foreach (var item in model.Items)
            {
                dataTable.Rows.Add(new object[] { item.ID, item.ProductCode, item.Name, item.Quantity, item.Price, item.Image});
            }

            workSheet.InsertDataTable(dataTable,
            new InsertDataTableOptions()
            {
                ColumnHeaders = true,
                StartRow = 0
            });

            for (int i = 0; i <= 6; i++)
            {
                workSheet.Columns[i].AutoFit();
            }

            exportFile.Save("wwwroot/exports/Export.xlsx");

            var path = Path.GetFullPath("wwwroot/exports/Export.xlsx");

            return path;
        }


    }



    
}
