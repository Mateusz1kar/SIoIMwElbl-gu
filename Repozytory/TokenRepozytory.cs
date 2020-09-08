using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PracaDyplomowa.Interface;
using PracaDyplomowa.Models;

namespace PracaDyplomowa.Repozytory
{
    public class TokenRepozytory : ITokenRepozytory
    {
        private readonly AppDbContext _appDbContext;
        public TokenRepozytory(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public void addToken(Token token)
        {
            _appDbContext.Tokens.Add(token);
            _appDbContext.SaveChanges();
        }

        public void dellToken(string TokenText)
        {
            _appDbContext.Tokens.Remove(getToken(TokenText));
            _appDbContext.SaveChanges();
        }

        public List<Token> getAll()
        {
            return _appDbContext.Tokens.ToList();
        }

        public Token getToken(string TokenText)
        {
            return _appDbContext.Tokens.FirstOrDefault(t => t.TokenText == TokenText);
        }
    }
}
