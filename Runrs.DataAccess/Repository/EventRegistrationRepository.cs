using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runrs.Models;
using Runrs.DataAccess;
using Runrs.DataAccess.Repository.IRepository;
using Runrs.DataAccess.Data;

namespace Runrs.DataAccess.Repository
{
    public class EventRegistrationRepository : Repository<EventRegistration>, IEventRegistrationRepository // <--- Byron (16/05/2026) Repository for Event Registrations
    {
        private ApplicationDbContext _db;
        public EventRegistrationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        void IEventRegistrationRepository.Update(EventRegistration obj)
        {
            _db.EventRegistrations.Update(obj);
        }

        void IEventRegistrationRepository.Save()
        {
            _db.SaveChanges();
        }

        IEnumerable<EventRegistration> IEventRegistrationRepository.GetByEventId(int eventId)
        {
            return _db.EventRegistrations.Where(er => er.EventId == eventId).ToList();
        }

        IEnumerable<EventRegistration> IEventRegistrationRepository.GetByUserId(int userId)
        {
            return _db.EventRegistrations.Where(er => er.UserId == userId).ToList();
        }

        bool IEventRegistrationRepository.IsRegistered(int userId, int eventId)
        {
            return _db.EventRegistrations.Any(r => r.UserId == userId && r.EventId == eventId);
        }
    }
}
