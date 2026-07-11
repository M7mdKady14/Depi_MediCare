using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.PrescriptionDTOs
{
    public class PrescriptionDTO : BaseDTO
    {
        public PrescriptionDTO()
        {
            Medicines = new List<PrescriptionMedicineDTO>();
        }

        public DateTime PrescriptionDate { get; set; }
        public Guid AppointmentId { get; set; }

        public virtual List<PrescriptionMedicineDTO> Medicines { get; set; }
    }
}
