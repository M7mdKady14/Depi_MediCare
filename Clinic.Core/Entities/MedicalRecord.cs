using Clinic.Core.Common;
using Clinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Entities
{
    public class MedicalRecord : BaseEntity
    {
        public MedicalRecord()
        {
            MedicalRadiologies = new HashSet<MedicalRadiology>();
            Tests = new HashSet<Test>();
        }

        public string? Allergy { get; set; }
        public string? Notes { get; set; }

        public string Diagnosis { get; set; }
        public string? ChronicDisease { get; set; }
        public string? CurrentMedications { get; set; }

        public Guid PatientId { get; set; }
        public virtual Patient? Patient { get; set; }

        public Guid DoctorId { get; set; }
        public virtual Doctor? Doctor { get; set; }

        public virtual ICollection<MedicalRadiology> MedicalRadiologies { get; set; }
        public virtual ICollection<Test> Tests { get; set; }

    }
}
