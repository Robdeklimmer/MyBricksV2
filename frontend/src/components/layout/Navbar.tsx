import { useLocation, useNavigate } from 'react-router-dom'
import { useAuthStore } from '../../store/authStore'
import { useThemeStore } from '../../store/themeStore'
import styles from './Navbar.module.css'

/* ── Route → Page title map ─────────────────────────────────── */
const PAGE_TITLES: Record<string, string> = {
  '/sets':          'My Sets',
  '/parts':         'Missing Parts',
  '/groups':        'Family Groups',
  '/shopping-list': 'Shopping List',
}

/* ── Icons ───────────────────────────────────────────────────── */
const IconLogOut = () => (
  <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/>
    <polyline points="16 17 21 12 16 7"/>
    <line x1="21" y1="12" x2="9" y2="12"/>
  </svg>
)

const IconSun = () => (
  <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <circle cx="12" cy="12" r="4"/>
    <line x1="12" y1="2" x2="12" y2="4"/>
    <line x1="12" y1="20" x2="12" y2="22"/>
    <line x1="4.22" y1="4.22" x2="5.64" y2="5.64"/>
    <line x1="18.36" y1="18.36" x2="19.78" y2="19.78"/>
    <line x1="2" y1="12" x2="4" y2="12"/>
    <line x1="20" y1="12" x2="22" y2="12"/>
    <line x1="4.22" y1="19.78" x2="5.64" y2="18.36"/>
    <line x1="18.36" y1="5.64" x2="19.78" y2="4.22"/>
  </svg>
)

const IconMoon = () => (
  <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z"/>
  </svg>
)

export function Navbar() {
  const { displayName, logout } = useAuthStore()
  const { theme, toggleTheme }  = useThemeStore()
  const location = useLocation()
  const navigate = useNavigate()

  const pageTitle = PAGE_TITLES[location.pathname] ?? 'MyBricks'

  const initials = displayName
    ? displayName.split(' ').map((n: string) => n[0]).join('').toUpperCase().slice(0, 2)
    : '?'

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  return (
    <header className={styles.navbar}>
      <div className={styles.left}>
        <h1 className={styles.pageTitle}>{pageTitle}</h1>
      </div>

      <div className={styles.right}>
        {/* Theme toggle */}
        <button
          id="btn-theme-toggle"
          className={styles.iconBtn}
          onClick={toggleTheme}
          title={theme === 'dark' ? 'Switch to light mode' : 'Switch to dark mode'}
          aria-label={theme === 'dark' ? 'Switch to light mode' : 'Switch to dark mode'}
        >
          {theme === 'dark' ? <IconSun /> : <IconMoon />}
        </button>

        {/* Divider */}
        <div className={styles.divider} />

        {/* User pill */}
        <div className={styles.userPill}>
          <div className={styles.avatar}>{initials}</div>
          <span className={styles.username}>{displayName}</span>
        </div>

        {/* Divider */}
        <div className={styles.divider} />

        {/* Logout button */}
        <button
          id="btn-logout"
          className={styles.logoutBtn}
          onClick={handleLogout}
          title="Sign out"
        >
          <IconLogOut />
          <span>Sign out</span>
        </button>
      </div>
    </header>
  )
}
