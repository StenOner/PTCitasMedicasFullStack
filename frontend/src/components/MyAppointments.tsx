import { useState, useEffect, useCallback } from 'react'
import { appointmentService } from '../services/api'
import type { PatientDto, AppointmentDto, AppointmentStatus, ApiError } from '../types/api'
import { format, parseISO, isPast } from 'date-fns'
import { es } from 'date-fns/locale'

interface MyAppointmentsProps {
  patient: PatientDto
  onNewAppointment: () => void
}

const statusNames: Record<AppointmentStatus, string> = {
  1: 'Programada',
  2: 'Confirmada',
  3: 'Cancelada',
  4: 'Completada',
  5: 'No Asistió'
}

const statusColors: Record<AppointmentStatus, string> = {
  1: 'status-scheduled',
  2: 'status-confirmed',
  3: 'status-cancelled',
  4: 'status-completed',
  5: 'status-no-show'
}

export const MyAppointments: React.FC<MyAppointmentsProps> = ({ patient, onNewAppointment }) => {
  const [appointments, setAppointments] = useState<AppointmentDto[]>([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [cancellingId, setCancellingId] = useState<number | null>(null)
  const [cancelReason, setCancelReason] = useState('')

  const loadAppointments = useCallback(async () => {
    setLoading(true)
    setError(null)

    try {
      const response = await appointmentService.getPatientAppointments(patient.id)
      setAppointments(response.data)
    } catch (err) {
      setError((err as ApiError).response?.data?.message || 'Error al cargar citas')
    } finally {
      setLoading(false)
    }
  }, [patient.id])

  useEffect(() => {
    loadAppointments()
  }, [loadAppointments])

  const handleCancelClick = (appointmentId: number) => {
    setCancellingId(appointmentId)
    setCancelReason('')
  }

  const handleCancelConfirm = async (appointmentId: number) => {
    if (!cancelReason.trim()) {
      alert('Por favor ingrese el motivo de cancelación')
      return
    }

    try {
      await appointmentService.cancel(appointmentId, { cancellationReason: cancelReason })
      setCancellingId(null)
      setCancelReason('')
      loadAppointments()
    } catch (err) {
      alert((err as ApiError).response?.data?.message || 'Error al cancelar la cita')
    }
  }

  const formatDateTime = (dateString: string, timeString: string) => {
    try {
      const date = format(parseISO(dateString), "d 'de' MMMM 'de' yyyy", { locale: es })
      return `${date} - ${timeString}`
    } catch {
      return `${dateString} - ${timeString}`
    }
  }

  const canCancel = (appointment: AppointmentDto): boolean => {
    if (appointment.status !== 1 && appointment.status !== 2) return false

    try {
      const appointmentDate = parseISO(appointment.appointmentDate)
      return !isPast(appointmentDate)
    } catch {
      return false
    }
  }

  return (
    <div className="my-appointments">
      <div className="appointments-header">
        <h2>Mis Citas Médicas</h2>
        <button onClick={onNewAppointment} className="btn btn-primary">
          + Nueva Cita
        </button>
      </div>

      {loading && <div className="loading">Cargando citas...</div>}
      {error && <div className="alert alert-error">{error}</div>}

      {!loading && appointments.length === 0 && (
        <div className="empty-state">
          <p>📅 No tienes citas programadas</p>
        </div>
      )}

      {!loading && appointments.length > 0 && (
        <div className="appointments-list">
          {appointments.map((appointment) => (
            <div key={appointment.id} className="appointment-card">
              <div className="appointment-header">
                <span className={`status-badge ${statusColors[appointment.status]}`}>
                  {statusNames[appointment.status]}
                </span>
                <span className="appointment-id">Cita #{appointment.id}</span>
              </div>

              <div className="appointment-body">
                <div className="appointment-info">
                  <h3>👨‍⚕️ {appointment.doctorName}</h3>
                  <p className="specialty">{appointment.specialtyName}</p>
                  <p className="datetime">
                    📅 {formatDateTime(appointment.appointmentDate, appointment.timeRange)}
                  </p>
                  {appointment.notes && (
                    <p className="notes">
                      <strong>Notas:</strong> {appointment.notes}
                    </p>
                  )}
                  {appointment.cancellationReason && (
                    <p className="cancellation-reason">
                      <strong>Motivo de cancelación:</strong> {appointment.cancellationReason}
                    </p>
                  )}
                </div>

                {canCancel(appointment) && (
                  <div className="appointment-actions">
                    {cancellingId === appointment.id ? (
                      <div className="cancel-form">
                        <textarea
                          value={cancelReason}
                          onChange={(e) => setCancelReason(e.target.value)}
                          placeholder="Motivo de cancelación..."
                          rows={3}
                        />
                        <div className="cancel-buttons">
                          <button
                            onClick={() => setCancellingId(null)}
                            className="btn btn-secondary btn-sm"
                          >
                            Volver
                          </button>
                          <button
                            onClick={() => handleCancelConfirm(appointment.id)}
                            className="btn btn-danger btn-sm"
                          >
                            Confirmar Cancelación
                          </button>
                        </div>
                      </div>
                    ) : (
                      <button
                        onClick={() => handleCancelClick(appointment.id)}
                        className="btn btn-danger btn-sm"
                      >
                        Cancelar Cita
                      </button>
                    )}
                  </div>
                )}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}