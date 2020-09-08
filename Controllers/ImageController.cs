using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracaDyplomowa.Interface;
using PracaDyplomowa.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PracaDyplomowa.Controllers
{
    public class ImageController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IImageRepozytory _imageRepozytory;
        private readonly IEventRepozytory _eventRepozytory;
        public ImageController(IImageRepozytory imageRepozytory, IHostingEnvironment hostingEnvironment, IEventRepozytory eventRepozytory)
        {
            _imageRepozytory = imageRepozytory;
            this.hostingEnvironment = hostingEnvironment;
            _eventRepozytory= eventRepozytory;
        }
        // GET: /<controller>/
        public IActionResult AddEventImage(IFormFile eventImage, int id)
        {
            string error = "";
            if (eventImage != null)
            {
                string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, @"Images\EventImages");
                string fName = eventImage.FileName.ToString().Split('\\').Last();
                var uploadFileName = Guid.NewGuid().ToString() + "_" + fName;
                string filePath = Path.Combine(uploadFolder, uploadFileName);
                var stream = new FileStream(filePath, FileMode.Create);
                eventImage.CopyTo(stream);
                stream.Close();
                var e = _eventRepozytory.findEvent(id);
                if (e != null)
                {
                    EventImages newEventImage = new EventImages()
                    {
                        ImageName = uploadFileName,
                        Event = e,
                        EventId = id
                    };
                    _imageRepozytory.addEventImage(newEventImage);
                }
                else error = "Event o id: " + id + "nie istnieje nie można dodać obrazu promującego ";



            }
            else error = "Nie podano obrazu promującego";
            return RedirectToAction("DetailsEvent","Event",new { id = id });
        }
    }
}
