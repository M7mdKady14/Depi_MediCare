using Clinic.Application.Contracts.DoctorContract;
using Clinic.Application.Contracts.ScheduleContracts;
using Clinic.Application.Contracts.SpecializationContract;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.AppointmentDTOs;
using Clinic.Application.DTOs.DoctorDTOs;
using Clinic.Application.DTOs.ScheduleDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Clinic.UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly ISpecializationService _specializationService;
        private readonly IUserService _userService;
        private readonly IScheduleService _scheduleService;
        private readonly IDoctorScheduleService _doctorScheduleService;

        public DoctorController(IDoctorService doctorService, ISpecializationService specializationService , 
            IUserService userService , IScheduleService scheduleService , IDoctorScheduleService doctorScheduleService)
        {
            _specializationService = specializationService;
            _doctorService = doctorService;
            _userService = userService;
            _scheduleService = scheduleService;
            _doctorScheduleService = doctorScheduleService;
        }

        public async Task<IActionResult> Index(SearchDoctor search, int pageNumber = 1, int pageSize = 10)
        {
            ViewBag.lstSpecializations = await _specializationService.GetAll();
            var result = await _doctorService.SearchDoctors(search, pageNumber, pageSize);
            return View(result);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToAction("Index");

            var doctor = await _doctorService.GetDoctorDetails(id);

            if (doctor == null)
                return RedirectToAction("Index");

            return View(doctor);
        }

        public async Task<IActionResult> Add(string userId)
        {
            if (string.IsNullOrEmpty(userId)) 
                return NotFound();

            ViewBag.lstSpecializations = await _specializationService.GetAll();
            ViewBag.lstSchedules = await _scheduleService.GetAll();

            var doctorDTO = await _doctorService.GetDocDTO(userId);

            return View(doctorDTO);
        }

        public async Task<IActionResult> Edit(Guid Id)
        {
            if (Id == Guid.Empty)
                return NotFound();
            
            ViewBag.lstSpecializations = await _specializationService.GetAll();
            ViewBag.lstSelectedSchedules = await _doctorScheduleService.GetDoctorSchedules(Id);
            ViewBag.lstSchedules = await _scheduleService.GetAll();

            var doctorDTO = await _doctorService.GetById(Id);

            if (doctorDTO == null)
                return NotFound();

            return View(doctorDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(DoctorDTO doctorDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.lstSpecializations = await _specializationService.GetAll();
                ViewBag.lstSchedules = await _scheduleService.GetAll();

                if (doctorDTO.Id != Guid.Empty)
                {
                    ViewBag.lstSelectedSchedules = await _doctorScheduleService.GetDoctorSchedules(doctorDTO.Id);
                }

                return doctorDTO.Id == Guid.Empty
                        ? View("Add", doctorDTO)
                        : View("Edit", doctorDTO);
            }

            if (doctorDTO.Id == Guid.Empty)
            {
                var result = await _doctorService.AddDoctor(doctorDTO);

                if (result.Item1)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.lstSpecializations = await _specializationService.GetAll();
                    ViewBag.lstSchedules = await _scheduleService.GetAll();
                    ModelState.AddModelError("", "Failed to add doctor.");
                    return View("Add", doctorDTO);
                }
            }
            else
            {
                var result = await _doctorService.UpdateDoctor(doctorDTO);

                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.lstSpecializations = await _specializationService.GetAll();
                    ViewBag.lstSchedules = await _scheduleService.GetAll();
                    ViewBag.lstSelectedSchedules = await _doctorScheduleService.GetDoctorSchedules(doctorDTO.Id);
                    ModelState.AddModelError("", "Failed to update doctor.");
                    return View("Edit", doctorDTO);
                }
            }
        }

        public async Task<IActionResult> Delete(Guid? Id)
        {
            if (Id == null)
            {
                ModelState.AddModelError("", "Invalid doctor ID.");
                return RedirectToAction("Index");
            }

            var result = await _doctorService.ChangeStatus((Guid)Id);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to delete doctor");
                return RedirectToAction("Index");
            }
        }
    }
}
