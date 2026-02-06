using CitasMedicas.Application.Services;
using CitasMedicas.Application.Validators;
using CitasMedicas.Domain.Interfaces;
using CitasMedicas.Infrastructure;
using CitasMedicas.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CitasMedicas.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
        });
    }

    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IScheduleService, ScheduleService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<ISpecialtyService, SpecialtyService>();
    }

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly("CitasMedicas.API")));
    }

    public static void AddValidationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateAppointmentValidator>();
        services.AddValidatorsFromAssemblyContaining<CreatePatientValidator>();
    }
}
