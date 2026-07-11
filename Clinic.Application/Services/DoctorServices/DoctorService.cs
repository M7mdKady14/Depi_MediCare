using AutoMapper;
using Clinic.Application.Contracts.DoctorContract;
using Clinic.Application.Contracts.ScheduleContracts;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.DoctorDTOs;
using Clinic.Application.DTOs.MedicalRecordDTOs;
using Clinic.Application.DTOs.ScheduleDTOs;
using Clinic.Application.DTOs.UserDTOs;
using Clinic.Core.Entities;
using Clinic.Core.Enums;
using Clinic.Infrastructure.Contracts;
using Clinic.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Services.DoctorServices
{
    public class DoctorService : BaseService<Doctor, DoctorDTO>, IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IDoctorScheduleService _doctorScheduleService;

        public DoctorService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService , IDoctorScheduleService doctorScheduleService) 
            : base(unitOfWork, mapper, userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _doctorScheduleService = doctorScheduleService;
        }

        public async Task<DoctorInfoDTO> GetDoctorDetails(Guid Id)
        {
            var result = await _unitOfWork.Repository<Doctor>().GetResultForOne(
                filter: a => a.Id == Id &&
                        a.CurrentState == CurrentState.Active,
                selectors : a => new DoctorInfoDTO
                {
                    SpecializationId = a.SpecializationId,
                    SpecializationName = a.Specialization.EName,
                    MaximumAppointments = a.MaximumAppointments,
                    Name = a.Name,
                    Description = a.Description,
                    PhoneNumber = a.PhoneNumber,
                    DoctorSchedules = a.DoctorSchedules.Where(a => a.CurrentState == CurrentState.Active).Select(a => new AppointmentDocSchedule
                    {
                        DoctorId = a.DoctorId,
                        ScheduleId = a.ScheduleId,
                        Day = a.Schedule.Day,
                        StartTime = a.Schedule.StartTime,
                        EndTime = a.Schedule.EndTime,
                    }).ToList(),
                }
            );

            return result;
        }

        public async Task<PageResult<DoctorInfoDTO>> GetPageDoctors(int pageNumber = 1 , int PageSize = 10)
        {
            var result = await _unitOfWork.Repository<Doctor>().GetPageResult(
                pageNumber,
                PageSize,
                filter: a => a.CurrentState == CurrentState.Active,
                selectors: a => new DoctorInfoDTO
                {
                    Id = a.Id,
                    SpecializationId = a.SpecializationId,
                    SpecializationName = a.Specialization.EName,
                    MaximumAppointments = a.MaximumAppointments,
                    Name = a.Name,
                    Description = a.Description,
                    PhoneNumber = a.PhoneNumber,
                },
                orderby: a => a.CreatedDate,
                isDesending: true,
                includers: a => a.Specialization
            );

            return result;
        }

        public async Task<DoctorInfoDTO> DoctorProfile()
        {
            string userId = _userService.GetLoggedInUser().ToString();

            var doctor = await _unitOfWork.Repository<Doctor>().GetFirstOrDefault(a => a.UserId == userId);

            if ( doctor == null )
            {
                return null;
            }

            var result = await _unitOfWork.Repository<Doctor>().GetResultForOne(
                filter: a => a.Id == doctor.Id &&
                        a.CurrentState == CurrentState.Active,
                selectors: a => new DoctorInfoDTO
                {
                    SpecializationId = a.SpecializationId,
                    SpecializationName = a.Specialization.EName,
                    MaximumAppointments= a.MaximumAppointments,
                    Name = a.Name,
                    Description = a.Description,
                    PhoneNumber = a.PhoneNumber,
                    DoctorSchedules = a.DoctorSchedules.Where(a => a.CurrentState == CurrentState.Active).Select(a => new AppointmentDocSchedule
                    {
                        DoctorId = a.DoctorId,
                        ScheduleId = a.ScheduleId,
                        Day = a.Schedule.Day,
                        StartTime = a.Schedule.StartTime,
                        EndTime = a.Schedule.EndTime,
                    }).ToList(),
                }
            );

            return result;
        }

        public async Task<PageResult<DoctorInfoDTO>> SearchDoctors(SearchDoctor searchDoctor, int pageNumber = 1, int PageSize = 10)
        {
            var result = await _unitOfWork.Repository<Doctor>().GetPageResult(
                pageNumber,
                PageSize,
                filter: a =>
                            a.CurrentState == CurrentState.Active
                            && (string.IsNullOrEmpty(searchDoctor.Name)
                                || a.Name.Contains(searchDoctor.Name))

                            && (string.IsNullOrEmpty(searchDoctor.PhoneNumber)
                                || a.PhoneNumber.Contains(searchDoctor.PhoneNumber))

                            && (!searchDoctor.ScheduleId.HasValue
                                || a.DoctorSchedules.Any(ds => ds.ScheduleId == searchDoctor.ScheduleId && ds.CurrentState == CurrentState.Active))

                            && (!searchDoctor.SpecializationId.HasValue
                                || a.SpecializationId == searchDoctor.SpecializationId),
                selectors: a => new DoctorInfoDTO
                {
                    Id = a.Id,
                    SpecializationId = a.SpecializationId,
                    SpecializationName = a.Specialization.EName,
                    MaximumAppointments = a.MaximumAppointments,
                    Name = a.Name,
                    Description = a.Description,
                    PhoneNumber = a.PhoneNumber,
                },
                orderby: a => a.CreatedDate,
                isDesending: true,
                includers: a => a.Specialization
            );

            return result;
        }

        public async Task<(bool , Guid)> AddDoctor(DoctorDTO doctorDTO)
        {
            if (doctorDTO == null || doctorDTO.Id != Guid.Empty)
                return (false , Guid.Empty);

            try
            {
                await _unitOfWork.BeginTransactionAsync();  

                var doctorResult = await Add(doctorDTO);

                if (!doctorResult.Item1)
                {
                    await _unitOfWork.RollbackAsync();
                    return (false , Guid.Empty);
                }

                var schedules = doctorDTO.Schedules;

                foreach (var item in schedules)
                {
                    var scheduleResult = await _doctorScheduleService.Add(new DoctorScheduleDTO
                    {
                        DoctorId = doctorResult.Item2,
                        ScheduleId = item
                    });

                    if (!scheduleResult.Item1)
                    {
                        await _unitOfWork.RollbackAsync();
                        return (false , Guid.Empty);
                    }
                }

                await _unitOfWork.CommitAsync();
                return (true , doctorResult.Item2);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateDoctor(DoctorDTO doctorDTO)
        {
            if (doctorDTO == null || doctorDTO.Id == Guid.Empty)
                return false;

            var doctor = await _unitOfWork.Repository<Doctor>().GetResultForOne(
                filter: a => a.Id == doctorDTO.Id
                        && a.CurrentState == CurrentState.Active,
                selectors: a => new DoctorDTO
                {
                    Id = a.Id,
                    SpecializationId = a.SpecializationId,
                    Name = a.Name,
                    Description = a.Description,
                    PhoneNumber = a.PhoneNumber,
                    UserId = a.UserId,
                }
            );

            if (doctor == null)
                return false;

            var user = await _userService.GetUserByIdAsync(doctor.UserId);

            if (user == null)
                return false;

            var schedules = doctorDTO.Schedules;

            try
            {
                await _unitOfWork.BeginTransactionAsync(); 

                if ((doctor.Name != doctorDTO.Name) || (doctor.PhoneNumber != doctorDTO.PhoneNumber))
                {
                    var updateUserDTO = new UpdateUserDTO
                    {
                        FirstName = doctorDTO.Name,
                        LastName = string.Empty,
                        PhoneNumber = doctorDTO.PhoneNumber,
                        UserName = doctorDTO.Name.Replace(" " ,""),
                        UserId = user.Id.ToString()
                    };

                    var userResult = await _userService.UpdateUserAsync(updateUserDTO);

                    if (!userResult.success)
                    {
                        await _unitOfWork.RollbackAsync();
                        return false;
                    }
                }

                var oldSchedules = await _doctorScheduleService.GetDoctorSchedules(doctor.Id);
                var oldScheduleIds = oldSchedules.Select(a => a.ScheduleId).ToHashSet();
                var newScheduleIds = (schedules ?? Enumerable.Empty<Guid>()).ToHashSet();

                var scheduleToAdd = newScheduleIds.Except(oldScheduleIds);
                var scheduleToRemove = oldSchedules.Where(x => !newScheduleIds.Contains(x.ScheduleId)).ToList(); // *****

                foreach (var schedule in scheduleToRemove)
                {
                    var removeSchedule = await _doctorScheduleService.ChangeStatus(schedule.Id);

                    if (!removeSchedule)
                    {
                        await _unitOfWork.RollbackAsync();
                        return false;
                    }
                }

                foreach (var schedule in scheduleToAdd)
                {
                    var addSchedule = await _doctorScheduleService.Add(new DoctorScheduleDTO
                    {
                        DoctorId = doctor.Id,
                        ScheduleId = schedule
                    });

                    if (!addSchedule.Item1)
                    {
                        await _unitOfWork.RollbackAsync();
                        return false;
                    }
                }

                var doctorResult = await Update(doctorDTO);

                if (!doctorResult)
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

        public async Task<DoctorDTO> GetDocDTO(string userId)
        {
            var doctorDTO = new DoctorDTO();

            var user = await _userService.GetUserByIdAsync(userId);

            doctorDTO.UserId = userId;
            doctorDTO.PhoneNumber = user.PhoneNumber;
            doctorDTO.Name = user.FirstName + " " + user.LastName;

            return doctorDTO;
        }

        public async Task<Guid> GetDoctorId()
        {
            var userId = _userService.GetLoggedInUser().ToString();

            var doctor = await _unitOfWork.Repository<Doctor>().GetResultForOne(
                filter : a => a.UserId == userId && a.CurrentState == CurrentState.Active ,
                selectors : a => new DoctorInfoDTO
                {
                    Id = a.Id,
                }
            );

            return doctor.Id;
        }
    }
}
