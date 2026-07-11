using Clinic.Application.Contracts.AppointmentContracts;
using Clinic.Application.Contracts.MedicalRecordContracts;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.MedicalRecordDTOs;
using Clinic.Application.Services.MedicalRecordServices;
using Clinic.Core.Entities;
using Clinic.UI.Areas.patient.Models;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.UI.Areas.patient.Controllers
{
    [Area("patient")]
    public class MedicalRecordController : Controller
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IUserService _userService;
        private readonly ITestService _testService;
        private readonly IMedicalRadiologyService _medicalRadiologyService;
        private readonly IAppointmentService _appointmentService;

        public MedicalRecordController(IMedicalRecordService medicalRecordService , IUserService userService 
            , ITestService testService , IMedicalRadiologyService medicalRadiologyService , IAppointmentService appointmentService)
        {
            _medicalRecordService = medicalRecordService;
            _userService = userService;
            _testService = testService;
            _medicalRadiologyService = medicalRadiologyService;
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _medicalRecordService.GetMedicalRecordsForUser();
            return View(result);
        }

        public async Task<IActionResult> Details(Guid Id, MedicalRecordPagination medicalRecordPagination)
        {
            var medicalRecord = new VmMedicalRecord();

            medicalRecord.DocMedicalRecord = await _medicalRecordService.MedicalRecordDetailsForUser(Id);

            var result = medicalRecord.DocMedicalRecord;

            medicalRecord.Tests = await _testService.GetPageTests(result.Id, medicalRecordPagination.TestPageNumber, medicalRecordPagination.TestPageSize);
            medicalRecord.MedicalRadiologies = await _medicalRadiologyService.GetPageMedicalRadiologies(result.Id, medicalRecordPagination.MedicalRadilogyPageNumber, medicalRecordPagination.MedicalRadilogyPageSize);
            medicalRecord.Appointments = await _appointmentService.GetMedicalRecordAppointments(result.PatientId, medicalRecordPagination.AppointmentPageNumber, medicalRecordPagination.AppointmentPageSize);

            return View(medicalRecord);
        }
    }
}

