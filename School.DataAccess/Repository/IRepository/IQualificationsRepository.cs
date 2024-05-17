using School.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.DataAccess.Repository.IRepository
{
    public interface IQualificationsRepository : IRepository<Qualifications>
    {
        void Update(Qualifications qualifications);
    }
}
