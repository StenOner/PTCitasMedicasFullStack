import { useState } from 'react'
import { patientService } from '../services/api'
import type { ApiError, PatientDto } from '../types/api'

interface PatientLoginProps {
  onSuccess: (patient: PatientDto) => void
  onRegisterClick: () => void
}

export const PatientLogin: React.FC<PatientLoginProps> = ({ onSuccess, onRegisterClick }) => {
  const [documentNumber, setDocumentNumber] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const handleSubmit = async (e: React.SubmitEvent) => {
    e.preventDefault()
    setLoading(true)
    setError(null)

    try {
      const response = await patientService.getByDocument(documentNumber)
      onSuccess(response.data)
    } catch (err) {
      setError((err as ApiError).response?.data?.message || 'Paciente no encontrado')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="login-form">
      <h2>Iniciar Sesión</h2>
      <p className="subtitle">Ingresa tu número de documento para continuar</p>

      {error && <div className="alert alert-error">{error}</div>}

      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="documentNumber">Número de Documento</label>
          <input
            type="text"
            id="documentNumber"
            value={documentNumber}
            onChange={(e) => setDocumentNumber(e.target.value)}
            placeholder="Ej: 12345678"
            required
            maxLength={20}
          />
        </div>

        <button type="submit" disabled={loading} className="btn btn-primary">
          {loading ? 'Verificando...' : 'Ingresar'}
        </button>
      </form>

      <div className="divider">o</div>

      <button onClick={onRegisterClick} className="btn btn-secondary">
        Registrarse como Nuevo Paciente
      </button>
    </div>
  )
}