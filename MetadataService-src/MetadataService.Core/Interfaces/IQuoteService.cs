using MetadataService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MetadataService.Core.Interfaces
{
    public interface IQuoteService
    {
        Task<IEnumerable<QuoteDto>> GetQuotesAsync(int customerId);
        Task<IEnumerable<QuoteDto>> SaveQuotesAsync(int customerId, IEnumerable<QuoteDto> quotes);
        Task<int> DeleteQuotesAsync(int customerId);
    }
}
