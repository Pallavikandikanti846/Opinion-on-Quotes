using Microsoft.AspNetCore.Mvc;
using Opinion_on_Quotes.Interfaces;
using Opinion_on_Quotes.Models;
using System.Collections.Generic;


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
        public IActionResult Index()
        {
            return View();
        }
    }
}
