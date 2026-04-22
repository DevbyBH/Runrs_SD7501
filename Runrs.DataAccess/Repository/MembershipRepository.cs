using Runrs.DataAccess.Repository.IRepository;
using Runrs_SD7501.Data;
using Runrs_SD7501.Models;

namespace Runrs.DataAccess.Repository
{

    public class MembershipRepository : Repository<Membership>, IMembershipRepository
    {
        private ApplicationDbContext _db;

        public MembershipRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        void IMembershipRepository.Update(Membership obj)
        {
            _db.Memberships.Update(obj);
        }

        void IMembershipRepository.Save()
        {
            _db.SaveChanges();
        }

        IEnumerable<Membership> IMembershipRepository.GetMembershipByUserId(int userId)
        {
            return _db.Memberships
                .Where(m => m.UserId == userId)
                .ToList();
        }

        IEnumerable<Membership> IMembershipRepository.GetMembershipByClubId(int clubId)
        {
            return _db.Memberships
                .Where(m => m.ClubId == clubId)
                .ToList();
        }

        bool IMembershipRepository.IsMember(int userId, int clubId)
        {
            return _db.Memberships
                .Any(m => m.UserId == userId && m.ClubId == clubId);
        }

        
    

    public void Update(Membership obj)
        {
            _db.Memberships.Update(obj);
        }
    }
}