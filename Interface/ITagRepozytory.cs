using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PracaDyplomowa.Models;

namespace PracaDyplomowa.Interface
{
    public interface ITagRepozytory
    {
        void addTag(Tag tag);
        void delTag(int id);
        IEnumerable<Tag> getAllTag();
        Tag getTag(int Id);
    }
}
