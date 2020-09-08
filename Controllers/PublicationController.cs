using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PracaDyplomowa.Models;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PracaDyplomowa.Controllers
{
    public class PublicationController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        public PublicationController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        // GET: /<controller>/
        public async Task<IActionResult> PublicToFacebook()
        {
            //FacebookApi facebookApi = new FacebookApi("101060971721836",
            //    "EAAXc743WXTkBAIX92wzfxON9AjGFo9Qr6iUz6Tjq6yFsA4LbsxVGBHeToYA1d5ovnSO7d1gqU4DuLhOYuE4nTETZCi0QKbHVQ6DEnuPNkvL5IO6kHs3XdPA3dBrSFLwqY3RbpPOgmtMazUc7QWkURGtXcZCQk7E7Fi3xUYgSjBFS2LvoZCKqiGtfAyVQts7k0rDaCWzAwZDZD");
            //var result = await facebookApi.PublishMessage("Test");
            Facebook facebook = new Facebook("EAAXc743WXTkBAIX92wzfxON9AjGFo9Qr6iUz6Tjq6yFsA4LbsxVGBHeToYA1d5ovnSO7d1gqU4DuLhOYuE4nTETZCi0QKbHVQ6DEnuPNkvL5IO6kHs3XdPA3dBrSFLwqY3RbpPOgmtMazUc7QWkURGtXcZCQk7E7Fi3xUYgSjBFS2LvoZCKqiGtfAyVQts7k0rDaCWzAwZDZD",
                "101060971721836");
            string image = Path.Combine(hostingEnvironment.WebRootPath, "Images\\","kotek.jpg");
            string imgeUrl = "https://localhost:44378/"+ "Images/kotek.jpg";
            string result = facebook.PublishToFacebook("some text", "https://localhost:44378/Images/kotek.jpg");//"~\\Images\\kotek.jpg"
            Console.WriteLine(result);
            return RedirectToAction("Index","Home");
        }
    }
}
