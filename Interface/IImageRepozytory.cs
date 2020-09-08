using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PracaDyplomowa.Models;

namespace PracaDyplomowa.Interface
{
    public interface IImageRepozytory
    {
        void addEventImage(EventImages eventImages);
        void delteEventImate(string imageName);
        void delteEventImate(int eventId);
        EventImages findEventImage(string imageName);
        List<EventImages> findEventImages(int eventId);
    }
}
