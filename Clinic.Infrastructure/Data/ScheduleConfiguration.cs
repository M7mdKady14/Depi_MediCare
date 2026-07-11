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
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            #region Properties
            builder.HasKey(a => a.Id);
            builder.Property(a => a.CurrentState).HasDefaultValue(CurrentState.Active).HasSentinel(0);

            builder.Property(a => a.Day)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(a => a.StartTime)
                .IsRequired();

            builder.Property(a => a.EndTime)
                .IsRequired();
            #endregion
        }
    }
}
