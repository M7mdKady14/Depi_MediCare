using Clinic.Application.DTOs.AppointmentDTOs;
using Clinic.Application.DTOs.PatientDTOs;
using Clinic.Application.DTOs.PrescriptionDTOs;
using Clinic.Core.Entities;
using Clinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.MedicalRecordDTOs
{
    public class DocMedicalRecord : BaseDTO
    {
        public string? Allergy { get; set; }
        public string? Notes { get; set; }
        public string Diagnosis { get; set; }
        public string? ChronicDisease { get; set; }
        public string? CurrentMedications { get; set; }

        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }

        public string PatientName { get; set; }
        public Guid PatientId { get; set; }

        public string NationalNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime BirthDate { get; set; }

        public BloodType BloodType { get; set; }
        public Gender Gender { get; set; }
    }
}
