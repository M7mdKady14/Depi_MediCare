using Clinic.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Clinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Infrastructure.Data
{
    public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            #region Properties
            builder.HasKey(m => m.Id);
            builder.Property(a => a.CurrentState).HasDefaultValue(CurrentState.Active).HasSentinel(0);

            builder.Property(a => a.Allergy)
                .HasMaxLength(1000);

            builder.Property(a => a.Notes)
                .HasMaxLength(1000);

            builder.Property(a => a.ChronicDisease)
                .HasMaxLength(1000);

            builder.Property(a => a.Diagnosis)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(a => a.CurrentMedications)
                .HasMaxLength(1000);
            #endregion

            #region RelationsShips
            builder.HasOne(a => a.Doctor)
                .WithMany(a => a.MedicalRecords)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Patient)
                .WithMany(a => a.MedicalRecords)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
