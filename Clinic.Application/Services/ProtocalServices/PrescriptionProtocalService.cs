using AutoMapper;
using Clinic.Application.Contracts.DoctorContract;
using Clinic.Application.Contracts.PrescriptionContracts;
using Clinic.Application.Contracts.ProtocalContracts;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.AppointmentDTOs;
using Clinic.Application.DTOs.PrescriptionDTOs;
using Clinic.Application.DTOs.ProtocalDTOs;
using Clinic.Application.Helper;
using Clinic.Core.Entities;
using Clinic.Core.Enums;
using Clinic.Infrastructure.Contracts;
using Clinic.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Services.ProtocalServices
{
    public class PrescriptionProtocalService : BaseService<PrescriptionProtocal, PrescriptionProtocalDTO>, IPrescriptionProtocalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IDoctorService _doctorService;
        private readonly IPrescriptionMedicineProtocalService _prescriptionMedicineProtocalService;

        public PrescriptionProtocalService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService
            , IPrescriptionMedicineProtocalService prescriptionMedicineProtocalService , IDoctorService doctorService)
            : base(unitOfWork, mapper, userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _prescriptionMedicineProtocalService = prescriptionMedicineProtocalService;
            _doctorService = doctorService;
        }

        #region Get Methods
        public async Task<PageResult<ProtocalInfoDTO>> GetPageProtocals(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _unitOfWork.Repository<PrescriptionProtocal>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.CurrentState == CurrentState.Active,
                selectors: a => new ProtocalInfoDTO
                {
                    Id = a.Id,
                    CurrentState = a.CurrentState,
                    Code = a.Code,
                    Description = a.Description,
                    Name = a.Name,
                    Disease = a.Disease,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.Name,
                    MedicineProtocal = a.PrescriptionMedicineProtocal.Where(a => a.CurrentState == CurrentState.Active).Select(a => new MedicineProtocalDTO
                    {
                        Id = a.Id,
                        ProtocalId = a.ProtocalId,
                        MedicineName = a.MedicineName,
                        Dosage = a.Dosage,
                        Duration = a.Duration,
                        Frequency = a.Frequency,
                        Instructions = a.Instructions,
                    }).ToList()
                },
                orderby: a => a.CreatedDate,
                isDesending: true
            );

            return result;
        }

        public async Task<PageResult<ProtocalInfoDTO>> GetPageDoctorProtocals(int pageNumber = 1, int pageSize = 10)
        {
            var doctorId = await _doctorService.GetDoctorId();

            var result = await _unitOfWork.Repository<PrescriptionProtocal>().GetPageResult(
                pageNumber,
                pageSize,
                filter: a => a.CurrentState == CurrentState.Active
                            && a.DoctorId == doctorId,
                selectors: a => new ProtocalInfoDTO
                {
                    Id = a.Id,
                    CurrentState = a.CurrentState,
                    Code = a.Code,
                    Description = a.Description,
                    Name = a.Name,
                    Disease = a.Disease,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.Name,
                    MedicineProtocal = a.PrescriptionMedicineProtocal.Where(a => a.CurrentState == CurrentState.Active).Select(a => new MedicineProtocalDTO
                    {
                        Id = a.Id,
                        ProtocalId = a.ProtocalId,
                        MedicineName = a.MedicineName,
                        Dosage = a.Dosage,
                        Duration = a.Duration,
                        Frequency = a.Frequency,
                        Instructions = a.Instructions,
                    }).ToList()
                },
                orderby: a => a.CreatedDate,
                isDesending: true
            );

            return result;
        }

        public async Task<List<ProtocalInfoDTO>> GetDoctorProtocals()
        {
            var doctorId = await _doctorService.GetDoctorId();

            var result = await _unitOfWork.Repository<PrescriptionProtocal>().GetResult(
                filter: a => a.CurrentState == CurrentState.Active
                            && a.DoctorId == doctorId,
                selectors: a => new ProtocalInfoDTO
                {
                    Id = a.Id,
                    CurrentState = a.CurrentState,
                    Code = a.Code,
                    Description = a.Description,
                    Name = a.Name,
                    Disease = a.Disease,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.Name,
                    MedicineProtocal = a.PrescriptionMedicineProtocal.Where(a => a.CurrentState == CurrentState.Active).Select(a => new MedicineProtocalDTO
                    {
                        Id = a.Id,
                        ProtocalId = a.ProtocalId,
                        MedicineName = a.MedicineName,
                        Dosage = a.Dosage,
                        Duration = a.Duration,
                        Frequency = a.Frequency,
                        Instructions = a.Instructions,
                    }).ToList()
                },
                orderby: a => a.CreatedDate,
                isDesending: true
            );

            return result;
        }

        public async Task<ProtocalInfoDTO> GetProtocalDetails(Guid protocalId)
        {
            var result = await _unitOfWork.Repository<PrescriptionProtocal>().GetResultForOne(
                filter: a => a.CurrentState == CurrentState.Active
                        && a.Id == protocalId,
                selectors: a => new ProtocalInfoDTO
                {
                    Id = a.Id,
                    CurrentState = a.CurrentState,
                    Code = a.Code,
                    Description = a.Description,
                    Name = a.Name,
                    Disease = a.Disease,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.Name,
                    MedicineProtocal = a.PrescriptionMedicineProtocal.Where(a => a.CurrentState == CurrentState.Active).Select(a => new MedicineProtocalDTO
                    {
                        Id = a.Id,
                        ProtocalId = a.ProtocalId,
                        MedicineName = a.MedicineName,
                        Dosage = a.Dosage,
                        Duration = a.Duration,
                        Frequency = a.Frequency,
                        Instructions = a.Instructions,
                    }).ToList()
                }
            );

            return result;
        }

        public async Task<PrescriptionProtocalDTO> GetProtocal(Guid protocalId)
        {
            var result = await _unitOfWork.Repository<PrescriptionProtocal>().GetResultForOne(
                filter: a => a.CurrentState == CurrentState.Active
                        && a.Id == protocalId,
                selectors: a => new PrescriptionProtocalDTO
                {
                    Id = a.Id,
                    CurrentState = a.CurrentState,
                    Code = a.Code,
                    Description = a.Description,
                    Name = a.Name,
                    Disease = a.Disease,
                    DoctorId = a.DoctorId,
                    MedicineProtocal = a.PrescriptionMedicineProtocal.Where(a => a.CurrentState == CurrentState.Active).Select(a => new MedicineProtocalDTO
                    {
                        Id = a.Id,
                        ProtocalId = a.ProtocalId,
                        MedicineName = a.MedicineName,
                        Dosage = a.Dosage,
                        Duration = a.Duration,
                        Frequency = a.Frequency,
                        Instructions = a.Instructions,
                    }).ToList()
                }
            );

            return result;
        }

        public async Task<List<ProtocalInfoDTO>> GetProtocals()
        {
            var result = await _unitOfWork.Repository<PrescriptionProtocal>().GetResult(
                filter: a => a.CurrentState == CurrentState.Active,
                selectors: a => new ProtocalInfoDTO
                {
                    Id = a.Id,
                    CurrentState = a.CurrentState,
                    Code = a.Code,
                    Description = a.Description,
                    Name = a.Name,
                    Disease = a.Disease,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.Name,
                    MedicineProtocal = a.PrescriptionMedicineProtocal.Where(a => a.CurrentState == CurrentState.Active).Select(a => new MedicineProtocalDTO
                    {
                        Id = a.Id,
                        ProtocalId = a.ProtocalId,
                        MedicineName = a.MedicineName,
                        Dosage = a.Dosage,
                        Duration = a.Duration,
                        Frequency = a.Frequency,
                        Instructions = a.Instructions,
                    }).ToList()
                },
                orderby: a => a.CreatedDate,
                isDesending: true
            );

            return result;
        }

        public async Task<PrescriptionProtocalDTO> GetProtocalDTO()
        {
            var protocal = new PrescriptionProtocalDTO();

            protocal.DoctorId = await _doctorService.GetDoctorId();

            return protocal;
        }

        public async Task<PrescriptionDTO> ProtocalToPrescription(Guid protocalId , Guid appointmentId)
        {
            var prescription = new PrescriptionDTO();

            var protocal = await GetProtocalDetails(protocalId);

            foreach (var item in protocal.MedicineProtocal)
            {
                prescription.Medicines.Add(new PrescriptionMedicineDTO
                {
                    MedicineName = item.MedicineName,
                    Dosage = item.Dosage,
                    Frequency = item.Frequency,
                    Instructions = item.Instructions,
                    Duration = item.Duration,
                });
            }

            prescription.AppointmentId = appointmentId;

            return prescription;
        }
        #endregion

        #region Create , Update , Delete
        public async Task<(bool, Guid)> CreateProtocal(PrescriptionProtocalDTO prescriptionProtocalDTO)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var result = await Add(prescriptionProtocalDTO);

                if (!result.Item1)
                {
                    await _unitOfWork.RollbackAsync();
                    return (false, Guid.Empty);
                }

                foreach (var item in prescriptionProtocalDTO.MedicineProtocal)
                {
                    item.ProtocalId = result.Item2;

                    var medResult = await _prescriptionMedicineProtocalService.Add(item);

                    if (!medResult.Item1)
                    {
                        await _unitOfWork.RollbackAsync();
                        return (false, Guid.Empty);
                    }
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

        public async Task<bool> UpdateProtocal(PrescriptionProtocalDTO prescriptionProtocalDTO)
        {
            var protocal = await _unitOfWork.Repository<PrescriptionProtocal>().GetResultForOne(
                filter: a => a.CurrentState == CurrentState.Active
                         && a.Id == prescriptionProtocalDTO.Id,
                selectors: a => new PrescriptionProtocalDTO
                {
                    Id = a.Id,
                    CurrentState = a.CurrentState,
                    Code = a.Code,
                    Description = a.Description,
                    Name = a.Name,
                    Disease = a.Disease,
                    DoctorId = a.DoctorId,
                    MedicineProtocal = a.PrescriptionMedicineProtocal.Where(a => a.CurrentState == CurrentState.Active).Select(a => new MedicineProtocalDTO
                    {
                        Id = a.Id,
                        ProtocalId = a.ProtocalId,
                        MedicineName = a.MedicineName,
                        Dosage = a.Dosage,
                        Duration = a.Duration,
                        Frequency = a.Frequency,
                        Instructions = a.Instructions,
                    }).ToList()
                }
            );

            if (protocal == null)
            {
                return false;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var dbMedicines = protocal.MedicineProtocal;
                var debMedicineDec = dbMedicines.ToDictionary(a => a.Id, a => a);

                foreach (var item in prescriptionProtocalDTO.MedicineProtocal)
                {
                    if (item.Id != Guid.Empty && debMedicineDec.ContainsKey(item.Id))
                    {
                        var dbItem = debMedicineDec[item.Id];
                        dbItem.MedicineName = item.MedicineName;
                        dbItem.Dosage = item.Dosage;
                        dbItem.Frequency = item.Frequency;
                        dbItem.Duration = item.Duration;
                        dbItem.Instructions = item.Instructions;

                        var updateResult = await _prescriptionMedicineProtocalService.Update(dbItem);

                        if (!updateResult)
                        {
                            await _unitOfWork.RollbackAsync();
                            return false;
                        }

                        debMedicineDec.Remove(item.Id);
                    }
                    else
                    {
                        item.ProtocalId = prescriptionProtocalDTO.Id;
                        var addResult = await _prescriptionMedicineProtocalService.Add(item);

                        if (!addResult.Item1)
                        {
                            await _unitOfWork.RollbackAsync();
                            return false;
                        }
                    }
                }

                foreach (var item in debMedicineDec.Values)
                {
                    var medResult = await _prescriptionMedicineProtocalService.ChangeStatus(item.Id);

                    if (!medResult)
                    {
                        await _unitOfWork.RollbackAsync();
                        return false;
                    }
                }

                var result = await Update(prescriptionProtocalDTO);

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

        public async Task<bool> DeleteProtocal(Guid Id)
        {
            var protocal = await _unitOfWork.Repository<PrescriptionProtocal>().GetResultForOne(
                filter: a => a.CurrentState == CurrentState.Active
                         && a.Id == Id,
                selectors: a => new PrescriptionProtocalDTO
                {
                    Id = a.Id,
                    CurrentState = a.CurrentState,
                    MedicineProtocal = a.PrescriptionMedicineProtocal.Where(a => a.CurrentState == CurrentState.Active).Select(a => new MedicineProtocalDTO
                    {
                        Id = a.Id,
                    }).ToList()
                }
            );

            if (protocal == null)
            {
                return false;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                foreach (var item in protocal.MedicineProtocal)
                {
                    var medResult = await _prescriptionMedicineProtocalService.ChangeStatus(item.Id);

                    if (!medResult)
                    {
                        await _unitOfWork.RollbackAsync();
                        return false;
                    }
                }

                var result = await ChangeStatus(protocal.Id);

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
