using Clinic.Application.DTOs.MedicalRecordDTOs;
using Clinic.Core.Entities;
using Clinic.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Contracts.MedicalRecordContracts
{
    public interface ITestService : IBaseService<Test, TestDTO>
    {

        Task<PageResult<TestInfoDTO>> GetPageTests(Guid medicalRecordId, int pageNumber = 1, int pageSize = 10);
        Task<List<TestInfoDTO>> GetTests(Guid medicalRecordId);
        Task<(bool, Guid)> DeleteTest(Guid Id);
    }
}
