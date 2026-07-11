using Clinic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.MedicalRecordDTOs
{
    public class MedicalRecordDTO : BaseDTO
    {
        public string? Allergy { get; set; }
        public string? Notes { get; set; }
        public string Diagnosis { get; set; }
        public string? ChronicDisease { get; set; }
        public string? CurrentMedications { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
    }
}
