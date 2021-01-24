using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PracaDyplomowa.Models
{
    public class Token
    {
        [StringLength(450)]
        public string UserName { get; set; }
        public FirmAccount FirmAccount { get; set; }
        public string TokenText { get; set; }
        public string PageId { get; set; }
        [StringLength(450)]
        public string NamePage { get; set; }
        //public string PublicationNeme { get; set; }
    }
}
