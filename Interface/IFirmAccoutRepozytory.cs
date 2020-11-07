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
        void updateFirmAccount(string userName,string firmName, string firmDescription);
        FirmAccount getFirmAccount(string userName);
        List<FirmAccount> getAllFirmAccount();
        void setConfirmatioCode(string userName, string confirmatioCode);
        void setComfirmed(string userName, bool bomfirmed);
    }
}
