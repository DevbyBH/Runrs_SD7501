using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runrs.Models;

namespace Runrs.DataAccess.Repository.IRepository
{
    public interface IAnnouncementRepository : IRepository<Announcement>
    {
        void Update(Announcement obj);
        void Save();
        IEnumerable<Announcement> GetByClubId(int clubId);
    }
}
