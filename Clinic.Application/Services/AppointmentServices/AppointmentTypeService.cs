using AutoMapper;
using Clinic.Application.Contracts.AppointmentContracts;
using Clinic.Application.Contracts.UserContract;
using Clinic.Application.DTOs.AppointmentDTOs;
using Clinic.Core.Entities;
using Clinic.Infrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Services.AppointmentServices
{
    public class AppointmentTypeService : BaseService<AppointmentType, AppointmentTypeDTO>, IAppointmentTypeService
    {
        public AppointmentTypeService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService) 
            : base(unitOfWork, mapper, userService)
        {
        }
    }
}
