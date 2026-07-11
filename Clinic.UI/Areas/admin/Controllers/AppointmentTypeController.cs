using Clinic.Application.Contracts.AppointmentContracts;
using Clinic.Application.DTOs.AppointmentDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles ="Admin")]
    public class AppointmentTypeController : Controller
    {
        private readonly IAppointmentTypeService _appointmentTypeService;

        public AppointmentTypeController(IAppointmentTypeService appointmentTypeService)
        {
            _appointmentTypeService = appointmentTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _appointmentTypeService.GetAll();
            return View(result);
        }

        public async Task<IActionResult> Edit(Guid? Id)
        {
            var appointmentTypeDTO = new AppointmentTypeDTO();

            if (Id != null)
            {
                appointmentTypeDTO = await _appointmentTypeService.GetById((Guid)Id);
            }

            return View(appointmentTypeDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(AppointmentTypeDTO appointmentTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", appointmentTypeDTO);
            }

            if (appointmentTypeDTO.Id == Guid.Empty)
            {
                var result = await _appointmentTypeService.Add(appointmentTypeDTO);

                if (result.Item1)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to add appointment type.");
                    return View("Edit", appointmentTypeDTO);
                }
            }
            else
            {
                var result = await _appointmentTypeService.Update(appointmentTypeDTO);

                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update appointment type.");
                    return View("Edit", appointmentTypeDTO);
                }
            }
        }

        public async Task<IActionResult> Delete(Guid? Id)
        {
            if (Id == null)
            {
                ModelState.AddModelError("", "Invalid appointment type ID.");
                return RedirectToAction("Index");
            }

            var result = await _appointmentTypeService.ChangeStatus((Guid)Id);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to delete appointment type.");
                return RedirectToAction("Index");
            }
        }
    }
}
