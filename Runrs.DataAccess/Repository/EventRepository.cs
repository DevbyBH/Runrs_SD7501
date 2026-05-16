using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runrs.DataAccess.Repository.IRepository;
using Runrs.Models;
using Runrs.DataAccess.Data;

namespace Runrs.DataAccess.Repository
{
    public class EventRepository : Repository<RunEvent>, IEventRepository // <--- Byron (16/05/2026) EventRepository inherits from the Repository class and implements the IEventRepository Interface
    {
        private ApplicationDbContext _db;

        public EventRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        void IEventRepository.Update(RunEvent obj)
        {
            _db.RunEvents.Update(obj);
        }

        void IEventRepository.Save()
        {
            _db.SaveChanges();
        }

        IEnumerable<RunEvent> IEventRepository.GetByClubId(int clubId)
        {
            return _db.RunEvents.Where(e => e.ClubId == clubId).ToList();
        }
    }
}
