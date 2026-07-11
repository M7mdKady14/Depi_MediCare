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
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            #region Properties
            builder.HasKey(p => p.Id);
            builder.Property(a => a.CurrentState).HasDefaultValue(CurrentState.Active).HasSentinel(0);

            #region Name Property
            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.MiddleName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(50);
            #endregion

            builder.Property(p => p.NationalNumber)
                .IsRequired()
                .HasMaxLength(14);

            builder.Property(p => p.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(p => p.Email)
                .HasMaxLength(150);

            builder.Property(p => p.BirthDate)
                .IsRequired();

            builder.Property(p => p.Gender)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(p => p.BloodType)
                .IsRequired()
                .HasConversion<string>();
            #endregion
        }
    }
}
