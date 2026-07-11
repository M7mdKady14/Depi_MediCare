using AutoMapper;
using Clinic.Application.Contracts.PrescriptionContracts;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.PrescriptionDTOs;
using Clinic.Core.Entities;
using Clinic.Core.Enums;
using Clinic.Infrastructure.Contracts;
using Clinic.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Services.PrescriptionService
{
    public class PrescriptionMedicineService : BaseService<PrescriptionMedicine, PrescriptionMedicineDTO>, IPrescriptionMedicineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public PrescriptionMedicineService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService) 
            : base(unitOfWork, mapper, userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<List<PrescriptionMedicineInfoDTO>> GetPrescritpionDetails(Guid prescriptionId)
        {
            var result = await _unitOfWork.Repository<PrescriptionMedicine>().GetResult(
                filter: a => a.CurrentState == CurrentState.Active
                        && a.Id == prescriptionId,
                selectors: p => new PrescriptionMedicineInfoDTO
                {
                    Id = p.Id,
                    PrescriptionId = p.PrescriptionId,
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

        public async Task<List<PrescriptionMedicineDTO>> GetPrescritpionMedicines(Guid prescriptionId)
        {
            var result = await _unitOfWork.Repository<PrescriptionMedicine>().GetResult(
                filter: a => a.CurrentState == CurrentState.Active
                        && a.Id == prescriptionId,
                selectors: p => new PrescriptionMedicineDTO
                {
                    Id = p.Id,
                    PrescriptionId = p.PrescriptionId,
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

        public async Task<PageResult<PrescriptionMedicineInfoDTO>> GetPrescritpionDetails(Guid prescriptionId , int pageNumber = 1 , int pageSize = 10)
        {
            var result = await _unitOfWork.Repository<PrescriptionMedicine>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.CurrentState == CurrentState.Active
                        && a.Id == prescriptionId,
                selectors: p => new PrescriptionMedicineInfoDTO
                {
                    Id = p.Id,
                    PrescriptionId = p.PrescriptionId,
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
