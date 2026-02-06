import { useState, useEffect } from 'react'
import type { PatientDto } from '../types/api'

const PATIENT_STORAGE_KEY = 'currentPatient'

export const usePatient = () => {
  const [patient, setPatient] = useState<PatientDto | null>(() => {
    const stored = localStorage.getItem(PATIENT_STORAGE_KEY)
    return stored ? JSON.parse(stored) : null
  })

  useEffect(() => {
    if (patient) {
      localStorage.setItem(PATIENT_STORAGE_KEY, JSON.stringify(patient))
    } else {
      localStorage.removeItem(PATIENT_STORAGE_KEY)
    }
  }, [patient])

  const login = (patientData: any) => {
    setPatient(patientData)
  }

  const logout = () => {
    setPatient(null)
  }

  return { patient, login, logout }
}