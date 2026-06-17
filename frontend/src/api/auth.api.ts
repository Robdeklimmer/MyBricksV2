import { apiClient } from './client'
import type { AuthResponse, LoginRequest, RegisterRequest } from '../types/api.types'

export const authApi = {
  login: (body: LoginRequest) =>
    apiClient.post<AuthResponse>('/auth/login', body).then((r) => r.data),

  register: (body: RegisterRequest) =>
    apiClient.post<AuthResponse>('/auth/register', body).then((r) => r.data),
}
