using Clinic.Application.DTOs.AppointmentDTOs;
using Clinic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Contracts.AppointmentContracts
{
    public interface IAppointmentTypeService : IBaseService<AppointmentType, AppointmentTypeDTO>
    { 
    }
}
