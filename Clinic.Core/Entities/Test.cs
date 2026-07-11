using Clinic.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Entities
{
    public class Test : BaseEntity
    {
        public string? File { get; set; }
        public string? Description { get; set; }

        public Guid MedicalRecordId { get; set; }
        public virtual MedicalRecord? MedicalRecord { get; set; }
    }
}
