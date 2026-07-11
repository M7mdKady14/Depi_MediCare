using Clinic.Application.DTOs.SpecializationDTOs;
using Clinic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Contracts.SpecializationContract
{
    public interface ISpecializationService : IBaseService<Specialization, SpecializationDTO>
    {
        Task<SpecializationDTO> GetSpecializationDetails(Guid Id);
        Task<bool> DeleteSpecialization(Guid Id);
    }
}
