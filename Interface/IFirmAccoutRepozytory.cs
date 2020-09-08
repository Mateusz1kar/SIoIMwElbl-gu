using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PracaDyplomowa.Models;

namespace PracaDyplomowa.Interface
{
    public interface IFirmAccoutRepozytory
    {
        void addFirmAccout(FirmAccount firmAccount);
        void dellFirmAccout(string userName);
       
        FirmAccount getFirmAccount(string userName);
        List<FirmAccount> getAllFirmAccount();
    }
}
