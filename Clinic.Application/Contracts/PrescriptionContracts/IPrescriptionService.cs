using Clinic.Application.DTOs.PrescriptionDTOs;
using Clinic.Core.Entities;
using Clinic.Infrastructure.Models;

namespace Clinic.Application.Contracts.PrescriptionContracts
{
    public interface IPrescriptionService : IBaseService<Prescription, PrescriptionDTO>
    {
        Task<PageResult<PrescriptionInfoDTO>> GetPagePrescriptions(int pageNumber = 1, int pageSize = 10);

        Task<bool> CheckPrescritpionForAppointment(Guid appointmentId);

        Task<PrescriptionInfoDTO> GetPrescritpionDetails(Guid prescriptionId);

        Task<PrescriptionDTO> GetPrescritpionWithMedicines(Guid prescriptionId);

        Task<PrescriptionInfoDTO> GetPrescritpionByAppointment(Guid appointmentId);

        Task<PageResult<PrescriptionInfoDTO>> GetPatientPrescritpion(Guid patientId, int pageNumber = 1, int pageSize = 10);

        Task<List<PrescriptionInfoDTO>> GetPatientPrescritpion(Guid patientId);

        Task<(bool, Guid)> CreatePrescription(PrescriptionDTO prescriptionDTO);

        Task<bool> UpdatePrescription(PrescriptionDTO prescriptionDTO);

        Task<bool> DeletePrescription(Guid id);
    }
}
