using Clinic.Application.DTOs.AppointmentDTOs;
using Clinic.Application.DTOs.MedicalRecordDTOs;
using Clinic.Infrastructure.Models;

namespace Clinic.UI.Areas.doctor.Models
{
    public class VmMedicalRecord
    {
        public VmMedicalRecord()
        {
            Tests = new PageResult<TestInfoDTO>();
            MedicalRadiologies = new PageResult<MedicalRadiologyInfoDTO>();
            Appointments = new PageResult<AppointmentInfoDTO>();
        }

        public virtual DocMedicalRecord DocMedicalRecord { get; set; }
        public virtual PageResult<TestInfoDTO> Tests { get; set; }
        public virtual PageResult<MedicalRadiologyInfoDTO> MedicalRadiologies { get; set; }
        public virtual PageResult<AppointmentInfoDTO> Appointments { get; set; }
    }
}
