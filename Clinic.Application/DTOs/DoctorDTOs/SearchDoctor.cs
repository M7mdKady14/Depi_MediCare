using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.DoctorDTOs
{
    public class SearchDoctor
    {
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid? ScheduleId { get; set; }
        public Guid? SpecializationId { get; set; }
    }
}

