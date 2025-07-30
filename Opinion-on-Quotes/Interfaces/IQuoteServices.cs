using Opinion_on_Quotes.Models;

namespace Opinion_on_Quotes.Interfaces
{
    public interface IQuoteServices
    {
        // base CRUD
        Task<IEnumerable<QuoteDto>> ListQuotes();


        Task<QuoteDto?> FindQuote(int id);


        Task<Models.ServiceResponse> UpdateQuote(QuoteDto QuoteDto);

        Task<Models.ServiceResponse> AddQuote(QuoteDto QuoteDto);

        Task<Models.ServiceResponse> DeleteQuote(int id);

        Task<Models.ServiceResponse> ListQuotesForDrama(int id);

    }
}
