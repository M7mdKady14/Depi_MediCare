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
    public class AppointmentTypeConfiguration : IEntityTypeConfiguration<AppointmentType>
    {
        public void Configure(EntityTypeBuilder<AppointmentType> builder)
        {
            #region Properties
            builder.HasKey(a => a.Id);
            builder.Property(a => a.CurrentState).HasDefaultValue(CurrentState.Active).HasSentinel(0);
            builder.Property(a => a.EName).IsRequired().HasMaxLength(100);
            builder.Property(a => a.AName).HasMaxLength(100);
            builder.Property(a => a.Description).HasMaxLength(500);
            #endregion
        }
    }
}
