using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.ProtocalDTOs
{
    public class MedicineProtocalDTO : BaseDTO
    {
        public string MedicineName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }
        public string? Instructions { get; set; }
        public Guid ProtocalId { get; set; }
    }
}

