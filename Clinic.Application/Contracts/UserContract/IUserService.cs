using Clinic.Application.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Contracts.UserContract
{
    public interface IUserService
    {
        Task<IEnumerable<UserInfoDTO>> GetAllUsers();
        Task<UserInfoDTO> GetUserByUserNameAsync(string userName);
        Task<UserInfoDTO> GetUserByIdAsync(string Id);

        Guid GetLoggedInUser();

        Task<UserResultDTO> LoginAsync(LoginDTO loginDto);
        Task LogoutAsync();
        Task<(UserResultDTO , UserRegistrationResult)> RegisterAsync(UserDTO registerDto);
        Task<UserResultDTO> DeleteUser(string Id);

        Task<UserResultDTO> ChangeUserRole(string userId);
        Task<UserResultDTO> ChangePassword(ChangePasswordDTO dto);
        Task<UserResultDTO> ResetPassword(ResetPasswordDTO dto);
        Task<UserResultDTO> UpdateUserAsync(UpdateUserDTO dto);

        Task<List<RoleDTO>> GetAllRoles();
        Task<bool> CheckDoctorRole(string Id);
    }
}
