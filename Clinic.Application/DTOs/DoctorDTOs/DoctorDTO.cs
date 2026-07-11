using Clinic.Core.Common;
using Clinic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.DoctorDTOs
{
    public class DoctorDTO : BaseEntity
    {
        public DoctorDTO()
        {
            Schedules = new List<Guid>();
        }

        public Guid SpecializationId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string PhoneNumber { get; set; }

        public string UserId { get; set; }

        public int MaximumAppointments { get; set; }

        public List<Guid> Schedules { get; set; }
    }
}
