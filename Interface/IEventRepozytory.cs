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
        void update(Event e);
    }
}
