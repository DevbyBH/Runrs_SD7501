using Runrs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runrs.DataAccess.Repository.IRepository
{
    public interface IEventRepository : IRepository<RunEvent> // IEventRepository inherits from the IRepository Interface for CRUD <--- Byron (16/05/2026)
    {
        void Update(RunEvent obj);
        void Save();
        IEnumerable<RunEvent> GetByClubId(int clubId);
    }
}
