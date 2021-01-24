using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PracaDyplomowa.Models
{
    public class FirmAccount
    {
        public string UserName { get; set; }
        [StringLength(450)]
        public string FirmName { get; set; }

        [StringLength(1000)]
        public string FirmDescriotion { get; set; }
        public string ConfirmatioCode { get; set; }
        public bool Comfirmed { get; set; }
        public List<Token> Tokens { get; set; }
        public List<Event> Events { get; set; }

    }
}
