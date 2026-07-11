using Clinic.Core.Entities;
using Clinic.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Infrastructure.Data
{
    public class AppoinmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            #region Properties
            builder.HasKey(a => a.Id);
            builder.Property(a => a.CurrentState).HasDefaultValue(CurrentState.Active).HasSentinel(0);
            builder.Property(a => a.AppointmentDate).IsRequired();
            builder.Property(a =>a.AppointmentStatus).IsRequired().HasDefaultValue(AppointmentStatus.Created).HasSentinel(0);
            #endregion

            #region RelationShips
            builder.HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.AppointmentType)
                .WithMany(at => at.Appointments)
                .HasForeignKey(a => a.AppointmentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Prescription)
                .WithOne(p => p.Appointment)
                .HasForeignKey<Prescription>(p => p.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(a => a.DoctorSchedule)
                .WithMany(at => at.Appointments)
                .HasForeignKey(a => a.DoctorScheduleId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
