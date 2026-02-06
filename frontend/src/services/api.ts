import axios from 'axios'
import type {
  ApiResponse,
  CreatePatientDto,
  PatientDto,
  SpecialtyDto,
  DoctorDto,
  ScheduleDto,
  CreateAppointmentDto,
  AppointmentDto,
  CancelAppointmentDto,
  CareType
} from '../types/api'

const API_BASE_URL = 'https://localhost:7000/api'

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
})

export const patientService = {
  register: async (data: CreatePatientDto): Promise<ApiResponse<PatientDto>> => {
    const response = await api.post('/patients', data)
    return response.data
  },

  getById: async (id: number): Promise<ApiResponse<PatientDto>> => {
    const response = await api.get(`/patients/${id}`)
    return response.data
  },

  getByDocument: async (documentNumber: string): Promise<ApiResponse<PatientDto>> => {
    const response = await api.get(`/patients/by-document/${documentNumber}`)
    return response.data
  }
}

export const specialtyService = {
  getAll: async (): Promise<ApiResponse<SpecialtyDto[]>> => {
    const response = await api.get('/specialties')
    return response.data
  }
}

export const doctorService = {
  search: async (specialtyId?: number, careType?: CareType): Promise<ApiResponse<DoctorDto[]>> => {
    const params = new URLSearchParams()
    if (specialtyId) params.append('specialtyId', specialtyId.toString())
    if (careType) params.append('careType', careType.toString())

    const response = await api.get(`/doctors/search?${params.toString()}`)
    return response.data
  },

  getById: async (id: number): Promise<ApiResponse<DoctorDto>> => {
    const response = await api.get(`/doctors/${id}`)
    return response.data
  }
}

export const scheduleService = {
  getAvailable: async (
    doctorId: number,
    startDate?: string,
    endDate?: string
  ): Promise<ApiResponse<ScheduleDto[]>> => {
    const params = new URLSearchParams({ doctorId: doctorId.toString() })
    if (startDate) params.append('startDate', startDate)
    if (endDate) params.append('endDate', endDate)

    const response = await api.get(`/schedules/available?${params.toString()}`)
    return response.data
  }
}

export const appointmentService = {
  create: async (data: CreateAppointmentDto): Promise<ApiResponse<AppointmentDto>> => {
    const response = await api.post('/appointments', data)
    return response.data
  },

  getPatientAppointments: async (
    patientId: number,
    includePast: boolean = false
  ): Promise<ApiResponse<AppointmentDto[]>> => {
    const response = await api.get(
      `/appointments/patient/${patientId}?includePast=${includePast}`
    )
    return response.data
  },

  cancel: async (
    appointmentId: number,
    data: CancelAppointmentDto
  ): Promise<ApiResponse<AppointmentDto>> => {
    const response = await api.patch(`/appointments/${appointmentId}/cancel`, data)
    return response.data
  }
}

export default api