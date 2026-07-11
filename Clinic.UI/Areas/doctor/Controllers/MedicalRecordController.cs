using Clinic.Application.Contracts.AppointmentContracts;
using Clinic.Application.Contracts.DoctorContract;
using Clinic.Application.Contracts.MedicalRecordContracts;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.MedicalRecordDTOs;
using Clinic.UI.Areas.doctor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clinic.UI.Areas.doctor.Controllers
{
    [Area("doctor")]
    [Authorize(Roles = "Doctor")]
    public class MedicalRecordController : Controller
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IUserService _userService;
        private readonly IDoctorService _doctorService;
        private readonly ITestService _testService;
        private readonly IMedicalRadiologyService _medicalRadiologyService;
        private readonly IAppointmentService _appointmentService;

        public MedicalRecordController(IMedicalRecordService medicalRecordService , IUserService userService 
            , IDoctorService doctorService , ITestService testService 
            , IMedicalRadiologyService medicalRadiologyService , IAppointmentService appointmentService)
        {
            _medicalRecordService = medicalRecordService;
            _userService = userService;
            _doctorService = doctorService;
            _testService = testService;
            _appointmentService = appointmentService;
            _medicalRadiologyService = medicalRadiologyService;
        }

        public async Task<IActionResult> Details(Guid patientId , MedicalRecordPagination medicalRecordPagination)
        {
            var medicalRecord = new VmMedicalRecord();

            medicalRecord.DocMedicalRecord = await _medicalRecordService.MedicalRecordDetails(patientId);

            var result = medicalRecord.DocMedicalRecord;

            medicalRecord.Tests = await _testService.GetPageTests(result.Id , medicalRecordPagination.TestPageNumber , medicalRecordPagination.TestPageSize);
            medicalRecord.MedicalRadiologies = await _medicalRadiologyService.GetPageMedicalRadiologies(result.Id , medicalRecordPagination.MedicalRadilogyPageNumber, medicalRecordPagination.MedicalRadilogyPageSize);
            medicalRecord.Appointments = await _appointmentService.GetMedicalRecordAppointments(result.PatientId , medicalRecordPagination.AppointmentPageNumber, medicalRecordPagination.AppointmentPageSize);

            return View(medicalRecord);
        }

        public async Task<IActionResult> MedicalRecord(Guid medicalRecordId)
        {
            var medicalRecord = await _medicalRecordService.GetById(medicalRecordId);

            return RedirectToAction(
                "Details",
                "MedicalRecord",
                new { area = "doctor", patientId = medicalRecord.PatientId }
            );
        }

        public async Task<IActionResult> Edit(Guid Id)
        {
            var medicalRecord = await _medicalRecordService.GetById(Id);

            if (medicalRecord == null)
            {
                return NotFound();
            }

            var doctorId = await _doctorService.GetDoctorId();

            if (medicalRecord.DoctorId != doctorId)
            {
                return NotFound();
            }

            return View(medicalRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(MedicalRecordDTO medicalRecordDTO)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", medicalRecordDTO);
            }

            if (medicalRecordDTO.Id == Guid.Empty)
            {
                return BadRequest();
            }

            var result = await _medicalRecordService.Update(medicalRecordDTO);
            
            if (!result)
            {
                return View("Edit", medicalRecordDTO);
            }

            return RedirectToAction("Details" , "MedicalRecord" , new { patientId = medicalRecordDTO.PatientId });
        }
    }
}


