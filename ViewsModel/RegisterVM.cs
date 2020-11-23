using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PracaDyplomowa.ViewsModel
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana.")]
        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Błędne hasło.\n" +
            "Hasło musi posiadać cojmniej 8 znaków i zawierać dużą i małą literę , liczbę i znak specyjalny")]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; } 
        public string PasswordRepeat { get; set; }
        [Required(ErrorMessage = "Nazwa firmy jest wymagana.")]
        [Display(Name = "Nazwa firmy")]
        public string FirmName { get; set; }
        public string FirmDescriotion { get; set; }
        public string ConfirmatioCode { get; set; }
        public bool Comfirmed { get; set; }

        [Required(ErrorMessage = "Email jest wymagana.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string error { get; set; }
    }
}
