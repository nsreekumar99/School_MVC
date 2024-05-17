using School.DataAccess.Data;
using School.DataAccess.Repository.IRepository;
using School.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.DataAccess.Repository
{
    public class QualificationsRepository : Repository<Qualifications>, IQualificationsRepository
    {
        private readonly ApplicationDbContext _context;

        public QualificationsRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Qualifications qualification)
        {
            var objFromDb = _context.Qualifications.FirstOrDefault(q => q.Id == qualification.Id);
            if (objFromDb != null)
            {
                objFromDb.Course = qualification.Course;
                objFromDb.University = qualification.University;
                objFromDb.StartYear = qualification.StartYear;
                objFromDb.EndYear = qualification.EndYear;
                objFromDb.Percentage = qualification.Percentage;

                _context.SaveChanges();
            }
        }
    }
}
