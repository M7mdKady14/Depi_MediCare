using Clinic.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Entities
{
    public class AppointmentType : BaseEntity
    {
        public AppointmentType()
        {
            Appointments = new HashSet<Appointment>();
        }

        public string EName { get; set; }
        public string? AName { get; set; }

        public string? Description { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
