using System.ComponentModel.DataAnnotations;
using Clinic.Application.DTOs;

namespace Clinic.Application.DTOs.UserDTOs
{
    public class UserDTO : BaseDTO
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9._]+$", ErrorMessage = "Username can only contain letters, numbers, dots, and underscores")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain uppercase, lowercase, number, and special character")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [MaxLength(11, ErrorMessage = "Phone number must be 11 digits")]
        [MinLength(11, ErrorMessage = "Phone number must be 11 digits")]
        [RegularExpression(@"^01[0125]\d{8}$", ErrorMessage = "Please enter a valid Egyptian phone number")]
        public string PhoneNumber { get; set; }

        [MaxLength(20)]
        public string? Role { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string RoleId { get; set; } 
    }
}
