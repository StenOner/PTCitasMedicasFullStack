// API Response Types
export interface ApiResponse<T> {
  data: T
  message: string
  errors?: string[]
}

// Enums
export enum CareType {
  Consulta = 1,
  Control = 2,
  Emergencia = 3,
  Procedimiento = 4
}

export enum AppointmentStatus {
  Programada = 1,
  Confirmada = 2,
  Cancelada = 3,
  Completada = 4,
  NoAsistio = 5
}

// Patient Types
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

// Specialty Types
export interface SpecialtyDto {
  id: number
  name: string
  description: string
}

// Doctor Types
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

// Schedule Types
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

// Appointment Types
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