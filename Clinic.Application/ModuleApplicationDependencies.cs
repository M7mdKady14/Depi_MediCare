using Clinic.Application.Contracts.AppointmentContracts;
using Clinic.Application.Contracts.DoctorContract;
using Clinic.Application.Contracts.MedicalRecordContracts;
using Clinic.Application.Contracts.PatientContract;
using Clinic.Application.Contracts.PrescriptionContracts;
using Clinic.Application.Contracts.ProtocalContracts;
using Clinic.Application.Contracts.ScheduleContracts;
using Clinic.Application.Contracts.SpecializationContract;
using Clinic.Application.Mapping;
using Clinic.Application.Services.AppointmentServices;
using Clinic.Application.Services.DoctorServices;
using Clinic.Application.Services.MedicalRecordServices;
using Clinic.Application.Services.PatientService;
using Clinic.Application.Services.PrescriptionService;
using Clinic.Application.Services.ProtocalServices;
using Clinic.Application.Services.ScheduleServices;
using Clinic.Application.Services.SpecializationServices;
using Clinic.Core.Entities;
using Clinic.Infrastructure.Contracts;
using Clinic.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Clinic.Application
{
    public static class ModuleApplicationDependencies 
    {
        public static IServiceCollection AddModuleApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAppointmentTypeService, AppointmentTypeService>();
            services.AddScoped<IAppointmentService, AppointmentService>();

            services.AddScoped<ISpecializationService, SpecializationService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IDoctorScheduleService, DoctorScheduleService>();

            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IMedicalRecordService, MedicalRecordService>();
            services.AddScoped<IMedicalRadiologyService, MedicalRadiologyService>();
            services.AddScoped<ITestService,TestService>();
            services.AddScoped<IPrescriptionService, PrescriptionService>();
            services.AddScoped<IPrescriptionMedicineService, PrescriptionMedicineService>();

            services.AddScoped<IPrescriptionProtocalService, PrescriptionProtocalService>();
            services.AddScoped<IPrescriptionMedicineProtocalService, PrescriptionMedicineProtocalService>();

            return services;
        }
    }
}

