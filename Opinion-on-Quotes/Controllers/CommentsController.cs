using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Opinion_on_Quotes.Interfaces;
using Opinion_on_Quotes.Models;

namespace Opinion_on_Quotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
       
            private readonly ICommentService _commentService;

            public CommentsController(ICommentService commentService)
            {
                _commentService = commentService;
            }

        /// <summary>
        /// Adds a new comment to a quote.
        /// </summary>
        /// <param name="createCommentDto">The comment data to add.</param>
        /// <returns>A ServiceResponse indicating the result of the operation.</returns>
        /// logged-in user must be authorized to add a comment
        [Authorize]
        [HttpPost("AddComment")]
            public async Task<IActionResult> AddComment([FromBody] CreateCommentDto createCommentDto)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // get logged-in user's ID

                var response = await _commentService.AddComment(createCommentDto, userId);

                if (response.Status == ServiceResponse.ServiceStatus.Error)
                    return StatusCode(500, response.Messages);

                return Ok(response);
            }

        /// <summary>
        /// Retrieves all comments associated with a specific quote.
        /// </summary>
        /// <param name="quoteId">The ID of the quote.</param>
        /// <returns>A list of CommentDto objects for the given quote.</returns>
        [HttpGet("GetCommentsForQuote/{quoteId}")]
            public async Task<IActionResult> GetCommentsForQuote(int quoteId)
            {
                var comments = await _commentService.GetCommentsForQuote(quoteId);
                return Ok(comments);
            }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <param name="commentId">The ID of the comment to update.</param>
        /// <param name="updatedText">The new text of the comment.</param>
        /// <returns>A ServiceResponse indicating the result of the update.</returns>
        /// only authorized users can update comments
        [Authorize]
        [HttpPut("UpdateComment/{commentId}")]
        public async Task<IActionResult> UpdateComment(int commentId, [FromBody] string updatedText)
        {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // get logged-in user's ID
            var response = await _commentService.UpdateComment(commentId, updatedText, userId);

                if (response.Status == ServiceResponse.ServiceStatus.NotFound)
                    return NotFound(response.Messages);

                if (response.Status == ServiceResponse.ServiceStatus.Forbidden)
                    return Forbid(); // Handle unauthorized update

                if (response.Status == ServiceResponse.ServiceStatus.Error)
                        return StatusCode(500, response.Messages);

                return Ok(response);
        }
            /// <summary>
            /// Deletes a comment by its ID.
            /// </summary>
            /// <param name="commentId">The ID of the comment to delete.</param>
            /// <returns>A ServiceResponse indicating success or failure.</returns>
            [HttpDelete("DeleteComment/{commentId}")]
            public async Task<IActionResult> DeleteComment(int commentId)
            {
             
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //  Get user ID from token
            var response = await _commentService.DeleteComment(commentId, userId); //  Pass userId


            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
                    return NotFound(response.Messages);

                if (response.Status == ServiceResponse.ServiceStatus.Error)
                    return StatusCode(500, response.Messages);

                return Ok(response);
            }

        }
}
