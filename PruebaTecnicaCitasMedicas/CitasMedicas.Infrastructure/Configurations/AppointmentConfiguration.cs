using CitasMedicas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitasMedicas.Infrastructure.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.AppointmentDate).HasColumnType("date");
        builder.Property(e => e.Status).HasConversion<int>();
        builder.Property(e => e.Notes).HasMaxLength(1000);
        builder.Property(e => e.CancellationReason).HasMaxLength(500);

        builder.HasIndex(e => new { e.PatientId, e.AppointmentDate, e.StartTime });
        builder.HasIndex(e => new { e.ScheduleId }).IsUnique()
            .HasFilter("[Status] != 3"); // No canceladas

        builder.HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Schedule)
            .WithMany()
            .HasForeignKey(a => a.ScheduleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(e => e.Patient).AutoInclude();
        builder.Navigation(e => e.Doctor).AutoInclude();
    }
}
