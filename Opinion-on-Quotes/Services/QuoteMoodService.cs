using Microsoft.EntityFrameworkCore;
using Opinion_on_Quotes.Data;
using Opinion_on_Quotes.Interfaces;
using Opinion_on_Quotes.Models;

namespace Opinion_on_Quotes.Services
{
    public class QuoteMoodService : IQuoteMoodServices
    {
        private readonly ApplicationDbContext _context;
        // dependency injection of database context
        public QuoteMoodService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse> GetQuotesForMood(String moodtype)
        {
            ServiceResponse response = new();

            List<QuoteMood> QuoteMood = await _context.QuoteMoods
                .Include(qm => qm.Mood)
                .Include(qm => qm.Quote)
                .ThenInclude(q => q.Drama)
               .Where(qm => qm.Mood.type == moodtype)
                .ToListAsync();

            // empty list of data transfer object CategoryDto
            List<QuoteOnMoodDto> QuoteOnMoodDtos = new List<QuoteOnMoodDto>();
            // foreach Order Item record in database
            foreach (var qm in QuoteMood)
            {
                // create new instance of CategoryDto, add to list
                QuoteOnMoodDtos.Add(new QuoteOnMoodDto()
                {
                    quote_id = qm.Quote.quote_id,
                    content = qm.Quote.content,
                    actor = qm.Quote.actor,
                    type = qm.Mood.type,
                    title = qm.Quote.Drama?.title,
                });
            }
            // If no quotes found
            if (!QuoteOnMoodDtos.Any())
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("No quotes found for the specified drama ID.");
                return response;
            }

            // Wrap the result in the response object
            response.Status = ServiceResponse.ServiceStatus.Success;
            response.Data = QuoteOnMoodDtos;

            return response;


        }
    }
}
