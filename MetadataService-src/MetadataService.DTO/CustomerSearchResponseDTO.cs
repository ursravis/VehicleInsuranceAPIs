using System;
using System.Collections.Generic;
using System.Text;

namespace MetadataService.DTO
{
    public class CustomerSearchResponseDTO
    {
        public PaginationDTO Pagination { get; set; }
        public IEnumerable<CustomerDto> SearchResponseData { get; set; }
    }
}
