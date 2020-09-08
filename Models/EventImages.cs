using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracaDyplomowa.Models
{
    public class EventImages
    {
        public string ImageName { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
