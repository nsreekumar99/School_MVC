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
            var qualification = new Qualifications
            {
                ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };
            return View(qualification);
        }

        // POST: Create Qualification
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Qualifications qualification)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ModelState.Remove("ApplicationUser");
            if (ModelState.IsValid)
            {
                //qualification.ApplicationUserId = userId;
                _unitOfWork.Qualifications.Add(qualification);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(qualification);
        }

        // GET: Edit Qualification
        public IActionResult Edit(int id)
        {
            var qualification = _unitOfWork.Qualifications.Get(id); // qualifications.js requests the the qualification based on id
                                                                    // from controller when use presses edit/delete button
            if (qualification == null || qualification.ApplicationUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }
            return View(qualification); //passes the qualification details based on id
        }

        // POST: Edit Qualification
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Qualifications qualification)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ModelState.Remove("ApplicationUser");
            
            if (ModelState.IsValid && qualification.ApplicationUserId == userId)
            {
                _unitOfWork.Qualifications.Update(qualification);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(qualification);
        }

        // POST: Delete Qualification
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var qualification = _unitOfWork.Qualifications.Get(id);
            if (qualification == null || qualification.ApplicationUserId != userId)
            {
                return NotFound();
            }

            _unitOfWork.Qualifications.Remove(qualification);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }


    }
}
