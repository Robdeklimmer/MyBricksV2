import { Outlet } from 'react-router-dom'
import { Sidebar } from './Sidebar'
import { Navbar }  from './Navbar'
import styles from './AppShell.module.css'

export function AppShell() {
  return (
    <div className={styles.shell}>
      <Sidebar />
      <div className={styles.main}>
        <Navbar />
        <main className={styles.content}>
          <Outlet />
        </main>
      </div>
    </div>
  )
}
