using AutoMapper;
using Clinic.Application.DTOs;
using Clinic.Application.DTOs.AppointmentDTOs;
using Clinic.Application.DTOs.DoctorDTOs;
using Clinic.Application.DTOs.MedicalRecordDTOs;
using Clinic.Application.DTOs.PatientDTOs;
using Clinic.Application.DTOs.PrescriptionDTOs;
using Clinic.Application.DTOs.ProtocalDTOs;
using Clinic.Application.DTOs.ScheduleDTOs;
using Clinic.Application.DTOs.SpecializationDTOs;
using Clinic.Core.Common;
using Clinic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BaseEntity, BaseDTO>().ReverseMap();
            CreateMap<Appointment, AppointmentDTO>().ReverseMap();
            CreateMap<AppointmentType, AppointmentTypeDTO>().ReverseMap();

            CreateMap<Specialization, SpecializationDTO>().ReverseMap();
            CreateMap<Doctor, DoctorDTO>().ReverseMap();
            CreateMap<DoctorSchedule, DoctorScheduleDTO>().ReverseMap();
            CreateMap<Schedule, ScheduleDTO>().ReverseMap();

            CreateMap<Patient, PatientDTO>().ReverseMap();
            CreateMap<MedicalRecord, MedicalRecordDTO>().ReverseMap();
            CreateMap<MedicalRadiology, MedicalRadiologyDTO>().ReverseMap();
            CreateMap<Test, TestDTO>().ReverseMap();

            CreateMap<Prescription, PrescriptionDTO>().ReverseMap();
            CreateMap<PrescriptionMedicine, PrescriptionMedicineDTO>().ReverseMap();

            CreateMap<PrescriptionProtocal, PrescriptionProtocalDTO>().ReverseMap();
            CreateMap<PrescriptionMedicineProtocal, MedicineProtocalDTO>().ReverseMap();
        }
    }
}
