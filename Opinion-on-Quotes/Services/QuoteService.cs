using Azure;
using Microsoft.EntityFrameworkCore;
using Opinion_on_Quotes.Data;
using Opinion_on_Quotes.Interfaces;
using Opinion_on_Quotes.Models;

namespace Opinion_on_Quotes.Services
{
    public class QuoteService: IQuoteServices
    {
        private readonly ApplicationDbContext _context;
        // dependency injection of database context
        public QuoteService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<QuoteDto>> ListQuotes()
        {
            // all Quotes
            List<Quote> Quotes = await _context.Quotes
                 .Include(q => q.Comments)
                .ToListAsync();
            // empty list of data transfer object QuoteDto
            List<QuoteDto> QuoteDtos = new List<QuoteDto>();
            // foreach Quote record in database
            foreach (Quote Quote in Quotes)
            {
                QuoteDtos.Add(new QuoteDto()
                {
                    quote_id = Quote.quote_id,
                    content = Quote.content,
                    actor = Quote.actor,
                    episode = Quote.episode,
                    drama_id = Quote.drama_id,
                    comments = Quote.Comments.Select(c => new CommentDto()
                    {
                        CommentId = c.CommentId,
                        CommentText = c.CommentText,
                        CreatedAt = c.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                       
                        quote_id = c.quote_id,
                        UserName = c.UserId
                    }).ToList()
                });
            }
            // return QuoteDtos
            return QuoteDtos;

        }


        public async Task<QuoteDto?> FindQuote(int id)
        {

            var Quote = await _context.Quotes
                 .Include(q => q.Comments)
                .FirstOrDefaultAsync(c => c.quote_id == id);

            // no Quote found
            if (Quote == null)
            {
                return null;
            }
            // create an instance of QuoteDto
            QuoteDto QuoteDtos = new QuoteDto()
            {
                quote_id = Quote.quote_id,
                content = Quote.content,
                actor = Quote.actor,
                episode = Quote.episode,
                drama_id = Quote.drama_id
            };
            return QuoteDtos;

        }


        public async Task<ServiceResponse> UpdateQuote(QuoteDto QuoteDto)
        {
            ServiceResponse serviceResponse = new();

            var drama = await _context.Dramas.FindAsync(QuoteDto.drama_id);

            var quoteToUpdate = await _context.Quotes.FindAsync(QuoteDto.quote_id);

            if (quoteToUpdate == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add($"Quote with ID {QuoteDto.quote_id} not found.");
                return serviceResponse;
            }

            if (drama == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add($"Drama with ID {QuoteDto.drama_id} not found.");
                return serviceResponse;
            }


            //quoteToUpdate.quote_id = QuoteDto.quote_id;
                quoteToUpdate.content = QuoteDto.content;
                quoteToUpdate.actor = QuoteDto.actor;
                quoteToUpdate.episode = QuoteDto.episode;
               quoteToUpdate.Drama = drama;
          

            try
            {
                // SQL Equivalent: Update Quotes set ... where QuoteId={id}
                await _context.SaveChangesAsync();
                serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("An error occurred updating the record");
                return serviceResponse;
            }

            //serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;
        }


        public async Task<ServiceResponse> AddQuote(QuoteDto QuoteDto)
        {
            ServiceResponse serviceResponse = new();

            var drama = await _context.Dramas.FindAsync(QuoteDto.drama_id);


            if (drama == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add($"Drama with ID {QuoteDto.drama_id} not found.");
                return serviceResponse;
            }


            // Create instance of Quote
            Quote Quote = new Quote()
            {
                quote_id = QuoteDto.quote_id,
                content = QuoteDto.content,
                actor = QuoteDto.actor,
                episode = QuoteDto.episode,
                Drama = drama
            };
            // SQL Equivalent: Insert into Quote (..) values (..)

            try
            {
                _context.Quotes.Add(Quote);
                var rows = await _context.SaveChangesAsync();
                if (rows > 0)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
                    serviceResponse.CreatedId = Quote.quote_id;
                }
                else
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                    serviceResponse.Messages.Add("Drama could not be added.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while saving quote: {ex.Message}");
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an error adding the Quote.");
                if (ex.InnerException != null)
                {
                    serviceResponse.Messages.Add(ex.InnerException.Message);
                }
            }


            //serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            //serviceResponse.CreatedId = Quote.quote_id;
            return serviceResponse;
        }


        public async Task<ServiceResponse> DeleteQuote(int id)
        {
            ServiceResponse response = new();
            // Quote must exist in the first place
            var Quote = await _context.Quotes.FindAsync(id);
            if (Quote == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Quote cannot be deleted because it does not exist.");
                return response;
            }

            try
            {
                _context.Quotes.Remove(Quote);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error encountered while deleting the Quote");
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Deleted;

            return response;

        }
        public async Task<ServiceResponse> ListQuotesForDrama(int id)
        {
            ServiceResponse response = new();

            List<Quote> Quotes = await _context.Quotes
                .Include(q => q.Drama)
               .Where(q => q.drama_id == id)
                .ToListAsync();

            // empty list of data transfer object CategoryDto
            List<QuoteDto> QuoteDtos = new List<QuoteDto>();
            // foreach Order Item record in database
            foreach (Quote Quote in Quotes)
            {
                // create new instance of CategoryDto, add to list
                QuoteDtos.Add(new QuoteDto()
                {
                    quote_id = Quote.quote_id,
                    content = Quote.content,
                    actor = Quote.actor,
                    episode = Quote.episode,
                    title=Quote.Drama?.title,
                    drama_id=Quote.drama_id
                });
            }
            // If no quotes found
            if (!QuoteDtos.Any())
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("No quotes found for the specified drama ID.");
                return response;
            }

            // Wrap the result in the response object
            response.Status = ServiceResponse.ServiceStatus.Success;
            response.Data = QuoteDtos;

            return response;


        }
    }
    }
