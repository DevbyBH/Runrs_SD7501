using Runrs.DataAccess.Repository.IRepository;
using Runrs_SD7501.Data;
using Runrs_SD7501.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runrs.DataAccess.Repository
{
    public class ClubRepository : Repository<Club>, IClubRepository
    {
        private ApplicationDbContext _db;
        public ClubRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        void IClubRepository.Save()
        {
            _db.SaveChanges();
        }

        void IClubRepository.Update(Club obj)
        {
            _db.Clubs.Update(obj);
        }
    }
}
