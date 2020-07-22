using System.Collections.Generic;

namespace Orders.Models
{
    public class PagedFilteredSortedResult<T> where T : class
    {
        public string SortBy { get; set; }
        public string SortDir { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int Count { get; set; }
        public int TotalPages { get; set; } 
        public IEnumerable<T> Items { get; set; }
        public string FilterValue { get; set; }
        public string CurrentElementIndex { get; set; }

    }
}

