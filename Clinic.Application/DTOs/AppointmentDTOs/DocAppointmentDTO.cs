using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.AppointmentDTOs
{
    public class DocAppointmentDTO : BaseDTO
    {
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DateOnly AppointmentDate {  get; set; }
    }
}
