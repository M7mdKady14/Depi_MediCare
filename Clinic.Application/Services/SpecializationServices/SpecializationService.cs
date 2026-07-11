using AutoMapper;
using Clinic.Application.Contracts.SpecializationContract;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.DoctorDTOs;
using Clinic.Application.DTOs.SpecializationDTOs;
using Clinic.Core.Entities;
using Clinic.Core.Enums;
using Clinic.Infrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Services.SpecializationServices
{
    public class SpecializationService : BaseService<Specialization, SpecializationDTO>, ISpecializationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public SpecializationService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
            : base(unitOfWork, mapper, userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<SpecializationDTO> GetSpecializationDetails(Guid Id)
        {
            var specialization = await _unitOfWork.Repository<Specialization>().GetResultForOne(
                filter: a => a.Id == Id && a.CurrentState == CurrentState.Active,
                selectors: a => new SpecializationDTO
                {
                    Id = a.Id,
                    EName = a.EName,
                    AName = a.AName,
                    Doctors = a.Doctors.Select(d => new DoctorDTO
                    {
                        Id = d.Id,
                        UserId = d.UserId,
                        PhoneNumber = d.PhoneNumber
                    }).ToList()
                }
            );

            return specialization;
        }

        public async Task<bool> DeleteSpecialization(Guid Id)
        {
            var specialization = await _unitOfWork.Repository<Specialization>().GetResultForOne(
                filter: a => a.Id == Id && a.CurrentState == CurrentState.Active,
                selectors: a => new SpecializationDTO
                {
                    Id = a.Id,
                    EName = a.EName,
                    AName = a.AName,
                    Doctors = a.Doctors.Where(a => a.CurrentState == CurrentState.Active).Select(d => new DoctorDTO
                    {
                        Id = d.Id,
                        UserId = d.UserId,
                        SpecializationId = d.SpecializationId,
                        PhoneNumber = d.PhoneNumber
                    }).ToList()
                }
            );

            if (specialization == null)
            {
                return false;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                if (specialization.Doctors.Any())
                {
                    await _unitOfWork.RollbackAsync();
                    return false;
                }
                else
                {
                    var result = await ChangeStatus(specialization.Id);

                    if (!result)
                    {
                        await _unitOfWork.RollbackAsync();
                        return false;
                    }
                }

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch 
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}

