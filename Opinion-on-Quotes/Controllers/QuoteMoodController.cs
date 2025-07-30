using Microsoft.AspNetCore.Mvc;
using Opinion_on_Quotes.Interfaces;
using Opinion_on_Quotes.Models;
using Opinion_on_Quotes.Services;

namespace Opinion_on_Quotes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuoteMoodController : ControllerBase
    {
        private readonly IQuoteMoodServices _quoteMoodServices;

        public QuoteMoodController(IQuoteMoodServices quoteMoodServices)
        {
            _quoteMoodServices = quoteMoodServices;
        }
    
        /// <summary>
        /// Returns a list of quotes for a specific mood by its {type}
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{QuoteDto},{QuoteDto},..]
        /// </returns>
        /// <example>
        /// GET:  https://localhost:7049/api/QuoteMood/ListQuotesOnMood/Witty-> [{"quote_id":12,"content":"You’ll regret rejecting me.","type":"witty","actor":"Kim Se-jeong","title":"Business Proposal"}]
        /// </example>
        [HttpGet(template: "ListQuotesOnMood/{moodtype}")]
        public async Task<IActionResult> ListQuotesOnMood(string moodtype)
        {
            // empty list of data transfer object QuoteDto
            Models.ServiceResponse serviceResponse = await _quoteMoodServices.GetQuotesForMood(moodtype);
            if (serviceResponse.Status == Models.ServiceResponse.ServiceStatus.Success)
            {
                // The Data property contains the actual list of quotes
                var quoteMoodDtos = serviceResponse.Data as IEnumerable<QuoteOnMoodDto>;
                return Ok(quoteMoodDtos);
            }
            else
            {
                // Return the error messages with a proper status code
                return BadRequest(serviceResponse.Messages);
            }
        }
    }
}
