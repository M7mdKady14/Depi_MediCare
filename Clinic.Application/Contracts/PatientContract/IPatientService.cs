using Clinic.Application.DTOs.DoctorDTOs;
using Clinic.Application.DTOs.PatientDTOs;
using Clinic.Core.Entities;
using Clinic.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Contracts.PatientContract
{
    public interface IPatientService : IBaseService<Patient, PatientDTO>
    {
        Task<PageResult<PatientInfoDTO>> GetPagePatients(int pageNumber, int pageSize);
        Task<PageResult<PatientInfoDTO>> SearchPatient(SearchPatient searchPatient, int pageNumber, int pageSize);
        Task<PatientInfoDTO> GetPatientDetails(Guid Id);
        Task<Guid> GetPatientId();
        Task<(bool, Guid)> AddPatient(PatientDTO patientDTO);
        Task<bool> UpdatePatient(PatientDTO patientDTO);
        Task<bool> DeletePatient(Guid Id);
    }
}
