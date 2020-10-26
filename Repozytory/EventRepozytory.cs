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
            _appDbContext.Events.Remove(findEvent(id));
            _appDbContext.SaveChanges();
        }

        public Event findEvent(int Id)
        {
            var img= _appDbContext.EventImages.ToList();
            var e = _appDbContext.Events.Find(Id);
            return e;
        }

        public IEnumerable<Event> searchEvents(string name, bool searchByDS, DateTime DateStart, bool searchByDE, DateTime DateEnd, bool typeSort)
        {
            IEnumerable<Event> eventlist;
            if(searchByDS==true & searchByDE==true)
                eventlist= _appDbContext.Events.Where(e => (e.Name.Contains(name) || name==null) & e.DateStart >= DateStart & e.DateEnd <= DateEnd).OrderBy(e => e.DateStart).ToList();
            else if (searchByDS == true & searchByDE == false)
                eventlist= _appDbContext.Events.Where(e => (e.Name.Contains(name) || name == null) & e.DateStart >= DateStart).OrderBy(e => e.DateStart).ToList();
            else if (searchByDS == false & searchByDE == true)
                eventlist= _appDbContext.Events.Where(e => (e.Name.Contains(name) || name == null) & e.DateEnd <= DateEnd).OrderBy(e => e.DateStart).ToList();
            else
                eventlist= _appDbContext.Events.Where(e => (e.Name.Contains(name) || name == null)).OrderBy(e => e.DateStart).ToList();
            if (!typeSort)
            {
                eventlist=eventlist.Reverse();
            }
            return eventlist;
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
