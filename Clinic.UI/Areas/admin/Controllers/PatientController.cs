using Clinic.Application.Contracts.AppointmentContracts;
using Clinic.Application.Contracts.DoctorContract;
using Clinic.Application.Contracts.PatientContract;
using Clinic.Application.DTOs.PatientDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;

        public PatientController(IPatientService patientService, IDoctorService doctorService, IAppointmentService appointmentService)
        {
            _patientService = patientService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> Index(SearchPatient searchPatient, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _patientService.SearchPatient(searchPatient, pageNumber, pageSize);
            return View(result);
        }

        public async Task<IActionResult> Details(Guid Id, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _patientService.GetPatientDetails(Id);
            ViewBag.lstAppointments = await _appointmentService.GetPatientAppointments(Id, pageNumber, pageSize);
            return View(result);
        }
    }
}
