using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PracaDyplomowa.Models;

namespace PracaDyplomowa.Interface
{
    public interface IPublicationRepozytory
    {
        void addPublication(Publication publication);
        void dellPublication(int eventId , string tokenText);
        Publication getPublication(int eventId, string tokenText);
        List<Publication> getAllPublication();
        List<Publication> getPublicationoForEvent(int eventId);
    }
}
