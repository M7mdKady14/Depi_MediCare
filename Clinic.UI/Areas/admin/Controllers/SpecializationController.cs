using Clinic.Application.Contracts.SpecializationContract;
using Clinic.Application.DTOs.SpecializationDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clinic.UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class SpecializationController : Controller
    {
        private readonly ISpecializationService _specializationService;

        public SpecializationController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _specializationService.GetAll();
            return View(result);
        }

        public async Task<IActionResult> Details(Guid Id)
        {
            if (Id == null)
            {
                ModelState.AddModelError("", "Invalid specialization ID.");
                return RedirectToAction("Index");
            }

            var specialization = await _specializationService.GetSpecializationDetails(Id);

            if (specialization == null)
            {
                ModelState.AddModelError("", "Specialization not found.");
                return RedirectToAction("Index");
            }

            return View(specialization);
        }

        public async Task<IActionResult> Edit(Guid? Id)
        {
            var specialization = new SpecializationDTO();

            if (Id != null)
            {
                specialization = await _specializationService.GetById((Guid)Id);
            }

            return View(specialization);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(SpecializationDTO specializationDTO)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", specializationDTO);
            }

            if (specializationDTO.Id == Guid.Empty)
            {
                var result = await _specializationService.Add(specializationDTO);

                if (result.Item1)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to add specialization.");
                    return View("Edit", specializationDTO);
                }
            }
            else
            {
                var result = await _specializationService.Update(specializationDTO);

                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update specialization.");
                    return View("Edit", specializationDTO);
                }
            }
        }

        public async Task<IActionResult> Delete(Guid? Id)
        {
            if (Id == null)
            {
                ModelState.AddModelError("", "Invalid specialization ID.");
                return RedirectToAction("Index");
            }

            var result = await _specializationService.DeleteSpecialization((Guid)Id);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to delete specialization.");
                return RedirectToAction("Index");
            }
        }
    }
}
