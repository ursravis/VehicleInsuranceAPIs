using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MetadataService.DTO
{
    public class CustomerSearchRequestDTO
    {
        public PaginationDTO Pagination { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
