using Runrs.Models;
using Runrs.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runrs.DataAccess.Repository.IRepository
{
    public interface IEventRegistrationRepository : IRepository<EventRegistration> // Inherits from the IRepository Interface for CRUD <--- Byron (16/05/2026)
    {
        void Update(EventRegistration obj);
        void Save();
        IEnumerable<EventRegistration> GetByEventId(int eventId);
        IEnumerable<EventRegistration> GetByUserId(int userId);
        bool IsRegistered (int userId, int eventId);
    }
}
