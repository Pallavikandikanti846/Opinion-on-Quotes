using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Opinion_on_Quotes.Data;
using Opinion_on_Quotes.Interfaces;
using Opinion_on_Quotes.Models;

namespace Opinion_on_Quotes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MoodController : Controller
    {
        private readonly IMoodServices _MoodServices;

        public MoodController(IMoodServices MoodServices)
        {
            _MoodServices = MoodServices;
        }
        /// <summary>
        /// Returns a list of Moods, each represented by an MoodDto
        /// </summary>
        /// <returns>
        /// 200 OK
        /// </returns>
        /// <example>
        /// GET:  https://localhost:7049/api/Mood/MoodList ->
        ///[{"mood_id":1,"type":"romantic"},{"mood_id":2,"type":"sad"},{"mood_id":3,"type":"inspirational"},{"mood_id":4,"type":"funny"},{"mood_id":5,"type":"emotional"},{"mood_id":6,"type":"hopeful"},{"mood_id":7,"type":"dark"},{"mood_id":8,"type":"mysterious"},{"mood_id":9,"type":"comforting"},{"mood_id":10,"type":"witty"}]
        /// </example>
        /// 


        [HttpGet("MoodList")]
        public async Task<ActionResult<IEnumerable<MoodDto>>> MoodList()
        {

            // returns a list of Moods dtos
            IEnumerable<MoodDto> MoodDtos = await _MoodServices.ListMoods();
            // return 200 OK with MoodDtos
            return Ok(MoodDtos);
        }
        /// <summary>
        /// Returns a single Mood details specified by its {id}
        /// </summary>
        /// <param name="id">The Mood id</param>
        /// <returns>
        /// 200 OK
        /// {MoodDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET:  https://localhost:7049/api/Mood/FindMood/7 -> 
        ///{"mood_id":7,"type":"dark"}
        /// </example>
        [HttpGet(template: "FindMood/{id}")]
        public async Task<ActionResult<MoodDto>> FindMood(int id)
        {
            var aMood = await _MoodServices.FindMood(id);

            // if the Mood could not be located, return 404 Not Found
            if (aMood == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(aMood);
            }
        }

        /// <summary>
        /// Updates a Mood
        /// </summary>
        /// <param name="id">The ID of the Mood to update</param>
        /// <param name="MoodDto">The required information to update the Mood</param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        /// <example>
        /// PUT: https://localhost:7049/api/Mood/UpdateMood/11
        /// Request Headers: Content-Type: application/json
        /// Request Body: {MoodDto}
        /// ->"{\"mood_id\":11,\"type\":\"Melancholic\"}"
        /// Response Code: Mood with id 11 Updated Successfully
        /// </example>
        /// admin only can update Mood
        [Authorize(Roles = "Admin")]
        [HttpPut(template: "UpdateMood/{id}")]
        public async Task<ActionResult> UpdateMood(int id, [FromBody] MoodDto MoodDto)
        {
            // {id} in URL must match Mood_id in POST Body
            if (id != MoodDto.mood_id)
            {
                //400 Bad Request
                return BadRequest();
            }

            ServiceResponse response = await _MoodServices.UpdateMood(MoodDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            //Status = Updated
            return Ok("Mood with id " + id + " Updated Successfully");

        }
        /// <summary>
        /// Adds a Mood
        /// </summary>
        /// <param name="MoodDto">The required information to add the Mood</param>
        /// <returns>
        /// 201 Created
        /// Location: api/Mood/FindMood/{MoodId}
        /// {MoodDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// POST: https://localhost:7049/api/Mood/AddMood/13
        /// Request Headers: Content-Type: application/json
        /// Request Body: {MoodDto}
        /// ->"{\"mood_id\":13,\"type\":\"Tense\"}"
        /// Response Code: {"mood_id":13,"type":"Tense"}
        /// Response Headers: Location: api/Mood/FindMood/{MoodId}
        /// </example>
        /// admin only can add Mood
        [Authorize(Roles = "Admin")]
        [HttpPost(template: "AddMood")]
        public async Task<ActionResult<Mood>> AddMood([FromBody] MoodDto MoodDto)
        {
            ServiceResponse response = await _MoodServices.AddMood(MoodDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return CreatedAtAction(nameof(FindMood), new { id = response.CreatedId }, MoodDto);

        }

        /// <summary>
        /// Deletes the Mood
        /// </summary>
        /// <param name="id">The id of the Mood to delete</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// DELETE: https://localhost:7049/api/Mood/DeleteMood/13
        /// ->Mood with id 13 Deleted Successfully
        /// Response Code: Mood with id 13 Deleted Successfully
        /// </example>
        /// admin only can delete Mood
        /// 
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteMood/{id}")]
        public async Task<ActionResult> DeleteMood(int id)
        {
            ServiceResponse response = await _MoodServices.DeleteMood(id);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return Ok("Mood with id " + id + " Deleted Successfully");

        }
    }
}
