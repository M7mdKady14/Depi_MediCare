using Clinic.Core.Common;
using Clinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Entities
{
    public class Schedule : BaseEntity
    {
        public Schedule()
        {
            DoctorSchedules = new HashSet<DoctorSchedule>();
        }

        public Day Day { get; set; } 
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public virtual ICollection<DoctorSchedule> DoctorSchedules { get; set; }
    }
}

