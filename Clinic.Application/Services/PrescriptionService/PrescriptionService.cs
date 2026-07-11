using AutoMapper;
using Clinic.Application.Contracts.PrescriptionContracts;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.AppointmentDTOs;
using Clinic.Application.DTOs.PrescriptionDTOs;
using Clinic.Application.Helper;
using Clinic.Core.Entities;
using Clinic.Core.Enums;
using Clinic.Infrastructure.Contracts;
using Clinic.Infrastructure.Models;

namespace Clinic.Application.Services.PrescriptionService
{
    public class PrescriptionService : BaseService<Prescription, PrescriptionDTO>, IPrescriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IPrescriptionMedicineService _prescriptionMedicineService;

        public PrescriptionService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService
            , IPrescriptionMedicineService prescriptionMedicineService)
            : base(unitOfWork, mapper, userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _prescriptionMedicineService = prescriptionMedicineService;
        }

        #region Get Methods
        public async Task<PageResult<PrescriptionInfoDTO>> GetPagePrescriptions(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _unitOfWork.Repository<Prescription>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.CurrentState == CurrentState.Active,
                selectors: a => new PrescriptionInfoDTO
                {
                    Id = a.Id,
                    AppointmentId = a.AppointmentId,
                    PrescriptionDate = a.PrescriptionDate,
                    PatientName = a.Appointment.Patient.FirstName + " " + a.Appointment.Patient.MiddleName + " " + a.Appointment.Patient.LastName,
                    PatientId = a.Appointment.PatientId,
                    DoctorId = a.Appointment.DoctorId,
                    DoctorName = a.Appointment.Doctor.Name,
                    PrescriptionMedicines = a.PrescriptionMedicines.Select(p => new PrescriptionMedicineDTO
                    {
                        Id = p.Id,
                        MedicineName = p.MedicineName,
                        Dosage = p.Dosage,
                        Duration = p.Duration,
                        Frequency = p.Frequency,
                        Instructions = p.Instructions,
                        CurrentState = p.CurrentState
                    }).ToList()
                },
                orderby: a => a.CreatedDate,
                isDesending: true
            );

            return result;
        }

        public async Task<PrescriptionInfoDTO> GetPrescritpionDetails(Guid prescriptionId)
        {
            var result = await _unitOfWork.Repository<Prescription>().GetResultForOne(
                filter: a => a.CurrentState == CurrentState.Active
                        && a.Id == prescriptionId,
                selectors: a => new PrescriptionInfoDTO
                {
                    Id = a.Id,
                    AppointmentId = a.AppointmentId,
                    PrescriptionDate = DateTimeHelper.ToEgyptTime(a.PrescriptionDate),
                    PatientName = a.Appointment.Patient.FirstName + " " + a.Appointment.Patient.MiddleName + " " + a.Appointment.Patient.LastName,
                    PatientId = a.Appointment.PatientId,
                    DoctorId = a.Appointment.DoctorId,
                    DoctorName = a.Appointment.Doctor.Name,
                    PrescriptionMedicines = a.PrescriptionMedicines.Select(p => new PrescriptionMedicineDTO
                    {
                        Id = p.Id,
                        MedicineName = p.MedicineName,
                        PrescriptionId = p.PrescriptionId,
                        Dosage = p.Dosage,
                        Duration = p.Duration,
                        Frequency = p.Frequency,
                        Instructions = p.Instructions,
                        CurrentState = p.CurrentState
                    }).ToList()
                }
            );

            return result;
        }

        public async Task<PrescriptionDTO> GetPrescritpionWithMedicines(Guid prescriptionId)
        {
            var result = await _unitOfWork.Repository<Prescription>().GetResultForOne(
                filter: a => a.CurrentState == CurrentState.Active
                        && a.Id == prescriptionId,
                selectors: a => new PrescriptionDTO
                {
                    Id = a.Id,
                    AppointmentId = a.AppointmentId,
                    PrescriptionDate = a.PrescriptionDate,
                    Medicines = a.PrescriptionMedicines.Select(p => new PrescriptionMedicineDTO
                    {
                        Id = p.Id,
                        MedicineName = p.MedicineName,
                        PrescriptionId = p.PrescriptionId,
                        Dosage = p.Dosage,
                        Duration = p.Duration,
                        Frequency = p.Frequency,
                        Instructions = p.Instructions,
                        CurrentState = p.CurrentState
                    }).ToList()
                }
            );

            return result;
        }

        public async Task<PrescriptionInfoDTO> GetPrescritpionByAppointment(Guid appointmentId)
        {
            var result = await _unitOfWork.Repository<Prescription>().GetResultForOne(
                filter: a => a.CurrentState == CurrentState.Active
                        && a.AppointmentId == appointmentId,
                selectors: a => new PrescriptionInfoDTO
                {
                    Id = a.Id,
                    AppointmentId = a.AppointmentId,
                    PrescriptionDate = DateTimeHelper.ToEgyptTime(a.PrescriptionDate),
                    PatientName = a.Appointment.Patient.FirstName + " " + a.Appointment.Patient.MiddleName + " " + a.Appointment.Patient.LastName,
                    PatientId = a.Appointment.PatientId,
                    DoctorId = a.Appointment.DoctorId,
                    DoctorName = a.Appointment.Doctor.Name,
                    PrescriptionMedicines = a.PrescriptionMedicines.Select(p => new PrescriptionMedicineDTO
                    {
                        Id = p.Id,
                        MedicineName = p.MedicineName,
                        PrescriptionId = p.PrescriptionId,
                        Dosage = p.Dosage,
                        Duration = p.Duration,
                        Frequency = p.Frequency,
                        Instructions = p.Instructions,
                        CurrentState = p.CurrentState
                    }).ToList()
                }
            );

            return result;
        }

        public async Task<PageResult<PrescriptionInfoDTO>> GetPatientPrescritpion(Guid patientId , int pageNumber = 1 , int pageSize = 10)
        {
            var result = await _unitOfWork.Repository<Prescription>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.CurrentState == CurrentState.Active
                        && a.Appointment.PatientId == patientId,
                selectors: a => new PrescriptionInfoDTO
                {
                    Id = a.Id,
                    AppointmentId = a.AppointmentId,
                    PrescriptionDate = a.PrescriptionDate,
                    PatientName = a.Appointment.Patient.FirstName + " " + a.Appointment.Patient.MiddleName + " " + a.Appointment.Patient.LastName,
                    PatientId = a.Appointment.PatientId,
                    DoctorId = a.Appointment.DoctorId,
                    DoctorName = a.Appointment.Doctor.Name,
                    PrescriptionMedicines = a.PrescriptionMedicines.Select(p => new PrescriptionMedicineDTO
                    {
                        Id = p.Id,
                        MedicineName = p.MedicineName,
                        Dosage = p.Dosage,
                        Duration = p.Duration,
                        Frequency = p.Frequency,
                        Instructions = p.Instructions,
                        CurrentState = p.CurrentState
                    }).ToList()
                }
            );

            return result;
        }

        public async Task<List<PrescriptionInfoDTO>> GetPatientPrescritpion(Guid patientId)
        {
            var result = await _unitOfWork.Repository<Prescription>().GetResult(
                filter: a => a.CurrentState == CurrentState.Active
                        && a.Appointment.PatientId == patientId,
                selectors: a => new PrescriptionInfoDTO
                {
                    Id = a.Id,
                    AppointmentId = a.AppointmentId,
                    PrescriptionDate = a.PrescriptionDate,
                    PatientName = a.Appointment.Patient.FirstName + " " + a.Appointment.Patient.MiddleName + " " + a.Appointment.Patient.LastName,
                    PatientId = a.Appointment.PatientId,
                    DoctorId = a.Appointment.DoctorId,
                    DoctorName = a.Appointment.Doctor.Name,
                    PrescriptionMedicines = a.PrescriptionMedicines.Select(p => new PrescriptionMedicineDTO
                    {
                        Id = p.Id,
                        MedicineName = p.MedicineName,
                        Dosage = p.Dosage,
                        Duration = p.Duration,
                        Frequency = p.Frequency,
                        Instructions = p.Instructions,
                        CurrentState = p.CurrentState
                    }).ToList()
                }
            );

            return result;
        }

        public async Task<bool> CheckPrescritpionForAppointment(Guid appointmentId)
        {
            var result = await _unitOfWork.Repository<Prescription>().GetResultForOne(
                filter: a => a.CurrentState == CurrentState.Active
                        && a.AppointmentId == appointmentId,
                selectors: a => new PrescriptionInfoDTO
                {
                    Id = a.Id,
                    AppointmentId = a.AppointmentId,
                }
            );

            if (result == null)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Create and Update
        public async Task<(bool , Guid)> CreatePrescription(PrescriptionDTO prescriptionDTO)
        {
            var appointment = await _unitOfWork.Repository<Appointment>().GetResultForOne(
                filter : a => a.CurrentState == CurrentState.Active 
                         && a.Id == prescriptionDTO.AppointmentId,                
                selectors : a => new AppointmentDTO
                {
                    AppointmentStatus = a.AppointmentStatus,
                    Id = a.Id,
                }
            );

            if ( appointment == null )
            {
                return (false , Guid.Empty);
            }

            if (appointment.AppointmentStatus != AppointmentStatus.InProgress
                && appointment.AppointmentStatus != AppointmentStatus.Completed)
            {
                return (false, Guid.Empty);
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                prescriptionDTO.PrescriptionDate = DateTime.UtcNow;

                var exists = await _unitOfWork.Repository<Prescription>().GetFirstOrDefault(
                    p => p.CurrentState == CurrentState.Active &&
                         p.AppointmentId == prescriptionDTO.AppointmentId);

                if (exists != null)
                {
                    await _unitOfWork.RollbackAsync();
                    return (false, Guid.Empty);
                }

                var result = await Add(prescriptionDTO);

                if (!result.Item1)
                {
                    await _unitOfWork.RollbackAsync();
                    return (false, Guid.Empty);
                }

                foreach (var item in prescriptionDTO.Medicines)
                {
                    item.PrescriptionId = result.Item2;

                    var medResult = await _prescriptionMedicineService.Add(item);

                    if (!medResult.Item1)
                    {
                        await _unitOfWork.RollbackAsync();
                        return (false, Guid.Empty);
                    }
                }

                await _unitOfWork.CommitAsync();
                return (true , result.Item2);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdatePrescription(PrescriptionDTO prescriptionDTO)
        {
            var appointment = await _unitOfWork.Repository<Appointment>().GetResultForOne(
                filter: a => a.CurrentState == CurrentState.Active
                         && a.Id == prescriptionDTO.AppointmentId,
                selectors: a => new AppointmentDTO
                {
                    AppointmentStatus = a.AppointmentStatus,
                    Id = a.Id,
                }
            );

            if (appointment == null)
            {
                return false;
            }

            if (appointment.AppointmentStatus != AppointmentStatus.InProgress
                && appointment.AppointmentStatus != AppointmentStatus.Completed)
            {
                return false;
            }

            var prescription = await _unitOfWork.Repository<Prescription>().GetResultForOne(
                filter : a => a.CurrentState == CurrentState.Active 
                         && a.Id == prescriptionDTO.Id
                         && a.AppointmentId == prescriptionDTO.AppointmentId,
                selectors : a => new PrescriptionDTO
                {
                    Id = a.Id,
                    AppointmentId = a.AppointmentId,
                    PrescriptionDate = a.PrescriptionDate,
                    Medicines = a.PrescriptionMedicines.Select(a => new PrescriptionMedicineDTO
                    {
                        Id = a.Id,
                        PrescriptionId = a.PrescriptionId,
                        MedicineName = a.MedicineName,
                        Dosage = a.Dosage,
                        Duration = a.Duration,
                        Frequency = a.Frequency,
                        Instructions = a.Instructions,
                    }).ToList()
                }
            );

            if (prescription == null)
            {
                return false;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var dbMedicines = prescription.Medicines;
                var debMedicineDec = dbMedicines.ToDictionary(a => a.Id, a => a);

                foreach (var item in prescriptionDTO.Medicines)
                {
                    if (item.Id != Guid.Empty && debMedicineDec.ContainsKey(item.Id))
                    {
                        var dbItem = debMedicineDec[item.Id];
                        dbItem.MedicineName = item.MedicineName;
                        dbItem.Dosage = item.Dosage;
                        dbItem.Frequency = item.Frequency;
                        dbItem.Duration = item.Duration; 
                        dbItem.Instructions = item.Instructions;

                        var updateResult = await _prescriptionMedicineService.Update(dbItem);

                        if (!updateResult)
                        {
                            await _unitOfWork.RollbackAsync();
                            return false;
                        }

                        debMedicineDec.Remove(item.Id);
                    }
                    else
                    {
                        item.PrescriptionId = prescriptionDTO.Id;
                        var addResult = await _prescriptionMedicineService.Add(item);

                        if (!addResult.Item1)
                        {
                            await _unitOfWork.RollbackAsync();
                            return false;
                        }
                    }
                }

                foreach (var item in debMedicineDec.Values)
                {
                    var medResult = await _prescriptionMedicineService.ChangeStatus(item.Id);

                    if (!medResult)
                    {
                        await _unitOfWork.RollbackAsync();
                        return false;
                    }
                }

                var result = await Update(prescriptionDTO);
                
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

        public async Task<bool> DeletePrescription(Guid Id)
        {
            var prescription = await _unitOfWork.Repository<Prescription>().GetResultForOne(
                filter: a => a.CurrentState == CurrentState.Active
                         && a.Id == Id ,
                selectors: a => new PrescriptionDTO
                {
                    Id = a.Id,
                    AppointmentId = a.AppointmentId,
                    PrescriptionDate = a.PrescriptionDate,
                    Medicines = a.PrescriptionMedicines.Select(a => new PrescriptionMedicineDTO
                    {
                        Id = a.Id,
                        PrescriptionId = a.PrescriptionId,
                        MedicineName = a.MedicineName,
                        Dosage = a.Dosage,
                        Duration = a.Duration,
                        Frequency = a.Frequency,
                        Instructions = a.Instructions,
                    }).ToList()
                }
            );

            if (prescription == null)
            {
                return false;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                
                foreach (var item in prescription.Medicines)
                {
                    var medResult = await _prescriptionMedicineService.ChangeStatus(item.Id);

                    if (!medResult)
                    {
                        await _unitOfWork.RollbackAsync();
                        return false;
                    }
                }

                var result = await ChangeStatus(prescription.Id);

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
    }
}
