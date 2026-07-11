using Clinic.Core.Common;
using Clinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Entities
{
    public class Appointment : BaseEntity
    {
        public Guid DoctorId { get; set; }
        public virtual Doctor? Doctor { get; set; }

        public Guid DoctorScheduleId { get; set; }
        public virtual DoctorSchedule? DoctorSchedule { get; set; }

        public Guid PatientId { get; set; }
        public virtual Patient? Patient { get; set; }

        public Guid AppointmentTypeId { get; set; }
        public virtual AppointmentType? AppointmentType { get; set; }

        public virtual Prescription? Prescription { get; set; }

        public DateOnly AppointmentDate { get; set; }

        public AppointmentStatus AppointmentStatus { get; set; }
    }
}
