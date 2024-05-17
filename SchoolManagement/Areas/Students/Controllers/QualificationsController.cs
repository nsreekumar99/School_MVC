using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using School.DataAccess.Repository.IRepository;
using School.Models.Models;

namespace SchoolManagement.Areas.Students.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Area("Students")]
    public class QualificationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public QualificationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var qualifications = _unitOfWork.Qualifications.GetAll();
            return Ok(qualifications);
        }

        [HttpPost]
        public IActionResult Create([FromForm] Qualifications qualification)
        {
            if (qualification == null)
            {
                return BadRequest();
            }

            _unitOfWork.Qualifications.Add(qualification);
            _unitOfWork.Save();
            return Ok(qualification);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] Qualifications qualification)
        {
            if (qualification == null || qualification.Id != id)
            {
                return BadRequest();
            }

            _unitOfWork.Qualifications.Update(qualification);
            _unitOfWork.Save();
            return Ok(qualification);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var qualification = _unitOfWork.Qualifications.Get(id);
            if (qualification == null)
            {
                return BadRequest();
            }

            _unitOfWork.Qualifications.Remove(qualification);
            _unitOfWork.Save();
            return Ok(id);
        }
    }
}
