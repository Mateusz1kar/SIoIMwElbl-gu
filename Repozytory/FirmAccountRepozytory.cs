using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PracaDyplomowa.Interface;
using PracaDyplomowa.Models;

namespace PracaDyplomowa.Repozytory
{
    public class FirmAccountRepozytory : IFirmAccoutRepozytory
    {
        private readonly AppDbContext _appDbContext;
        public FirmAccountRepozytory(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public void addFirmAccout(FirmAccount firmAccount)
        {
            _appDbContext.FirmAccounts.Add(firmAccount);
            _appDbContext.SaveChanges();
        }

        public void dellFirmAccout( string userName)
        {
            _appDbContext.FirmAccounts.Remove(getFirmAccount(userName));
            _appDbContext.SaveChanges();
        }

        public List<FirmAccount> getAllFirmAccount()
        {
            return _appDbContext.FirmAccounts.ToList();
        }

        public FirmAccount getFirmAccount(string userName)
        {
            return _appDbContext.FirmAccounts.FirstOrDefault(fa => fa.UserName == userName);

        }
    }
}
