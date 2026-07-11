using Clinic.Application.DTOs.DoctorDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.SpecializationDTOs
{
    public class SpecializationDTO : BaseDTO
    {
        public SpecializationDTO()
        {
            Doctors = new List<DoctorDTO>();
        }

        public string EName { get; set; }
        public string? AName { get; set; }

        public List<DoctorDTO> Doctors { get; set; }
    }
}
