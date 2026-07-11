using AutoMapper;
using Clinic.Application.Contracts;
using Clinic.Application.Contracts.UserContract;
using Clinic.Core.Common;
using Clinic.Core.Enums;
using Clinic.Infrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Services
{
    public class BaseService<T, DTO> : IBaseService<T, DTO> where T : BaseEntity
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<List<DTO>> GetAll()
        {
            var data = await _unitOfWork.Repository<T>().GetAll();
            return _mapper.Map<List<T>, List<DTO>>(data);
        }

        public async Task<DTO> GetById(Guid Id)
        {
            var data = await _unitOfWork.Repository<T>().GetById(Id);
            return _mapper.Map<T, DTO>(data);
        }

        public async Task<(bool, Guid)> Add(DTO entity)
        {
            var data = _mapper.Map<DTO, T>(entity);
            data.CreatedBy = _userService.GetLoggedInUser();
            var result = await _unitOfWork.Repository<T>().Add(data);
            await _unitOfWork.SaveChangesAsync();
            return (true, result.Item2);
        }

        public async Task<bool> Update(DTO entity)
        {
            var data = _mapper.Map<DTO, T>(entity);
            data.UpdatedBy = _userService.GetLoggedInUser();
            var result = await _unitOfWork.Repository<T>().Update(data);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<bool> ChangeStatus(Guid Id, CurrentState status = CurrentState.Deleted)
        {
            var userId = _userService.GetLoggedInUser();
            var result = await _unitOfWork.Repository<T>().ChangeStatus(Id, userId, status);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
