using Clinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.ScheduleDTOs
{
    public class AppointmentDocSchedule : BaseDTO
    {
        public Guid DoctorId { get; set; }
        public Guid ScheduleId { get; set; }
        public Day Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
