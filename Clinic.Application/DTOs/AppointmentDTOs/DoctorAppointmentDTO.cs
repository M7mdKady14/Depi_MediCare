using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.AppointmentDTOs
{
    public class DoctorAppointmentDTO : BaseDTO
    {
        [Required(ErrorMessage = "please select doctor")]
        public Guid DoctorId { get; set; }

        public DateOnly? Date { get; set; }
    }
}
