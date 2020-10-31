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
        public async Task<IActionResult> Login(LoginVM loginvm)
        {
            if (!ModelState.IsValid)
                return View(loginvm);

            var user = await _userManager.FindByNameAsync(loginvm.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginvm.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("AccountPanel");
                }
            }
            ModelState.AddModelError("", "Niepoprawna nazwa użytkownika lub hasło");
            return View(loginvm);
        }

        // GET: /<controller>/
        public IActionResult Register()
        {
            return View(new RegisterVM());
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser() { UserName = registerVM.UserName };
                var result = await _userManager.CreateAsync(user, registerVM.Password);
                if (result.Succeeded)
                {
                    FirmAccount newFirmAccount = new FirmAccount()
                    {
                        FirmDescriotion = registerVM.FirmDescriotion,
                        FirmName = registerVM.FirmName,
                        Events = new List<Event>(),
                        Tokens = new List<Token>(),
                        UserName = user.UserName
                    };
                    _firmAccountRepozytory.addFirmAccout(newFirmAccount);
                    return RedirectToAction("Index", "Home");
                }
            }


            return View(registerVM);
        }


        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccountPanel()
        {
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






        static bool mailSent = false;
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            mailSent = true;
        }
        public IActionResult SenndEmail(string email)
        {
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress("kar.matgogle",
            "kar.matgogle@gmail.com");
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress("kar.matgogle",
             "kar.matgogle@gmail.com");
            message.To.Add(to);

            message.Subject = "This is email subject";

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<h1>Hello World!</h1>";
            bodyBuilder.TextBody = "Hello World!";
            message.Body = bodyBuilder.ToMessageBody();

            MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
            // client.Connect("smtp.wp.pl", 465, false);
            //client.Authenticate("kar.mateusz@wp.pl", "KAMI21`kami");
            client.Connect("smtp.gmail.com", 465);
            client.Authenticate("kar.matgogle@gmail.com", "NieUrzGmail@");
            client.Send(message);
            client.Disconnect(true);
            client.Dispose();

            // Command-line argument must be the SMTP host.
            //SmtpClient client = new SmtpClient("smtp.wp.pl", 465);
            //// Specify the email sender.
            //// Create a mailing address that includes a UTF8 character
            //// in the display name.
            //MailAddress from = new MailAddress("kar.mateusz@wp.pl",
            //   "KAMI21`kami",
            //System.Text.Encoding.UTF8);
            //// Set destinations for the email message.
            //MailAddress to = new MailAddress("kar.mateusz@wp.pl");
            //// Specify the message content.
            //MailMessage message = new MailMessage(from, to);
            //message.Body = "This is a test email message sent by an application. ";
            //// Include some non-ASCII characters in body and subject.
            //string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
            //message.Body += Environment.NewLine + someArrows;
            //message.BodyEncoding = System.Text.Encoding.UTF8;
            //message.Subject = "test message 1" + someArrows;
            //message.SubjectEncoding = System.Text.Encoding.UTF8;
            //// Set the method that is called back when the send operation ends.
            //client.SendCompleted += new
            //SendCompletedEventHandler(SendCompletedCallback);
            //// The userState can be any object that allows your callback
            //// method to identify this send operation.
            //// For this example, the userToken is a string constant.
            //string userState = "test message1";
            //client.SendAsync(message, userState);
            ////Console.WriteLine("Sending message... press c to cancel mail. Press any other key to exit.");
            ////string answer = Console.ReadLine();
            //// If the user canceled the send, and mail hasn't been sent yet,
            //// then cancel the pending operation.
            ////if (answer.StartsWith("c") && mailSent == false)
            ////{
            ////    client.SendAsyncCancel();
            ////}
            //// Clean up.
            //message.Dispose();
            //Console.WriteLine("Goodbye.");

            return RedirectToAction("index","Home");
        }
    
    public IActionResult SenndEmail2(string email)
        {


            //Command - line argument must be the SMTP host.
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.wp.pl", 465);
            // Specify the email sender.
            // Create a mailing address that includes a UTF8 character
            // in the display name.
            MailAddress from = new MailAddress("kar.mateusz@wp.pl",
               "KAMI21`kami",
            System.Text.Encoding.UTF8);
            // Set destinations for the email message.
            MailAddress to = new MailAddress("kar.mateusz@wp.pl");
            // Specify the message content.
            MailMessage message = new MailMessage(from, to);
            message.Body = "This is a test email message sent by an application. ";
            // Include some non-ASCII characters in body and subject.
            string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
            message.Body += Environment.NewLine + someArrows;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = "test message 1" + someArrows;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            // Set the method that is called back when the send operation ends.
            client.SendCompleted += new
            SendCompletedEventHandler(SendCompletedCallback);
            // The userState can be any object that allows your callback
            // method to identify this send operation.
            // For this example, the userToken is a string constant.
            string userState = "test message1";
            client.SendAsync(message, userState);
            //Console.WriteLine("Sending message... press c to cancel mail. Press any other key to exit.");
            //string answer = Console.ReadLine();
            // If the user canceled the send, and mail hasn't been sent yet,
            // then cancel the pending operation.
            //if (answer.StartsWith("c") && mailSent == false)
            //{
            //    client.SendAsyncCancel();
            //}
            // Clean up.
            message.Dispose();
        

            return RedirectToAction("index","Home");
        }
        public IActionResult SenndEmail3(string email)
        {
            System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("smtp.wp.pl", 25);

            smtpClient.Credentials = new System.Net.NetworkCredential("kar.mateusz@wp.pl", "KAMI21`kami");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage mailMessage = new MailMessage("kar.mateusz@wp.pl", "kar.mateusz@wp.pl");
            mailMessage.Subject = "test";
            mailMessage.Body = "please";

            try
            {
                smtpClient.Send(mailMessage);

            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "Home");
            }
            return RedirectToAction("index", "Home");
        }

        public IActionResult SenndEmail4(string email)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("kar.matgogle@gmail.com");
                mail.To.Add("kar.matgogle@gmail.com");
                mail.Subject = "Test send";
                mail.Body = "<h1>Test</h1>";
                mail.IsBodyHtml = true;
                using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new System.Net.NetworkCredential("kar.matgogle@gmail.com", "NieUrzGmail@");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                    
                }
            }

            return RedirectToAction("index", "Home");
        }
    }
   
}

