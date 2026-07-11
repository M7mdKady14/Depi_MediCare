using Clinic.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Entities
{
    public class PrescriptionProtocal : BaseEntity
    {
        public PrescriptionProtocal()
        {
            PrescriptionMedicineProtocal = new HashSet<PrescriptionMedicineProtocal>();
        }

        public string Name { get; set; }
        public string Disease { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }

        public Guid DoctorId { get; set; }
        public virtual Doctor? Doctor { get; set; }

        public virtual ICollection<PrescriptionMedicineProtocal> PrescriptionMedicineProtocal { get; set; }
    }
}

