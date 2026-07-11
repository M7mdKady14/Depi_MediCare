using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clinic.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles ="Reciptionist , Admin , Doctor")]
        public async Task<IActionResult> Profile()
        {
            var userId = _userService.GetLoggedInUser().ToString();

            var user = await _userService.GetUserByIdAsync(userId);

            if (user != null)
            {
                return View(user);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found.");

                TempData["ErrorMessage"] = "profile Not Found";
                return RedirectToAction("Index");
            }
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginDTO());
        }

        [AllowAnonymous]
        public async Task<IActionResult> SignIn(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Account/Login.cshtml", loginDTO);
            }

            var result = await _userService.LoginAsync(loginDTO);

            if (result.success)
            {
                var user = await _userService.GetUserByUserNameAsync(loginDTO.UserName);

                if (user == null)
                {
                    return View("~/Views/Account/Login.cshtml", loginDTO);
                }

                var role = user.Role;

                if (role == "Admin")
                {
                    return Redirect("/admin/Patient/Index");
                }
                else if (role == "Doctor")
                {
                    return Redirect("/doctor/Doctor/Index");
                }
                else if (role == "Reciptionist")
                {
                    return Redirect("/reciptionist/Appointment/Today");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid role.");
                    return View("~/Views/Account/Login.cshtml", loginDTO);
                }        
            }
            else
            {
                var errors = result.errors.Select(errors => errors).FirstOrDefault();

                ModelState.AddModelError(string.Empty, errors);
                return View("~/Views/Account/Login.cshtml", loginDTO);
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            var user = _userService.GetLoggedInUser();

            if (user != Guid.Empty)
            {
                await _userService.LogoutAsync();
            }

            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

