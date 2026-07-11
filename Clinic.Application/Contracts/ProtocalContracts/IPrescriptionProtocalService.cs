using Clinic.Application.DTOs.PrescriptionDTOs;
using Clinic.Application.DTOs.ProtocalDTOs;
using Clinic.Core.Entities;
using Clinic.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Contracts.ProtocalContracts
{
    public interface IPrescriptionProtocalService : IBaseService<PrescriptionProtocal , PrescriptionProtocalDTO>
    {
        #region Get Methods

        Task<PageResult<ProtocalInfoDTO>> GetPageProtocals(
            int pageNumber = 1,
            int pageSize = 10);

        Task<PageResult<ProtocalInfoDTO>> GetPageDoctorProtocals(int pageNumber = 1 , int pageSize = 10);

        Task<List<ProtocalInfoDTO>> GetDoctorProtocals();

        Task<ProtocalInfoDTO> GetProtocalDetails(Guid protocalId);

        Task<List<ProtocalInfoDTO>> GetProtocals();

        Task<PrescriptionDTO> ProtocalToPrescription(Guid protocalId , Guid appointmentId);

        Task<PrescriptionProtocalDTO> GetProtocalDTO();

        Task<PrescriptionProtocalDTO> GetProtocal(Guid protocalId);

        #endregion

        #region Create, Update, Delete

        Task<(bool, Guid)> CreateProtocal(
            PrescriptionProtocalDTO prescriptionProtocalDTO);

        Task<bool> UpdateProtocal(
            PrescriptionProtocalDTO prescriptionProtocalDTO);

        Task<bool> DeleteProtocal(
            Guid id);

        #endregion
    }
}
