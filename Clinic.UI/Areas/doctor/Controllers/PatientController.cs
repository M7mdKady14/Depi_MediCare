using Clinic.Application.Contracts.AppointmentContracts;
using Clinic.Application.Contracts.MedicalRecordContracts;
using Clinic.Application.Contracts.PatientContract;
using Clinic.Application.DTOs.PatientDTOs;
using Clinic.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Clinic.UI.Areas.doctor.Controllers
{
    [Area("doctor")]
    public class PatientController : Controller
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;

        public PatientController(IMedicalRecordService medicalRecordService, IPatientService patientService, IAppointmentService appointmentService)
        {
            _medicalRecordService = medicalRecordService;
            _patientService = patientService;
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> Index(SearchPatient searchPatient , int pageNumber = 1 , int pageSize = 10)
        {
            var result = await _medicalRecordService.GetDocPageMedicalRecords(searchPatient , pageNumber , pageSize);
            return View(result);
        }

        public async Task<IActionResult> Details(Guid Id, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _patientService.GetPatientDetails(Id);
            ViewBag.lstAppointments = await _appointmentService.GetPatientAppointments(Id, pageNumber, pageSize);
            return View(result);
        }

        public async Task<IActionResult> Edit(Guid patientId)
        {
            var result = await _patientService.GetById(patientId);

            ViewBag.lstGenders = new SelectList(Enum.GetValues(typeof(Gender)));
            ViewBag.lstBloodTypes = new SelectList(Enum.GetValues(typeof(BloodType)));

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(PatientDTO patientDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.lstGenders = new SelectList(Enum.GetValues(typeof(Gender)));
                ViewBag.lstBloodTypes = new SelectList(Enum.GetValues(typeof(BloodType)));

                return View("Edit", patientDTO);
            }

            if (patientDTO.Id != Guid.Empty)
            {
                var result = await _patientService.UpdatePatient(patientDTO);

                if (!result)
                {
                    ViewBag.lstGenders = new SelectList(Enum.GetValues(typeof(Gender)));
                    ViewBag.lstBloodTypes = new SelectList(Enum.GetValues(typeof(BloodType)));
                    ModelState.AddModelError("", "Failed to Update Patient Info.");
                    return View("Edit", patientDTO);
                }
            }

            return RedirectToAction("Index");
        }
    }
}

