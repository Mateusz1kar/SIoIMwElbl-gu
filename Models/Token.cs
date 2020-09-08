using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracaDyplomowa.Models
{
    public class Token
    {
        public string UserName { get; set; }
        public FirmAccount FirmAccount { get; set; }
        public string TokenText { get; set; }
        //public string PublicationNeme { get; set; }
    }
}
