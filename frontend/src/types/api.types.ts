// ── Generic API response shapes ───────────────────────────────

export interface PaginatedResponse<T> {
  count: number
  next: string | null
  previous: string | null
  results: T[]
}

export interface ApiError {
  message: string
  errors?: Record<string, string[]>
}

// ── Auth ──────────────────────────────────────────────────────

export interface LoginRequest {
  email: string
  password: string
}

export interface RegisterRequest {
  email: string
  password: string
  displayName: string
}

export interface AuthResponse {
  token: string
  refreshToken: string
  userId: number
  displayName: string
  email: string
}
