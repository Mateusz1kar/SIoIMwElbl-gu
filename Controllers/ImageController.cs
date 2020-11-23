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
            if (User.Identity.IsAuthenticated)
                if (_eventRepozytory.findEvent(id).UserName == User.Identity.Name)
                    if (eventImage != null)
                    {
                        var buf = eventImage.FileName.Split('.')[1];
                        if (buf == "png" || buf == "jpg" || buf == "jpeg")
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
                        else error = "Nie poprawny format pliku. Proszę podać plik typu jpg/jpeg/png";
                    }
                    else error = "Nie podano obrazu promującego";
                else error = "Brak uprwnień";
            else error = "Do wykonania ej kacji musisz być zalogowany";
            return RedirectToAction("DetailsEvent","Event",new { id = id ,error=error});
        }
        public IActionResult DeleteEventImage(string DeleteFileName, int id)
        {
            string error = "";
            if (User.Identity.IsAuthenticated)
                if (_eventRepozytory.findEvent(id).UserName == User.Identity.Name)
                       if (DeleteFileName != null)
                        {
                            string deletePath = Path.Combine(hostingEnvironment.WebRootPath,"Images\\EventImages\\"+ DeleteFileName);
                            System.IO.File.Delete(deletePath);
                            _imageRepozytory.delteEventImate(DeleteFileName);

                        }
                        else
                        {
                            error = "Błędna nazwa obrazu promującego";
                        }
                else error = "Nie podano obrazu promującego";
            else error = "Brak uprwnień";
            return RedirectToAction("DetailsEvent", "Event", new { id = id, error = error });
        }
        public IActionResult DeleteAllEventImage(List<string>images,int eventId) 
        {
            string error = "";
            if (User.Identity.IsAuthenticated)
            {
                if (_eventRepozytory.findEvent(eventId).UserName == User.Identity.Name)
                {
                    foreach (var item in images)
                    {
                        string deletePath = Path.Combine(hostingEnvironment.WebRootPath, "Images\\EventImages\\" + item);
                        System.IO.File.Delete(deletePath);
                        _imageRepozytory.delteEventImate(item);
                    }
                    return RedirectToAction("DeleteEvent", "Event", new { id = eventId });
                }                
              else error = "Nie podano obrazu promującego";
            }               
            else error = "Brak uprwnień";
            return RedirectToAction("DetailsEvent", "Event", new { id = eventId, error = error });
        }
    }
}
