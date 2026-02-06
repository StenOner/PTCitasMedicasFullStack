using CitasMedicas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitasMedicas.Infrastructure.Configurations;

public class DoctorScheduleConfiguration : IEntityTypeConfiguration<DoctorSchedule>
{
    public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
    {
        builder.ToTable("DoctorSchedules");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.ScheduleDate).HasColumnType("date");

        builder.HasIndex(e => new { e.DoctorId, e.ScheduleDate, e.StartTime }).IsUnique();

        builder.HasOne(ds => ds.Doctor)
            .WithMany(d => d.Schedules)
            .HasForeignKey(ds => ds.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

        #region setup data
        var schedules = new List<DoctorSchedule>();
        int scheduleId = 1;

        for (int day = 1; day <= 7; day++)
        {
            var today = new DateTime(2026, 02, 06);
            var date = today.AddDays(day);

            for (int doctorId = 1; doctorId <= 5; doctorId++)
            {
                // Horarios de mañana: 8:00 AM - 12:00 PM (cada hora)
                for (int hour = 8; hour < 12; hour++)
                {
                    schedules.Add(new DoctorSchedule
                    {
                        Id = scheduleId++,
                        DoctorId = doctorId,
                        ScheduleDate = date,
                        StartTime = new TimeSpan(hour, 0, 0),
                        EndTime = new TimeSpan(hour + 1, 0, 0),
                        IsAvailable = true,
                        CreatedAt = today,
                        IsActive = true
                    });
                }

                // Horarios de tarde: 2:00 PM - 6:00 PM (cada hora)
                for (int hour = 14; hour < 18; hour++)
                {
                    schedules.Add(new DoctorSchedule
                    {
                        Id = scheduleId++,
                        DoctorId = doctorId,
                        ScheduleDate = date,
                        StartTime = new TimeSpan(hour, 0, 0),
                        EndTime = new TimeSpan(hour + 1, 0, 0),
                        IsAvailable = true,
                        CreatedAt = today,
                        IsActive = true
                    });
                }
            }
        }

        builder.HasData(schedules);
        #endregion
    }
}
