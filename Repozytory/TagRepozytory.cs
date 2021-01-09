using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PracaDyplomowa.Interface;
using PracaDyplomowa.Models;

namespace PracaDyplomowa.Repozytory
{
    public class TagRepozytory : ITagRepozytory
    {
        private readonly AppDbContext _appDbContext;
        public TagRepozytory(AppDbContext appDbContex)
        {
            _appDbContext = appDbContex;
        }
        public void delTag(int id)
        {
            _appDbContext.Tags.Remove(getTag(id));
            _appDbContext.SaveChanges();
        }

        public void addTag(Tag tag)
        {
            _appDbContext.Tags.Add(tag);
            _appDbContext.SaveChanges();
        }

        public Tag getTag(int Id)
        {
            return _appDbContext.Tags.FirstOrDefault(t => t.TagId == Id);

        }


        IEnumerable<Tag> ITagRepozytory.getAllTag()
        {
            return _appDbContext.Tags;
        }
    }
}
