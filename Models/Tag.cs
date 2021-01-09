using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PracaDyplomowa.Models
{
    public class Tag
    {
        public int TagId { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana!")]
        public string Name { get; set; }

        public List<Event> EventeTag { get; set; }
    }
    public class EventeTag
    {
        // public int MiejsceTagId { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
