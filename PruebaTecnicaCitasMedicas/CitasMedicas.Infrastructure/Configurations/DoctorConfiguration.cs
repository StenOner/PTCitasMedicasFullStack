using CitasMedicas.Domain.Entities;
using CitasMedicas.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitasMedicas.Infrastructure.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctors");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.LicenseNumber).IsRequired().HasMaxLength(50);
        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Email).IsRequired().HasMaxLength(150);
        builder.Property(e => e.Phone).IsRequired().HasMaxLength(20);
        builder.Property(e => e.CareType).HasConversion<int>();

        builder.HasIndex(e => e.LicenseNumber).IsUnique();
        builder.HasIndex(e => new { e.SpecialtyId, e.CareType });

        builder.HasOne(d => d.Specialty)
            .WithMany(s => s.Doctors)
            .HasForeignKey(d => d.SpecialtyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Doctor { Id = 1, LicenseNumber = "MED-001", FirstName = "Juan", LastName = "Pérez", Email = "juan.perez@hospital.com", Phone = "+51987654321", SpecialtyId = 1, CareType = CareType.Consulta, CreatedAt = new DateTime(2026, 02, 06), IsActive = true },
            new Doctor { Id = 2, LicenseNumber = "MED-002", FirstName = "María", LastName = "García", Email = "maria.garcia@hospital.com", Phone = "+51987654322", SpecialtyId = 2, CareType = CareType.Consulta, CreatedAt = new DateTime(2026, 02, 06), IsActive = true },
            new Doctor { Id = 3, LicenseNumber = "MED-003", FirstName = "Carlos", LastName = "López", Email = "carlos.lopez@hospital.com", Phone = "+51987654323", SpecialtyId = 3, CareType = CareType.Control, CreatedAt = new DateTime(2026, 02, 06), IsActive = true },
            new Doctor { Id = 4, LicenseNumber = "MED-004", FirstName = "Ana", LastName = "Martínez", Email = "ana.martinez@hospital.com", Phone = "+51987654324", SpecialtyId = 4, CareType = CareType.Emergencia, CreatedAt = new DateTime(2026, 02, 06), IsActive = true },
            new Doctor { Id = 5, LicenseNumber = "MED-005", FirstName = "Luis", LastName = "Rodríguez", Email = "luis.rodriguez@hospital.com", Phone = "+51987654325", SpecialtyId = 5, CareType = CareType.Consulta, CreatedAt = new DateTime(2026, 02, 06), IsActive = true }
        );

        builder.Navigation(e => e.Specialty).AutoInclude();
    }
}
