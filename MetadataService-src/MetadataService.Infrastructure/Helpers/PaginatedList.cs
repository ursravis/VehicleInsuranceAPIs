using MetadataService.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetadataService.Infrastructure.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int RowCount { get; private set; }
        public int PageSize { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            RowCount = count;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
        public static PaginationDTO ConvertToPagination(PaginatedList<T> paginattedList)
        {
            if(paginattedList != null)
            {
                return new PaginationDTO
                {
                    HasNextPage = paginattedList.HasNextPage,
                    HasPreviousPage = paginattedList.HasPreviousPage,
                    PageIndex = paginattedList.PageIndex,
                    PageSize = paginattedList.PageSize,
                    TotalNumberOfRecords = paginattedList.RowCount,
                    TotalPages = paginattedList.TotalPages
                };
            }
            return null;
        }

    }
    
}
