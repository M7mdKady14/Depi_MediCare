using Clinic.Application.DTOs.ScheduleDTOs;
using Clinic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Contracts.ScheduleContracts
{
    public interface IDoctorScheduleService : IBaseService<DoctorSchedule , DoctorScheduleDTO>
    {
        Task<List<DoctorScheduleDTO>> GetDoctorSchedules(Guid doctorId);
        Task<List<AppointmentDocSchedule>> GetAppointmentDoctorSchedules(Guid doctorId);
    }
}
