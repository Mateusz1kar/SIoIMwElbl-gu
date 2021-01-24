using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PracaDyplomowa.Models;

namespace PracaDyplomowa.ViewsModel
{
    public class EventsListVM
    {
        public int id { get; set; }
        public List<Event> eventList { get; set; }
        public string error { get; set; }

        public string searchName { get; set; }
        public string sortByDS { get; set; }
        public DateTime searcDateStart { get; set; }
        public string sortByDE { get; set; }
        public string searchPlace { get; set; }
        public DateTime searcDateEnd { get; set; }
        public string typeSort { get; set; }
        public List<Tag> Tags { get; set; }
        [BindProperty]
        public List<int> CheckedTags { get; set; }
        [BindProperty]
        public string userEventSearch { get; set; }
    }
}
