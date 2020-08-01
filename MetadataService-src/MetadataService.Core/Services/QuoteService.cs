using MetadataService.Core.Interfaces;
using MetadataService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MetadataService.Core.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteRepository quoteRepository;

        public QuoteService(IQuoteRepository quoteRepository)
        {
            this.quoteRepository = quoteRepository;
        }
        public Task<int> DeleteQuotesAsync(int customerId)
        {
            return quoteRepository.DeleteQuotesAsync(customerId);
        }

        public Task<IEnumerable<QuoteDto>> GetQuotesAsync(int customerId)
        {
            return quoteRepository.GetQuotesAsync(customerId);
        }

        public Task<IEnumerable<QuoteDto>> SaveQuotesAsync(int customerId, IEnumerable<QuoteDto> quotes)
        {
            return quoteRepository.SaveQuotesAsync(customerId, quotes);
        }
    }
}
