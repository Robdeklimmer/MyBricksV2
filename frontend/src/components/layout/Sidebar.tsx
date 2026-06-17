import { NavLink } from 'react-router-dom'
import styles from './Sidebar.module.css'

const NAV_ITEMS = [
  { to: '/sets',          label: 'My Sets',       icon: '🧱' },
  { to: '/parts',         label: 'Missing Parts',  icon: '🔍' },
  { to: '/groups',        label: 'Family Groups',  icon: '👨‍👩‍👧‍👦' },
  { to: '/shopping-list', label: 'Shopping List',  icon: '🛒' },
]

export function Sidebar() {
  return (
    <aside className={styles.sidebar}>
      <div className={styles.logo}>
        <span className={styles.logoIcon}>🧩</span>
        <span className={styles.logoText}>MyBricks</span>
      </div>

      <nav className={styles.nav}>
        {NAV_ITEMS.map(({ to, label, icon }) => (
          <NavLink
            key={to}
            to={to}
            className={({ isActive }) =>
              `${styles.navItem} ${isActive ? styles.active : ''}`
            }
          >
            <span className={styles.navIcon}>{icon}</span>
            <span>{label}</span>
          </NavLink>
        ))}
      </nav>
    </aside>
  )
}
