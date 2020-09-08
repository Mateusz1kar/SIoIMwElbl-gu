using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PracaDyplomowa.Interface;
using PracaDyplomowa.Models;

namespace PracaDyplomowa.Repozytory
{
    public class EventRepozytory : IEventRepozytory
    {
        private readonly AppDbContext _appDbContext;
        public EventRepozytory(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public void addEvent(Event e)
        {
            e.FirmAccount = _appDbContext.FirmAccounts.Find(e.UserName);
            _appDbContext.Events.Add(e);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<Event> allEvent()
        {
            return _appDbContext.Events.ToList();
        }

        public IEnumerable<Event> allUserEvents(string userName)
        {
            return _appDbContext.Events.Where(e => e.UserName == userName).OrderBy(e => e.DateStart);
        }

        public void delEvent(int id)
        {
            throw new NotImplementedException();
        }

        public Event findEvent(int Id)
        {
            var img= _appDbContext.EventImages.ToList();
            var e = _appDbContext.Events.Find(Id);
            return e;
        }

        public void update(Event e)
        {
            var ecentUpdate = _appDbContext.Events.FirstOrDefault(ev=>ev.EventId== e.EventId);
            ecentUpdate.Name =e.Name;
            ecentUpdate.Place = e.Place;
            ecentUpdate.ShortDescription =e.ShortDescription;
            ecentUpdate.Description =e.Description;
            ecentUpdate.DateStart = e.DateStart;
            ecentUpdate.DateEnd = e.DateEnd;
            _appDbContext.SaveChanges();
        }
    }
}
