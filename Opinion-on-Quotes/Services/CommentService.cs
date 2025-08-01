using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Opinion_on_Quotes.Data;
using Opinion_on_Quotes.Interfaces;
using Opinion_on_Quotes.Models;

namespace Opinion_on_Quotes.Services
{
    public class CommentService : ICommentService
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly ApplicationDbContext _context;
        // dependency injection of database context
        public CommentService(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public async Task<ServiceResponse> AddComment(CreateCommentDto createCommentDto, string userId)
        {
            var response = new ServiceResponse();

            var quote = await _context.Quotes.FindAsync(createCommentDto.quote_id);
            if (quote == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Quote not found.");
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

            var commentDtos = new List<CommentDto>();

            var comments = await _context.Comments
                .Where(c => c.quote_id == quote.quote_id)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            foreach (var c in comments)
            {
                var user = await _userManager.FindByIdAsync(c.UserId);
                string username = user?.UserName ?? "Anonymous";

                commentDtos.Add(new CommentDto
                {
                    CommentId = c.CommentId,
                    CommentText = c.CommentText,
                    CreatedAt = c.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    UserName = username,
                    quote_id = c.quote_id
                });
            }

            response.QuoteData = new QuoteDto
            {
                quote_id = quote.quote_id,
                content = quote.content,
                actor = quote.actor,
                episode = quote.episode,
                drama_id = quote.drama_id,
                comments = commentDtos
            };

            return response;
        }

        public async Task<IEnumerable<CommentDto>> ListCommentsByQuote(int quote_id)
        {
            var comments = await _context.Comments
                .Where(c => c.quote_id == quote_id)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            var commentDtos = new List<CommentDto>();

            foreach (var comment in comments)
            {
                var user = await _userManager.FindByIdAsync(comment.UserId);
                string username = user?.UserName ?? "Anonymous";

                commentDtos.Add(new CommentDto
                {
                    CommentId = comment.CommentId,
                    CommentText = comment.CommentText,
                    CreatedAt = comment.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    UserName = username,
                    quote_id = comment.quote_id
                });
            }

            return commentDtos;
        }

        public async Task<ServiceResponse> DeleteComment(int commentId, string userId)
        {
            var response = new ServiceResponse();
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Comment not found.");
                return response;
            }

            // Check if user is admin
            var user = await _userManager.FindByIdAsync(userId);
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            //  Ownership check
            if (comment.UserId != userId && !isAdmin)
            {
                response.Status = ServiceResponse.ServiceStatus.Forbidden;
                response.Messages.Add("You are not authorized to delete this comment.");
                return response;
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            response.Status = ServiceResponse.ServiceStatus.Deleted;
            response.Messages.Add("Comment deleted successfully.");
            return response;
        }


        public async Task<ServiceResponse> UpdateComment(int commentId, string newText, string userId)
        {
            var response = new ServiceResponse();

            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Comment not found.");
                return response;
            }

            // Check if user is admin
            var user = await _userManager.FindByIdAsync(userId);
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            //  Ownership check
            if (comment.UserId != userId &&!isAdmin)
            {
                response.Status = ServiceResponse.ServiceStatus.Forbidden;
                response.Messages.Add("You are not authorized to update this comment.");
                return response;
            }

            comment.CommentText = newText;

           
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Updated;
                response.Messages.Add("Comment updated successfully.");
            
            
                          

            return response;
        }
        public async Task<IEnumerable<CommentDto>> GetCommentsForQuote(int quote_id)
        {
            var comments = await _context.Comments
                .Where(c => c.quote_id == quote_id)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            var commentDtos = new List<CommentDto>();

            foreach (var comment in comments)
            {
                var user = await _userManager.FindByIdAsync(comment.UserId);
                string username = user?.UserName ?? "Anonymous";

                commentDtos.Add(new CommentDto
                {
                    CommentId = comment.CommentId,
                    CommentText = comment.CommentText,
                    CreatedAt = comment.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    UserName = username,
                    quote_id = comment.quote_id
                });
            }

            return commentDtos;
        }


    }
}