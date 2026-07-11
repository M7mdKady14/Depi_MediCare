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
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            #region Properties
            builder.HasKey(a => a.Id);
            builder.Property(a => a.CurrentState).HasDefaultValue(CurrentState.Active).HasSentinel(0);
            builder.Property(a => a.PhoneNumber).HasMaxLength(15).IsRequired();
            builder.Property(a => a.Description).HasMaxLength(500);
            builder.Property(a => a.MaximumAppointments).IsRequired();
            #endregion

            #region Relationships
            builder.HasOne(a => a.Specialization)
                .WithMany(s => s.Doctors)
                .HasForeignKey(a => a.SpecializationId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
