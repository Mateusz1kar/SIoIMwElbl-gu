using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PracaDyplomowa.Models
{
    public class EventImages
    {
        [StringLength(450)]
        public string ImageName { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
