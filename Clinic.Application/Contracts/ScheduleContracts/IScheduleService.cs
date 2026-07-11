using Clinic.Application.DTOs.ScheduleDTOs;
using Clinic.Core.Entities;
using Clinic.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Contracts.ScheduleContracts
{
    public interface IScheduleService : IBaseService<Schedule,ScheduleDTO>
    {
        Task<PageResult<ScheduleDTO>> GetPageSchedule(int pageNumber = 1, int pageSize = 10);
        Task<ScheduleDTO> GetScheduleDetails(Guid Id);
        Task<bool> DeleteSchedule(Guid Id);
    }
}
