using Clinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Contracts
{
    public interface IBaseService<T,DTO>
    {
        public Task<List<DTO>> GetAll();
        public Task<DTO> GetById(Guid Id);
        public Task<(bool, Guid)> Add(DTO entity);
        public Task<bool> Update(DTO entity);
        public Task<bool> ChangeStatus(Guid Id, CurrentState status = CurrentState.Deleted);
    }
}
