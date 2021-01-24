using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PracaDyplomowa.Interface;
using PracaDyplomowa.Models;
using PracaDyplomowa.ViewsModel;

namespace PracaDyplomowa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventRepozytory _eventRepozytory;

        public HomeController(ILogger<HomeController> logger, IEventRepozytory eventRepozytory)
        {
            _logger = logger;
            _eventRepozytory = eventRepozytory;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(new IndexVM() {  SearchDate = DateTime.Today, DataEvents = _eventRepozytory.searchDayEvents(DateTime.Today).ToList()  });
        }
        [HttpPost]
        public IActionResult Index(IndexVM model)
        {
            if (model.SearchDate == new DateTime())
            {
                ModelState.AddModelError("SearchDate", "NiepoprwanaData ");
                model.DataEvents = new List<Event>();
            }
            else
            {
                model.DataEvents = _eventRepozytory.searchDayEvents(model.SearchDate).ToList();
            }
          
            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
