using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.AppointmentDTOs
{
    public class DoctorAppointmentDTO : BaseDTO
    {
        public Guid DoctorId { get; set; }
        public DateOnly? Date { get; set; }
    }
}
