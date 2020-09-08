using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PracaDyplomowa.Models;

namespace PracaDyplomowa.ViewsModel
{
    public class AccoutVM
    {
        public string UserName { get; set; }
        public string FirmName { get; set; }
        public string FirmDescriotion { get; set; }
        public List<Token> Tokens { get; set; }
        public List<Event> Events { get; set; }

    }
}
