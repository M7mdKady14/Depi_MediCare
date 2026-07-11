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
    public class DoctorScheduleConfgiuration : IEntityTypeConfiguration<DoctorSchedule>
    {
        public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
        {
            #region Properties
            builder.HasKey(x => x.Id);
            builder.Property(a => a.CurrentState).HasDefaultValue(CurrentState.Active).HasSentinel(0);
            #endregion

            #region RelationsShips
            builder.HasOne(a => a.Doctor)
                .WithMany(a => a.DoctorSchedules)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Schedule)
                .WithMany(a => a.DoctorSchedules)
                .HasForeignKey(a => a.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}

