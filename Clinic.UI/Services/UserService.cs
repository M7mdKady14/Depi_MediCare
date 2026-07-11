using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.UserDTOs;
using Clinic.Core.Enums;
using Clinic.Infrastructure.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Clinic.UI.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<UserInfoDTO>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var userDtos = new List<UserInfoDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Developer"))
                    continue;

                userDtos.Add(new UserInfoDTO
                {
                    Id = Guid.Parse(user.Id),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "No Role"
                });
            }

            return userDtos;
        }

        public Guid GetLoggedInUser()
        {
            var Id = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return (Id != null) ? Guid.Parse(Id) : Guid.Empty;
        }

        public async Task<UserInfoDTO> GetUserByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                return new UserInfoDTO
                {
                    UserName = user.UserName,
                    Id = Guid.Parse(user.Id),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Role = userRoles.FirstOrDefault()
                };
            }

            return null;
        }

        public async Task<UserInfoDTO> GetUserByIdAsync(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                return new UserInfoDTO
                {
                    UserName = user.UserName,
                    Id = Guid.Parse(user.Id),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Role = userRoles.FirstOrDefault()
                };
            }

            return null;
        }

        public async Task<UserResultDTO> LoginAsync(LoginDTO loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null)
            {
                return new UserResultDTO
                {
                    success = false,
                    errors = new[] { "there isn't a user with this UserName !" }
                };
            }

            var loginResult = await _signInManager.PasswordSignInAsync(user, loginDto.Password, true, true);

            if (loginResult.Succeeded)
            {
                return new UserResultDTO
                {
                    success = true,
                    errors = Array.Empty<string>(),
                };
            }

            return new UserResultDTO
            {
                success = false,
                errors = new[] { "Incorrect password. Please try again." }
            };
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<(UserResultDTO, UserRegistrationResult)> RegisterAsync(UserDTO registerDto)
        {
            var checkUser = await _userManager.FindByNameAsync(registerDto.UserName);

            if (checkUser != null)
            {
                return ( 
                    new UserResultDTO
                    {
                        success = false,
                        errors = new[] { "UserName is already in use." }
                    }
                    , 
                    new UserRegistrationResult
                    {
                        UserId = string.Empty,
                        RoleId = string.Empty,
                    }
                );
            }

            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return (
                    new UserResultDTO
                    {
                        success = false,
                        errors = new[] { "Passwords don't match." }
                    }
                    , 
                    new UserRegistrationResult
                    {
                        UserId = string.Empty,
                        RoleId = string.Empty,
                    }
                );
            }

            registerDto.CurrentState = CurrentState.Active;

            AppUser appUser = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.UserName,
                Email = registerDto.UserName + "@Dream.com"
            };

            var registerResult = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (registerResult.Succeeded)
            {
                var role = await _roleManager.FindByIdAsync(registerDto.RoleId);

                if (role == null)
                {
                    return (
                        new UserResultDTO
                        {
                            success = false,
                            errors = new[] { "Invalid Role Selected." }
                        }
                        , 
                        new UserRegistrationResult
                        {
                            UserId = string.Empty,
                            RoleId = string.Empty,
                        }
                    );
                }

                var roleName = role.Name;

                var RoleResult = await _userManager.AddToRoleAsync(appUser, roleName);

                var user = await _userManager.FindByNameAsync(appUser.UserName);

                if (user == null)
                {
                    return (
                        new UserResultDTO
                        {
                            success = false,
                            errors = new[] { "Invalid User With this UserName." }
                        }
                        ,
                        new UserRegistrationResult
                        {
                            UserId = string.Empty,
                            RoleId = string.Empty,
                        }
                    );
                }

                if (RoleResult.Succeeded)
                {
                    return (
                        new UserResultDTO
                        {
                            success = true,
                            errors = Array.Empty<string>(),
                        }
                        ,
                        new UserRegistrationResult
                        {
                            UserId = user.Id,
                            RoleId = role.Id,
                        }
                    );
                }

                return (
                    new UserResultDTO
                    {
                        success = false,
                        errors = RoleResult.Errors.Select(e => e.Description).ToArray()
                    }
                    , 
                    new UserRegistrationResult
                    {
                        UserId = string.Empty,
                        RoleId = string.Empty,
                    }
                );
            }

            return (
                new UserResultDTO
                {
                    success = false,
                    errors = registerResult.Errors.Select(e => e.Description).ToArray()
                } 
                ,
                new UserRegistrationResult
                {
                    UserId = string.Empty,
                    RoleId = string.Empty,
                }
            );
        }

        public async Task<UserResultDTO> DeleteUser(string Id)
        {
            var users = await GetAllUsers();

            if (users.Count() == 1)
            {
                return new UserResultDTO
                {
                    success = false,
                    errors = new[] { "This is the last user in system" }
                };
            }

            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                return new UserResultDTO
                {
                    success = false,
                    errors = new[] { "This username is not registered." }
                };
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Contains("Admin"))
            {
                var admins = users.Where(u => u.Role == "Admin").ToList();
                if (admins.Count == 1)
                {
                    return new UserResultDTO
                    {
                        success = false,
                        errors = new[] { "This is the last admin in system" }
                    };
                }
            }

            if (userRoles.Contains("Developer"))
            {
                var devs = users.Where(u => u.Role == "Developer").ToList();
                if (devs.Count == 1)
                {
                    return new UserResultDTO
                    {
                        success = false,
                        errors = new[] { "This is the last Developer in system" }
                    };
                }
            }

            foreach (var role in userRoles)
            {
                var roleResult = await _userManager.RemoveFromRoleAsync(user, role);
                if (!roleResult.Succeeded)
                {
                    return new UserResultDTO
                    {
                        success = false,
                        errors = new[] { "Failed to remove user roles." }
                    };
                }
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return new UserResultDTO
                {
                    success = true,
                    errors = new[] { "Deleted Successfully !" }
                };
            }

            return new UserResultDTO
            {
                success = false,
                errors = new[] { "Delete Fail !" }
            };
        }

        public async Task<UserResultDTO> ChangeUserRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new UserResultDTO
                {
                    success = false,
                    errors = new[] { "User not found." }
                };
            }

            var users = await GetAllUsers();

            if (users.Count() == 1)
            {
                return new UserResultDTO
                {
                    success = false,
                    errors = new[] { "This is the last user in system" }
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Any())
            {
                return new UserResultDTO
                {
                    success = false,
                    errors = new[] { "User has no role assigned." }
                };
            }

            if (roles.Count != 1)
            {
                return new UserResultDTO
                {
                    success = false,
                    errors = new[] { "User must have exactly one role." }
                };
            }

            var currentRole = roles.Single();

            if (currentRole == "Doctor")
            {
                return new UserResultDTO
                {
                    success = false,
                    errors = new[] { "Not Allowed !" }
                };           
            }

            if (currentRole == "Admin")
            {
                var admins = users.Where(u => u.Role == "Admin").ToList();
                if (admins.Count == 1)
                {
                    return new UserResultDTO
                    {
                        success = false,
                        errors = new[] { "This is the last admin in system" }
                    };
                }
            }

            string newRole = currentRole == "Admin" ? "Reciptionist" : "Admin";

            var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, currentRole);

            if (!removeRoleResult.Succeeded)
            {
                return new UserResultDTO
                {
                    success = false,
                    errors = new[] { "Failed to remove current role." }
                };
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, newRole);

            if (!addRoleResult.Succeeded)
            {
                return new UserResultDTO
                {
                    success = false,
                    errors = new[] { "Failed to assign new role." }
                };
            }

            return new UserResultDTO
            {
                success = true,
                errors = Array.Empty<string>()
            };
        }

        public async Task<UserResultDTO> ChangePassword(ChangePasswordDTO dto)
        {
            var userId = GetLoggedInUser();

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return new UserResultDTO
                {
                    success = false,
                    errors = new[] { "there isn't a user with this Id !" }
                };
            }

            var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);

            if (!result.Succeeded)
            {
                return new UserResultDTO
                {
                    success = false,
                    errors = result.Errors.Select(e => e.Description).ToArray()
                };
            }

            return new UserResultDTO
            {
                success = true,
                errors = Array.Empty<string>()
            };
        }

        public async Task<UserResultDTO> ResetPassword(ResetPasswordDTO dto)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                return new UserResultDTO
                {
                    success = false,
                    errors = new[] { "Passwords don't match" }
                };
            }

            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
            {
                return new UserResultDTO
                {
                    success = false,
                    errors = new[] { "there isn't a user with this Id !" }
                };
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetToken, dto.Password);

            if (!result.Succeeded)
            {
                return new UserResultDTO
                {
                    success = false,
                    errors = result.Errors.Select(e => e.Description).ToArray()
                };
            }

            return new UserResultDTO
            {
                success = true,
                errors = Array.Empty<string>()
            };
        }

        public async Task<UserResultDTO> UpdateUserAsync(UpdateUserDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
            {
                return new UserResultDTO
                {
                    success = false,
                    errors = new[] { "there isn't a user with this Id !" }
                };
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.UserName = dto.UserName;
            user.PhoneNumber = dto.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return new UserResultDTO
                {
                    success = false,
                    errors = result.Errors.Select(e => e.Description).ToArray()
                };
            }

            return new UserResultDTO
            {
                success = true,
                errors = Array.Empty<string>()
            };
        }

        public async Task<List<RoleDTO>> GetAllRoles()
        {
            var result =  await _roleManager.Roles.Select(a => new RoleDTO
            {
                Name = a.Name,
                Id = Guid.Parse(a.Id),
            }).ToListAsync();  
            
            return result;
        }

        public async Task<bool> CheckDoctorRole(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);

            if (role == null) 
                return false; 

            if (role.Name == "Doctor")
                return true;
            
            return false;
        }
    }
}
