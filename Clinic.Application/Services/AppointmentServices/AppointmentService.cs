using AutoMapper;
using Clinic.Application.Contracts.AppointmentContracts;
using Clinic.Application.Contracts.DoctorContract;
using Clinic.Application.Contracts.MedicalRecordContracts;
using Clinic.Application.Contracts.PatientContract;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.AppointmentDTOs;
using Clinic.Application.DTOs.DoctorDTOs;
using Clinic.Application.DTOs.MedicalRecordDTOs;
using Clinic.Application.Helper;
using Clinic.Core.Entities;
using Clinic.Core.Enums;
using Clinic.Infrastructure.Contracts;
using Clinic.Infrastructure.Models;

namespace Clinic.Application.Services.AppointmentServices
{
    public class AppointmentService : BaseService<Appointment, AppointmentDTO>, IAppointmentService
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        #endregion

        #region Constructor
        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService
            , IMedicalRecordService medicalRecordService , IDoctorService doctorService , IPatientService patientService) 
            : base(unitOfWork, mapper, userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _medicalRecordService = medicalRecordService;
            _doctorService = doctorService;
            _patientService = patientService;
        }
        #endregion

        #region Get Methods
        public async Task<PageResult<AppointmentInfoDTO>> GetPageAppointments(int pageNumber = 1 , int pageSize = 10)
        {
            var result = await _unitOfWork.Repository<Appointment>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.CurrentState == CurrentState.Active,
                selectors: a => new AppointmentInfoDTO
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    PatientId = a.PatientId,
                    AppointmentTypeId = a.AppointmentTypeId,
                    DoctorName = a.Doctor.Name,
                    PatientName = a.Patient.FirstName + a.Patient.MiddleName +  a.Patient.LastName,
                    AppointmentTypeName = a.AppointmentType.EName,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentStatus = a.AppointmentStatus,
                },
                orderby: a => a.CreatedDate,
                isDesending: true
            );

            return result;
        }
        
        public async Task<PageResult<AppointmentInfoDTO>> GetPatientAppointments(Guid patientId, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _unitOfWork.Repository<Appointment>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.CurrentState == CurrentState.Active
                        && a.PatientId == patientId,
                selectors: a => new AppointmentInfoDTO
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    PatientId = a.PatientId,
                    AppointmentTypeId = a.AppointmentTypeId,
                    DoctorName = a.Doctor.Name,
                    PatientName = a.Patient.FirstName + a.Patient.MiddleName + a.Patient.LastName,
                    AppointmentTypeName = a.AppointmentType.EName,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentStatus = a.AppointmentStatus,
                },
                orderby: a => a.CreatedDate,
                isDesending: true
            );

            return result;
        }
        
        public async Task<PageResult<AppointmentInfoDTO>> GetMedicalRecordAppointments(Guid patientId, int pageNumber = 1, int pageSize = 10)
        {
            var doctorId = await _doctorService.GetDoctorId();

            var result = await _unitOfWork.Repository<Appointment>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.CurrentState == CurrentState.Active
                        && a.PatientId == patientId
                        && a.DoctorId == doctorId,
                selectors: a => new AppointmentInfoDTO
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    PatientId = a.PatientId,
                    AppointmentTypeId = a.AppointmentTypeId,
                    DoctorName = a.Doctor.Name,
                    PatientName = a.Patient.FirstName + a.Patient.MiddleName + a.Patient.LastName,
                    AppointmentTypeName = a.AppointmentType.EName,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentStatus = a.AppointmentStatus,
                },
                orderby: a => a.CreatedDate,
                isDesending: true
            );

            return result;
        }

        public async Task<AppointmentInfoDTO> GetAppointmentDetails(Guid appointmentId)
        {
            var result = await _unitOfWork.Repository<Appointment>().GetResultForOne(
                filter: a => a.CurrentState == CurrentState.Active
                        && a.Id == appointmentId,
                selectors: a => new AppointmentInfoDTO
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    PatientId = a.PatientId,
                    AppointmentTypeId = a.AppointmentTypeId,
                    DoctorName = a.Doctor.Name,
                    PatientName = a.Patient.FirstName + " " + a.Patient.MiddleName + " " + a.Patient.LastName,
                    AppointmentTypeName = a.AppointmentType.EName,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentStatus = a.AppointmentStatus,
                    Day = a.DoctorSchedule.Schedule.Day,
                    StartTime = a.DoctorSchedule.Schedule.StartTime,
                    EndTime = a.DoctorSchedule.Schedule.EndTime
                }
            );

            return result;
        }

        public async Task<PageResult<AppointmentInfoDTO>> GetDoctorAppointments(DoctorAppointmentDTO docAppointmentDTO, int pageNumber = 1, int pageSize = 10)
        {
            var date = docAppointmentDTO.Date;

            if (date == null)
            {
                date = DateOnly.FromDateTime(DateTime.UtcNow);
            }

            var result = await _unitOfWork.Repository<Appointment>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.CurrentState == CurrentState.Active
                        && a.DoctorId == docAppointmentDTO.DoctorId
                        && (!date.HasValue
                                || a.AppointmentDate == date.Value),
                            selectors: a => new AppointmentInfoDTO
                            {
                                Id = a.Id,
                                DoctorId = a.DoctorId,
                                PatientId = a.PatientId,
                                AppointmentTypeId = a.AppointmentTypeId,
                                DoctorName = a.Doctor.Name,
                                PatientName = a.Patient.FirstName + " " + a.Patient.MiddleName + " " + a.Patient.LastName,
                                AppointmentTypeName = a.AppointmentType.EName,
                                AppointmentDate = a.AppointmentDate,
                                AppointmentStatus = a.AppointmentStatus,
                            }
            );
            return result;
        }

        private async Task<AppointmentDTO> GetAppointmentById (Guid AppointmentId)
        {
            var appointment = await _unitOfWork.Repository<Appointment>().GetResultForOne(
                filter: a => a.CurrentState == CurrentState.Active
                        && a.Id == AppointmentId,
                selectors: a => new AppointmentDTO
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    AppointmentStatus = a.AppointmentStatus,
                    DoctorScheduleId = a.DoctorScheduleId,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTypeId = a.AppointmentTypeId
                }
            );

            return appointment;
        }
        #endregion

        #region Search Appointments
        public async Task<PageResult<AppointmentInfoDTO>> SearchAppointments(SearchAppointment searchAppointment, int pageNumber = 1, int pageSize = 10)
        {
            var startDate = searchAppointment.DateStart;
            var endDate = searchAppointment.DateEnd;

            if (startDate != null && endDate == null)
            {
                endDate = DateOnly.FromDateTime(DateTime.UtcNow);
            }

            var result = await _unitOfWork.Repository<Appointment>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.CurrentState == CurrentState.Active
                        && (!searchAppointment.DoctorId.HasValue
                                || a.DoctorId == searchAppointment.DoctorId)
                        && (!searchAppointment.DoctorScheduleId.HasValue
                                || a.DoctorScheduleId == searchAppointment.DoctorScheduleId)
                        && (!searchAppointment.AppointmentTypeId.HasValue
                                || a.AppointmentTypeId == searchAppointment.AppointmentTypeId)
                        && (!searchAppointment.AppointmentStatus.HasValue
                                || a.AppointmentStatus == searchAppointment.AppointmentStatus)
                        && (string.IsNullOrEmpty(searchAppointment.PatientNationalNumber)
                                || a.Patient.NationalNumber == searchAppointment.PatientNationalNumber)
                        && (string.IsNullOrEmpty(searchAppointment.PatientPhoneNumber)
                                || a.Patient.PhoneNumber == searchAppointment.PatientPhoneNumber)
                        && (string.IsNullOrEmpty(searchAppointment.PatientName)
                                || (a.Patient.FirstName + " " + a.Patient.MiddleName + " " + a.Patient.LastName)
                                        .Contains(searchAppointment.PatientName))
                        && (!startDate.HasValue
                                || a.AppointmentDate >= startDate.Value)
                        && (!endDate.HasValue
                                || a.AppointmentDate <= endDate.Value),
                            selectors: a => new AppointmentInfoDTO
                            {
                                Id = a.Id,
                                DoctorId = a.DoctorId,
                                PatientId = a.PatientId,
                                AppointmentTypeId = a.AppointmentTypeId,
                                DoctorName = a.Doctor.Name,
                                PatientName = a.Patient.FirstName + " " + a.Patient.MiddleName + " " + a.Patient.LastName,
                                AppointmentTypeName = a.AppointmentType.EName,
                                AppointmentDate = a.AppointmentDate,
                                AppointmentStatus = a.AppointmentStatus,
                            }
            );

            return result;
        }

        public async Task<PageResult<AppointmentInfoDTO>> ReceptionSearchAppointments(ReceptionSearchAppointment searchAppointment, int pageNumber = 1, int pageSize = 10)
        {
            var startDate = searchAppointment.DateStart;
            var endDate = searchAppointment.DateEnd;

            if (startDate != null && endDate == null)
            {
                endDate = DateOnly.FromDateTime(DateTime.UtcNow);
            }

            var result = await _unitOfWork.Repository<Appointment>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.CurrentState == CurrentState.Active
                        && (!searchAppointment.DoctorId.HasValue
                                || a.DoctorId == searchAppointment.DoctorId)
                        && (!searchAppointment.AppointmentTypeId.HasValue
                                || a.AppointmentTypeId == searchAppointment.AppointmentTypeId)
                        && (!searchAppointment.AppointmentStatus.HasValue
                                || a.AppointmentStatus == searchAppointment.AppointmentStatus)
                        && (string.IsNullOrEmpty(searchAppointment.PatientNationalNumber)
                                || a.Patient.NationalNumber == searchAppointment.PatientNationalNumber)
                        && (string.IsNullOrEmpty(searchAppointment.PatientPhoneNumber)
                                || a.Patient.PhoneNumber.Contains(searchAppointment.PatientPhoneNumber))
                        && (string.IsNullOrEmpty(searchAppointment.PatientName)
                                || (a.Patient.FirstName + " " + a.Patient.MiddleName + " " + a.Patient.LastName)
                                        .Contains(searchAppointment.PatientName))
                        && (!startDate.HasValue
                                || a.AppointmentDate >= startDate.Value)
                        && (!endDate.HasValue
                                || a.AppointmentDate <= endDate.Value),
                            selectors: a => new AppointmentInfoDTO
                            {
                                Id = a.Id,
                                DoctorId = a.DoctorId,
                                PatientId = a.PatientId,
                                AppointmentTypeId = a.AppointmentTypeId,
                                DoctorName = a.Doctor.Name,
                                PatientName = a.Patient.FirstName + " " + a.Patient.MiddleName + " " + a.Patient.LastName,
                                AppointmentTypeName = a.AppointmentType.EName,
                                AppointmentDate = a.AppointmentDate,
                                AppointmentStatus = a.AppointmentStatus,
                            }
            );

            return result;
        }

        public async Task<PageResult<AppointmentInfoDTO>> DoctorSearchAppointments(DoctorSearchAppointment searchAppointment, int pageNumber = 1, int pageSize = 10)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var doctorId = await _doctorService.GetDoctorId();

            var result = await _unitOfWork.Repository<Appointment>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.CurrentState == CurrentState.Active
                        && a.DoctorId == doctorId
                        && (!searchAppointment.DoctorScheduleId.HasValue
                                || a.DoctorScheduleId == searchAppointment.DoctorScheduleId)
                        && (!searchAppointment.AppointmentTypeId.HasValue
                                || a.AppointmentTypeId == searchAppointment.AppointmentTypeId)
                        && (!searchAppointment.AppointmentStatus.HasValue
                                || a.AppointmentStatus == searchAppointment.AppointmentStatus)
                        && (string.IsNullOrEmpty(searchAppointment.PatientNationalNumber)
                                || a.Patient.NationalNumber == searchAppointment.PatientNationalNumber)
                        && (string.IsNullOrEmpty(searchAppointment.PatientPhoneNumber)
                                || a.Patient.PhoneNumber.Contains(searchAppointment.PatientPhoneNumber))
                        && (string.IsNullOrEmpty(searchAppointment.PatientName)
                                || (a.Patient.FirstName + " " + a.Patient.MiddleName + " " + a.Patient.LastName)
                                        .Contains(searchAppointment.PatientName))
                        && ((!searchAppointment.Date.HasValue) ? a.AppointmentDate == today :
                                a.AppointmentDate == searchAppointment.Date.Value),
                            selectors: a => new AppointmentInfoDTO
                            {
                                Id = a.Id,
                                DoctorId = a.DoctorId,
                                PatientId = a.PatientId,
                                AppointmentTypeId = a.AppointmentTypeId,
                                DoctorName = a.Doctor.Name,
                                PatientName = a.Patient.FirstName + " " + a.Patient.MiddleName + " " + a.Patient.LastName,
                                AppointmentTypeName = a.AppointmentType.EName,
                                AppointmentDate = a.AppointmentDate,
                                AppointmentStatus = a.AppointmentStatus,
                            }
            );

            return result;
        }

        public async Task<PageResult<AppointmentInfoDTO>> PatientSearchAppointments(PatientSearchAppointment searchAppointment, int pageNumber = 1, int pageSize = 10)
        {
            var patientId = await _patientService.GetPatientId();

            var startDate = searchAppointment.DateStart;
            var endDate = searchAppointment.DateEnd;

            if (startDate != null && endDate == null)
            {
                endDate = DateOnly.FromDateTime(DateTime.UtcNow);
            }

            var result = await _unitOfWork.Repository<Appointment>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.CurrentState == CurrentState.Active
                        && a.PatientId == patientId
                        && (!searchAppointment.DoctorId.HasValue
                                || a.DoctorId == searchAppointment.DoctorId)
                        && (!searchAppointment.AppointmentTypeId.HasValue
                                || a.AppointmentTypeId == searchAppointment.AppointmentTypeId)
                        && (!searchAppointment.AppointmentStatus.HasValue
                                || a.AppointmentStatus == searchAppointment.AppointmentStatus)
                        && (!startDate.HasValue
                                || a.AppointmentDate >= startDate.Value)
                        && (!endDate.HasValue
                                || a.AppointmentDate <= endDate.Value),
                            selectors: a => new AppointmentInfoDTO
                            {
                                Id = a.Id,
                                DoctorId = a.DoctorId,
                                PatientId = a.PatientId,
                                AppointmentTypeId = a.AppointmentTypeId,
                                DoctorName = a.Doctor.Name,
                                PatientName = a.Patient.FirstName + " " + a.Patient.MiddleName + " " + a.Patient.LastName,
                                AppointmentTypeName = a.AppointmentType.EName,
                                AppointmentDate = a.AppointmentDate,
                                AppointmentStatus = a.AppointmentStatus,
                            }
            );

            return result;
        }

        #endregion

        #region Create && Update
        public async Task<(bool, Guid)> CreateAppointment(AppointmentDTO appointmentDTO)
        {
            var doctor = await _unitOfWork
                .Repository<Doctor>()
                .GetFirstOrDefault(d => d.Id == appointmentDTO.DoctorId && d.CurrentState == CurrentState.Active);

            if (doctor == null)
            {
                return (false, Guid.Empty);
            }

            var doctorSchedule = await _unitOfWork
                .Repository<DoctorSchedule>()
                .GetFirstOrDefault(ds =>
                    ds.Id == appointmentDTO.DoctorScheduleId &&
                    ds.DoctorId == appointmentDTO.DoctorId
                );

            if (doctorSchedule == null)
            {
                return (false, Guid.Empty);
            }

            if (appointmentDTO.AppointmentDate < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                return (false, Guid.Empty);
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var appointmentDate = appointmentDTO.AppointmentDate;

                var todayAppointments = await _unitOfWork
                    .Repository<Appointment>()
                    .GetList(a =>
                        a.DoctorScheduleId == appointmentDTO.DoctorScheduleId &&
                        a.CurrentState == CurrentState.Active &&
                        a.AppointmentDate == appointmentDate
                    );

                if (todayAppointments.Count >= doctor.MaximumAppointments)
                {
                    await _unitOfWork.RollbackAsync();
                    return (false, Guid.Empty);
                }

                var patientMedicalRecords = await _medicalRecordService.GetPatientMedicalRecords(appointmentDTO.PatientId);

                var hasMedicalRecord = patientMedicalRecords.Any(r => r.DoctorId == appointmentDTO.DoctorId);

                if (!hasMedicalRecord)
                {
                    var medicalRecordDto = new MedicalRecordDTO
                    {
                        DoctorId = appointmentDTO.DoctorId,
                        PatientId = appointmentDTO.PatientId,
                        Diagnosis = string.Empty,
                        ChronicDisease = string.Empty,
                        CurrentMedications = string.Empty,
                        Notes = string.Empty,
                        Allergy = string.Empty
                    };

                    var medicalRecordResult =
                        await _medicalRecordService.Add(medicalRecordDto);

                    if (!medicalRecordResult.Item1)
                    {
                        await _unitOfWork.RollbackAsync();
                        return (false, Guid.Empty);
                    }
                }

                appointmentDTO.AppointmentStatus = AppointmentStatus.Created;

                var appointmentResult = await Add(appointmentDTO);

                if (!appointmentResult.Item1)
                {
                    await _unitOfWork.RollbackAsync();
                    return (false, Guid.Empty);
                }

                await _unitOfWork.CommitAsync();

                return (true, appointmentResult.Item2);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        public async Task<bool> UpdateAppointment(AppointmentDTO appointmentDTO)
        {
            var dbAppointment = await _unitOfWork.Repository<Appointment>().GetResultForOne(
                filter : a => a.Id == appointmentDTO.Id 
                            && a.CurrentState == CurrentState.Active,
                selectors : a => new AppointmentDTO
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentStatus = a.AppointmentStatus,
                }
            );

            if (dbAppointment == null)
            {
                return false;
            }

            if (dbAppointment.AppointmentStatus == AppointmentStatus.Canceled || dbAppointment.AppointmentStatus == AppointmentStatus.Completed)
            {
                return false;
            }

            var doctor = await _unitOfWork
                .Repository<Doctor>()
                .GetFirstOrDefault(d => d.Id == appointmentDTO.DoctorId && d.CurrentState == CurrentState.Active);

            if (doctor == null)
            {
                return false;
            }

            var doctorSchedule = await _unitOfWork
                .Repository<DoctorSchedule>()
                .GetFirstOrDefault(ds =>
                    ds.Id == appointmentDTO.DoctorScheduleId &&
                    ds.DoctorId == appointmentDTO.DoctorId
                );

            if (doctorSchedule == null)
            {
                return false;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var appointmentDate = appointmentDTO.AppointmentDate;

                var appointmentsCount = await _unitOfWork
                    .Repository<Appointment>()
                    .GetList(a =>
                        a.Id != appointmentDTO.Id &&
                        a.DoctorScheduleId == appointmentDTO.DoctorScheduleId &&
                        a.CurrentState == CurrentState.Active &&
                        a.AppointmentDate == appointmentDate);


                if (appointmentsCount.Count >= doctor.MaximumAppointments)
                {
                    await _unitOfWork.RollbackAsync();
                    return false;
                }

                if (dbAppointment.DoctorId != appointmentDTO.DoctorId)
                {
                    var lastAppiontments = await _unitOfWork.Repository<Appointment>().GetResult(
                        filter : a => a.PatientId == appointmentDTO.PatientId 
                            && a.DoctorId == appointmentDTO.DoctorId
                            && a.CurrentState == CurrentState.Active ,
                        selectors : a => new AppointmentDTO
                        {
                            Id = a.Id,
                            PatientId = a.PatientId,
                            DoctorId = a.DoctorId,
                        }
                    );

                    var numberOfAppointments = lastAppiontments.Count();

                    if (numberOfAppointments == 1)
                    {
                        var userId = _userService.GetLoggedInUser();
                        var dbMedicalRecord = await _medicalRecordService.GetDocPatientMedicalRecords(appointmentDTO.PatientId);
                        var deleteMedRecord = await _unitOfWork.Repository<MedicalRecord>().ChangeStatus(dbMedicalRecord.Id , userId);

                        if (!deleteMedRecord)
                        {
                            return false;
                        }
                    }
                }

                var lastMedicalRecords = await _medicalRecordService.GetPatientMedicalRecords(appointmentDTO.PatientId);

                var recordResult = lastMedicalRecords.Any(a => a.DoctorId == appointmentDTO.DoctorId);

                if (!recordResult)
                {
                    var newMedicalRecord = new MedicalRecordDTO
                    {
                        DoctorId = appointmentDTO.DoctorId,
                        PatientId = appointmentDTO.PatientId,
                        Diagnosis = string.Empty,
                        ChronicDisease = string.Empty,
                        CurrentMedications = string.Empty,
                        Notes = string.Empty,
                        Allergy = string.Empty,
                    };

                    var newRecordResult = await _medicalRecordService.Add(newMedicalRecord);

                    if (!newRecordResult.Item1)
                    {
                        await _unitOfWork.RollbackAsync();
                        return false;
                    }
                }

                var result = await Update(appointmentDTO);

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
        #endregion

        #region Appointment Management Status 
        public async Task<bool> AppointmentWaiting(Guid AppointmentId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var appointment = await GetAppointmentById(AppointmentId);

                if (appointment == null || appointment.AppointmentDate != DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    await _unitOfWork.RollbackAsync();
                    return false;
                }

                if (appointment.AppointmentStatus != AppointmentStatus.Created)
                {
                    await _unitOfWork.RollbackAsync();
                    return false;
                }

                appointment.AppointmentStatus = AppointmentStatus.Waiting;

                var result = await Update(appointment);

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

        public async Task<bool> AppointmentInProgress(Guid AppointmentId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var appointment = await GetAppointmentById(AppointmentId);

                if (appointment == null || appointment.AppointmentDate != DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    await _unitOfWork.RollbackAsync();
                    return false;
                }

                var checkAppointmentsInprogress = await _unitOfWork.Repository<Appointment>().GetResult(
                    filter: a => a.CurrentState == CurrentState.Active
                            && a.DoctorId == appointment.DoctorId 
                            && a.AppointmentStatus == AppointmentStatus.InProgress,
                    selectors: a => new AppointmentInfoDTO
                    {
                        Id = a.Id
                    }
                );

                if (checkAppointmentsInprogress.Any())
                {
                    await _unitOfWork.RollbackAsync();
                    return false;
                }

                if (appointment.AppointmentStatus != AppointmentStatus.Waiting)
                {
                    await _unitOfWork.RollbackAsync();
                    return false;
                }

                #region Time Validate
                //var now = DateTime.UtcNow;

                //var appointmentTime = appointment.AppointmentDate.ToDateTime(new TimeOnly(0, 0)).AddDays(1).AddHours(5);

                //if (appointmentTime > now)
                //{
                //    await _unitOfWork.RollbackAsync();
                //    return false;
                //}
                #endregion

                appointment.AppointmentStatus = AppointmentStatus.InProgress;

                var result = await Update(appointment);

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

        public async Task<bool> AppointmentCompeleted(Guid AppointmentId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var appointment = await GetAppointmentById(AppointmentId);

                if (appointment == null)
                {
                    await _unitOfWork.RollbackAsync();
                    return false;
                }

                if (appointment.AppointmentStatus != AppointmentStatus.InProgress)
                {
                    await _unitOfWork.RollbackAsync();
                    return false;
                }

                appointment.AppointmentStatus = AppointmentStatus.Completed;

                var result = await Update(appointment);

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

        public async Task<bool> AppointmentCanceled(Guid AppointmentId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var appointment = await GetAppointmentById(AppointmentId);

                if (appointment == null)
                {
                    await _unitOfWork.RollbackAsync();
                    return false;
                }

                if (appointment.AppointmentStatus != AppointmentStatus.Waiting &&
                    appointment.AppointmentStatus != AppointmentStatus.Created)
                {
                    await _unitOfWork.RollbackAsync();
                    return false;
                }

                appointment.AppointmentStatus = AppointmentStatus.Canceled;

                var result = await Update(appointment);

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
        #endregion

        #region Validations
        public async Task<bool> ValidateDoctorAppointment(Guid appointmentId)
        {
            var appointment = await GetAppointmentDetails(appointmentId);

            if (appointment == null ||
                (appointment.AppointmentStatus != AppointmentStatus.InProgress &&
                 appointment.AppointmentStatus != AppointmentStatus.Completed))
            {
                return false;
            }

            var doctorId = await _doctorService.GetDoctorId();

            if (doctorId == Guid.Empty)
                return false;

            if (appointment.DoctorId == doctorId)
                return true;
            else
                return false;
        }
        #endregion
    }
}
