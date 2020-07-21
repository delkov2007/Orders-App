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
        public  IEnumerable<ItemModel> ReadFromXml(string path = @"C:\Users\HP\source\repos\Orders\Orders\wwwroot\xml\autogentestbase.xml")
        {
            var reader = new StreamReader(path);
            string dataSource = reader.ReadToEnd();

            return dataSource.DeserializeToObject<List<ItemModel>>();
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
            IEnumerable<ItemModel> itemsList = ReadFromXml();
            itemsList = itemsList.Where(el => el.ID == model.ID);

            if(itemsList.ToList().Count==0)
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
                itemsList.FirstOrDefault<ItemModel>().dasdas
            }
            XDocument record = new XDocument();////

            var elements = record.Root
                .Descendants("Item")
                .Where(t => t.Element("ID").Value == model.ID);

            if (!(elements.ToList().Count == 0))
            {

                foreach (var element in elements)
                {
                    element.SetElementValue("ProductCode", model.ProductCode.ToString());
                    element.SetElementValue("Name", model.Name);
                    element.SetElementValue("Quantity", model.Quantity.ToString());
                    element.SetElementValue("Price", model.Price);
                    element.SetElementValue("Image", model.Image);
                }
            }
            else
            {
                XElement newElement = new XElement("Item",
                    new XElement[]
                    {
                        new XElement("ID", model.ID),
                        new XElement("ProductCode", model.ProductCode.ToString()),
                        new XElement("Name", model.Name),
                        new XElement("Quantity", model.Quantity.ToString()),
                        new XElement("Price", model.Price.ToString()),
                        new XElement("Image", model.Image)
                    }
                );

                record.Root.Add(newElement);

            }

            record.Save(@"C:\Users\HP\source\repos\Orders\Orders\wwwroot\xml\testbase.xml");

        }
        public ItemModel ItemInfo(string id)
        {
            var model = new ItemModel 
            { 
                ID = Math.Abs(DateTime.Now.GetHashCode()).ToString() 
            };

            //if (!string.IsNullOrEmpty(id))
                //model =  GetDataResult(id).First();

            return model;
        }
        public void DeleteItem(string id)
        {
            XDocument dataResult = new XDocument();///
            var elementForRemove = from element in dataResult.Elements("Items").Elements("Item")
                                   where element.Element("ID").Value == id
                                   select element;
            foreach (var el in elementForRemove)
            {
                el.Remove();
            }

            dataResult.Save(@"C:\Users\HP\source\repos\Orders\Orders\wwwroot\xml\testbase.xml");
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
