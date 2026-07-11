using Clinic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.ScheduleDTOs
{
    public class DoctorScheduleDTO : BaseDTO
    {
        public Guid DoctorId { get; set; }
        public Guid ScheduleId { get; set; }
    }
}
