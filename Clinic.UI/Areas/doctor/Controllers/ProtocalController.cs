using Clinic.Application.Contracts.ProtocalContracts;
using Clinic.Application.DTOs.PrescriptionDTOs;
using Clinic.Application.DTOs.ProtocalDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clinic.UI.Areas.doctor.Controllers
{
    [Area("doctor")]
    [Authorize(Roles ="Doctor")]
    public class ProtocalController : Controller
    {
        private readonly IPrescriptionProtocalService _prescriptionProtocalService;

        public ProtocalController(IPrescriptionProtocalService prescriptionProtocalService)
        {
            _prescriptionProtocalService = prescriptionProtocalService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _prescriptionProtocalService.GetPageDoctorProtocals();
            return View(result);
        }

        public async Task<IActionResult> Details(Guid Id)
        {
            var result = await _prescriptionProtocalService.GetProtocalDetails(Id);
            return View(result);
        }

        public async Task<IActionResult> Edit(Guid? Id)
        {
            var protocal = await _prescriptionProtocalService.GetProtocalDTO();

            if (Id.HasValue)
            {
                protocal = await _prescriptionProtocalService.GetProtocal((Guid)Id);
            }

            return View(protocal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(PrescriptionProtocalDTO protocalDTO)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", protocalDTO);
            }

            if (protocalDTO.MedicineProtocal.Count == 0)
            {
                return View("Edit", protocalDTO);
            }

            if (protocalDTO.Id == Guid.Empty)
            {
                var result = await _prescriptionProtocalService.CreateProtocal(protocalDTO);

                if (!result.Item1)
                {
                    return View("Edit", protocalDTO);
                }
            }
            else
            {
                var result = await _prescriptionProtocalService.UpdateProtocal(protocalDTO);

                if (!result)
                {
                    return View("Edit", protocalDTO);
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _prescriptionProtocalService.DeleteProtocal(Id);
            return RedirectToAction("Index");
        }
    }
}
