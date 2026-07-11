using Clinic.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Entities
{
    public class Specialization : BaseEntity
    {
        public Specialization()
        {
            Doctors = new HashSet<Doctor>();
        }

        public string EName { get; set; }
        public string? AName { get; set; }

        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
