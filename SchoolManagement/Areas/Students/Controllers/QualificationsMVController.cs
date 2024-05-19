using Microsoft.AspNetCore.Mvc;
using School.DataAccess.Repository.IRepository;
using School.Models.Models;
using System.Collections.Generic;
using System.Security.Claims;

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Qualifications> qualifications = _unitOfWork.Qualifications.GetAll(q => q.ApplicationUserId == userId).ToList();
            return View(qualifications);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Create Qualification
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Qualifications qualification)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ModelState.Remove("ApplicationUser");
            ModelState.Remove("ApplicationUserId");
            if (ModelState.IsValid)
            {
                qualification.ApplicationUserId = userId;
                _unitOfWork.Qualifications.Add(qualification);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(qualification);
        }

    }
}
