using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Enums
{
    public enum AppointmentStatus
    {
        Created = 1,
        Waiting = 2,
        InProgress = 3,
        Completed = 4,
        Canceled = 5
    }
}

