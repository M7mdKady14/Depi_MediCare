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
    public class MedicalRadiologyConfiguration : IEntityTypeConfiguration<MedicalRadiology>
    {
        public void Configure(EntityTypeBuilder<MedicalRadiology> builder)
        {
            #region Properties
            builder.HasKey(x => x.Id);
            builder.Property(a => a.CurrentState).HasDefaultValue(CurrentState.Active).HasSentinel(0);

            //builder.Property(a => a.File).IsRequired();
            builder.Property(a => a.Description).HasMaxLength(1000);
            #endregion

            #region RelationShips
            builder.HasOne(a => a.MedicalRecord)
                .WithMany(a => a.MedicalRadiologies)
                .HasForeignKey(a => a.MedicalRecordId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
