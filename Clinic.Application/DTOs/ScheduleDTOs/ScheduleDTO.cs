using Clinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.ScheduleDTOs
{
    public class ScheduleDTO : BaseDTO
    {
        public ScheduleDTO()
        {
            DoctorSchedules = new List<DoctorScheduleDTO>();
        }

        public Day Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public List<DoctorScheduleDTO> DoctorSchedules {  get; set; }
    }
}
