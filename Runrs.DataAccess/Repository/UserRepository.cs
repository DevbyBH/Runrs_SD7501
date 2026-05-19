using Runrs.DataAccess.Repository.IRepository;
using Runrs.DataAccess.Data;
using Runrs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Runrs.DataAccess.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        void IUserRepository.Update(User obj)
        {
            _db.Users.Update(obj);
        }

        void IUserRepository.Save()
        {
            _db.SaveChanges();
        }

        User? IUserRepository.GetByUsername(string username)
        {
            return _db.Users.FirstOrDefault(u => u.Username == username);
        }
    }
}
