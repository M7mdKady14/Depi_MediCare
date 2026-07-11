using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.AppointmentDTOs
{
    public class AppointmentTypeDTO : BaseDTO
    {
        public string EName { get; set; }
        public string? AName { get; set; }

        public string? Description { get; set; }
    }
}
