using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PracaDyplomowa.Models;
using PracaDyplomowa.Interface;

using PracaDyplomowa.ViewsModel;

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

    }
}

