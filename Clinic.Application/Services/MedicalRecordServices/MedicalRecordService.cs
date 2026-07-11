using AutoMapper;
using Clinic.Application.Contracts.DoctorContract;
using Clinic.Application.Contracts.MedicalRecordContracts;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.AppointmentDTOs;
using Clinic.Application.DTOs.DoctorDTOs;
using Clinic.Application.DTOs.MedicalRecordDTOs;
using Clinic.Application.DTOs.PatientDTOs;
using Clinic.Core.Entities;
using Clinic.Core.Enums;
using Clinic.Infrastructure.Contracts;
using Clinic.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Services.MedicalRecordServices
{
    public class MedicalRecordService : BaseService<MedicalRecord, MedicalRecordDTO>, IMedicalRecordService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IDoctorService _doctorService;

        public MedicalRecordService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService
            , IDoctorService doctorService)
            : base(unitOfWork, mapper, userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _doctorService = doctorService;
        }

        public async Task<List<DocMedicalRecord>> GetDocMedicalRecords()
        {
            var doctorId = await _doctorService.GetDoctorId();

            if (doctorId == Guid.Empty)
            {
                return null;
            }

            var result = await _unitOfWork.Repository<MedicalRecord>().GetResult(
                filter: a => a.DoctorId == doctorId &&
                        a.CurrentState == CurrentState.Active,
                selectors: a => new DocMedicalRecord
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    PatientId = a.PatientId,
                    DoctorName = a.Doctor.Name,
                    PatientName = a.Patient.FirstName + " " + a.Patient.LastName,
                },
                orderby: a => a.CreatedDate,
                includers: a => a.Patient
            );

            return result;
        }

        public async Task<PageResult<DocMedicalRecord>> GetDocPageMedicalRecords(SearchPatient searchPatient, int pageNumber = 1, int pageSize = 10)
        {
            var doctorId = await _doctorService.GetDoctorId();

            if (doctorId == Guid.Empty)
            {
                return null;
            }

            var result = await _unitOfWork.Repository<MedicalRecord>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.DoctorId == doctorId
                        && a.CurrentState == CurrentState.Active
                        && (string.IsNullOrEmpty(searchPatient.Name)
                                || (a.Patient.FirstName + a.Patient.MiddleName + a.Patient.LastName).Contains(searchPatient.Name))

                            && (string.IsNullOrEmpty(searchPatient.PhoneNumber)
                                || a.Patient.PhoneNumber.Contains(searchPatient.PhoneNumber))

                            && (string.IsNullOrEmpty(searchPatient.NationalNumber)
                                || a.Patient.NationalNumber == searchPatient.NationalNumber),
                selectors: a => new DocMedicalRecord
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.Name,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FirstName + " " + a.Patient.MiddleName + " " + a.Patient.LastName,
                    NationalNumber = a.Patient.NationalNumber,
                    PhoneNumber = a.Patient.PhoneNumber
                },
                orderby: a => a.CreatedDate,
                isDesending: true,
                includers: a => a.Patient
            );

            return result;
        }

        public async Task<DocMedicalRecord> MedicalRecordDetails(Guid patientId)
        {
            var doctorId = await _doctorService.GetDoctorId();

            if (doctorId == Guid.Empty)
            {
                return null;
            }

            var result = await _unitOfWork.Repository<MedicalRecord>().GetResultForOne(
                filter: a => a.PatientId == patientId &&
                        a.DoctorId == doctorId &&
                        a.CurrentState == CurrentState.Active,
                selectors: a => new DocMedicalRecord
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FirstName + " " + a.Patient.MiddleName + " " + a.Patient.LastName,
                    NationalNumber = a.Patient.NationalNumber,
                    BloodType = a.Patient.BloodType,
                    Gender = a.Patient.Gender,
                    BirthDate = a.Patient.BirthDate,
                    PhoneNumber = a.Patient.PhoneNumber,
                    Email = a.Patient.Email,
                    Allergy = a.Allergy,
                    Diagnosis = a.Diagnosis,
                    ChronicDisease = a.ChronicDisease,
                    CurrentMedications = a.CurrentMedications,
                    DoctorName = a.Doctor.Name,
                    Notes = a.Notes
                },
                includers: a => a.Patient
            );

            return result;
        }

        public async Task<DocMedicalRecord> MedicalRecordDetailsForUser(Guid Id)
        {
            var result = await _unitOfWork.Repository<MedicalRecord>().GetResultForOne(
                filter: a => a.Id == Id &&
                        a.CurrentState == CurrentState.Active,
                selectors: a => new DocMedicalRecord
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FirstName + " " + a.Patient.MiddleName + " " + a.Patient.LastName,
                    NationalNumber = a.Patient.NationalNumber,
                    BloodType = a.Patient.BloodType,
                    Gender = a.Patient.Gender,
                    BirthDate = a.Patient.BirthDate,
                    PhoneNumber = a.Patient.PhoneNumber,
                    Email = a.Patient.Email,
                    Allergy = a.Allergy,
                    Diagnosis = a.Diagnosis,
                    ChronicDisease = a.ChronicDisease,
                    CurrentMedications = a.CurrentMedications,
                    DoctorName = a.Doctor.Name,
                    Notes = a.Notes
                },
                includers: a => a.Patient
            );

            return result;
        }

        public async Task<List<MedicalRecordDTO>> GetPatientMedicalRecords(Guid patientId)
        {
            var result = await _unitOfWork.Repository<MedicalRecord>().GetResult(
                filter: a => a.CurrentState == CurrentState.Active
                        && a.PatientId == patientId,
                selectors: a => new MedicalRecordDTO
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId
                },
                orderby: a => a.CreatedDate,
                isDesending: true
            );

            return result;
        }

        public async Task<List<MedicalRecordDTO>> GetMedicalRecordsForUser()
        {
            var userId = _userService.GetLoggedInUser().ToString();

            var patient = await _unitOfWork.Repository<Patient>()
                .GetFirstOrDefault(a => a.CurrentState == CurrentState.Active
                    && a.UserId == userId
                );

            var result = await _unitOfWork.Repository<MedicalRecord>().GetResult(
                filter: a => a.CurrentState == CurrentState.Active
                        && a.PatientId == patient.Id,
                selectors: a => new MedicalRecordDTO
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId
                },
                orderby: a => a.CreatedDate,
                isDesending: true
            );

            return result;
        }

        public async Task<MedicalRecordDTO> GetDocPatientMedicalRecords(Guid PatientId)
        {
            var doctorId = await _doctorService.GetDoctorId();

            if (doctorId == Guid.Empty)
            {
                return null;
            }

            var result = await _unitOfWork.Repository<MedicalRecord>().GetResultForOne(
                filter: a => a.CurrentState == CurrentState.Active
                        && a.PatientId == PatientId
                        && a.DoctorId == doctorId,
                selectors: a => new MedicalRecordDTO
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId
                }
            );

            return result;
        }
    }
}
