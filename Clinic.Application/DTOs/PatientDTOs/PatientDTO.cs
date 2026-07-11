using Clinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.PatientDTOs
{
    public class PatientDTO : BaseDTO
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string? UserId { get; set; }

        public string NationalNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime BirthDate { get; set; }

        public BloodType BloodType { get; set; }
        public Gender Gender { get; set; }
    }
}
