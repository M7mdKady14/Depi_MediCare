using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clinic.UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        //[Authorize(Roles = "Admin , Developer")]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsers();
            return View(users);
        }

        //[Authorize(Roles = "Admin , Developer")]
        public async Task<IActionResult> Details(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                var allUsers = await _userService.GetAllUsers();
                return View("Index", allUsers);
            }
        }

        //[Authorize(Roles = "Admin , Developer")]
        public async Task<IActionResult> Register()
        {
            ViewBag.lstRoles = await _userService.GetAllRoles();
            return View(new UserDTO());
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
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var errors = result.errors.Select(errors => errors).FirstOrDefault();

                ModelState.AddModelError(string.Empty, errors);
                return View("~/Views/Account/Login.cshtml", loginDTO);
            }
        }

        //[Authorize(Roles = "Admin , Developer")]
        public async Task<IActionResult> SignUp(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.lstRoles = await _userService.GetAllRoles();
                return View("Register", userDTO);
            }

            var result = await _userService.RegisterAsync(userDTO);

            var roleCheck = await _userService.CheckDoctorRole(result.Item2.RoleId);

            if (result.Item1.success && roleCheck)
            {
                return RedirectToAction("Add", "Doctor", new { userId = result.Item2.UserId });
            }
            else if (result.Item1.success && !roleCheck)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var errors = result.Item1.errors.Select(errors => errors).FirstOrDefault();

                ViewBag.lstRoles = await _userService.GetAllRoles();

                ModelState.AddModelError(string.Empty, errors);
                return View("Register", userDTO);
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

        //[Authorize(Roles = "Admin , Developer")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userService.DeleteUser(userId);

            if (result.success)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                var errors = result.errors.Select(errors => errors).FirstOrDefault();

                ModelState.AddModelError(string.Empty, errors);

                var allUsers = await _userService.GetAllUsers();
                return View("Index", allUsers);
            }
        }

        //[Authorize(Roles = "Admin , Developer")]
        public async Task<IActionResult> ChangeUserRole(string userId)
        {
            var result = await _userService.ChangeUserRole(userId);

            if (result.success)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                var errors = result.errors.Select(errors => errors).FirstOrDefault();

                ModelState.AddModelError(string.Empty, errors);

                var allUsers = await _userService.GetAllUsers();
                return View("Index", allUsers);
            }
        }

        //[Authorize(Roles = "Admin , Developer")]
        [HttpGet]
        public async Task<IActionResult> UpdateUser(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var updateUserDTO = new UpdateUserDTO
            {
                UserId = user.Id.ToString(),
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
            };

            return View(updateUserDTO);
        }

        //[Authorize(Roles = "Admin , Developer")]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO updateUserDTO)
        {
            var result = await _userService.UpdateUserAsync(updateUserDTO);

            if (result.success)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                var errors = result.errors.Select(errors => errors).FirstOrDefault();
                ModelState.AddModelError(string.Empty, errors);

                var allUsers = await _userService.GetAllUsers();
                return View("Index", allUsers);
            }
        }

        //[Authorize(Roles = "Admin , Developer")]
        public IActionResult ResetPassword(string userId)
        {
            return View(new ResetPasswordDTO { UserId = userId });
        }

        //[Authorize(Roles = "Admin , Developer")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var result = await _userService.ResetPassword(resetPasswordDTO);

            if (result.success)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                var errors = result.errors.Select(errors => errors).FirstOrDefault();
                ModelState.AddModelError(string.Empty, errors);

                var allUsers = await _userService.GetAllUsers();
                return View("Index", allUsers);
            }
        }
    }
}
