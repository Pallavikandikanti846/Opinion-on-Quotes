using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Opinion_on_Quotes.Interfaces;
using Opinion_on_Quotes.Models;


namespace Opinion_on_Quotes.Controllers
{
    public class DramaPageController : Controller
    {
        private readonly IDramaServices _dramaServices;
        public DramaPageController(IDramaServices dramaServices)
        {
            _dramaServices = dramaServices;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 

        public async Task<IActionResult> List()
        {
            IEnumerable<DramaDto> dramaList = await _dramaServices.ListDramas();
            return View(dramaList);
        }

        /// <summary>
        ///     
        [HttpGet]
        public IActionResult Create()
        {
            return View(); //show form to add drama
        }
        [HttpPost]
        public async Task<IActionResult> Add(DramaDto dramaDto)
        {
            ServiceResponse response = await _dramaServices.AddDrama(dramaDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                // Redirect to the list of dramas after successful creation
                return RedirectToAction("List");
            }
            else {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });

            }

        }

        // GET: /DramaPage/DeleteConfirmation/5
        [HttpGet]
        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var dramaToDelete = await _dramaServices.FindDrama(id);
            if (dramaToDelete == null)
            {
                return NotFound();
            }
            return View(dramaToDelete); // This loads DeleteConfirmation.cshtml
        }
        // POST: /DramaPage/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _dramaServices.DeleteDrama(id);

            if (response.Status != ServiceResponse.ServiceStatus.Deleted)
            {
                return BadRequest(response.Messages);
            }

            return RedirectToAction("List");
        }

        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            var dramaToEdit = await _dramaServices.FindDrama(id);
            if (dramaToEdit == null)
            {
                return NotFound();
            }
            return View(dramaToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Update(DramaDto dramaDto)
        {
            ServiceResponse response = await _dramaServices.UpdateDrama(dramaDto);
            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("List");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        } 
        public IActionResult Index()
        {
            return View();
        }
    }
}
