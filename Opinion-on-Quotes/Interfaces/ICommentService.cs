using Opinion_on_Quotes.Models;

namespace Opinion_on_Quotes.Interfaces
{
    public interface ICommentService
    {
        /// <summary>
        /// Adds a new comment to a quote.
        /// </summary>
        /// <param name="createCommentDto">DTO containing comment details.</param>
        /// <returns>A ServiceResponse indicating success or failure.</returns>
        Task<ServiceResponse> AddComment(CreateCommentDto createCommentDto, string? userId);

        /// <summary>
        /// Lists all comments for a specific quote.
        /// </summary>
        /// <param name="quoteId">The ID of the quote.</param>
        /// <returns>A list of CommentDto objects.</returns>
        Task<IEnumerable<CommentDto>> ListCommentsByQuote(int quoteId);

        /// <summary>
        /// Deletes a comment by its ID.
        /// </summary>
        /// <param name="commentId">The ID of the comment to delete.</param>
        /// <returns>A ServiceResponse indicating success or failure.</returns>
        Task<ServiceResponse> DeleteComment(int commentId);

        /// <summary>
        /// Retrieves all comments for a specific quote.
        /// </summary>
        /// <param name="quoteId">The ID of the quote.</param>
        /// <returns>A list of CommentDto objects associated with the quote.</returns>
        Task<IEnumerable<CommentDto>> GetCommentsForQuote(int quoteId);

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <param name="commentId">The ID of the comment to update.</param>
        /// <param name="newText">The new comment text.</param>
        /// <returns>A ServiceResponse indicating success or failure.</returns>
        Task<ServiceResponse> UpdateComment(int commentId, string newText);


    }
}