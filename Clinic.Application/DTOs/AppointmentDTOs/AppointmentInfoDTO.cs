using Clinic.Application.DTOs.ScheduleDTOs;
using Clinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.AppointmentDTOs
{
    public class AppointmentInfoDTO : BaseDTO
    {
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }

        public Guid PatientId { get; set; }
        public string PatientName { get; set; }

        public Guid AppointmentTypeId { get; set; }
        public string AppointmentTypeName { get; set; }

        public Guid DoctorScheduleId {  get; set; }

        // ========== display this 3 as a unit ========== //
        public Day Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        // ======================================= //

        public DateOnly AppointmentDate { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
    }
}

