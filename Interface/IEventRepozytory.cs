using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PracaDyplomowa.Models;

namespace PracaDyplomowa.Interface
{
    public interface IEventRepozytory
    {
        void addEvent(Event e);
        void delEvent(int id);
        IEnumerable<Event> allEvent();
        IEnumerable<Event> allUserEvents(string userName);
        Event findEvent(int Id);
        IEnumerable<Event> searchEvents(string name, bool searchByDS, DateTime DateStart, bool searchByDE, DateTime DateEnd, bool typeSort, string userName, string searchPlace);
        IEnumerable<Event> searchDayEvents(DateTime DateStart);
        void update(Event e);
        public List<Tag> getEventTag(Event e);
        public List<string> getEventImage(Event e);
    }
}
