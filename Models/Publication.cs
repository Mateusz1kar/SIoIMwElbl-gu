using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracaDyplomowa.Models
{
    public class Publication
    {
        public int EventId { get; set; }
        public Event Event { get; set; }        
        //public string PublicationName { get; set; }
        public string TokenText { get; set; }
        //public string FirmName { get; set; }
        public Token Token { get; set; }
    }
}
