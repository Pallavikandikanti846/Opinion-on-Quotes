using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Opinion_on_Quotes.Interfaces;
using Opinion_on_Quotes.Models;
using Opinion_on_Quotes.Services;

namespace Opinion_on_Quotes.Controllers
{
    public class QuotePageController : Controller
    {
        private readonly IQuoteServices _quoteService;
        private readonly IMoodServices _moodService;
        private readonly ICommentService _commentService;

        public QuotePageController(IQuoteServices quoteService, IMoodServices moodService, ICommentService commentService)
        {
            _quoteService = quoteService;
            _moodService = moodService;
            _commentService = commentService;
        }

        // Redirect to List by default
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        //GET: /QuotePage/List

        public async Task<IActionResult> List()
        {
            var quotes = await _quoteService.ListQuotes();
            return View(quotes);
        }

        //GET:/QuotePage/Details/3
        public async Task<IActionResult> Details(int id)
        {
            var response = await _quoteService.FindQuote(id);

            if (response == null)
            {
                return View("Error", new ErrorViewModel { Errors = new List<string> { "Topic not found." } });

            }
            return View(response);
        }

        //GET:/QuotePage/New
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> New()
        {

            var moods = await _moodService.ListMoods();
            ViewBag.CategoryList = moods; //send to the view

            return View();
        }

        // POST: /QuotePage/Add
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(QuoteDto QuoteDto)
        {
            // Get the loggedin user Id
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _quoteService.AddQuote(QuoteDto, userId); // passing both args

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", new { id = response.CreatedId });
            }

            return View("Error", new ErrorViewModel { Errors = response.Messages });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _quoteService.FindQuote(id);

            if (response== null)
            {
                return View("Error", new ErrorViewModel { Errors = new List<string> { "Quote not found." } });
            }

            var moods = await _moodService.ListMoods();
            ViewBag.MoodList = moods;

            var quoteDto = new QuoteDto
            {
                quote_id = response.quote_id,
                content = response.content,
                actor = response.actor,
                episode=response.episode
            };

            return View(quoteDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var response = await _quoteService.FindQuote(id);

            if (response == null)
            {
                return View("Error", new ErrorViewModel { Errors = new List<string> { "Quote not found." } });
            }

            return View(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _quoteService.DeleteQuote(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List");
            }

            return View("Error", new ErrorViewModel { Errors = response.Messages });
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CreateCommentDto createCommentDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _commentService.AddComment(createCommentDto, userId);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", new { id = createCommentDto.quote_id });
            }

            return View("Error", new ErrorViewModel { Errors = response.Messages });
        }


    }
}