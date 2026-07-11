using Clinic.Application.Contracts.AppointmentContracts;
using Clinic.Application.Contracts.DoctorContract;
using Clinic.Application.Contracts.ScheduleContracts;
using Clinic.Application.DTOs.AppointmentDTOs;
using Clinic.Application.DTOs.DoctorDTOs;
using Clinic.Application.DTOs.ScheduleDTOs;
using Clinic.Application.Helper;
using Clinic.Application.Services.ScheduleServices;
using Clinic.Core.Entities;
using Clinic.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clinic.UI.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;
        private readonly IDoctorScheduleService _doctorScheduleService;
        private readonly IAppointmentTypeService _appointmentTypeService; 

        public AppointmentController(IAppointmentService appointmentService, IDoctorService doctorService,
            IDoctorScheduleService doctorScheduleService, IAppointmentTypeService appointmentTypeService)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _doctorScheduleService = doctorScheduleService;
            _appointmentTypeService = appointmentTypeService;
        }

        public async Task<IActionResult> Index(ReceptionSearchAppointment searchAppointment, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _appointmentService.ReceptionSearchAppointments(searchAppointment, pageNumber, pageSize);

            ViewBag.lstDoctors = await _doctorService.GetAll();
            ViewBag.lstAppointmentTypes = await _appointmentTypeService.GetAll();

            return View(result);
        }

        public async Task<IActionResult> Today(int pageNumber = 1, int pageSize = 10)
        {
            var search = new ReceptionSearchAppointment
            {
                DateStart = DateOnly.FromDateTime(DateTime.Today)
            };

            var result = await _appointmentService.ReceptionSearchAppointments(search, pageNumber, pageSize);

            ViewBag.lstDoctors = await _doctorService.GetAll();
            ViewBag.lstAppointmentTypes = await _appointmentTypeService.GetAll();

            return View("Index", result);
        }

        public async Task<IActionResult> Details(Guid Id)
        {
            var appiontment = await _appointmentService.GetAppointmentDetails(Id);
            return View(appiontment);
        }

        public async Task<IActionResult> Edit(Guid? Id , Guid patientId)
        {
            if (patientId == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var appointment = new AppointmentDTO();

            appointment.PatientId = patientId;

            if (Id != null && Id != Guid.Empty) 
            {
                appointment = await _appointmentService.GetById((Guid)Id);
            }

            ViewBag.lstDoctors = await _doctorService.GetAll();
            ViewBag.lstAppointmentTypes = await _appointmentTypeService.GetAll();

            if (appointment.DoctorId != Guid.Empty)
                ViewBag.lstDoctorSchedules = await _doctorScheduleService.GetAppointmentDoctorSchedules(appointment.DoctorId);
            else
                ViewBag.lstDoctorSchedules = new List<AppointmentDocSchedule>();

            return View(appointment);
        }

        [HttpGet]
        public async Task<JsonResult> GetDoctorSchedules(Guid doctorId, DateTime? appointmentDate)
        {
            if (doctorId == Guid.Empty)
                return Json(new List<object>());

            var schedules = await _doctorScheduleService.GetAppointmentDoctorSchedules(doctorId);

            if (appointmentDate.HasValue)
            {
                var customDay = DateTimeHelper.ConvertToCustomDay(appointmentDate.Value.DayOfWeek);
                schedules = schedules.Where(s => s.Day == customDay).ToList();
            }

            var result = schedules.Select(s => new
            {
                id = s.Id,
                day = s.Day.ToString(),
                startTime = s.StartTime.ToString(@"hh\:mm"),
                endTime = s.EndTime.ToString(@"hh\:mm")
            });

            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(AppointmentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.lstDoctors = await _doctorService.GetAll();
                ViewBag.lstAppointmentTypes = await _appointmentTypeService.GetAll();
                ViewBag.lstDoctorSchedules = await _doctorScheduleService.GetAppointmentDoctorSchedules(dto.DoctorId);
                return View("Edit", dto);
            }

            if (dto.Id == Guid.Empty)
            {
                var result = await _appointmentService.CreateAppointment(dto);

                if (result.Item1)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.lstDoctors = await _doctorService.GetAll();
                    ViewBag.lstAppointmentTypes = await _appointmentTypeService.GetAll();
                    ViewBag.lstDoctorSchedules = await _doctorScheduleService.GetAppointmentDoctorSchedules(dto.DoctorId);
                    return View("Edit", dto);
                }
            }
            else
            {
                var result = await _appointmentService.UpdateAppointment(dto);

                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.lstDoctors = await _doctorService.GetAll();
                    ViewBag.lstAppointmentTypes = await _appointmentTypeService.GetAll();
                    ViewBag.lstDoctorSchedules = await _doctorScheduleService.GetAppointmentDoctorSchedules(dto.DoctorId);
                    return View("Edit", dto);
                }
            }
        }

        public async Task<IActionResult> Delete(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                await _appointmentService.ChangeStatus(Id);
            }

            return RedirectToAction("Index");
        }

        #region Appointment Status
        public async Task<IActionResult> AppointmentWaiting(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                await _appointmentService.AppointmentWaiting(Id);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AppointmentInProgress(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                await _appointmentService.AppointmentInProgress(Id);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AppointmentCompeleted(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                await _appointmentService.AppointmentCompeleted(Id);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AppointmentCanceled(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                await _appointmentService.AppointmentCanceled(Id);
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}
