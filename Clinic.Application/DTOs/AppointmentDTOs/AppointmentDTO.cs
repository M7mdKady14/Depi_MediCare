using Clinic.Core.Entities;
using Clinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.AppointmentDTOs
{
    public class AppointmentDTO : BaseDTO
    {
        [Required(ErrorMessage ="Please Enter Doctor")]
        public Guid DoctorId { get; set; }

        [Required]
        public Guid PatientId { get; set; }

        [Required(ErrorMessage = "Select Time")]
        public Guid DoctorScheduleId { get; set; }

        [Required(ErrorMessage = "please appointment type")]
        public Guid AppointmentTypeId { get; set; }

        [Required(ErrorMessage = "please enter date")]
        public DateOnly AppointmentDate { get; set; }

        [Required]
        public AppointmentStatus AppointmentStatus { get; set; }
    }
}
