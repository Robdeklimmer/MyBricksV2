import { create } from 'zustand'
import { persist } from 'zustand/middleware'

interface AuthState {
  token: string | null
  userId: number | null
  displayName: string | null
  email: string | null
  isAuthenticated: boolean

  setAuth: (payload: {
    token: string
    userId: number
    displayName: string
    email: string
  }) => void
  logout: () => void
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      token: null,
      userId: null,
      displayName: null,
      email: null,
      isAuthenticated: false,

      setAuth: ({ token, userId, displayName, email }) =>
        set({ token, userId, displayName, email, isAuthenticated: true }),

      logout: () =>
        set({
          token: null,
          userId: null,
          displayName: null,
          email: null,
          isAuthenticated: false,
        }),
    }),
    {
      name: 'mybricks-auth',   // persisted to localStorage
      partialize: (state) => ({
        token: state.token,
        userId: state.userId,
        displayName: state.displayName,
        email: state.email,
        isAuthenticated: state.isAuthenticated,
      }),
    },
  ),
)
