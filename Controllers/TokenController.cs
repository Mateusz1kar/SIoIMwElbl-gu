using System;
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
            string error = "";
            if (User.Identity.IsAuthenticated)
            {
                if (model.NewTokenText!=null & model.NewPageId!= null & model.NewNamePage!= null)
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
                    else
                    {
                        error= "Podany Token został już zarejestrowany ";
                    }
                }
                else
                {
                    error ="Nie prawidłowe dane przę wypełnić wszystkie pola formularza ";
                }


            }
           
            return RedirectToAction("AccountPanel","Account", new { error = error});
        }
        public IActionResult DeleteToken(AccoutVM model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (_tokenRepozytory.TokenExist(model.DelTokentText))
                {
                    Token token = _tokenRepozytory.getToken(model.DelTokentText);
                    if (token.UserName == User.Identity.Name)
                    {
                        _tokenRepozytory.dellToken(model.DelTokentText);
                    }
                    
                }

            }
            return RedirectToAction("AccountPanel", "Account");
        }
    }
}
