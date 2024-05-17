using Microsoft.AspNetCore.Mvc;
using School.DataAccess.Repository.IRepository;
using School.Models.Models;
using System.Collections.Generic;

namespace SchoolManagement.Areas.Students.Controllers
{
    [Area("Students")]
    public class QualificationsMVController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public QualificationsMVController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Qualifications> qualifications = _unitOfWork.Qualifications.GetAll().ToList();
            return View(qualifications);
        }
    }
}
