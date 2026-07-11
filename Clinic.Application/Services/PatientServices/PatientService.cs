using AutoMapper;
using Clinic.Application.Contracts.DoctorContract;
using Clinic.Application.Contracts.MedicalRecordContracts;
using Clinic.Application.Contracts.PatientContract;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.AppointmentDTOs;
using Clinic.Application.DTOs.DoctorDTOs;
using Clinic.Application.DTOs.MedicalRecordDTOs;
using Clinic.Application.DTOs.PatientDTOs;
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

namespace Clinic.Application.Services.PatientService
{
    public class PatientService : BaseService<Patient, PatientDTO>, IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IDoctorService _doctorService;

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService , IMedicalRecordService medicalRecordService , IDoctorService doctorService) 
            : base(unitOfWork, mapper, userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _medicalRecordService = medicalRecordService;
            _doctorService = doctorService;
        }


        public async Task<PageResult<PatientInfoDTO>> GetPagePatients(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _unitOfWork.Repository<Patient>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.CurrentState == CurrentState.Active,
                selectors: a => new PatientInfoDTO
                {
                    Id = a.Id,
                    PhoneNumber = a.PhoneNumber,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    MiddleName = a.MiddleName,
                    BloodType = a.BloodType,
                    NationalNumber = a.NationalNumber,
                    Email = a.Email,
                    BirthDate = a.BirthDate,
                    Gender = a.Gender,
                },
                orderby: a => a.CreatedDate,
                isDesending: true
            );

            return result;
        }

        public async Task<PageResult<PatientInfoDTO>> SearchPatient(SearchPatient searchPatient , int pageNumber = 1, int pageSize = 10)
        {
            var result = await _unitOfWork.Repository<Patient>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.CurrentState == CurrentState.Active
                 && (string.IsNullOrEmpty(searchPatient.Name)
                                || (a.FirstName + a.MiddleName + a.LastName).Contains(searchPatient.Name))

                            && (string.IsNullOrEmpty(searchPatient.PhoneNumber)
                                || a.PhoneNumber.Contains(searchPatient.PhoneNumber))

                            && (string.IsNullOrEmpty(searchPatient.NationalNumber)
                                || a.NationalNumber == searchPatient.NationalNumber),
                selectors: a => new PatientInfoDTO
                {
                    Id = a.Id,
                    PhoneNumber = a.PhoneNumber,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    MiddleName = a.MiddleName,
                    BloodType = a.BloodType,
                    NationalNumber = a.NationalNumber,
                    Email = a.Email,
                    BirthDate = a.BirthDate,
                    Gender = a.Gender,
                },
                orderby: a => a.CreatedDate,
                isDesending: true
            );

            return result;
        }

        public async Task<PageResult<PatientInfoDTO>> SearchDocPatient(SearchPatient searchPatient, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _unitOfWork.Repository<Patient>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.CurrentState == CurrentState.Active
                 && (string.IsNullOrEmpty(searchPatient.Name)
                                || (a.FirstName + a.MiddleName + a.LastName).Contains(searchPatient.Name))

                            && (string.IsNullOrEmpty(searchPatient.PhoneNumber)
                                || a.PhoneNumber.Contains(searchPatient.PhoneNumber))

                            && (string.IsNullOrEmpty(searchPatient.NationalNumber)
                                || a.NationalNumber == searchPatient.NationalNumber),
                selectors: a => new PatientInfoDTO
                {
                    Id = a.Id,
                    PhoneNumber = a.PhoneNumber,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    MiddleName = a.MiddleName,
                    BloodType = a.BloodType,
                    NationalNumber = a.NationalNumber,
                    Email = a.Email,
                    BirthDate = a.BirthDate,
                    Gender = a.Gender,
                },
                orderby: a => a.CreatedDate,
                isDesending: true
            );

            return result;
        }

        public async Task<Guid> GetPatientId()
        {
            var userId = _userService.GetLoggedInUser().ToString();

            var patient = await _unitOfWork.Repository<Patient>()
                .GetFirstOrDefault(a => a.CurrentState == CurrentState.Active
                                    && a.UserId == userId
                );

            return patient.Id;
        }

        public async Task<PatientInfoDTO> GetPatientDetails(Guid Id)
        {
            var result = await _unitOfWork.Repository<Patient>().GetResultForOne(
                filter: a => a.CurrentState == CurrentState.Active
                        && a.Id == Id,
                selectors: a => new PatientInfoDTO
                {
                    Id = a.Id,
                    PhoneNumber = a.PhoneNumber,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    MiddleName = a.MiddleName,
                    BloodType = a.BloodType,
                    NationalNumber = a.NationalNumber,
                    Email = a.Email,
                    BirthDate = a.BirthDate,
                    Gender = a.Gender,
                }
            );

            return result;
        }

        public async Task<(bool, Guid)> AddPatient(PatientDTO patientDTO)
        {
            if (patientDTO.Id != Guid.Empty)
            {
                return (false, Guid.Empty);
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var result = await Add(patientDTO);

                if (!result.Item1)
                {
                    await _unitOfWork.RollbackAsync();
                    return (false, Guid.Empty);
                }

                await _unitOfWork.CommitAsync();

                return (true, result.Item2);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdatePatient(PatientDTO patientDTO)
        {
            var userId = _userService.GetLoggedInUser().ToString();

            var user = await _userService.GetUserByIdAsync(userId);

            if (user.Role != "Doctor")
            {
                return false;
            }

            if (patientDTO.Id == Guid.Empty)
            {
                return false;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var result = await Update(patientDTO);

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

        public async Task<bool> DeletePatient(Guid Id)
        {
            var patient = await _unitOfWork.Repository<Patient>().GetResultForOne(
                filter: a => a.CurrentState == CurrentState.Active 
                        && a.Id == Id,
                selectors: a => new PatientDTO
                {
                    Id = Id,
                }
            );

            if ( patient == null )
            {
                return false;
            }

            var userId = _userService.GetLoggedInUser().ToString();

            var user = await _userService.GetUserByIdAsync(userId);

            if (user.Role != "Doctor")
            {
                return false;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var medicalRecords = await _unitOfWork.Repository<MedicalRecord>().GetResult(
                    filter: a => a.CurrentState == CurrentState.Active
                            && a.PatientId == Id,
                    selectors: a => new MedicalRecordDTO
                    {
                        Id = a.Id,
                    },
                    orderby: a => a.CreatedDate,
                    isDesending: true
                );

                if (medicalRecords.Any())
                {
                    foreach (var record in medicalRecords)
                    {
                        var result = await _unitOfWork.Repository<MedicalRecord>().ChangeStatus(record.Id, Guid.Parse(userId));
                    }
                }

                var appointments = await _unitOfWork.Repository<Appointment>().GetResult(
                    filter: a => a.CurrentState == CurrentState.Active
                            && a.PatientId == Id,
                    selectors: a => new AppointmentDTO
                    {
                        Id = a.Id,
                    },
                    orderby: a => a.CreatedDate,
                    isDesending: true
                );

                if (appointments.Any())
                {
                    foreach (var appointment in appointments)
                    {
                        var result = await _unitOfWork.Repository<Appointment>().ChangeStatus(appointment.Id , Guid.Parse(userId));
                    }
                }

                var prescriptions = await _unitOfWork.Repository<Prescription>().GetResult(
                    filter: a => a.CurrentState == CurrentState.Active
                            && a.Appointment.PatientId == Id,
                    selectors: a => new PrescriptionDTO
                    {
                        Id = Id,
                    },
                    orderby: a => a.CreatedDate,
                    isDesending: true
                );

                if (prescriptions.Any())
                {
                    foreach (var appointment in appointments)
                    {
                        var result = await _unitOfWork.Repository<Appointment>().ChangeStatus(appointment.Id, Guid.Parse(userId));
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
