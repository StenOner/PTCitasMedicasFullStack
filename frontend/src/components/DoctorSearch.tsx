import { useState, useEffect } from 'react'
import { specialtyService, doctorService } from '../services/api'
import type { SpecialtyDto, DoctorDto, CareType, ApiError } from '../types/api'

interface DoctorSearchProps {
  onDoctorSelect: (doctor: DoctorDto) => void
}

const careTypeNames: Record<number, string> = {
  1: 'Consulta',
  2: 'Control',
  3: 'Emergencia',
  4: 'Procedimiento'
}

export const DoctorSearch: React.FC<DoctorSearchProps> = ({ onDoctorSelect }) => {
  const [specialties, setSpecialties] = useState<SpecialtyDto[]>([])
  const [doctors, setDoctors] = useState<DoctorDto[]>([])
  const [selectedSpecialty, setSelectedSpecialty] = useState<number | ''>('')
  const [selectedCareType, setSelectedCareType] = useState<CareType | ''>('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    loadSpecialties()
  }, [])

  const loadSpecialties = async () => {
    try {
      const response = await specialtyService.getAll()
      setSpecialties(response.data)
    } catch {
      setError('Error al cargar especialidades')
    }
  }

  const handleSearch = async () => {
    setLoading(true)
    setError(null)

    try {
      const response = await doctorService.search(
        selectedSpecialty || undefined,
        selectedCareType || undefined
      )
      setDoctors(response.data)

      if (response.data.length === 0) {
        setError('No se encontraron médicos con los criterios seleccionados')
      }
    } catch (err) {
      setError((err as ApiError).response?.data?.message || 'Error al buscar médicos')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="doctor-search">
      <h2>Buscar Médicos</h2>

      <div className="search-filters">
        <div className="form-group">
          <label htmlFor="specialty">Especialidad</label>
          <select
            id="specialty"
            value={selectedSpecialty}
            onChange={(e) => setSelectedSpecialty(e.target.value ? Number(e.target.value) : '')}
          >
            <option value="">Todas las especialidades</option>
            {specialties.map((specialty) => (
              <option key={specialty.id} value={specialty.id}>
                {specialty.name}
              </option>
            ))}
          </select>
        </div>

        <div className="form-group">
          <label htmlFor="careType">Tipo de Atención</label>
          <select
            id="careType"
            value={selectedCareType}
            onChange={(e) => setSelectedCareType(e.target.value ? Number(e.target.value) as CareType : '')}
          >
            <option value="">Todos los tipos</option>
            <option value="1">Consulta</option>
            <option value="2">Control</option>
            <option value="3">Emergencia</option>
            <option value="4">Procedimiento</option>
          </select>
        </div>

        <button className="btn btn-primary form-group" onClick={handleSearch} disabled={loading}>
          {loading ? 'Buscando...' : 'Buscar'}
        </button>
      </div>

      {error && <div className="alert alert-error">{error}</div>}

      {doctors.length > 0 && (
        <div className="doctors-list">
          <h3>Médicos Disponibles ({doctors.length})</h3>
          <div className="doctors-grid">
            {doctors.map((doctor) => (
              <div key={doctor.id} className="doctor-card">
                <div className="doctor-icon">👨‍⚕️</div>
                <h4>{doctor.fullName}</h4>
                <p className="specialty">{doctor.specialtyName}</p>
                <p className="care-type">
                  <span className="badge">{careTypeNames[doctor.careType]}</span>
                </p>
                <p className="doctor-info">
                  📧 {doctor.email}<br />
                  📞 {doctor.phone}
                </p>
                <button
                  onClick={() => onDoctorSelect(doctor)}
                  className="btn btn-outline"
                >
                  Ver Horarios
                </button>
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  )
}