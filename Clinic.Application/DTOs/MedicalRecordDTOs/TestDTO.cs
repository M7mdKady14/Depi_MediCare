using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.MedicalRecordDTOs
{
    public class TestDTO : BaseDTO
    {
        public string? File { get; set; }
        public string? Description { get; set; }
        public Guid MedicalRecordId { get; set; }
    }
}
