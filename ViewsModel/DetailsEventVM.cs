using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PracaDyplomowa.Models;

namespace PracaDyplomowa.ViewsModel
{
    public class DetailsEventVM
    {
        public int id { get; set; }
        public Event eventDetail { get; set; }
        public string error { get; set; }
        public IFormFile eventImage { get; set; }
    }
}
