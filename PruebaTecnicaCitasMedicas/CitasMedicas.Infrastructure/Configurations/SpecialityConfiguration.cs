using CitasMedicas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitasMedicas.Infrastructure.Configurations;

public class SpecialityConfiguration : IEntityTypeConfiguration<Specialty>
{
    public void Configure(EntityTypeBuilder<Specialty> builder)
    {
        builder.ToTable("Specialties");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Description).HasMaxLength(500);

        builder.HasIndex(e => e.Name).IsUnique();

        builder.HasData(
            new Specialty { Id = 1, Name = "Cardiología", Description = "Especialidad médica del corazón", CreatedAt = new DateTime(2026, 02, 06), IsActive = true },
            new Specialty { Id = 2, Name = "Pediatría", Description = "Especialidad médica infantil", CreatedAt = new DateTime(2026, 02, 06), IsActive = true },
            new Specialty { Id = 3, Name = "Dermatología", Description = "Especialidad médica de la piel", CreatedAt = new DateTime(2026, 02, 06), IsActive = true },
            new Specialty { Id = 4, Name = "Traumatología", Description = "Especialidad del tratamiento a lesiones traumáticas", CreatedAt = new DateTime(2026, 02, 06), IsActive = true },
            new Specialty { Id = 5, Name = "Medicina General", Description = "Atención médica general", CreatedAt = new DateTime(2026, 02, 06), IsActive = true }
            );
    }
}
