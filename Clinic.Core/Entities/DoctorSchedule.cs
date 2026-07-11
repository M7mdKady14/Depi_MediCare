using Clinic.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Entities
{
    public class DoctorSchedule : BaseEntity
    {
        public DoctorSchedule()
        {
            Appointments = new HashSet<Appointment>();
        }

        public Guid DoctorId { get; set; }
        public virtual Doctor? Doctor { get; set; }

        public Guid ScheduleId { get; set; }
        public virtual Schedule? Schedule { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
