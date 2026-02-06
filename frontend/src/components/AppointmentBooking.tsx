import React, { useState } from 'react'
import { appointmentService } from '../services/api'
import type { DoctorDto, ScheduleDto, PatientDto, CreateAppointmentDto } from '../types/api'
import { format, parseISO } from 'date-fns'
import { es } from 'date-fns/locale'

interface AppointmentBookingProps {
  doctor: DoctorDto
  schedule: ScheduleDto
  patient: PatientDto
  onSuccess: () => void
  onCancel: () => void
}

export const AppointmentBooking: React.FC<AppointmentBookingProps> = ({
  doctor,
  schedule,
  patient,
  onSuccess,
  onCancel
}) => {
  const [notes, setNotes] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const formatDate = (dateString: string) => {
    try {
      return format(parseISO(dateString), "EEEE d 'de' MMMM 'de' yyyy", { locale: es })
    } catch {
      return dateString
    }
  }

  const handleConfirm = async () => {
    setLoading(true)
    setError(null)

    const appointmentData: CreateAppointmentDto = {
      patientId: patient.id,
      scheduleId: schedule.id,
      notes: notes || undefined
    }

    try {
      await appointmentService.create(appointmentData)
      onSuccess()
    } catch (err: any) {
      const errorMessage = err.response?.data?.message || 'Error al reservar la cita'
      setError(errorMessage)
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="appointment-booking">
      <h2>Confirmar Cita Médica</h2>

      <div className="booking-summary">
        <div className="summary-section">
          <h3>👨‍⚕️ Médico</h3>
          <p className="name">{doctor.fullName}</p>
          <p className="specialty">{doctor.specialtyName}</p>
        </div>

        <div className="summary-section">
          <h3>📅 Fecha y Hora</h3>
          <p className="date">{formatDate(schedule.scheduleDate)}</p>
          <p className="time">{schedule.timeRange}</p>
        </div>

        <div className="summary-section">
          <h3>👤 Paciente</h3>
          <p className="name">{patient.fullName}</p>
          <p className="detail">DNI: {patient.documentNumber}</p>
        </div>
      </div>

      <div className="form-group">
        <label htmlFor="notes">Notas o Motivo de Consulta (Opcional)</label>
        <textarea
          id="notes"
          value={notes}
          onChange={(e) => setNotes(e.target.value)}
          placeholder="Describe brevemente el motivo de tu consulta..."
          rows={4}
          maxLength={1000}
        />
      </div>

      {error && <div className="alert alert-error">{error}</div>}

      <div className="booking-actions">
        <button onClick={onCancel} className="btn btn-secondary" disabled={loading}>
          Cancelar
        </button>
        <button onClick={handleConfirm} className="btn btn-primary" disabled={loading}>
          {loading ? 'Reservando...' : 'Confirmar Cita'}
        </button>
      </div>
    </div>
  )
}