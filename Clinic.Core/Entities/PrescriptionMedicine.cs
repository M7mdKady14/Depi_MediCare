using Clinic.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Entities
{
    public class PrescriptionMedicine : BaseEntity
    {
        public string MedicineName { get; set; } 
        public string Dosage { get; set; } 
        public string Frequency { get; set; } 
        public string Duration { get; set; } 
        public string? Instructions { get; set; } 


        public Guid PrescriptionId { get; set; }
        public virtual Prescription? Prescription { get; set; }
    }
}
