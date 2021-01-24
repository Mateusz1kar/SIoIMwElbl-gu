using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PracaDyplomowa.Models;

namespace PracaDyplomowa.ViewsModel
{
    public class IndexVM
    {
        public List<Event> DataEvents { get; set; }
        [Required(ErrorMessage = "Data jest wymagana")]
        [DataType(DataType.Date, ErrorMessage = "Nipoprawna data")]
        public DateTime SearchDate { get; set; }
        public int id { get; set; }
    }
}
