﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PracaDyplomowa.Models;

namespace PracaDyplomowa.Interface
{
    public interface ITokenRepozytory
    {
        void addToken(Token token);
        void dellToken(string TokenText );
        Token getToken(string TokenText );
        List<Token> getAll();
        bool TokenExist(string TokenText);
    }
}
