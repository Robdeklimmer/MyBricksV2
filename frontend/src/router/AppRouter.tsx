import { BrowserRouter, Routes, Route, Navigate, Outlet } from 'react-router-dom'
import { useAuthStore } from '../store/authStore'

// ── Auth pages ────────────────────────────────────────────────
import { LoginPage }    from '../features/auth/LoginPage'
import { RegisterPage } from '../features/auth/RegisterPage'

// ── Layout ────────────────────────────────────────────────────
import { AppShell } from '../components/layout/AppShell'

// ── Feature stubs ─────────────────────────────────────────────
import { SetsPage }          from '../features/sets/SetsPage'
import { PartsPage }         from '../features/parts/PartsPage'
import { GroupsPage }        from '../features/groups/GroupsPage'
import { ShoppingListPage }  from '../features/shoppingList/ShoppingListPage'

// ── ProtectedRoute ────────────────────────────────────────────
function ProtectedRoute() {
  const isAuthenticated = useAuthStore((s) => s.isAuthenticated)
  return isAuthenticated ? <Outlet /> : <Navigate to="/login" replace />
}

// ── Router ────────────────────────────────────────────────────
export function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Public routes */}
        <Route path="/login"    element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />

        {/* Protected routes (wrapped in AppShell layout) */}
        <Route element={<ProtectedRoute />}>
          <Route element={<AppShell />}>
            <Route index                 element={<Navigate to="/sets" replace />} />
            <Route path="/sets"          element={<SetsPage />} />
            <Route path="/parts"         element={<PartsPage />} />
            <Route path="/groups"        element={<GroupsPage />} />
            <Route path="/shopping-list" element={<ShoppingListPage />} />
          </Route>
        </Route>

        {/* Catch-all */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  )
}
