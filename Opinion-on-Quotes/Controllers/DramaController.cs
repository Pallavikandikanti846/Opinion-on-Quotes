using System;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Opinion_on_Quotes.Data;
using Opinion_on_Quotes.Interfaces;
using Opinion_on_Quotes.Models;

namespace Opinion_on_Quotes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DramaController : ControllerBase
    {
        private readonly IDramaServices _dramaServices;

        public DramaController(IDramaServices dramaServices)
        {
            _dramaServices = dramaServices;
        }

        /// <summary>
        /// Returns a list of Dramas, each represented by an DramaDto
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{OrderItemDto},{OrderItemDto},..]
        /// </returns>
        /// <example>
        /// GET: https://localhost:7049/api/Drama/DramaList ->
        ///[{"drama_id":1,"title":"Crash Landing on You","release_year":2019,"genre":"Romance, Drama","synopsis":"A South Korean heiress crash-lands in North Korea."},{"drama_id":2,"title":"Goblin","release_year":2016,"genre":"Fantasy, Romance","synopsis":"An immortal goblin searches for his human bride."},{"drama_id":3,"title":"Itaewon Class","release_year":2020,"genre":"Drama, Business","synopsis":"An ex-con fights to build a food empire."},{"drama_id":4,"title":"My Mister","release_year":2018,"genre":"Drama, Psychological","synopsis":"A man and a woman form an unexpected bond."},{"drama_id":5,"title":"Reply 1988","release_year":2015,"genre":"Slice of Life, Family","synopsis":"A nostalgic story about five families in 1988."},{"drama_id":6,"title":"Vincenzo","release_year":2021,"genre":"Crime, Comedy","synopsis":"A mafia consigliere returns to Korea for justice."},{"drama_id":7,"title":"Hotel Del Luna","release_year":2019,"genre":"Fantasy, Romance","synopsis":"A ghost hotel owner must settle her past."},{"drama_id":8,"title":"Its Okay to Not Be Okay","release_year":2020,"genre":"Psychological, Romance","synopsis":"A writer and a caretaker find healing."},{"drama_id":9,"title":"Start-Up","release_year":2020,"genre":"Romance, Business","synopsis":"Young entrepreneurs chase dreams in Koreas Silicon Valley."},{"drama_id":10,"title":"Business Proposal","release_year":2022,"genre":"Rom-Com","synopsis":"A girl goes on a blind date pretending to be someone else."},{"drama_id":11,"title":"Hometown Cha-Cha-Cha","release_year":2021,"genre":"Romance, Slice of Life","synopsis":"A dentist starts over in a seaside village."},{"drama_id":12,"title":"The King: Eternal Monarch","release_year":2020,"genre":"Fantasy, Romance","synopsis":"A modern emperor crosses dimensions to save his world."},{"drama_id":13,"title":"Extraordinary Attorney Woo","release_year":2022,"genre":"Legal, Drama","synopsis":"A lawyer with autism solves complex cases."},{"drama_id":14,"title":"Signal","release_year":2016,"genre":"Thriller, Crime","synopsis":"Detectives across time solve cold cases."},{"drama_id":15,"title":"Descendants of the Sun","release_year":2016,"genre":"Romance, Military","synopsis":"A soldier and a doctor fall in love in a war zone."},{"drama_id":17,"title":"I dont know","release_year":2025,"genre":"Sad","synopsis":"Canada's Life"}]
        /// </example>
        [HttpGet("DramaList")]
        public async Task<ActionResult<IEnumerable<DramaDto>>> DramaList()
        {
            
            // returns a list of dramas dtos
            IEnumerable<DramaDto> dramaDtos = await _dramaServices.ListDramas();
            // return 200 OK with OrderItemDtos
            return Ok(dramaDtos);
        }
        /// <summary>
        /// Returns a single Drama details specified by its {id}
        /// </summary>
        /// <param name="id">The Drama id</param>
        /// <returns>
        /// 200 OK
        /// {DramaDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: https://localhost:7049/api/Drama/FindDrama/5 -> 
        /// {"drama_id":5,"title":"Reply 1988","release_year":2015,"genre":"Slice of Life, Family","synopsis":"A nostalgic story about five families in 1988."}
        /// </example>
        [HttpGet(template: "FindDrama/{id}")]
        public async Task<ActionResult<DramaDto>> FindDrama(int id)
        {
            var aDrama = await _dramaServices.FindDrama(id);

            // if the drama could not be located, return 404 Not Found
            if (aDrama == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(aDrama);
            }
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        /// <summary>
        /// Updates a Drama
        /// </summary>
        /// <param name="id">The ID of the Drama to update</param>
        /// <param name="DramaDto">The required information to update the Drama</param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        /// <example>
        /// PUT:  https://localhost:7049/api/Category/UpdateDrama/17
        /// Request Headers: Content-Type: application/json
        /// Request Body: {DramaDto}
        /// ->"{\"drama_id\":17,\"title\":\"The Glory\",\"release_year\":2023,\"genre\":\"Revenge, Thriller\",\"synopsis\":\"A former bullying victim orchestrates a chilling plan for vengeance.\"}"
        /// Response Code: Drama with id 17 Updated Successfully
        /// </example>
        [HttpPut(template: "UpdateDrama/{id}")]
        public async Task<ActionResult> UpdateDrama(int id, [FromBody]DramaDto DramaDto)
        {
            // {id} in URL must match drama_id in POST Body
            if (id != DramaDto.drama_id)
            {
                //400 Bad Request
                return BadRequest();
            }

            ServiceResponse response = await _dramaServices.UpdateDrama(DramaDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            //Status = Updated
            return Ok("Drama with id " +id+ " Updated Successfully");

        }
        /// <summary>
        /// Adds a Drama
        /// </summary>
        /// <param name="DramaDto">The required information to add the Drama</param>
        /// <returns>
        /// 201 Created
        /// Location: api/Drama/FindDrama/{DramaId}
        /// {DramaDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// POST: https://localhost:7049/api/Drama/AddDrama
        /// Request Headers: Content-Type: application/json
        /// Request Body: {DramaDto}
        /// ->"{\"drama_id\":18,\"title\":\"Doctor Romantic\",\"release_year\":2016,\"genre\":\"Medical, Drama\",\"synopsis\":\"A brilliant surgeon mentors young doctors in a countryside hospital.\"}"
        /// Response Code: {"drama_id":18,"title":"Doctor Romantic","release_year":2016,"genre":"Medical, Drama","synopsis":"A brilliant surgeon mentors young doctors in a countryside hospital."}
        /// </example>
        /// <example>
        /// POST: https://localhost:7049/api/Drama/AddDrama
        /// Request Headers: Content-Type: application/json
        /// Request Body: {DramaDto}
        /// ->"{\"drama_id\":19,\"title\":\"The Uncanny Counter\",\"release_year\":2020,\"genre\":\"Fantasy, Action\",\"synopsis\":\"Demon hunters disguise themselves as noodle shop workers.\"}"
        /// Response Code: {"drama_id":19,"title":"The Uncanny Counter","release_year":2020,"genre":"Fantasy, Action","synopsis":"Demon hunters disguise themselves as noodle shop workers."}
        /// </example>
        [HttpPost(template: "AddDrama")]
        public async Task<ActionResult<Drama>> AddDrama([FromBody] DramaDto DramaDto)
        {
            ServiceResponse response = await _dramaServices.AddDrama(DramaDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            // returns 201 Created with Location
            return Created($"api/Drama/FindDrama/{response.CreatedId}", DramaDto);
        }

        /// <summary>
        /// Deletes the Drama
        /// </summary>
        /// <param name="id">The id of the Drama to delete</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// DELETE:  https://localhost:7049/api/Drama/DeleteDrama/21
        /// ->Drama with id 21 Deleted Successfully
        /// Response Code: Drama with id " +id+ " Deleted Successfully
        /// </example>
        [HttpDelete("DeleteDrama/{id}")]
        public async Task<ActionResult> DeleteDrama(int id)
        {
            ServiceResponse response = await _dramaServices.DeleteDrama(id);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return Ok("Drama with id " +id+ " Deleted Successfully");

        }
    }
}
