using Clinic.Application.Contracts.AppointmentContracts;
using Clinic.Application.Contracts.DoctorContract;
using Clinic.Application.Contracts.PatientContract;
using Clinic.Application.Contracts.ScheduleContracts;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.AppointmentDTOs;
using Clinic.Application.DTOs.ScheduleDTOs;
using Clinic.Application.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.UI.Areas.patient.Controllers
{
    [Area("patient")]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly IDoctorScheduleService _doctorScheduleService;
        private readonly IAppointmentTypeService _appointmentTypeService;

        public AppointmentController(IAppointmentService appointmentService, IDoctorService doctorService,
            IDoctorScheduleService doctorScheduleService, IAppointmentTypeService appointmentTypeService
            , IPatientService patientService)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _doctorScheduleService = doctorScheduleService;
            _appointmentTypeService = appointmentTypeService;
            _patientService = patientService;
        }

        public async Task<IActionResult> Index(PatientSearchAppointment searchAppointment , int pageNumber = 1, int pageSize = 10)
        {
            var patientId = await _patientService.GetPatientId();
            var result = await _appointmentService.PatientSearchAppointments(searchAppointment, pageNumber, pageSize);

            ViewBag.lstDoctors = await _doctorService.GetAll();
            ViewBag.lstAppointmentTypes = await _appointmentTypeService.GetAll();

            return View(result);
        }

        public async Task<IActionResult> Details(Guid Id)
        {
            var appiontment = await _appointmentService.GetAppointmentDetails(Id);
            return View(appiontment);
        }

        public async Task<IActionResult> Edit(Guid? Id)
        {
            var patientId = await _patientService.GetPatientId();

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

        #region Cancel Appointment
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
