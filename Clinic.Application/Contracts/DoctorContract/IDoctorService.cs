using Clinic.Application.DTOs;
using Clinic.Application.DTOs.DoctorDTOs;
using Clinic.Application.DTOs.MedicalRecordDTOs;
using Clinic.Core.Entities;
using Clinic.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Contracts.DoctorContract
{
    public interface IDoctorService : IBaseService <Doctor, DoctorDTO>
    {
        Task<PageResult<DoctorInfoDTO>> GetPageDoctors(int pageNumber, int PageSize);
        Task<DoctorInfoDTO> GetDoctorDetails(Guid Id);
        Task<DoctorInfoDTO> DoctorProfile();
        Task<PageResult<DoctorInfoDTO>> SearchDoctors(SearchDoctor searchDoctor, int pageNumber = 1, int PageSize = 10);
        Task<(bool , Guid)> AddDoctor(DoctorDTO doctorDTO);
        Task<bool> UpdateDoctor(DoctorDTO doctorDTO);
        Task<DoctorDTO> GetDocDTO(string userId);
        Task<Guid> GetDoctorId();
    }
}
