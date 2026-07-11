using AutoMapper;
using Clinic.Application.Contracts.ScheduleContracts;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.ScheduleDTOs;
using Clinic.Core.Entities;
using Clinic.Core.Enums;
using Clinic.Infrastructure.Contracts;
using Clinic.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Services.ScheduleServices
{
    public class ScheduleService : BaseService<Schedule, ScheduleDTO>, IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService) 
            : base(unitOfWork, mapper, userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<PageResult<ScheduleDTO>> GetPageSchedule(int pageNumber = 1 , int pageSize = 10)
        {
            var data = await _unitOfWork.Repository<Schedule>().GetPageResult(
                pageNumber, 
                pageSize,
                filter: a => a.CurrentState == CurrentState.Active,
                selectors: a => new ScheduleDTO
                {
                    Id = a.Id,
                    Day = a.Day,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                },
                orderby: a => a.CreatedDate,
                isDesending: true
            );

            return data;
        }

        public async Task<ScheduleDTO> GetScheduleDetails(Guid Id)
        {
            var data = await _unitOfWork.Repository<Schedule>().GetResultForOne(
                filter: a => a.CurrentState == CurrentState.Active &&
                        a.Id == Id,
                selectors: a => new ScheduleDTO
                {
                    Id = a.Id,
                    Day = a.Day,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    DoctorSchedules = a.DoctorSchedules.Select(a => new DoctorScheduleDTO
                    {
                        DoctorId = a.DoctorId,
                        Id = a.Id,
                        ScheduleId = a.ScheduleId
                    }).ToList()
                }
            );

            return data;
        }

        public async Task<bool> DeleteSchedule(Guid Id)
        {
            var data = await _unitOfWork.Repository<Schedule>().GetResultForOne(
                filter: a => a.CurrentState == CurrentState.Active &&
                        a.Id == Id,
                selectors: a => new ScheduleDTO
                {
                    Id = a.Id,
                    Day = a.Day,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    DoctorSchedules = a.DoctorSchedules.Where(a => a.CurrentState == CurrentState.Active).Select(a => new DoctorScheduleDTO
                    {
                        DoctorId = a.DoctorId,
                        Id = a.Id,
                        ScheduleId = a.ScheduleId
                    }).ToList()
                }
            );

            if (data == null)
            {
                return false;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                if (data.DoctorSchedules.Any())
                {
                    await _unitOfWork.RollbackAsync();
                    return false;
                }

                var result = await ChangeStatus(data.Id);

                if (!result)
                {
                    await _unitOfWork.RollbackAsync();
                    return false;
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
    
