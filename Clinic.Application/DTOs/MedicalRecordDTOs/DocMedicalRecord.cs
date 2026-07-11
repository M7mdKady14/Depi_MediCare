using Clinic.Application.DTOs.AppointmentDTOs;
using Clinic.Application.DTOs.PatientDTOs;
using Clinic.Application.DTOs.PrescriptionDTOs;
using Clinic.Core.Entities;
using Clinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Clinic.Application.DTOs.MedicalRecordDTOs
{
    public class DocMedicalRecord : BaseDTO
    {
        [MaxLength(1000, ErrorMessage ="allergy too long")]
        public string? Allergy { get; set; }

        [MaxLength(1000, ErrorMessage ="Notes too long")]
        public string? Notes { get; set; }

        [Required]
        [MaxLength(2000, ErrorMessage = "Diagnosis too long")]
        public string Diagnosis { get; set; }

        [MaxLength(1000, ErrorMessage = "Chronic Disease too long")]
        public string? ChronicDisease { get; set; }

        [MaxLength(1000, ErrorMessage = "Current Medications too long")]
        public string? CurrentMedications { get; set; }

        [Required]
        public Guid DoctorId { get; set; }

        [Required]
        public string DoctorName { get; set; }

        [Required]
        public string PatientName { get; set; }

        [Required]
        public Guid PatientId { get; set; }

        [Required]
        [MaxLength(14, ErrorMessage = "enter a valid egyptian national number")]
        public string NationalNumber { get; set; }

        [Required]
        [MaxLength(15, ErrorMessage = "enter a valid phone number")]
        public string PhoneNumber { get; set; }

        [MaxLength(150, ErrorMessage = "enter a valid email address")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public BloodType BloodType { get; set; }

        [Required]
        public Gender Gender { get; set; }
    }
}
