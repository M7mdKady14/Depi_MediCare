using AutoMapper;
using Clinic.Application.Contracts.ScheduleContracts;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.ScheduleDTOs;
using Clinic.Core.Entities;
using Clinic.Core.Enums;
using Clinic.Infrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Services.ScheduleServices
{
    public class DoctorScheduleService : BaseService<DoctorSchedule, DoctorScheduleDTO>, IDoctorScheduleService
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public DoctorScheduleService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService) 
            : base(unitOfWork, mapper, userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<List<DoctorScheduleDTO>> GetDoctorSchedules(Guid doctorId)
        {
            var schedules = await _unitOfWork.Repository<DoctorSchedule>().GetResult(
                filter: a => a.DoctorId == doctorId
                        && a.CurrentState == CurrentState.Active,
                selectors: a => new DoctorScheduleDTO
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    ScheduleId = a.ScheduleId,
                },
                orderby: a => a.ScheduleId,
                isDesending: true
            );

            return schedules;
        }

        public async Task<List<AppointmentDocSchedule>> GetAppointmentDoctorSchedules(Guid doctorId)
        {
            var schedules = await _unitOfWork.Repository<DoctorSchedule>().GetResult(
                filter: a => a.DoctorId == doctorId
                        && a.CurrentState == CurrentState.Active,
                selectors: a => new AppointmentDocSchedule
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    ScheduleId = a.ScheduleId,
                    Day = a.Schedule.Day,
                    StartTime = a.Schedule.StartTime,
                    EndTime = a.Schedule.EndTime,
                    CurrentState = a.CurrentState,
                },
                orderby: a => a.ScheduleId,
                isDesending: true
            );

            return schedules;
        }

    }
}
