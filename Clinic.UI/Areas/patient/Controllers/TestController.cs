using Clinic.Application.Contracts.MedicalRecordContracts;
using Clinic.Application.DTOs.MedicalRecordDTOs;
using Clinic.Application.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.UI.Areas.patient.Controllers
{
    [Area("patient")]
    public class TestController : Controller
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        public IActionResult Add(Guid medicalRecordId)
        {
            var test = new TestDTO();

            test.MedicalRecordId = medicalRecordId;

            return View(test);
        }

        public async Task<IActionResult> Edit(Guid Id)
        {
            var test = await _testService.GetById(Id);
            return View(test);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(TestDTO testDTO, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                testDTO.File = await FileHelper.UploadFile(file, "Tests", new[] { ".pdf ", ".png", ".jpg" });
            }

            if (!ModelState.IsValid)
            {
                return View(testDTO.Id == Guid.Empty ? "Add" : "Edit", testDTO);
            }

            if (testDTO.Id == Guid.Empty)
            {
                var result = await _testService.Add(testDTO);

                if (!result.Item1)
                {
                    ModelState.AddModelError("", "Failed to add test.");
                    return View("Add", testDTO);
                }
            }
            else
            {
                var result = await _testService.Update(testDTO);

                if (!result)
                {
                    ModelState.AddModelError("", "Failed to update test.");
                    return View("Edit", testDTO);
                }
            }

            return RedirectToAction(
                "MedicalRecord",
                "MedicalRecord",
                new { area = "doctor", medicalRecordId = testDTO.MedicalRecordId }
            );
        }
    }

}
