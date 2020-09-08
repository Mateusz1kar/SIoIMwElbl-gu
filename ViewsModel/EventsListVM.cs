using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PracaDyplomowa.Models;

namespace PracaDyplomowa.ViewsModel
{
    public class EventsListVM
    {
        public int id { get; set; }
        public List<Event> eventList { get; set; }
        public string error { get; set; }
    }
}
