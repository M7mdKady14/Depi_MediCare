using Clinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Common
{
    public class BaseEntity 
    {
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        
        public CurrentState CurrentState { get; set; }
    }
}

