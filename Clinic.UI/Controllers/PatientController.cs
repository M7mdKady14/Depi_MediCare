using Clinic.Application.Contracts.AppointmentContracts;
using Clinic.Application.Contracts.DoctorContract;
using Clinic.Application.Contracts.PatientContract;
using Clinic.Application.DTOs.DoctorDTOs;
using Clinic.Application.DTOs.PatientDTOs;
using Clinic.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Clinic.UI.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;

        public PatientController(IPatientService patientService , IDoctorService doctorService , IAppointmentService appointmentService)
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

        public async Task<IActionResult> Details(Guid Id , int pageNumber = 1 , int pageSize = 10)
        {
            var result = await _patientService.GetPatientDetails(Id);
            ViewBag.lstAppointments = await _appointmentService.GetPatientAppointments(Id, pageNumber, pageSize);
            return View(result);
        }

        public IActionResult Add()
        {
            var patient = new PatientDTO();
            
            ViewBag.lstGenders = new SelectList(Enum.GetValues(typeof(Gender)));
            ViewBag.lstBloodTypes = new SelectList(Enum.GetValues(typeof(BloodType)));

            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(PatientDTO patientDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.lstGenders = new SelectList(Enum.GetValues(typeof(Gender)));
                ViewBag.lstBloodTypes = new SelectList(Enum.GetValues(typeof(BloodType))); 
                
                return View("Add", patientDTO);
            }

            if (patientDTO.Id == Guid.Empty)
            {
                var result = await _patientService.AddPatient(patientDTO);

                if (!result.Item1)
                {
                    ViewBag.lstGenders = new SelectList(Enum.GetValues(typeof(Gender)));
                    ViewBag.lstBloodTypes = new SelectList(Enum.GetValues(typeof(BloodType)));
                    ModelState.AddModelError("", "Failed to add patient.");
                    return View("Edit", patientDTO);
                }
            }

            return RedirectToAction("Index");
        }
    }
}

