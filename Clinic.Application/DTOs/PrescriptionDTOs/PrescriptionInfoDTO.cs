using Clinic.Application.DTOs.AppointmentDTOs;
using Clinic.Application.DTOs.PatientDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.PrescriptionDTOs
{
    public class PrescriptionInfoDTO : BaseDTO
    {
        public PrescriptionInfoDTO()
        {
            PrescriptionMedicines = new List<PrescriptionMedicineDTO>();
        }

        public DateTime PrescriptionDate { get; set; }

        public Guid AppointmentId { get; set; }
        public virtual AppointmentDTO Appointment { get; set; }

        public Guid PatientId { get; set; }
        public string PatientName { get; set; }

        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }

        public virtual List<PrescriptionMedicineDTO> PrescriptionMedicines { get; set; }
    }
}

