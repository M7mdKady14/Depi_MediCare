using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.MedicalRecordDTOs
{
    public class MedicalRadiologyInfoDTO : BaseDTO
    {
        public string? File {  get; set; }
        public string PatientName { get; set; }
        public string Diagnosis { get; set; }
        public Guid MedicalRecordId { get; set; }
        public Guid DoctorId { get; set; }
        public string Description { get; set; }
    }
}
