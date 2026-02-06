import { useState, useEffect, useCallback } from 'react'
import { scheduleService } from '../services/api'
import type { ApiError, DoctorDto, ScheduleDto } from '../types/api'
import { format, parseISO } from 'date-fns'
import { es } from 'date-fns/locale'

interface ScheduleSelectorProps {
  doctor: DoctorDto
  onScheduleSelect: (schedule: ScheduleDto) => void
  onBack: () => void
}

export const ScheduleSelector: React.FC<ScheduleSelectorProps> = ({
  doctor,
  onScheduleSelect,
  onBack
}) => {
  const [, setSchedules] = useState<ScheduleDto[]>([])
  const [groupedSchedules, setGroupedSchedules] = useState<Record<string, ScheduleDto[]>>({})
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const loadSchedules = useCallback(async () => {
    setLoading(true)
    setError(null)

    try {
      const today = new Date().toISOString().split('T')[0]
      const endDate = new Date()
      endDate.setDate(endDate.getDate() + 14) // Próximos 14 días
      const endDateStr = endDate.toISOString().split('T')[0]

      const response = await scheduleService.getAvailable(doctor.id, today, endDateStr)
      setSchedules(response.data)

      // Group schedules by date
      const grouped = response.data.reduce((acc, schedule) => {
        const date = schedule.scheduleDate
        if (!acc[date]) {
          acc[date] = []
        }
        acc[date].push(schedule)
        return acc
      }, {} as Record<string, ScheduleDto[]>)

      setGroupedSchedules(grouped)

      if (response.data.length === 0) {
        setError('No hay horarios disponibles para este médico en los próximos días')
      }
    } catch (err) {
      setError((err as ApiError).response?.data?.message || 'Error al cargar horarios')
    } finally {
      setLoading(false)
    }
  }, [doctor.id])

  useEffect(() => {
    loadSchedules()
  }, [loadSchedules])

  const formatDate = (dateString: string) => {
    try {
      return format(parseISO(dateString), "EEEE d 'de' MMMM", { locale: es })
    } catch {
      return dateString
    }
  }

  return (
    <div className="schedule-selector">
      <button onClick={onBack} className="btn btn-link">← Volver a búsqueda</button>

      <div className="doctor-header">
        <h2>Horarios de {doctor.fullName}</h2>
        <p className="specialty">{doctor.specialtyName}</p>
      </div>

      {loading && <div className="loading">Cargando horarios...</div>}
      {error && <div className="alert alert-error">{error}</div>}

      {!loading && Object.keys(groupedSchedules).length > 0 && (
        <div className="schedules-by-date">
          {Object.entries(groupedSchedules).map(([date, dateSchedules]) => (
            <div key={date} className="date-group">
              <h3 className="date-header">{formatDate(date)}</h3>
              <div className="time-slots">
                {dateSchedules.map((schedule) => (
                  <button
                    key={schedule.id}
                    className="time-slot"
                    onClick={() => onScheduleSelect(schedule)}
                  >
                    <span className="time">{schedule.timeRange}</span>
                    <span className="available">✓ Disponible</span>
                  </button>
                ))}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}