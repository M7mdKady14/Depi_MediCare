using Clinic.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Entities
{
    public class Prescription : BaseEntity
    {
        public Prescription()
        {
            PrescriptionMedicines = new HashSet<PrescriptionMedicine>();
        }

        public DateTime PrescriptionDate { get; set; }

        public Guid AppointmentId { get; set; }
        public virtual Appointment? Appointment { get; set; }

        public virtual ICollection<PrescriptionMedicine> PrescriptionMedicines { get; set; }
    }
}
