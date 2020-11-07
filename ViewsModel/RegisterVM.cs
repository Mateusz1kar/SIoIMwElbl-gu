using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PracaDyplomowa.ViewsModel
{
    public class RegisterVM
    {
        [Required]
        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; } 
        public string PasswordRepeat { get; set; }
        [Required]
        [Display(Name = "Nazwa firmy")]
        public string FirmName { get; set; }
        public string FirmDescriotion { get; set; }
        public string ConfirmatioCode { get; set; }
        public bool Comfirmed { get; set; }
        [Required]
        public string Email { get; set; }
        public string error { get; set; }
    }
}
