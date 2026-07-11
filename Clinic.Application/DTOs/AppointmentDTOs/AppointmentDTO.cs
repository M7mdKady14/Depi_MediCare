using Clinic.Core.Entities;
using Clinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.AppointmentDTOs
{
    public class AppointmentDTO : BaseDTO
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorScheduleId { get; set; }
        public Guid AppointmentTypeId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
    }
}
