using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PracaDyplomowa.Interface;
using PracaDyplomowa.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PracaDyplomowa.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventApiController : Controller
    {
        private readonly IEventRepozytory _eventRepozytory;
        public EventApiController(IEventRepozytory eventRepozytory)
        {
            _eventRepozytory = eventRepozytory;
        }
        //[Authorize]
        [HttpGet("[action]")]
        public IActionResult AllEvent(int? pageNumber, int? pageSize)
        {
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 10;
            var evnets = from e in _eventRepozytory.allEvent()
                         select new
                         {
                             id = e.EventId,
                             name = e.Name,
                             place = e.Place,
                             dateStart = e.DateStart.ToString(),
                             dateEnd = e.DateEnd.ToString(),
                             shortDescription = e.ShortDescription,
                             tag = e.Tags == null ? new List<Tag>() : _eventRepozytory.getEventTag(e)
                         };
           
            return Ok(evnets.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [HttpGet("[action]")]
        public IActionResult SearchEvent(int? pageNumber, int? pageSize, string? searchName, DateTime? searcDateStart, DateTime? searcDateEnd, bool? sortUp, string? searchPlace)
        {
            DateTime checkIsDateSorted = new DateTime();
            var currentSearchName = searchName ?? "";
            var currentSearcDateStart = searcDateStart ?? new DateTime();
            var currentSsearcDateEnd = searcDateEnd ?? new DateTime();
            var currentSortUp = sortUp ?? false;
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 10;
            var evnets = from e in _eventRepozytory.searchEvents(currentSearchName, currentSearcDateStart != checkIsDateSorted, currentSearcDateStart, currentSsearcDateEnd != checkIsDateSorted, currentSsearcDateEnd, currentSortUp,null, searchPlace)
            select new
                         {
                             id = e.EventId,
                             name = e.Name,
                             place = e.Place,
                             dateStart = e.DateStart.ToString(),
                             dateEnd = e.DateEnd.ToString(),
                             shortDescription = e.ShortDescription,
                             tag = e.Tags == null ? new List<Tag>() : _eventRepozytory.getEventTag(e)
                         };

            return Ok(evnets.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        //[Authorize]
        [HttpGet("[action]/{id}")]
        public IActionResult EventDetail(int id)
        {
            var e = _eventRepozytory.findEvent(id);
            if (e == null)
            {
                return NotFound();
            }
            var details = new
            {
                id = e.EventId,
                name = e.Name,
                place = e.Place,
                dateStart = e.DateStart.ToString(),
                dateEnd = e.DateEnd.ToString(),
                shortDescription = e.ShortDescription,
                description = e.Description,
                image = e.Images == null ? new List<string>() : _eventRepozytory.getEventImage(e),
                tag = e.Tags == null ? new List<Tag>() : _eventRepozytory.getEventTag(e),
                tags = _eventRepozytory.getEventTag(e)

            };
            return Ok(details);
        }
    }
}
