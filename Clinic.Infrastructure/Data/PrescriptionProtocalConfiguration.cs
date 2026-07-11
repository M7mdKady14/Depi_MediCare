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
    public class PrescriptionProtocalConfiguration : IEntityTypeConfiguration<PrescriptionProtocal>
    {
        public void Configure(EntityTypeBuilder<PrescriptionProtocal> builder)
        {
            #region Properties Configuration
            builder.HasKey(x => x.Id);
            builder.Property(a => a.CurrentState).HasDefaultValue(CurrentState.Active).HasSentinel(0);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.Code).HasMaxLength(30);
            builder.Property(x => x.Disease).IsRequired().HasMaxLength(100);
            #endregion

            #region Relationship Configuration 
            builder.HasOne(a => a.Doctor)
                .WithMany(a => a.PrescriptionProtocals)
                    .HasForeignKey(a => a.DoctorId)
                        .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
