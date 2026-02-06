using CitasMedicas.Domain.Entities;

namespace CitasMedicas.Domain.Interfaces;

public interface IUnitOfWork
{
    IRepository<Patient> Patients { get; }
    IRepository<Doctor> Doctors { get; }
    IRepository<Specialty> Specialties { get; }
    IRepository<DoctorSchedule> DoctorSchedules { get; }
    IRepository<Appointment> Appointments { get; }

    Task<int> SaveChangesAsync();
}
