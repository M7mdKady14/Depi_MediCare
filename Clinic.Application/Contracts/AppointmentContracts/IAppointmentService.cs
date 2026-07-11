using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.AppointmentDTOs;
using Clinic.Application.DTOs.MedicalRecordDTOs;
using Clinic.Core.Entities;
using Clinic.Core.Enums;
using Clinic.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Contracts.AppointmentContracts
{
    public interface IAppointmentService : IBaseService<Appointment, AppointmentDTO>
    {
        #region Get Methods
        Task<PageResult<AppointmentInfoDTO>> GetPageAppointments(int pageNumber = 1, int pageSize = 10);
        Task<bool> ValidateDoctorAppointment(Guid appointmentId);
        Task<PageResult<AppointmentInfoDTO>> SearchAppointments(SearchAppointment searchAppointment, int pageNumber = 1, int pageSize = 10);
        Task<AppointmentInfoDTO> GetAppointmentDetails(Guid appointmentId);
        Task<PageResult<AppointmentInfoDTO>> ReceptionSearchAppointments(ReceptionSearchAppointment searchAppointment, int pageNumber = 1, int pageSize = 10);
        Task<PageResult<AppointmentInfoDTO>> DoctorSearchAppointments(DoctorSearchAppointment searchAppointment, int pageNumber = 1, int pageSize = 10);
        Task<PageResult<AppointmentInfoDTO>> GetDoctorAppointments(DoctorAppointmentDTO docAppointmentDTO, int pageNumber = 1, int pageSize = 10);
        Task<PageResult<AppointmentInfoDTO>> GetPatientAppointments(Guid patientId, int pageNumber = 1, int pageSize = 10);
        Task<PageResult<AppointmentInfoDTO>> GetMedicalRecordAppointments(Guid patientId, int pageNumber = 1, int pageSize = 10);
        Task<PageResult<AppointmentInfoDTO>> PatientSearchAppointments(PatientSearchAppointment searchAppointment, int pageNumber = 1, int pageSize = 10);
        
        #endregion

        #region Create && Update
            Task<(bool, Guid)> CreateAppointment(AppointmentDTO appointmentDTO);
        Task<bool> UpdateAppointment(AppointmentDTO appointmentDTO);
        #endregion

        #region Appointment Status Methods
        Task<bool> AppointmentWaiting(Guid AppointmentId);
        Task<bool> AppointmentInProgress(Guid AppointmentId);
        Task<bool> AppointmentCompeleted(Guid AppointmentId);
        Task<bool> AppointmentCanceled(Guid AppointmentId);
        #endregion
    }
}
