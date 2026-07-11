using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.MedicalRecordDTOs
{
    public class MedicalRecordPagination
    {
        public int TestPageNumber { get; set; } = 1;
        public int TestPageSize { get; set; } = 5;

        public int MedicalRadilogyPageNumber { get; set; } = 1;
        public int MedicalRadilogyPageSize { get; set; } = 5;

        public int AppointmentPageNumber { get; set; } = 1;
        public int AppointmentPageSize { get; set; } = 5;
    }
}