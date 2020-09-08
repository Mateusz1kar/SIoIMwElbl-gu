using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PracaDyplomowa.Interface;
using PracaDyplomowa.Models;

namespace PracaDyplomowa.Repozytory
{
    public class ImageRepozytory : IImageRepozytory
    {
        private readonly AppDbContext _appDbContext;
        public ImageRepozytory(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public void addEventImage(EventImages eventImages)
        {
            _appDbContext.EventImages.Add(eventImages);
            _appDbContext.SaveChanges();
        }

        public void delteEventImate(string imageName)
        {
            var eventImage = findEventImage(imageName);
            _appDbContext.EventImages.Remove(eventImage);
            _appDbContext.SaveChanges();
        }

        public void delteEventImate(int eventId)
        {
            var eventImages = findEventImages(eventId);
            foreach (var ei in eventImages)
            {
                _appDbContext.EventImages.Remove(ei);                
            }            
            _appDbContext.SaveChanges();
        }

        public EventImages findEventImage(string imageName)
        {
            return _appDbContext.EventImages.Find(imageName);
        }

        public List<EventImages> findEventImages(int eventId)
        {
            return _appDbContext.EventImages.Where(ei => ei.EventId == eventId).ToList();
        }
    }
}
