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
    public interface IPrescriptionMedicineProtocalService : IBaseService<PrescriptionMedicineProtocal, MedicineProtocalDTO>
    {
        Task<List<MedicineProtocalInfoDTO>> GetProtocalMedicines(Guid protocalId);
        Task<PageResult<MedicineProtocalInfoDTO>> GetProtocalDetails(Guid protocalId, int pageNumber = 1, int pageSize = 10);
    }
}
