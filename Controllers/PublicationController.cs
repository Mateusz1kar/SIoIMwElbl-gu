using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PracaDyplomowa.Interface;
using PracaDyplomowa.Models;
using PracaDyplomowa.ViewsModel;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PracaDyplomowa.Controllers
{
    public class PublicationController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IImageRepozytory _imageRepozytory;
        public PublicationController(IHostingEnvironment hostingEnvironment, IImageRepozytory imageRepozytory)
        {
            this.hostingEnvironment = hostingEnvironment;
            _imageRepozytory = imageRepozytory;
        }
        // GET: /<controller>/
        //public async Task<IActionResult> PublicToFacebook()
        //{
        //    //FacebookApi facebookApi = new FacebookApi("103888034769355",
        //    //    "EAAXc743WXTkBAPDtK2tyTfiGvOMGiTG4FW79Ljgd7c5uMvxo8GB27vr6VEKFCcafNwIjeJ9nw7sjrFr7FHq0rCaBLP9wTJLKHZAwtG5TxCMlPTZC7c5EkspemK8AvFwOLZCH79m8jMXRDoMiiAlinbKDZByPNw3FOp5ZBB0wO5gVZCESKMOkFuNVXFb4ZBFTkOTwG6wltXPWwZDZD");
        //    //var result = await facebookApi.PublishMessage("Test");
        //    Facebook facebook = new Facebook(
        //        "EAAXc743WXTkBAPDtK2tyTfiGvOMGiTG4FW79Ljgd7c5uMvxo8GB27vr6VEKFCcafNwIjeJ9nw7sjrFr7FHq0rCaBLP9wTJLKHZAwtG5TxCMlPTZC7c5EkspemK8AvFwOLZCH79m8jMXRDoMiiAlinbKDZByPNw3FOp5ZBB0wO5gVZCESKMOkFuNVXFb4ZBFTkOTwG6wltXPWwZDZD",
        //        "103888034769355");
        //    string image = Path.Combine(hostingEnvironment.WebRootPath, "Images\\", "kotek.jpg");
        //    string imgeUrl = "https://localhost:44378/" + "Images/kotek.jpg";
        //    string result = facebook.PublishToFacebook("some text", "https://5.allegroimg.com/s512/03352a/ba4fe19545a98511914e9eb03325/PODKLADKA-LAMINOWANA-A2-NA-BIURKO-KOT-KOTEK-KOTKI"); //"https://localhost:44378/Images/kotek.jpg");//"~\\Images\\kotek.jpg"
        //    //var result = await facebook.PublishSimplePost("some text");
        //    //Console.WriteLine(result);
        //    return RedirectToAction("Index","Home");
        //}
        public async Task<IActionResult> PublicToFacebook(DetailsEventVM model)
        {
            var error = "";
            Facebook facebook = new Facebook(
                model.PublicationTokenText,
                model.PublicationPageId
               //"EAAXc743WXTkBAGtC3WFZBLFxKgRRbrZAgvsarX7ZCN0ubCTIwaXZBeF0CbjF1VLLdxkXr0THi5ZBX3biK522GaMWneeZBOHTl5ESqCVwHW1SNNvl5JTuL7Gl0sclAhkPOyztukhVu8TdGE2lmLRqUmf9cvoZAZBiDVPxt0QE86PfhptJtHN61XzZC",
               //"103888034769355"
               );
            var result = await facebook.PublishSimplePost(model.PublicationText);
            if (result.Item1 != 200)
            {
                error = "Wystąpił błąd publikacja nie została wykonana";
            }
            return RedirectToAction("DetailsEvent", "Event", new { id = model.id, error = error });

        }
        public async Task<IActionResult> PublicImageToFacebook(DetailsEventVM model)
        {
            var error = "";
            Facebook facebook = new Facebook(
                model.PublicationTokenText,
                model.PublicationPageId
               );
            List<EventImages> imageList = _imageRepozytory.findEventImages(model.id);
            if (imageList.Count>0)
            {
                //string image = Path.Combine(hostingEnvironment.WebRootPath, "Images/EventImages/" + imageList[0].ImageName);
              
                //string image2 = Path.Combine("~/Images/EventImages/", "Images/EventImages/" + imageList[0].ImageName);
                string image3 = Path.Combine("https://sioimwelblągu.azurewebsites.net/Images/EventImages/" , imageList[0].ImageName);

                //string imgeUrl = "https://localhost:44378/" + "Images/kotek.jpg";
                //string img = "https://dziendobry.tvn.pl/media/cache/content_cover/imie-dla-kotki-jak-wybrac-oryginalne-imie-i-dobrze-dopasowac-je-do-kotki-jpg.jpg";
                //string img2 = "https://i.ytimg.com/vi/S4UCxJK27D8/hqdefault.jpg";
                //List<string> imgList = new List<string>() { img, img2 };
                //var result = facebook.PublishToFacebook("image1", image); 
                //var result2 = facebook.PublishToFacebook("image2", image2); 
                var result3 = facebook.PublishToFacebook(model.PublicationText, image3);

            }
            else
            {
                error = "Brak zdjęć do piblikacji";
            }

            return RedirectToAction("DetailsEvent", "Event", new { id = model.id, error = error });

        }
    }
}
