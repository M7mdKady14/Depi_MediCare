using Clinic.Application.Contracts.AppointmentContracts;
using Clinic.Application.Contracts.DoctorContract;
using Clinic.Application.Contracts.ScheduleContracts;
using Clinic.Application.DTOs.AppointmentDTOs;
using Clinic.Application.Services.AppointmentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clinic.UI.Areas.doctor.Controllers
{
    [Area("doctor")]
    [Authorize(Roles ="Doctor")]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IAppointmentTypeService _appointmentTypeService;
        private readonly IDoctorScheduleService _doctorScheduleService;
        private readonly IDoctorService _doctorService;

        public AppointmentController(IAppointmentService appointmentService , IAppointmentTypeService appointmentTypeService 
            , IDoctorScheduleService doctorScheduleService , IDoctorService doctorService)
        {
            _appointmentTypeService = appointmentTypeService;
            _appointmentService = appointmentService;
            _doctorScheduleService = doctorScheduleService;
            _doctorService = doctorService;
        }
        
        public async Task<IActionResult> Index(DoctorSearchAppointment doctorSearchAppointment , int pageNumber = 1 , int pageSize = 10)
        {
            var doctorId = await _doctorService.GetDoctorId();
            var result = await _appointmentService.DoctorSearchAppointments(doctorSearchAppointment, pageNumber, pageSize);
            ViewBag.lstAppointmentTypes = await _appointmentTypeService.GetAll();
            ViewBag.lstDoctorSchedules = await _doctorScheduleService.GetAppointmentDoctorSchedules(doctorId);
            return View(result);
        }

        public async Task<IActionResult> Today(int pageNumber = 1, int pageSize = 10)
        {
            var search = new DoctorSearchAppointment
            {
                Date = DateOnly.FromDateTime(DateTime.Today)
            };

            var doctorId = await _doctorService.GetDoctorId();


            var result = await _appointmentService.DoctorSearchAppointments(search, pageNumber, pageSize);

            ViewBag.lstAppointmentTypes = await _appointmentTypeService.GetAll();
            ViewBag.lstDoctorSchedules = await _doctorScheduleService.GetAppointmentDoctorSchedules(doctorId);

            return View("Index", result);
        }

        public async Task<IActionResult> Details(Guid appointmentId)
        {
            var appiontment = await _appointmentService.GetAppointmentDetails(appointmentId);
            return View(appiontment);
        }
    }
}

