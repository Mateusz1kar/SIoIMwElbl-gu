using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PracaDyplomowa.Interface;
using PracaDyplomowa.Models;

namespace PracaDyplomowa.Repozytory
{
    public class PublicationRepozytory : IPublicationRepozytory
    {
        private readonly AppDbContext _appDbContext;
        public PublicationRepozytory(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public void addPublication(Publication publication)
        {
            _appDbContext.Publications.Add(publication);
            _appDbContext.SaveChanges();
        }

        public void dellPublication(int eventId, string tokenText)
        {
            _appDbContext.Publications.Remove(getPublication(eventId, tokenText));
            _appDbContext.SaveChanges();
        }

        public List<Publication> getAllPublication()
        {
            return _appDbContext.Publications.ToList();
        }

        public Publication getPublication(int eventId, string tokenText)
        {
            return _appDbContext.Publications.FirstOrDefault(p => p.EventId == eventId & p.TokenText == tokenText);
        }

        public List<Publication> getPublicationoForEvent(int eventId)
        {
            return _appDbContext.Publications.Where(p => p.EventId == eventId).ToList();
        }
    }
}
