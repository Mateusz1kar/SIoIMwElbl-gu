using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PracaDyplomowa.Interface;
using PracaDyplomowa.Models;
using PracaDyplomowa.ViewsModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PracaDyplomowa.Controllers
{
    //[Route("[controller]")]
  
    public class EventController : Controller
    {
        private readonly IEventRepozytory _eventRepozytory ;
        private readonly ITokenRepozytory _tokenRepozytory;
        public EventController(IEventRepozytory eventRepozytory, ITokenRepozytory tokenRepozytory)
        {
            _eventRepozytory = eventRepozytory;
            _tokenRepozytory = tokenRepozytory;
        }
        // GET: /<controller>/
        [HttpGet]
        public IActionResult AddEvent()
        {
            AddEventVM model = new AddEventVM();
            return View(model);
        }
        [HttpPost]
        public IActionResult AddEvent(AddEventVM model)
        {
            if (model.DateStart == null)
            {
                ModelState.AddModelError("", "Prosze wprowadzić datę rozpoczęcia ");
            }
            if (model.DateEnd == null)
            {
                ModelState.AddModelError("", "Prosze wprowadzić datę zakończenia ");
            }
            if (ModelState.IsValid & User.Identity.IsAuthenticated & model.DateEnd != null & model.DateStart == null)
            {
                
                Event e = new Event
                {
                    Name = model.Name,
                    ShortDescription = model.ShortDescription,
                    Description = model.Description,
                    Place = model.Place,
                    DateStart = model.DateStart.Value,
                    DateEnd = model.DateEnd.Value,
                     UserName=User.Identity.Name,
                      Publications = new List<Publication>()
                };
                _eventRepozytory.addEvent(e);


                return RedirectToAction("ShowEvents");
            }
            return View(model);

        }
        public IActionResult ShowEvents(EventsListVM model)
        {
            model.eventList = _eventRepozytory.allEvent().ToList();
            return View(model);
        }
        public IActionResult MyEvents()
        {
            EventsListVM myEventsList = new EventsListVM();
            myEventsList.eventList = _eventRepozytory.allUserEvents(User.Identity.Name).ToList();
            return View(myEventsList);
        }
        //[HttpGet("[action]/{id}")]
        public IActionResult DetailsEvent(int id,string error="")
        {
            if (error!="")
            {
                ModelState.AddModelError("", error);
            }
            
                Event e = _eventRepozytory.findEvent(id);
                var model = new DetailsEventVM() { eventDetail = e, error = error, Tokens = _tokenRepozytory.getAll() };
                return View(model);
            
            return RedirectToAction("ShowEvents");
        }
        public IActionResult UpdateEvent(DetailsEventVM model)
        {
            //Event e = new Event();
            //e.EventId = model.evemtDetail.EventId;
            //e.Name = model.evemtDetail.Name;
            //e.Place = model.evemtDetail.Place;
            //e.ShortDescription = model.evemtDetail.ShortDescription;
            //e.Description = model.evemtDetail.Description;
            //e.DateStart = model.evemtDetail.DateStart;
            //e.DateEnd = model.evemtDetail.DateEnd;
            if (ModelState.IsValid & User.Identity.IsAuthenticated)
            {
                var buf = _eventRepozytory.findEvent(model.eventDetail.EventId);
                if (buf.UserName==User.Identity.Name)
                {
                    _eventRepozytory.update(model.eventDetail);
                }
                else
                {
                    ModelState.AddModelError("", "Brak uprwinień ");
                }
                
            }
           
            return RedirectToAction("DetailsEvent", new {id= model.eventDetail.EventId});
        }
        public IActionResult DeleteEvent(int id)
        {
            var e = _eventRepozytory.findEvent(id);
            if (e!= null)
            {
                if (e.Images!= null)
                {
                    var imageNameList = new List<string>();
                    foreach (var item in (e.Images))
                    {
                        imageNameList.Add(item.ImageName);
                    }
                    return RedirectToAction("DeleteAllEventImage", "Image", new { images = imageNameList, eventId= id });
                }
                _eventRepozytory.delEvent(id);
            }
            return RedirectToAction("MyEvents");
        }

        public IActionResult SearchEvents(EventsListVM model)
        {
            model.eventList = _eventRepozytory.searchEvents(model.searchName, model.sortByDS == "true", model.searcDateStart, model.sortByDE == "true", model.searcDateEnd, model.typeSort == "Up").ToList();
            return View("ShowEvents", model);
        }

    }
}
