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
    public class MedicineProtocalConfiguration : IEntityTypeConfiguration<PrescriptionMedicineProtocal>
    {
        public void Configure(EntityTypeBuilder<PrescriptionMedicineProtocal> builder)
        {
            #region Properties Configuration
            builder.HasKey(x => x.Id);
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

            #region Relationship Configuration 
            builder.HasOne(a => a.PrescriptionProtocal)
                .WithMany(a => a.PrescriptionMedicineProtocal)
                    .HasForeignKey(a => a.ProtocalId)
                        .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }     
    }
}
