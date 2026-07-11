using Clinic.Application.Contracts.MedicalRecordContracts;
using Clinic.Application.DTOs.MedicalRecordDTOs;
using Clinic.Application.Helper;
using Clinic.Application.Services.MedicalRecordServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.UI.Areas.doctor.Controllers
{
    [Area("doctor")]
    [Authorize(Roles = "Doctor")]
    public class MedicalRadiologyController : Controller
    {
        private readonly IMedicalRadiologyService _medicalRadiologyService;

        public MedicalRadiologyController(IMedicalRadiologyService medicalRadiologyService)
        {
            _medicalRadiologyService = medicalRadiologyService;
        }

        public IActionResult Add(Guid medicalRecordId)
        {
            var medicalRadiology = new MedicalRadiologyDTO();

            medicalRadiology.MedicalRecordId = medicalRecordId;

            return View(medicalRadiology);
        }

        public async Task<IActionResult> Edit(Guid Id)
        {
            var medicalRadiology = await _medicalRadiologyService.GetById(Id);
            return View(medicalRadiology);
        }

        public async Task<IActionResult> Save(MedicalRadiologyDTO medicalRadiologyDTO , IFormFile file)
        {
            if (file != null && file.Length > 0) 
            {
                medicalRadiologyDTO.File = await FileHelper.UploadFile(
                    file,
                    "MedicalRadiologies",
                    new[] { ".pdf", ".png", ".jpg" }
                );
            }

            if (!ModelState.IsValid)
            {
                if (medicalRadiologyDTO.Id != Guid.Empty)
                    return View("Edit", medicalRadiologyDTO);
                else
                    return View("Add", medicalRadiologyDTO);
            }

            if (medicalRadiologyDTO.Id == Guid.Empty)
            {
                var result = await _medicalRadiologyService.Add(medicalRadiologyDTO);

                if (!result.Item1)
                {
                    ModelState.AddModelError("", "Failed to add medical radiology.");
                    return View("Edit", medicalRadiologyDTO);
                }
            }
            else
            {
                var result = await _medicalRadiologyService.Update(medicalRadiologyDTO);

                if (!result)
                {
                    ModelState.AddModelError("", "Failed to update medical radiology.");
                    return View("Edit", medicalRadiologyDTO);
                }
            }

            return RedirectToAction(
                "MedicalRecord",
                "MedicalRecord",
                new { area = "doctor", medicalRecordId = medicalRadiologyDTO.MedicalRecordId }
            );
        }

        public async Task<IActionResult> Delete(Guid Id)
        {
            var deleteMedicalRadiology = await _medicalRadiologyService.DeleteMedicalRadiology(Id);
            return RedirectToAction("Details", "MedicalRecord", new { patientId = deleteMedicalRadiology.Item2 });
        }
    }
}
