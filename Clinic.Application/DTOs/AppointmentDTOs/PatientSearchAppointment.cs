using Clinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.AppointmentDTOs
{
    public class PatientSearchAppointment
    {
        public Guid? DoctorId { get; set; }
        public Guid? AppointmentTypeId { get; set; }

        public AppointmentStatus? AppointmentStatus { get; set; }

        public DateOnly? DateStart { get; set; }
        public DateOnly? DateEnd { get; set; }
    }
}
