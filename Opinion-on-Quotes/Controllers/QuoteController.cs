using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using Opinion_on_Quotes.Data;
using Opinion_on_Quotes.Interfaces;
using Opinion_on_Quotes.Models;
using ServiceResponse = Opinion_on_Quotes.Models.ServiceResponse;

namespace Opinion_on_Quotes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class QuoteController : ControllerBase
    {
        private readonly IQuoteServices _QuoteServices;

        public QuoteController(IQuoteServices QuoteServices)
        {
            _QuoteServices = QuoteServices;
        }

        [HttpGet("Test")]
        public IActionResult Test() => Ok("API is working!");
        /// <summary>
        /// Returns a list of Quotes, each represented by an QuoteDto
        /// </summary>
        /// <returns>
        /// 200 OK
        /// </returns>
        /// <example>
        /// GET: https://localhost:7049/api/Quote/QuoteList ->
        ///[{"quote_id":1,"content":"Even if we’re not together, I’ll always be on your side.","actor":"Hyun Bin","episode":16,"drama_id":1},{"quote_id":2,"content":"Fate brought you to me.","actor":"Son Ye-jin","episode":10,"drama_id":1},{"quote_id":3,"content":"Every moment I spent with you shined.","actor":"Gong Yoo","episode":16,"drama_id":2},{"quote_id":4,"content":"I want to be your last love.","actor":"Kim Go-eun","episode":12,"drama_id":2},{"quote_id":5,"content":"I don’t have to win. I just have to not lose.","actor":"Park Seo-joon","episode":8,"drama_id":3},{"quote_id":6,"content":"Pain is meant to be felt.","actor":"Lee Sun-kyun","episode":5,"drama_id":4},{"quote_id":7,"content":"You’ll always have a home to return to.","actor":"Ryu Jun-yeol","episode":20,"drama_id":5},{"quote_id":8,"content":"Evil is punished by evil.","actor":"Song Joong-ki","episode":15,"drama_id":6},{"quote_id":9,"content":"Even in death, I’ll protect you.","actor":"IU","episode":10,"drama_id":7},{"quote_id":10,"content":"It’s okay to not be okay.","actor":"Kim Soo-hyun","episode":16,"drama_id":8},{"quote_id":11,"content":"Dreams are not something you wait for.","actor":"Nam Joo-hyuk","episode":3,"drama_id":9},{"quote_id":12,"content":"You’ll regret rejecting me.","actor":"Kim Se-jeong","episode":2,"drama_id":10},{"quote_id":13,"content":"Healing is mutual.","actor":"Shin Min-a","episode":14,"drama_id":11},{"quote_id":14,"content":"Not everything that exists can be explained.","actor":"Lee Min-ho","episode":9,"drama_id":12},{"quote_id":15,"content":"I’m different, not less.","actor":"Park Eun-bin","episode":6,"drama_id":13}]
        /// </example>
        [HttpGet("QuoteList")]
        public async Task<ActionResult<IEnumerable<QuoteDto>>> QuoteList()
        {

            // returns a list of Quotes dtos
            IEnumerable<QuoteDto> QuoteDtos = await _QuoteServices.ListQuotes();
            // return 200 OK with QuoteDtos
            return Ok(QuoteDtos);
        }
        /// <summary>
        /// Returns a single Quote details specified by its { id }
        /// </summary>
        /// <param name = "id" > The Quote id</param>
        /// <returns>
        /// 200 OK
        /// { QuoteDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: https://localhost:7049/api/Quote/FindQuote/4 -> 
        ///{"quote_id":4,"content":"I want to be your last love.","actor":"Kim Go-eun","episode":12,"drama_id":2}
        /// </example>
        [HttpGet(template: "FindQuote/{id}")]
        public async Task<ActionResult<QuoteDto>> FindQuote(int id)
        {
            var aQuote = await _QuoteServices.FindQuote(id);

            // if the Quote could not be located, return 404 Not Found
            if (aQuote == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(aQuote);
            }
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        /// <summary>
        /// Updates a Quote
        /// </summary>
        /// <param name="id">The ID of the Quote to update</param>
        /// <param name="QuoteDto">The required information to update the Quote</param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        /// <example>
        /// PUT: https://localhost:7049/api/Category/UpdateQuote/17
        /// Request Headers: Content-Type: application/json
        /// Request Body: {QuoteDto}
        /// -> "{\"quote_id\":16,\"content\":\"Success is the sweetest revenge.I waited 900 years just to meet you.\",\"actor\":\"Park Seo-joon\",\"episode\":12,\"drama_id\":3}"
        /// Response Code: Quote with id 16 Updated Successfully
        /// </example>
        /// update action here will be restricted to Admin
        [Authorize(Roles = "Admin")]
        [HttpPut(template: "UpdateQuote/{id}")]
        public async Task<ActionResult> UpdateQuote(int id, [FromBody] QuoteDto QuoteDto)
        {
            // {id} in URL must match Quote_id in POST Body
            if (id != QuoteDto.quote_id)
            {
                //400 Bad Request
                return BadRequest();
            }

            ServiceResponse response = await _QuoteServices.UpdateQuote(QuoteDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            //Status = Updated
            return Ok("Quote with id " + id + " Updated Successfully");

        }
        /// <summary>
        /// Adds a Quote
        /// </summary>
        /// <param name="QuoteDto">The required information to add the Quote</param>
        /// <returns>
        /// 201 Created
        /// Location: api/Quote/FindQuote/{QuoteId}
        /// {QuoteDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// POST: https://localhost:7049/api/Quote/AddQuote
        /// {"quote_id":16,"content":"You're the calm in my chaos.","actor":"Hyun Bin","episode":14,"drama_id":1}
        /// Request Headers: Content-Type: application/json
        /// Request Body: {QuoteDto}
        /// ->"{\"quote_id\":16,\"content\":\"You're the calm in my chaos.\",\"actor\":\"Hyun Bin\",\"episode\":14,\"drama_id\":1}"
        /// Response Code: {"quote_id":16,"content":"You're the calm in my chaos.","actor":"Hyun Bin","episode":14,"drama_id":1}
        /// Response Headers: Location: api/Quote/FindQuote/{QuoteId}
        /// </example>
        /// add action here will be restricted to Admin
        [Authorize(Roles = "Admin")]
        [HttpPost(template: "AddQuote")]
        public async Task<ActionResult<Quote>> AddQuote([FromBody] QuoteDto QuoteDto)
        {
            try
            {
                Console.WriteLine($"Received AddQuote request: {System.Text.Json.JsonSerializer.Serialize(QuoteDto)}");


                ServiceResponse response = await _QuoteServices.AddQuote(QuoteDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            //else if (response.Status == ServiceResponse.ServiceStatus.Error)
            //{
            //    return StatusCode(500,response.Messages);
            //}

            // returns 201 Created with Location
            return Created($"api/Quote/FindQuote/{response.CreatedId}", QuoteDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in AddQuote: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        /// <summary>
        /// Deletes the Quote
        /// </summary>
        /// <param name="id">The id of the Quote to delete</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// DELETE: https://localhost:7049/api/Quote/DeleteQuote/17
        /// ->Quote with id 17 Deleted Successfully
        /// Response Code: Quote with id 17 Deleted Successfully
        /// </example>
        /// action here will be restricted to Admin
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteQuote/{id}")]
        public async Task<ActionResult> DeleteQuote(int id)
        {
            ServiceResponse response = await _QuoteServices.DeleteQuote(id);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return Ok("Quote with id " + id + " Deleted Successfully");

        }
        /// <summary>
        /// Returns a list of quotes for a specific drama by its {id}
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{QuoteDto},{QuoteDto},..]
        /// </returns>
        /// <example>
        /// GET:  https://localhost:7049/api/Quote/ListForQuotes/1 -> [{"quote_id":1,"content":"Even if we’re not together, I’ll always be on your side.","actor":"Hyun Bin","episode":16,"title":"Crash Landing on You","drama_id":1},{"quote_id":2,"content":"Fate brought you to me.","actor":"Son Ye-jin","episode":10,"title":"Crash Landing on You","drama_id":1}]
        /// </example>
        [HttpGet(template: "ListForQuotes/{id}")]
        public async Task<IActionResult> ListQuotesForDrama(int id)
        {
            // empty list of data transfer object QuoteDto
            Models.ServiceResponse serviceResponse = await _QuoteServices.ListQuotesForDrama(id);
            if (serviceResponse.Status == Models.ServiceResponse.ServiceStatus.Success)
            {
                // The Data property contains the actual list of quotes
                var quoteDtos = serviceResponse.Data as IEnumerable<QuoteDto>;
                return Ok(quoteDtos);
            }
            else
            {
                // Return the error messages with a proper status code
                return BadRequest(serviceResponse.Messages);
            }
        }
    }
}
