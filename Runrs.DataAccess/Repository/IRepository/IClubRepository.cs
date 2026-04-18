using Runrs_SD7501.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runrs.DataAccess.Repository.IRepository
{
    public interface IClubRepository:IRepository<Club>
    {
        void Update(Club obj);
        void Save();
    }
}
