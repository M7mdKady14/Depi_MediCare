using Clinic.Application.Contracts.UserContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.UI.Areas.patient.Controllers
{
    [Area("patient")]
    [Authorize(Roles="Patient")]
    public class PatientController : Controller
    {
        private readonly IUserService _userService;
        public PatientController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Details()
        {
            var userId = _userService.GetLoggedInUser().ToString();
            var result = await _userService.GetUserByIdAsync(userId);
            return View(result);
        }
    }
}

