using Clinic.Application.DTOs.ScheduleDTOs;
using Clinic.Application.DTOs.SpecializationDTOs;

namespace Clinic.Application.DTOs.DoctorDTOs
{
    public class DoctorInfoDTO : BaseDTO
    {
        public DoctorInfoDTO()
        {
            DoctorSchedules = new List<AppointmentDocSchedule>();
        }

        public Guid SpecializationId { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
        public string PhoneNumber { get; set; }

        public string UserId { get; set; }

        public int MaximumAppointments { get; set; }

        public string SpecializationName { get; set; }

        public virtual SpecializationDTO? Specialization { get; set; }

        public virtual List<AppointmentDocSchedule> DoctorSchedules { get; set; }
    }
}

