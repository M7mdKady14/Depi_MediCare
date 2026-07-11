using Clinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs
{
    public class BaseDTO
    {
        public Guid Id { get; set; }
        public CurrentState CurrentState { get; set; }
    }
}
