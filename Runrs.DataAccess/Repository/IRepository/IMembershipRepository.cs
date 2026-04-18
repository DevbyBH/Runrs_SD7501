using Runrs_SD7501.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runrs.DataAccess.Repository.IRepository
{
    public interface IMembershipRepository : IRepository<Membership>
    {
        void Update(Membership obj);
        void Save();
        IEnumerable<Membership> GetMembershipByUserId(int userId);
        IEnumerable<Membership> GetMembershipByClubId(int clubId);
        bool IsMember(int userId, int clubId);
    }
}
