using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Opinion_on_Quotes.Interfaces;
using Opinion_on_Quotes.Models;



namespace Opinion_on_Quotes.Controllers
{
    public class QuotePageController : Controller
    {
        private readonly IQuoteServices _quoteServices;
        private readonly IDramaServices _dramaServices;
        public QuotePageController(IQuoteServices quoteServices, IDramaServices dramaServices)
        {
            _quoteServices = quoteServices;
            _dramaServices = dramaServices;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public async Task<IActionResult> List()
        {
            IEnumerable<QuoteDto> quoteList = await _quoteServices.ListQuotes();
            return View(quoteList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task<IActionResult> Details(int id)
        {
            QuoteDto quoteToView = await _quoteServices.FindQuote(id);
            if (quoteToView == null)
            {
                return NotFound();
            }
            return View(quoteToView);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var quoteToEdit = await _quoteServices.FindQuote(id);
            if (quoteToEdit == null)
            {
                return NotFound();
            }
            return View(quoteToEdit);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="quoteDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Update(QuoteDto quoteDto)
        {
            ServiceResponse response = await _quoteServices.UpdateQuote(quoteDto);
            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                // Redirect to the list of quotes after successful update
                return RedirectToAction("List");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var dramas = await _dramaServices.ListDramas();
            ViewBag.Dramas = dramas; // Pass the list of dramas to the view
            return View(); //show form to add drama
        }
        [HttpPost]
        public async Task<IActionResult> Add(QuoteDto quoteDto)
        {
            ServiceResponse response = await _quoteServices.AddQuote(quoteDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                // Redirect to the list of dramas after successful creation
                return RedirectToAction("List");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });

            }

        }

        // GET: /DramaPage/DeleteConfirmation/5
        [HttpGet]
        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var quoteToDelete = await _quoteServices.FindQuote(id);
            if (quoteToDelete == null)
            {
                return NotFound();
            }
            return View(quoteToDelete); // This loads DeleteConfirmation.cshtml
        }
        // POST: /DramaPage/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _quoteServices.DeleteQuote(id);

            if (response.Status != ServiceResponse.ServiceStatus.Deleted)
            {
                return BadRequest(response.Messages);
            }

            return RedirectToAction("List");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
