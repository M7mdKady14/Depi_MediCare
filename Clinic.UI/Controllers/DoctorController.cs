using Clinic.Application.Contracts.DoctorContract;
using Clinic.Application.Contracts.ScheduleContracts;
using Clinic.Application.Contracts.SpecializationContract;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.DoctorDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.UI.Controllers
{
    [Authorize]
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly ISpecializationService _specializationService;
        private readonly IScheduleService _scheduleService;

        public DoctorController(IDoctorService doctorService, ISpecializationService specializationService,
            IScheduleService scheduleService)
        {
            _specializationService = specializationService;
            _doctorService = doctorService;
            _scheduleService = scheduleService;
        }

        public async Task<IActionResult> Index(SearchDoctor search, int pageNumber = 1, int pageSize = 10)
        {
            ViewBag.lstSpecializations = await _specializationService.GetAll();
            ViewBag.lstSchedules = await _scheduleService.GetAll();
            var result = await _doctorService.SearchDoctors(search, pageNumber, pageSize);
            return View(result);
        }

        public async Task<IActionResult> Details(Guid Id)
        {
            if (Id == Guid.Empty)
                return RedirectToAction("Index");

            var doctor = await _doctorService.GetDoctorDetails(Id);

            if (doctor == null)
                return RedirectToAction("Index");

            return View(doctor);
        }
    }
}
