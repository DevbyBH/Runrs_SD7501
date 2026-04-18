using Runrs.DataAccess.Repository.IRepository;
using Runrs_SD7501.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runrs.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IClubRepository Club { get; private set; }
        public IUserRepository User { get; private set; }
        // public IMembershipRepository Membership { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Club = new ClubRepository(_db);
            User = new UserRepository(_db);
            // Membership = new MembershipRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
