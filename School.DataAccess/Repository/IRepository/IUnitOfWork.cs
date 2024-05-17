using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.Models.Models;

namespace School.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Student> Student { get; }
        IQualificationsRepository Qualifications { get; }
        void Save();
    }
}