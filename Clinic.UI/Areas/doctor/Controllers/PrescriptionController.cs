using Clinic.Application.Contracts.AppointmentContracts;
using Clinic.Application.Contracts.PrescriptionContracts;
using Clinic.Application.Contracts.ProtocalContracts;
using Clinic.Application.DTOs.MedicalRecordDTOs;
using Clinic.Application.DTOs.PrescriptionDTOs;
using Clinic.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.UI.Areas.doctor.Controllers
{
    [Area("doctor")]
    [Authorize(Roles = "Doctor")]
    public class PrescriptionController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPrescriptionService _prescriptionService;
        private readonly IPrescriptionMedicineService _prescriptionMedicineService;
        private readonly IPrescriptionProtocalService _prescriptionProtocalService;

        public PrescriptionController(IPrescriptionService prescriptionService 
            , IPrescriptionMedicineService prescriptionMedicineService , IAppointmentService appointmentService 
            , IPrescriptionProtocalService prescriptionProtocalService)
        {
            _prescriptionService = prescriptionService;
            _prescriptionMedicineService = prescriptionMedicineService;
            _appointmentService = appointmentService;
            _prescriptionProtocalService = prescriptionProtocalService;
        }

        public async Task<IActionResult> Details(Guid appointmentId)
        {
            if (appointmentId == Guid.Empty)
            {
                return NotFound();
            }

            var prescription = await _prescriptionService.GetPrescritpionByAppointment(appointmentId);

            if (prescription == null)
            {
                return NotFound("Not Found");
            }

            return View(prescription);
        }

        public async Task<IActionResult> GetProtocal(Guid protocalId, Guid appointmentId)
        {
            if (protocalId == Guid.Empty)
            {
                return NotFound();
            }

            var protocal = await _prescriptionProtocalService.ProtocalToPrescription(protocalId, appointmentId);

            if (protocal == null)
            {
                return NotFound("Not Found");
            }

            return View("Edit", protocal);
        }

        public async Task<IActionResult> Edit(Guid? Id , Guid appointmentId)
        {
            var appointmentValidation = await _appointmentService.ValidateDoctorAppointment(appointmentId);

            if (!appointmentValidation)
            {
                return NotFound("No Prescription Found");
            }

            var prescriptionValidation = await _prescriptionService.CheckPrescritpionForAppointment(appointmentId);

            if (prescriptionValidation)
            {
                return RedirectToAction("Details" , new {appointmentId = appointmentId });
            }

            var prescription = new PrescriptionDTO();

            if (Id.HasValue)
            {
                prescription = await _prescriptionService.GetPrescritpionWithMedicines((Guid)Id);
            }

            ViewBag.lstProtocals = await _prescriptionProtocalService.GetDoctorProtocals();

            prescription.AppointmentId = appointmentId;

            return View(prescription);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(PrescriptionDTO prescriptionDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.lstProtocals = await _prescriptionProtocalService.GetDoctorProtocals();
                return View("Edit", prescriptionDTO);
            }

            if (prescriptionDTO.Medicines.Count == 0)
            {
                ViewBag.lstProtocals = await _prescriptionProtocalService.GetDoctorProtocals();
                return View("Edit", prescriptionDTO);
            }

            if (prescriptionDTO.Id == Guid.Empty)
            {
                var result = await _prescriptionService.CreatePrescription(prescriptionDTO);

                if (!result.Item1)
                {
                    ViewBag.lstProtocals = await _prescriptionProtocalService.GetDoctorProtocals();
                    return View("Edit", prescriptionDTO);
                }
            }
            else
            {
                var result = await _prescriptionService.UpdatePrescription(prescriptionDTO);

                if (!result)
                {
                    ViewBag.lstProtocals = await _prescriptionProtocalService.GetDoctorProtocals();
                    return View("Edit", prescriptionDTO);
                }
            }

            return RedirectToAction("Details" , "Appointment" , new { appointmentId = prescriptionDTO.AppointmentId});
        }
    }
}
