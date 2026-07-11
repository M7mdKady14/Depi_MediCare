using Clinic.Application.DTOs.PrescriptionDTOs;
using Clinic.Core.Entities;
using Clinic.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Contracts.PrescriptionContracts
{
    public interface IPrescriptionMedicineService : IBaseService<PrescriptionMedicine, PrescriptionMedicineDTO>
    {
        Task<List<PrescriptionMedicineInfoDTO>> GetPrescritpionDetails(Guid prescriptionId);
        Task<List<PrescriptionMedicineDTO>> GetPrescritpionMedicines(Guid prescriptionId);
        Task<PageResult<PrescriptionMedicineInfoDTO>> GetPrescritpionDetails(Guid prescriptionId, int pageNumber = 1, int pageSize = 10);
    }
}
