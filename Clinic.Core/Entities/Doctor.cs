using Clinic.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Entities
{
    public class Doctor : BaseEntity
    {
        public Doctor()
        {
            DoctorSchedules = new HashSet<DoctorSchedule>();
            MedicalRecords = new HashSet<MedicalRecord>();
            Appointments = new HashSet<Appointment>();
        }

        public Guid SpecializationId { get; set; }
        public virtual Specialization? Specialization { get; set; }

        public string Name {  get; set; }
        public string? Description { get; set; }
        public string PhoneNumber { get; set; }

        public string UserId { get; set; }

        public int MaximumAppointments { get; set; }

        public virtual ICollection<DoctorSchedule> DoctorSchedules { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<PrescriptionProtocal> PrescriptionProtocals { get; set; }
    }
}
