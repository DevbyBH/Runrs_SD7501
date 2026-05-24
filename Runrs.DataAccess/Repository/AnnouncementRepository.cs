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
    public class AnnouncementRepository : Repository<Announcement>, IAnnouncementRepository
    {
        private ApplicationDbContext _db;
        public AnnouncementRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        IEnumerable<Announcement> IAnnouncementRepository.GetByClubId(int clubId)
        {
            return _db.Announcements.Where(a => a.ClubId == clubId).OrderByDescending(a => a.CreatedDate).ToList();
        }

        void IAnnouncementRepository.Save()
        {
            _db.SaveChanges();
        }

        void IAnnouncementRepository.Update(Announcement obj)
        {
            _db.Announcements.Update(obj);
        }
    }
}
