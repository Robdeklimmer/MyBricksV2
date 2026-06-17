import { useAuthStore } from '../../store/authStore'
import { useNavigate }  from 'react-router-dom'
import styles from './Navbar.module.css'

export function Navbar() {
  const { displayName, logout } = useAuthStore()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  return (
    <header className={styles.navbar}>
      <div className={styles.left}>
        {/* Breadcrumb / page title can be injected here later */}
      </div>
      <div className={styles.right}>
        <span className={styles.username}>{displayName}</span>
        <button
          id="btn-logout"
          className={styles.logoutBtn}
          onClick={handleLogout}
        >
          Sign out
        </button>
      </div>
    </header>
  )
}
