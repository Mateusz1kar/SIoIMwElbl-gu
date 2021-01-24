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
            var buf = _appDbContext.EventeTags.ToList();
            var bufTag = _appDbContext.Tags.ToList();
            _appDbContext.EventImages.ToList();
            return _appDbContext.Events.ToList();
        }

        public IEnumerable<Event> allUserEvents(string userName)
        {
            var buf = _appDbContext.EventeTags.ToList();
            var bufTag = _appDbContext.Tags.ToList();
            return _appDbContext.Events.Where(e => e.UserName == userName).OrderBy(e => e.DateStart);
        }

        public void delEvent(int id)
        {
            _appDbContext.Events.Remove(findEvent(id));
            _appDbContext.SaveChanges();
        }

        public Event findEvent(int Id)
        {
            var buf = _appDbContext.EventeTags.ToList();
            var bufTag = _appDbContext.Tags.ToList();
            var img= _appDbContext.EventImages.ToList();
            var owner = _appDbContext.FirmAccounts.ToList();
            var e = _appDbContext.Events.Find(Id);
            return e;
        }

        public IEnumerable<Event> searchEvents(string name, bool searchByDS, DateTime DateStart, bool searchByDE, DateTime DateEnd, bool typeSort, string userName, string searchPlace)
        {
            var buf = _appDbContext.EventeTags.ToList();
            var bufTag = _appDbContext.Tags.ToList();
            IEnumerable<Event> eventlist;
            if(searchByDS==true & searchByDE==true)
                eventlist= _appDbContext.Events.Where(e => (e.Name.Contains(name) || name == null) & (e.Place.Contains(searchPlace) || searchPlace == null) & (e.UserName == userName || userName == null) &
                e.DateStart >= DateStart & e.DateStart <= DateEnd).OrderBy(e => e.DateStart).ToList();
            else if (searchByDS == true & searchByDE == false)
                eventlist= _appDbContext.Events.Where(e => (e.Name.Contains(name) || name == null) & (e.Place.Contains(searchPlace) || searchPlace == null) & (e.UserName == userName || userName == null) & 
                ((e.DateEnd >= DateStart & e.DateStart <= DateStart) || e.DateStart >= DateStart)).OrderBy(e => e.DateStart).ToList();
            else if (searchByDS == false & searchByDE == true)
                eventlist= _appDbContext.Events.Where(e => (e.Name.Contains(name) || name == null) & (e.Place.Contains(searchPlace) || searchPlace == null) & (e.UserName == userName || userName == null) & 
                (e.DateEnd <= DateEnd || e.DateStart <= DateEnd)).OrderBy(e => e.DateStart).ToList();
            else
                eventlist= _appDbContext.Events.Where(e => (e.Name.Contains(name) || name == null) & (e.Place.Contains(searchPlace) || searchPlace == null) & 
                (e.UserName == userName || userName == null)).OrderBy(e => e.DateStart).ToList();
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
            ecentUpdate.Tags = e.Tags;
            _appDbContext.SaveChanges();
        }

        public List<Tag> getEventTag(Event e)
        {

            List<Tag> tags = new List<Tag>();
            if (e.Tags != null)
            {
                foreach (var item in e.Tags)
                {
                    tags.Add(item.Tag);
                }
            }
            return tags;
        }
        public List<string> getEventImage(Event e)
        {

            List<string> images = new List<string>();
            if (e.Images != null)
            {
                foreach (var item in e.Images)
                {
                    images.Add(item.ImageName);
                }
            }
            return images;
        }

        public IEnumerable<Event> searchDayEvents(DateTime date)
        {

            var buf = _appDbContext.EventeTags.ToList();
            var bufTag = _appDbContext.Tags.ToList();
            IEnumerable<Event> eventlist;
                eventlist = _appDbContext.Events.Where(e => (e.DateStart <= new DateTime(date.Year, date.Month, date.Day, 23,59,59) & e.DateEnd >=date)).ToList();
            
            return eventlist;
        }
    }
}
