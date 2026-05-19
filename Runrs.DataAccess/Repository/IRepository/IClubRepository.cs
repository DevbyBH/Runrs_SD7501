using System;
using Runrs.DataAccess.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runrs.Models;

namespace Runrs.DataAccess.Repository.IRepository
{
    public interface IClubRepository:IRepository<Club>
    {
        void Update(Club obj);
        void Save();
    }
}
