using Clinic.Application.Contracts.DoctorContract;
using Clinic.Application.Contracts.MedicalRecordContracts;
using Clinic.Application.Contracts.PatientContract;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.Services.MedicalRecordServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clinic.UI.Areas.doctor.Controllers
{
    [Area("doctor")]
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        private readonly IUserService _userService;
        private readonly IDoctorService _doctorService;
        private readonly IMedicalRecordService _medicalRecordService;

        public DoctorController(IUserService userService , IDoctorService doctorService , IMedicalRecordService medicalRecordService)
        {
            _userService = userService;
            _doctorService = doctorService;
            _medicalRecordService = medicalRecordService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _doctorService.DoctorProfile();   

            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }
    }
}
