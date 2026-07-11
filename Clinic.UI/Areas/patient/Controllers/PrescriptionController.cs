using Clinic.Application.Contracts.PrescriptionContracts;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.UI.Areas.patient.Controllers
{
    [Area("patient")]
    public class PrescriptionController : Controller
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        public async Task<IActionResult> Details(Guid appointmentId)
        {
            var prescription = await _prescriptionService.GetPrescritpionByAppointment(appointmentId);

            if (prescription == null)
            {
                return NotFound("Not Created Yet");
            }

            return View(prescription);
        }
    }
}
