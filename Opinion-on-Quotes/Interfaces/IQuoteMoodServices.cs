using Opinion_on_Quotes.Models;

namespace Opinion_on_Quotes.Interfaces
{
    public interface IQuoteMoodServices
    {
        // base CRUD
        Task<Models.ServiceResponse> GetQuotesForMood(string moodtype);


        //Task<QuoteDto?> FindQuote(int id);


        //Task<Models.ServiceResponse> UpdateQuote(QuoteDto QuoteDto);

        //Task<Models.ServiceResponse> AddQuote(QuoteDto QuoteDto);

        //Task<Models.ServiceResponse> DeleteQuote(int id);
    }
}
