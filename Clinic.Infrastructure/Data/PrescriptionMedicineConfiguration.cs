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
    public class PrescriptionMedicineConfiguration : IEntityTypeConfiguration<PrescriptionMedicine>
    {
        public void Configure(EntityTypeBuilder<PrescriptionMedicine> builder)
        {
            #region Properties
            builder.HasKey(p => p.Id);
            builder.Property(a => a.CurrentState).HasDefaultValue(CurrentState.Active).HasSentinel(0);

            builder.Property(a => a.MedicineName)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(a => a.Dosage)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(a => a.Frequency)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(a => a.Duration)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(a => a.Instructions)
                .HasMaxLength(500);
            #endregion

            #region RelationShips
            builder.HasOne(p => p.Prescription)
                .WithMany(p => p.PrescriptionMedicines)
                .HasForeignKey(p => p.PrescriptionId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
