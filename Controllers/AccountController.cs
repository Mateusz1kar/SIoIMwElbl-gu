using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PracaDyplomowa.Models;
using PracaDyplomowa.Interface;
using PracaDyplomowa.ViewsModel;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Mail;
using System.ComponentModel;

//using System.Net;
//using System.Net.Mail;
//using System.Net.Mime;
//using System.Threading;
//using System.ComponentModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PracaDyplomowa.Controllers
{
    public class AccountController : Controller
    {
        // GET: /<controller>/
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IFirmAccoutRepozytory _firmAccountRepozytory;
        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IFirmAccoutRepozytory firmAccountRepozytory)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _firmAccountRepozytory = firmAccountRepozytory;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginvm, string error = "")
        {
            if (!ModelState.IsValid)
                return View(loginvm);

            var user = await _userManager.FindByNameAsync(loginvm.UserName);

            if (user != null)
            {
                var buf = _firmAccountRepozytory.getFirmAccount(loginvm.UserName);
                if (!buf.Comfirmed)
                {
                    ModelState.AddModelError("", "Konot nie zostało aktywowane");
                    //loginvm.error = "Konot nie zostało aktywowane";
                    return View(loginvm);
                }
                var result = await _signInManager.PasswordSignInAsync(user, loginvm.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("AccountPanel");
                }
            }
            ModelState.AddModelError("", "Niepoprawna nazwa użytkownika lub hasło");
            return View(loginvm);
        }

        public IActionResult ActivateAccount(string error="")
        {
            ModelState.AddModelError("", error);

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ActivateAccount(RegisterVM model)
        {

            
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                var buf = _firmAccountRepozytory.getFirmAccount(model.UserName);
                if (buf.ConfirmatioCode==model.ConfirmatioCode && buf.FirmName == model.FirmName)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        _firmAccountRepozytory.setComfirmed(user.UserName, true);
                        _firmAccountRepozytory.setConfirmatioCode(user.UserName, "");
                        return RedirectToAction("AccountPanel");
                    }
                    else
                        ModelState.AddModelError("", "Niepoprawne hasło");
                }
                else
                    ModelState.AddModelError("", "Niepoprwne dane.");

            }else
                ModelState.AddModelError("", "Niepoprawna nazwa użytkownika lub hasło");
            return View(model);
        } 
        public async Task<IActionResult> ForgetPasswort(RegisterVM model, string error = "")
        {

            if (!ModelState.IsValid)
                return View(model);
            if (model.Password!=model.PasswordRepeat)
            {
                ModelState.AddModelError("", "Hasła nie są takie same");
                //model.error = "Hasła nie są takie same";

                return View("ActivateAccount", model);
            }
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {

                
                var buf = _firmAccountRepozytory.getFirmAccount(model.UserName);
                if (buf.ConfirmatioCode==model.ConfirmatioCode && buf.FirmName==model.FirmName)
                {

                  
                        var resultChangePassword = await _userManager.ResetPasswordAsync(user, model.ConfirmatioCode, model.Password);
                        if (resultChangePassword.Succeeded)
                        {
                            _firmAccountRepozytory.setComfirmed(user.UserName, true);
                            _firmAccountRepozytory.setConfirmatioCode(user.UserName, "");
                            return RedirectToAction("Login");
                         }
                        else
                        {
                        ModelState.AddModelError("", "Błędne hasło.\n" +
                                 "Hasło musi posiadać cojmniej 8 znaków i zawierać dużą i małą literę , liczbę i znak specyjalny");
                        //model.error = "Błąd resetu hasła";
                        }
                      
                        return RedirectToAction("ForgetPasswort", model);
                    
                }
                else
                {
                    ModelState.AddModelError("", "Podane dane nie są poprawne ");
                    //model.error = "Błędne dane";
                }
               
            }
            ModelState.AddModelError("", "Niepoprawna nazwa użytkownika lub hasło");
            return View(model);
        }

        // GET: /<controller>/
        public IActionResult Register(string error = "")
        {
            return View(new RegisterVM());
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                if (registerVM.Password != registerVM.PasswordRepeat)
                {
                    ModelState.AddModelError("", "Hasła nie są takie same");
                }
                else
                {
                    if (_firmAccountRepozytory.getFirmAccount(registerVM.UserName) == null)
                    {
                        var user = new IdentityUser() { UserName = registerVM.UserName, Email = registerVM.Email };
                        var result = await _userManager.CreateAsync(user, registerVM.Password);
                        if (result.Succeeded)
                        {
                            var code = _userManager.GeneratePasswordResetTokenAsync(user).ToString();
                            FirmAccount newFirmAccount = new FirmAccount()
                            {
                                FirmDescriotion = registerVM.FirmDescriotion,
                                FirmName = registerVM.FirmName,
                                Events = new List<Event>(),
                                Tokens = new List<Token>(),
                                UserName = user.UserName,
                                Comfirmed = false,
                                ConfirmatioCode = code
                            };
                            _firmAccountRepozytory.addFirmAccout(newFirmAccount);

                            return RedirectToAction("SenndAccountConfirmEmail", registerVM);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Błędne hasło.\n" +
                                 "Hasło musi posiadać cojmniej 8 znaków i zawierać dużą i małą literę oraz liczbę i znak specyjalny");
                            //registerVM.error = "Błędne hasło.\n" +
                            //     "Hasło musi posiadać cojmniej 8 znaków i zawierać dużą i małą literę , liczbę i znak specyjalny";
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Podany urżtkownik  już istnieje proszę zmienić login");
                        //registerVM.error = "Podany urżtkownik  już istnieje proszę zmienić login";
                    }
                }
                    
               
            }


            return View(registerVM);
        }


        public async Task<IActionResult> LogOut( string error = "")
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccountPanel(string error = "")
        {
            if (error!="")
            {
                ModelState.AddModelError("", error);
            }
            if (_signInManager.IsSignedIn(User))
            {
                FirmAccount firm = _firmAccountRepozytory.getFirmAccount(User.Identity.Name);
                AccoutVM model = new AccoutVM()
                {
                    FirmDescriotion = firm.FirmDescriotion,
                    FirmName = firm.FirmName,
                    Events = firm.Events,
                    Tokens = firm.Tokens,
                    UserName = firm.UserName
                };
                return View(model);
            }
           
            return View("Login");
        }

        public IActionResult UpdateFirmAccount(AccoutVM accoutUpdate)
        {
            if (_signInManager.IsSignedIn(User))
            {
                _firmAccountRepozytory.updateFirmAccount(User.Identity.Name, accoutUpdate.FirmName, accoutUpdate.FirmDescriotion);

            }
            return RedirectToAction("AccountPanel");
        }

   
        public async Task<IActionResult> SenndAccountConfirmEmail(RegisterVM model)
        {
           

            var user = await _userManager.FindByNameAsync(model.UserName);
            string error = "";
            if (user != null && user.Email==model.Email)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var firm = _firmAccountRepozytory.getFirmAccount(model.UserName);
                Random random = new Random();
                _firmAccountRepozytory.setComfirmed(user.UserName, false);
                _firmAccountRepozytory.setConfirmatioCode(user.UserName, code);
                using (MailMessage mail = new MailMessage())
                {
                    var body = "<div>" +
                        "<h1>System informacyjny o inprezach masowych w Elblągu</h1>" +
                        "<h2>Kod:</h2>" +
                        "<h3>" + firm.ConfirmatioCode + "</h3>" +
                        "</div>";
                    mail.From = new MailAddress("kar.matgogle@gmail.com");
                    mail.To.Add(user.Email);
                    mail.Subject = "Kod aktywacyjny";
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    _firmAccountRepozytory.setComfirmed(model.UserName, false);
                    using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587))
                    {
                        
                        try
                        {
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new System.Net.NetworkCredential("SIoIMwElblag@gmail.com", "PraceDyplomowa@@!");
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                        }
                        catch (Exception e)
                        {
                            error = e.Message;

                            return RedirectToAction("ActivateAccount", new { error = error });
                        }
                       

                    }
                }
            }
            else
                error= "Niepoprwne dane";

            return RedirectToAction("ActivateAccount",new { error = error });
        }
    }
   
}

