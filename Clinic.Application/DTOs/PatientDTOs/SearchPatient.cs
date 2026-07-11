using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.PatientDTOs
{
    public class SearchPatient
    {
        public string? Name { get; set; }
        public string? NationalNumber { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
