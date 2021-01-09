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
        private readonly ITagRepozytory _tagRepozytory;
        public EventController(IEventRepozytory eventRepozytory, ITokenRepozytory tokenRepozytory, ITagRepozytory tagRepozytory)
        {
            _eventRepozytory = eventRepozytory;
            _tokenRepozytory = tokenRepozytory;
            _tagRepozytory = tagRepozytory;
        }
        // GET: /<controller>/
        [HttpGet]
        public IActionResult AddEvent()
        {
            AddEventVM model = new AddEventVM() { Tags = _tagRepozytory.getAllTag().ToList()};
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
            if (model.DateEnd < model.DateStart)
            {
                ModelState.AddModelError("", "Data zakończenia nie mołe być wcześniejsza niż data rozpoczęcia  ");
            }

            if (ModelState.IsValid & User.Identity.IsAuthenticated & model.DateEnd != null & model.DateStart != null & model.DateEnd >= model.DateStart)
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
                List<EventeTag> eventeTags = new List<EventeTag>();
                if (model.CheckedTags != null)
                {
                    foreach (var tag in model.CheckedTags)
                    {
                        eventeTags.Add(new EventeTag()
                        {
                            TagId = tag,
                            Tag = _tagRepozytory.getTag(tag),
                            Event = e,
                            EventId = e.EventId
                        });
                    }
                }
                e.Tags = eventeTags;
                _eventRepozytory.addEvent(e);
                return RedirectToAction("ShowEvents");
            }
            return View(model);

        }
        public IActionResult ShowEvents(EventsListVM model)
        {
            model.eventList = _eventRepozytory.searchEvents(null, false, new DateTime(), false, new DateTime(), false, null).ToList();
            model.Tags = _tagRepozytory.getAllTag().ToList();
            return View(model);
        }
        public IActionResult MyEvents()
        {
            EventsListVM model = new EventsListVM()
            {
                eventList = _eventRepozytory.searchEvents(null, false, new DateTime(), false, new DateTime(), false, User.Identity.Name).ToList(),
                Tags = _tagRepozytory.getAllTag().ToList()
            };
            
            return View("ShowEvents", model);
        }
        //[HttpGet("[action]/{id}")]
        public IActionResult DetailsEvent(int id,string error="")
        {
            if (error!="")
            {
                ModelState.AddModelError("", error);
            }
            Event e = _eventRepozytory.findEvent(id);
                var model = new DetailsEventVM() { eventDetail = e, error = error, Tokens = _tokenRepozytory.getAll(), Tags = _tagRepozytory.getAllTag().ToList(), author=e.FirmAccount };
                return View(model);
            
            return RedirectToAction("ShowEvents");
        }
        public IActionResult UpdateEvent(DetailsEventVM model)
        {
            string error = "";
            if (model.eventDetail.DateEnd < model.eventDetail.DateStart)
            {
                error= "Data zakończenia nie mołe być wcześniejsza niż data rozpoczęcia  ";
            }
            if (ModelState.IsValid & User.Identity.IsAuthenticated )
            {
                if (model.eventDetail.DateEnd >= model.eventDetail.DateStart & model.eventDetail.ShortDescription != null & model.eventDetail.Description != null & model.eventDetail.ShortDescription != null)
                {
                    var buf = _eventRepozytory.findEvent(model.eventDetail.EventId);
                    if (buf.UserName == User.Identity.Name)
                    {
                        List<EventeTag> eventeTags = new List<EventeTag>();

                        if (model.CheckedTags != null)
                        {
                            foreach (var tag in model.CheckedTags)
                            {
                                eventeTags.Add(new EventeTag()
                                {
                                    TagId = tag,
                                    Tag = _tagRepozytory.getTag(tag),
                                    Event = model.eventDetail,
                                    EventId = model.eventDetail.EventId
                                });
                            }
                        }
                        model.eventDetail.Tags = eventeTags;
                        _eventRepozytory.update(model.eventDetail);
                    }
                    else
                    {
                        error = "Brak uprwinień ";
                    }

                }
                else
                {
                    error = "Niepoprawne dane ";
                }
                
            }
            else
            {
                error = "Niepoprawne dane ";
            }

            return RedirectToAction("DetailsEvent", new {id= model.eventDetail.EventId, error = error });
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
            var checkIsDateSorted = new DateTime();
            model.eventList = new List<Event>();
            var event_TagNotExluted = _eventRepozytory.searchEvents(model.searchName, model.searcDateStart != checkIsDateSorted, model.searcDateStart, model.searcDateEnd != checkIsDateSorted, model.searcDateEnd, model.typeSort == "Up", model.userEventSearch).ToList();
            foreach (var item in event_TagNotExluted)
            {
                bool isTagExluted = false;
                if (model.CheckedTags != null)
                {
                    foreach (var t in model.CheckedTags)
                    {
                        if (item.Tags != null)
                            foreach (var eventTag in item.Tags)
                            {
                                if (eventTag.TagId == t)
                                {
                                    isTagExluted = true;
                                }
                            }

                    }
                    if (isTagExluted)
                    {
                        model.eventList.Add(item);
                    }
                }
                else
                {
                    model.eventList = event_TagNotExluted;
                }
                
            }
            model.Tags = _tagRepozytory.getAllTag().ToList();
            return View("ShowEvents", model);
        }

    }
}
