import { useEffect, useState } from 'react'
import { usePatient } from './hooks/usePatient'
import { PatientLogin } from './components/PatientLogin'
import { PatientRegistration } from './components/PatientRegistration'
import { DoctorSearch } from './components/DoctorSearch'
import { ScheduleSelector } from './components/ScheduleSelector'
import { AppointmentBooking } from './components/AppointmentBooking'
import { MyAppointments } from './components/MyAppointments'
import { LogoutIcon } from './components/LogoutIcon'
import type { DoctorDto, PatientDto, ScheduleDto } from './types/api'
import './App.css'

type View = 'login' | 'register' | 'my-appointments' | 'new-appointment' | 'schedules' | 'booking' | 'success'

function App() {
  const { patient, login, logout } = usePatient()
  const [currentView, setCurrentView] = useState<View>('login')
  const [selectedDoctor, setSelectedDoctor] = useState<DoctorDto | null>(null)
  const [selectedSchedule, setSelectedSchedule] = useState<ScheduleDto | null>(null)

  useEffect(() => {
    if (patient) setCurrentView('my-appointments')
  }, [patient])

  // Navigation handlers
  const handleLoginSuccess = (patientData: PatientDto) => {
    login(patientData)
    setCurrentView('my-appointments')
  }

  const handleRegisterClick = () => {
    setCurrentView('register')
  }

  const handleRegistrationSuccess = async (patientId: number) => {
    const { patientService } = await import('./services/api')
    const response = await patientService.getById(patientId)
    login(response.data)
    setCurrentView('my-appointments')
  }

  const handleNewAppointmentClick = () => {
    setCurrentView('new-appointment')
    setSelectedDoctor(null)
    setSelectedSchedule(null)
  }

  const handleDoctorSelect = (doctor: DoctorDto) => {
    setSelectedDoctor(doctor)
    setCurrentView('schedules')
  }

  const handleScheduleSelect = (schedule: ScheduleDto) => {
    setSelectedSchedule(schedule)
    setCurrentView('booking')
  }

  const handleBookingSuccess = () => {
    setCurrentView('success')
    setTimeout(() => {
      setCurrentView('my-appointments')
      setSelectedDoctor(null)
      setSelectedSchedule(null)
    }, 3000)
  }

  const handleBackToSearch = () => {
    setCurrentView('new-appointment')
    setSelectedDoctor(null)
    setSelectedSchedule(null)
  }

  const handleBackToSchedules = () => {
    setCurrentView('schedules')
    setSelectedSchedule(null)
  }

  const handleLogout = () => {
    logout()
    setCurrentView('login')
    setSelectedDoctor(null)
    setSelectedSchedule(null)
  }

  return (
    <div className="app">
      <header className="app-header">
        <div className="container">
          <h1 className="cursor-pointer" onClick={() => setCurrentView('my-appointments')}>
            Sistema de Citas Médicas
          </h1>
          {patient && (
            <div className="user-info">
              <span>Hola, {patient.firstName}</span>
              <button onClick={handleLogout} className="btn btn-link">
                <LogoutIcon color='white' />
              </button>
            </div>
          )}
        </div>
      </header>

      <main className="app-main">
        <div className="container">
          {/* Login View */}
          {!patient && currentView === 'login' && (
            <PatientLogin
              onSuccess={handleLoginSuccess}
              onRegisterClick={handleRegisterClick}
            />
          )}

          {/* Registration View */}
          {!patient && currentView === 'register' && (
            <div>
              <button onClick={() => setCurrentView('login')} className="btn btn-link">
                ← Volver al inicio
              </button>
              <PatientRegistration onSuccess={handleRegistrationSuccess} />
            </div>
          )}

          {/* My Appointments View */}
          {patient && currentView === 'my-appointments' && (
            <MyAppointments
              patient={patient}
              onNewAppointment={handleNewAppointmentClick}
            />
          )}

          {/* New Appointment - Doctor Search */}
          {patient && currentView === 'new-appointment' && (
            <div>
              <button onClick={() => setCurrentView('my-appointments')} className="btn btn-link">
                ← Volver a mis citas
              </button>
              <DoctorSearch onDoctorSelect={handleDoctorSelect} />
            </div>
          )}

          {/* Schedule Selection */}
          {patient && currentView === 'schedules' && selectedDoctor && (
            <ScheduleSelector
              doctor={selectedDoctor}
              onScheduleSelect={handleScheduleSelect}
              onBack={handleBackToSearch}
            />
          )}

          {/* Appointment Booking */}
          {patient && currentView === 'booking' && selectedDoctor && selectedSchedule && (
            <AppointmentBooking
              doctor={selectedDoctor}
              schedule={selectedSchedule}
              patient={patient}
              onSuccess={handleBookingSuccess}
              onCancel={handleBackToSchedules}
            />
          )}

          {/* Success View */}
          {currentView === 'success' && (
            <div className="success-view">
              <div className="success-icon">✅</div>
              <h2>¡Cita Reservada Exitosamente!</h2>
              <p>Tu cita ha sido programada correctamente.</p>
              <p className="redirect-message">Redirigiendo a tus citas...</p>
            </div>
          )}
        </div>
      </main>

      <footer className="app-footer">
        <div className="container">
          <p>Prueba Tecnica - Sistema de Citas Médicas</p>
          <p>Leonardo Acevedo</p>
        </div>
      </footer>
    </div>
  )
}

export default App