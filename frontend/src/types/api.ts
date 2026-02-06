export interface ApiResponse<T> {
  data: T
  message: string
  errors?: string[]
}

export interface ApiError {
  response?: {
    data?: {
      message?: string
      errors?: string[]
    }
  }
}

export interface CreatePatientDto {
  documentNumber: string
  firstName: string
  lastName: string
  email: string
  phone: string
  dateOfBirth: string
  address: string
}

export interface PatientDto {
  id: number
  documentNumber: string
  firstName: string
  lastName: string
  fullName: string
  email: string
  phone: string
  dateOfBirth: string
  address: string
}

export interface SpecialtyDto {
  id: number
  name: string
  description: string
}

export interface DoctorDto {
  id: number
  licenseNumber: string
  firstName: string
  lastName: string
  fullName: string
  email: string
  phone: string
  specialtyId: number
  specialtyName: string
  careType: CareType
  careTypeName: string
}

export interface ScheduleDto {
  id: number
  doctorId: number
  doctorName: string
  scheduleDate: string
  startTime: string
  endTime: string
  isAvailable: boolean
  timeRange: string
}

export interface CreateAppointmentDto {
  patientId: number
  scheduleId: number
  notes?: string
}

export interface AppointmentDto {
  id: number
  patientId: number
  patientName: string
  doctorId: number
  doctorName: string
  specialtyName: string
  appointmentDate: string
  startTime: string
  endTime: string
  timeRange: string
  status: AppointmentStatus
  statusName: string
  notes?: string
  cancellationReason?: string
}

export interface CancelAppointmentDto {
  cancellationReason: string
}
const CareType = {
  Consulta: 1,
  Control: 2,
  Emergencia: 3,
  Procedimiento: 4
} as const

export type CareType = typeof CareType[keyof typeof CareType]

const AppointmentStatus = {
  Programada: 1,
  Confirmada: 2,
  Cancelada: 3,
  Completada: 4,
  NoAsistio: 5
}

export type AppointmentStatus = typeof AppointmentStatus[keyof typeof AppointmentStatus]
// export enum CareType {
//   Consulta = 1,
//   Control = 2,
//   Emergencia = 3,
//   Procedimiento = 4
// }

// export enum AppointmentStatus {
//   Programada = 1,
//   Confirmada = 2,
//   Cancelada = 3,
//   Completada = 4,
//   NoAsistio = 5
// }
