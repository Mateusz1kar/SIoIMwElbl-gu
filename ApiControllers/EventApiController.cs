using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PracaDyplomowa.Interface;

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
        public IActionResult AllEvent(int? pageNymber, int? pageSize)
        {
            var currentPageNumber = pageNymber ?? 1;
            var currentPageSize = pageSize ?? 5;
            var evnets = from e in _eventRepozytory.allEvent()
                         select new
                         {
                             id = e.EventId,
                             name = e.Name,
                             place = e.Place,
                             dateStart = e.DateStart.ToString(),
                             dataEnd = e.DateEnd.ToString(),
                             shortDescription = e.ShortDescription,
                             description = e.Description
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
            return Ok(e);
        }
    }
}
