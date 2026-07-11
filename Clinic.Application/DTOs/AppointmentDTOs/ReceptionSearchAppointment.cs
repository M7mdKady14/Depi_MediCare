using Clinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.AppointmentDTOs
{
    public class ReceptionSearchAppointment
    {
        public Guid? DoctorId { get; set; }
        public Guid? AppointmentTypeId { get; set; }

        public string? PatientName { get; set; }
        public string? PatientPhoneNumber { get; set; }
        public string? PatientNationalNumber { get; set; }
        public AppointmentStatus? AppointmentStatus { get; set; }

        public DateOnly? DateStart { get; set; }
        public DateOnly? DateEnd { get; set; }
    }
}
