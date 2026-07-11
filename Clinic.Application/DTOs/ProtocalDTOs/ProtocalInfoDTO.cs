using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.ProtocalDTOs
{
    public class ProtocalInfoDTO : BaseDTO
    {
        public ProtocalInfoDTO()
        {
            MedicineProtocal = new List<MedicineProtocalDTO>();
        }

        public string Name { get; set; }
        public string Disease { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }

        public virtual List<MedicineProtocalDTO> MedicineProtocal { get; set; }
    }
}
