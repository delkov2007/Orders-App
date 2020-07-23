using Microsoft.AspNetCore.Mvc;
using Orders.Models;
using Orders.Services;
using System;
using System.Diagnostics;
using System.IO;


namespace Orders.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAdminServices _services;


        public HomeController(IAdminServices currentServices)
        {
            _services = currentServices;
        }

        [HttpGet]
        public IActionResult Index(string sortBy = "id", string sortDir = "asc", int itemsPerPage = 5, int currentPage = 1, string filterValue = "", string currentLine=null)
        {
            
            PagedFilteredSortedResult<ItemModel> model = _services.ForgePageSortFilterResult(sortBy, sortDir,currentPage,itemsPerPage,filterValue,currentLine);
            

            return View(model);

        }
        [HttpPost]
        public IActionResult Index(PagedFilteredSortedResult<ItemModel> model)
        {
            PagedFilteredSortedResult<ItemModel> filteredModel = _services.ForgePageSortFilterResult(model.SortBy, model.SortDir, model.CurrentPage, model.ItemsPerPage, model.FilterValue);


            return View("Index", filteredModel);
                
                

        }
        [HttpGet]
        public IActionResult ItemInfo(string id)
        {
            ItemModel model = _services.ItemInfo(id);
            return View(model);
        }
        [HttpPost]
        public IActionResult Update(ItemModel model)
        {
            _services.EditOrAddItem(model);
            return Redirect("/home/iteminfo?id=" + model.ID);
        }
        
        [HttpGet]
        public IActionResult Delete(string id)
        {
            _services.DeleteItem(id);
            return Redirect("/home/index");
        }
        [HttpGet]
        public IActionResult InlineEdit()
        {
            return PartialView("InlineEdit");
        }
        
        [HttpPost]
        public IActionResult InlineEdit(ItemModel model, string sortBy, string sortDir, int itemsPerPage, int currentPage, string filterValue)
        {
            _services.EditOrAddItem(model);

            String path = String.Format(@"/home/index?sortBy={0}&sortDir={1}&itemsPerPage={2}&currentPage={3}&filterValue={4}",sortBy, sortDir, itemsPerPage, currentPage, filterValue);
            return Redirect(path);
        }

        [HttpGet]
        public IActionResult Export(string sortBy = "id", string sortDir = "asc", int itemsPerPage = 5, int currentPage = 1, string filterValue = "", string currentLine = null)
        {
            PagedFilteredSortedResult<ItemModel> model = _services.ForgePageSortFilterResult(sortBy, sortDir, currentPage, itemsPerPage, filterValue, currentLine);
            var path = _services.ExportToXLS(model);

            return File(System.IO.File.ReadAllBytes(path), "application/octet-stream", "Export.xlsx");
        }
       
    }
}
