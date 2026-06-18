import { NavLink } from 'react-router-dom'
import { useAuthStore } from '../../store/authStore'
import { GroupSwitcher } from './GroupSwitcher'
import styles from './Sidebar.module.css'

/* ── Inline SVG icons (no external dependency) ──────────────── */
const IconBrick = () => (
  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <rect x="2" y="8" width="20" height="12" rx="2"/>
    <path d="M6 8V6a2 2 0 0 1 2-2h8a2 2 0 0 1 2 2v2"/>
    <line x1="10" y1="4" x2="10" y2="8"/>
    <line x1="14" y1="4" x2="14" y2="8"/>
  </svg>
)

const IconSearch = () => (
  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <circle cx="11" cy="11" r="8"/>
    <line x1="21" y1="21" x2="16.65" y2="16.65"/>
  </svg>
)

const IconUsers = () => (
  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/>
    <circle cx="9" cy="7" r="4"/>
    <path d="M23 21v-2a4 4 0 0 0-3-3.87"/>
    <path d="M16 3.13a4 4 0 0 1 0 7.75"/>
  </svg>
)

const IconShoppingCart = () => (
  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <circle cx="9" cy="21" r="1"/>
    <circle cx="20" cy="21" r="1"/>
    <path d="M1 1h4l2.68 13.39a2 2 0 0 0 2 1.61h9.72a2 2 0 0 0 2-1.61L23 6H6"/>
  </svg>
)

const IconLogo = () => (
  <svg width="28" height="28" viewBox="0 0 32 32" fill="none">
    <rect x="2" y="10" width="28" height="18" rx="3" fill="#2563eb"/>
    <rect x="7" y="5" width="6" height="8" rx="2" fill="#60a5fa"/>
    <rect x="19" y="5" width="6" height="8" rx="2" fill="#60a5fa"/>
    <rect x="5" y="15" width="4" height="3" rx="1" fill="white" opacity="0.4"/>
    <rect x="14" y="15" width="4" height="3" rx="1" fill="white" opacity="0.4"/>
    <rect x="23" y="15" width="4" height="3" rx="1" fill="white" opacity="0.4"/>
  </svg>
)

const NAV_ITEMS = [
  { to: '/sets',          label: 'My Sets',       Icon: IconBrick },
  { to: '/parts',         label: 'Missing Parts',  Icon: IconSearch },
  { to: '/groups',        label: 'Family Groups',  Icon: IconUsers },
  { to: '/shopping-list', label: 'Shopping List',  Icon: IconShoppingCart },
]

export function Sidebar() {
  const { displayName } = useAuthStore()

  const initials = displayName
    ? displayName.split(' ').map((n: string) => n[0]).join('').toUpperCase().slice(0, 2)
    : '?'

  return (
    <aside className={styles.sidebar}>
      {/* Logo */}
      <div className={styles.logo}>
        <div className={styles.logoIcon}>
          <IconLogo />
        </div>
        <span className={styles.logoText}>MyBricks</span>
      </div>

      <GroupSwitcher />

      {/* Nav label */}
      <p className={styles.navLabel}>Navigation</p>

      {/* Nav items */}
      <nav className={styles.nav}>
        {NAV_ITEMS.map(({ to, label, Icon }) => (
          <NavLink
            key={to}
            to={to}
            className={({ isActive }) =>
              `${styles.navItem} ${isActive ? styles.active : ''}`
            }
          >
            <span className={styles.navIcon}><Icon /></span>
            <span>{label}</span>
          </NavLink>
        ))}
      </nav>

      {/* Spacer */}
      <div className={styles.spacer} />

      {/* User section at the bottom */}
      <div className={styles.userSection}>
        <div className={styles.avatar}>{initials}</div>
        <div className={styles.userInfo}>
          <span className={styles.userName}>{displayName ?? 'User'}</span>
          <span className={styles.userRole}>Member</span>
        </div>
      </div>
    </aside>
  )
}
