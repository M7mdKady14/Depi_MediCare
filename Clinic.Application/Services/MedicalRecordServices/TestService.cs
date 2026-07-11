using AutoMapper;
using Clinic.Application.Contracts.DoctorContract;
using Clinic.Application.Contracts.MedicalRecordContracts;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.MedicalRecordDTOs;
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
    public class TestService : BaseService<Test, TestDTO>, ITestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IDoctorService _doctorService;

        public TestService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService, IDoctorService doctorService)
            : base(unitOfWork, mapper, userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _doctorService = doctorService;
        }

        public async Task<PageResult<TestInfoDTO>> GetPageTests(Guid medicalRecordId, int pageNumber = 1, int pageSize = 10)
        {
            var doctorId = await _doctorService.GetDoctorId();

            var result = await _unitOfWork.Repository<Test>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.MedicalRecordId == medicalRecordId
                        && a.CurrentState == CurrentState.Active
                        && a.MedicalRecord.DoctorId == doctorId,
                selectors: a => new TestInfoDTO
                {
                    Id = a.Id,
                    File = a.File,
                    MedicalRecordId = a.MedicalRecordId,
                    CurrentState = a.CurrentState,
                    Diagnosis = a.MedicalRecord.Diagnosis,
                    DoctorId = a.MedicalRecord.DoctorId,
                    Description = a.Description,
                    PatientName = a.MedicalRecord.Patient.FirstName + " " + a.MedicalRecord.Patient.MiddleName + " " + a.MedicalRecord.Patient.LastName,
                },
                orderby: a => a.CreatedDate,
                isDesending: true,
                includers: a => a.MedicalRecord
            );

            return result;
        }

        public async Task<List<TestInfoDTO>> GetTests(Guid medicalRecordId)
        {
            var doctorId = await _doctorService.GetDoctorId();

            var result = await _unitOfWork.Repository<Test>().GetResult(
                filter: a => a.MedicalRecordId == medicalRecordId
                        && a.CurrentState == CurrentState.Active
                        && a.MedicalRecord.DoctorId == doctorId,
                selectors: a => new TestInfoDTO
                {
                    Id = a.Id,
                    File = a.File,
                    MedicalRecordId = a.MedicalRecordId,
                    CurrentState = a.CurrentState,
                    DoctorId = a.MedicalRecord.DoctorId,
                    Diagnosis = a.MedicalRecord.Diagnosis,
                    Description = a.Description,
                    PatientName = a.MedicalRecord.Patient.FirstName + " " + a.MedicalRecord.Patient.MiddleName + " " + a.MedicalRecord.Patient.LastName,
                },
                orderby: a => a.CreatedDate,
                isDesending: true,
                includers: a => a.MedicalRecord
            );

            return result;
        }

        public async Task<(bool , Guid)> DeleteTest(Guid Id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var test = await _unitOfWork.Repository<Test>().GetResultForOne(
                    filter: a => a.CurrentState == CurrentState.Active
                            && a.Id == Id,
                    selectors : a => new
                    {
                        Id = a.Id,
                        patientId = a.MedicalRecord.PatientId
                    }
                );

                if ( test == null )
                {
                    await _unitOfWork.RollbackAsync();
                    return (false, Guid.Empty);
                }

                var result = await ChangeStatus(Id);

                if (!result)
                {
                    await _unitOfWork.RollbackAsync();
                    return (false , Guid.Empty);
                }
                
                await _unitOfWork.CommitAsync();
                return (true , test.patientId);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
