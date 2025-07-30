using Microsoft.EntityFrameworkCore;
using Opinion_on_Quotes.Data;
using Opinion_on_Quotes.Interfaces;
using Opinion_on_Quotes.Models;

namespace Opinion_on_Quotes.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse> AddComment(CreateCommentDto createCommentDto, string userId)
        {
            var response = new ServiceResponse();

            var quote = await _context.Quotes.FindAsync(createCommentDto.quote_id);
            if (quote == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Topic not found.");
                return response;
            }

            var comment = new Comment
            {
                CommentText = createCommentDto.CommentText,
                CreatedAt = DateTime.Now,
                quote_id = createCommentDto.quote_id,
                Quote = quote,
                UserId = userId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            response.Status = ServiceResponse.ServiceStatus.Created;
            response.Messages.Add("Comment added successfully.");
            return response;
        }

        public async Task<IEnumerable<CommentDto>> ListCommentsByTopic(int quote_id)
        {
            return await _context.Comments
                .Where(c => c.quote_id == quote_id)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentDto
                {
                    CommentId = c.CommentId,
                    CommentText = c.CommentText,
                    CreatedAt = c.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    UserName = "Anonymous"
                })
                .ToListAsync();
        }

        public async Task<ServiceResponse> DeleteComment(int commentId)
        {
            var response = new ServiceResponse();
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Comment not found.");
                return response;
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            response.Status = ServiceResponse.ServiceStatus.Deleted;
            response.Messages.Add("Comment deleted successfully.");
            return response;
        }

        public async Task<ServiceResponse> UpdateComment(int commentId, string newText)
        {
            var response = new ServiceResponse();

            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Comment not found.");
                return response;
            }

            comment.CommentText = newText;

            try
            {
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Updated;
                response.Messages.Add("Comment updated successfully.");
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error updating comment.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }
        public async Task<IEnumerable<CommentDto>> GetCommentsForTopic(int quote_id)
        {
            var comments = await _context.Comments
                .Where(c => c.quote_id == quote_id)
                .Select(c => new CommentDto
                {
                    CommentId = c.CommentId,
                    CommentText = c.CommentText,
                    CreatedAt = c.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    UserName = "Anonymous"
                })
                .ToListAsync();

            return comments;
        }

    }
}