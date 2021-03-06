﻿using Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Services
{
    public interface IAdminServices
    { 

        public PagedFilteredSortedResult<ItemModel> ForgePageSortFilterResult(string sortBy = "id", string sortDir = "asc", int currentPage = 1, int pageSize = 4, string filterValue = "", string userId = "", string currentLine = null);


        public void EditOrAddItem(ItemModel model);


        public ItemModel ItemInfo(string id);

        public void DeleteItem(string id);

        public string ExportToXLS(PagedFilteredSortedResult<ItemModel> model);
    }
}
