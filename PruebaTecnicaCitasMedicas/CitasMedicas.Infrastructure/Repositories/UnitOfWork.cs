using CitasMedicas.Domain.Entities;
using CitasMedicas.Domain.Interfaces;

namespace CitasMedicas.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IRepository<Patient> Patients { get; }
    public IRepository<Doctor> Doctors { get; }
    public IRepository<Specialty> Specialties { get; }
    public IRepository<DoctorSchedule> DoctorSchedules { get; }
    public IRepository<Appointment> Appointments { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Patients = new Repository<Patient>(_context);
        Doctors = new Repository<Doctor>(_context);
        Specialties = new Repository<Specialty>(_context);
        DoctorSchedules = new Repository<DoctorSchedule>(_context);
        Appointments = new Repository<Appointment>(_context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
