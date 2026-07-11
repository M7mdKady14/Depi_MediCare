using Clinic.Core.Entities;
using Clinic.Infrastructure.Data;
using Clinic.Infrastructure.UserModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Infrastructure.Context
{
    public class ClinicContext : IdentityDbContext<AppUser> 
    {
        public ClinicContext(DbContextOptions<ClinicContext> options) : base(options)
        {
        }

        public virtual DbSet<Specialization> TbSpecializations { get; set; }
        public virtual DbSet<Doctor> TbDoctors { get; set; }
        public virtual DbSet<Schedule> TbSchedules { get; set; }
        public virtual DbSet<DoctorSchedule> TbDoctorSchedules { get; set; }
        public virtual DbSet<Patient> TbPatients { get; set; }
        public virtual DbSet<Appointment> TbAppointments { get; set; }
        public virtual DbSet<AppointmentType> TbAppointmentTypes { get; set; }
        public virtual DbSet<Prescription> TbPrescriptions { get; set; }
        public virtual DbSet<PrescriptionMedicine> TbPrescriptionMedicines { get; set; }
        public virtual DbSet<MedicalRecord> TbMedicalRecords { get; set; }
        public virtual DbSet<Test> TbTests { get; set; }
        public virtual DbSet<MedicalRadiology> TbMedicalRadiologies { get; set; }
        public virtual DbSet<PrescriptionProtocal> TbPrescriptionProtocal { get; set; }
        public virtual DbSet<PrescriptionMedicineProtocal> TbPrescriptionMedicineProtocal { get; set; }
        public virtual DbSet<Report> TbReports { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new AppoinmentConfiguration());
            builder.ApplyConfiguration(new AppointmentTypeConfiguration());
            builder.ApplyConfiguration(new DoctorConfiguration());
            builder.ApplyConfiguration(new DoctorScheduleConfgiuration());
            builder.ApplyConfiguration(new MedicalRadiologyConfiguration());
            builder.ApplyConfiguration(new MedicalRecordConfiguration());
            builder.ApplyConfiguration(new PatientConfiguration());
            builder.ApplyConfiguration(new PrescriptionConfiguration());
            builder.ApplyConfiguration(new PrescriptionMedicineConfiguration());
            builder.ApplyConfiguration(new ScheduleConfiguration());
            builder.ApplyConfiguration(new SpecializationConfiguration());
            builder.ApplyConfiguration(new TestConfiguration());
            builder.ApplyConfiguration(new PrescriptionProtocalConfiguration());
            builder.ApplyConfiguration(new MedicineProtocalConfiguration());
        }
    }
}
