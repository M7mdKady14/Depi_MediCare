using Clinic.Application.DTOs.MedicalRecordDTOs;
using Clinic.Application.DTOs.PatientDTOs;
using Clinic.Core.Entities;
using Clinic.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Contracts.MedicalRecordContracts
{
    public interface IMedicalRecordService : IBaseService<MedicalRecord, MedicalRecordDTO>
    {
        Task<List<DocMedicalRecord>> GetDocMedicalRecords();
        Task<DocMedicalRecord> MedicalRecordDetailsForUser(Guid Id);
        Task<List<MedicalRecordDTO>> GetMedicalRecordsForUser();
        Task<DocMedicalRecord> MedicalRecordDetails(Guid patientId);
        Task<PageResult<DocMedicalRecord>> GetDocPageMedicalRecords(SearchPatient searchPatient, int pageNumber = 1, int pageSize = 10);
        Task<List<MedicalRecordDTO>> GetPatientMedicalRecords(Guid patientId);
        Task<MedicalRecordDTO> GetDocPatientMedicalRecords(Guid PatientId);
    }
}

