using Clinic.Application.Contracts.ScheduleContracts;
using Clinic.Application.Contracts.SpecializationContract;
using Clinic.Application.DTOs.ScheduleDTOs;
using Clinic.Application.DTOs.SpecializationDTOs;
using Clinic.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinic.UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        public async Task<IActionResult> Index(int pageNumber = 1 , int pageSize = 10)
        {
            var result = await _scheduleService.GetPageSchedule(pageNumber , pageSize);
            return View(result);
        }

        public async Task<IActionResult> Details(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                ModelState.AddModelError("", "Invalid schedule ID.");
                return RedirectToAction("Index");
            }

            var schedule = await _scheduleService.GetScheduleDetails(Id);

            if (schedule == null)
            {
                ModelState.AddModelError("", "Schedule not found.");
                return RedirectToAction("Index");
            }

            return View(schedule);
        }

        public async Task<IActionResult> Edit(Guid? Id)
        {
            var scheduleDTO = new ScheduleDTO();

            ViewBag.lstDays = new SelectList(Enum.GetValues(typeof(Day)));

            if (Id != null)
            {
                scheduleDTO = await _scheduleService.GetById((Guid)Id);
            }

            return View(scheduleDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ScheduleDTO scheduleDTO)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", scheduleDTO);
            }

            if (scheduleDTO.Id == Guid.Empty)
            {
                var result = await _scheduleService.Add(scheduleDTO);

                if (result.Item1)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to add schedule.");
                    return View("Edit", scheduleDTO);
                }
            }
            else
            {
                var result = await _scheduleService.Update(scheduleDTO);

                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update schedule.");
                    return View("Edit", scheduleDTO);
                }
            }
        }

        public async Task<IActionResult> Delete(Guid? Id)
        {
            if (Id == null)
            {
                ModelState.AddModelError("", "Invalid schedule ID.");
                return RedirectToAction("Index");
            }

            var result = await _scheduleService.DeleteSchedule((Guid)Id);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to delete schedule.");
                return RedirectToAction("Index");
            }
        }
    }
}
