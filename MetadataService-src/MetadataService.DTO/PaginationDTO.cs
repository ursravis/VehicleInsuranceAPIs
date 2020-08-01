using System;
using System.Collections.Generic;
using System.Text;

namespace MetadataService.DTO
{
    public class PaginationDTO
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int TotalNumberOfRecords { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }

    }
}
