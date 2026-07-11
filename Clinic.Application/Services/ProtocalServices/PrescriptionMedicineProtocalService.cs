using AutoMapper;
using Clinic.Application.Contracts.ProtocalContracts;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.PrescriptionDTOs;
using Clinic.Application.DTOs.ProtocalDTOs;
using Clinic.Core.Entities;
using Clinic.Core.Enums;
using Clinic.Infrastructure.Contracts;
using Clinic.Infrastructure.Models;

namespace Clinic.Application.Services.ProtocalServices
{
    public class PrescriptionMedicineProtocalService : BaseService<PrescriptionMedicineProtocal, MedicineProtocalDTO>, IPrescriptionMedicineProtocalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public PrescriptionMedicineProtocalService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
            : base(unitOfWork, mapper, userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<PageResult<MedicineProtocalInfoDTO>> GetProtocalDetails(Guid protocalId, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _unitOfWork.Repository<PrescriptionMedicineProtocal>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.CurrentState == CurrentState.Active
                        && a.Id == protocalId,
                selectors: p => new MedicineProtocalInfoDTO
                {
                    Id = p.Id,
                    ProtocalId = p.ProtocalId,
                    ProtocalName = p.PrescriptionProtocal.Name,
                    MedicineName = p.MedicineName,
                    Dosage = p.Dosage,
                    Duration = p.Duration,
                    Frequency = p.Frequency,
                    Instructions = p.Instructions,
                    CurrentState = p.CurrentState
                },
                orderby: a => a.CreatedDate,
                isDesending: true
            );

            return result;
        }

        public async Task<List<MedicineProtocalInfoDTO>> GetProtocalMedicines(Guid protocalId)
        {
            var result = await _unitOfWork.Repository<PrescriptionMedicineProtocal>().GetResult(
                filter: a => a.CurrentState == CurrentState.Active
                        && a.Id == protocalId,
                selectors: p => new MedicineProtocalInfoDTO
                {
                    Id = p.Id,
                    ProtocalId = p.ProtocalId,
                    ProtocalName = p.PrescriptionProtocal.Name,
                    MedicineName = p.MedicineName,
                    Dosage = p.Dosage,
                    Duration = p.Duration,
                    Frequency = p.Frequency,
                    Instructions = p.Instructions,
                    CurrentState = p.CurrentState
                },
                orderby: a => a.CreatedDate,
                isDesending: true
            );

            return result;
        }
    }
}
