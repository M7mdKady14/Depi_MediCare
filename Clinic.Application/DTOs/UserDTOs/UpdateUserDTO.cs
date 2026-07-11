using System.ComponentModel.DataAnnotations;

namespace Clinic.Application.DTOs.UserDTOs
{
    public class UpdateUserDTO
    {
        public string UserId { get; set; }

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

        [Required(ErrorMessage = "Phone Number is required")]
        [MaxLength(11, ErrorMessage = "Phone number must be exactly 11 digits")]
        [MinLength(11, ErrorMessage = "Phone number must be exactly 11 digits")]
        [RegularExpression(@"^01[0125]\d{8}$", ErrorMessage = "Please enter a valid Egyptian phone number")]
        public string PhoneNumber { get; set; }
    }
}