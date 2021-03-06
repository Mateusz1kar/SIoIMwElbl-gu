﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PracaDyplomowa.Interface;
using PracaDyplomowa.Models;
using PracaDyplomowa.ViewsModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PracaDyplomowa.Controllers
{
    public class TokenController : Controller
    {
        private readonly ITokenRepozytory _tokenRepozytory;
        private readonly IFirmAccoutRepozytory _firmAccoutRepozytory;
        public TokenController(ITokenRepozytory tokenRepozytory, IFirmAccoutRepozytory firmAccoutRepozytory)
        {
            _firmAccoutRepozytory = firmAccoutRepozytory;
            _tokenRepozytory = tokenRepozytory;
        }
        // GET: /<controller>/
        public IActionResult AddToken(AccoutVM model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!_tokenRepozytory.TokenExist(model.NewTokenText))
                {
                    var newToken = new Token()
                    {
                        FirmAccount = _firmAccoutRepozytory.getFirmAccount(User.Identity.Name),
                        PageId = model.NewPageId,
                        TokenText = model.NewTokenText,
                        UserName = User.Identity.Name,
                        NamePage = model.NewNamePage
                    };
                    _tokenRepozytory.addToken(newToken);
                }
               
        }
           
            return RedirectToAction("AccountPanel","Account");
        }
    }
}
