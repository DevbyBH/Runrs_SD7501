using Runrs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runrs.DataAccess.Data;

namespace Runrs.DataAccess.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        void Update(User obj);
        void Save();
        User? GetByUsername(string username);
    }
}
