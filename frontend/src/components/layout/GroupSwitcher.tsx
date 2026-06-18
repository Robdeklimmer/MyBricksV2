import { useState, useRef, useEffect } from 'react'
import { useGroupStore } from '../../store/groupStore'
import { useGroups } from '../../features/groups/hooks/useGroups'
import styles from './GroupSwitcher.module.css'

/* ── Icons ───────────────────────────────────────────────────── */
const IconUser = () => (
  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
    <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/>
    <circle cx="12" cy="7" r="4"/>
  </svg>
)

const IconUsers = () => (
  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
    <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path>
    <circle cx="9" cy="7" r="4"></circle>
    <path d="M23 21v-2a4 4 0 0 0-3-3.87"></path>
    <path d="M16 3.13a4 4 0 0 1 0 7.75"></path>
  </svg>
)

const IconChevron = ({ open }: { open: boolean }) => (
  <svg 
    width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"
    style={{ transform: open ? 'rotate(180deg)' : 'rotate(0deg)', transition: 'transform 200ms ease' }}
  >
    <polyline points="6 9 12 15 18 9"/>
  </svg>
)

export function GroupSwitcher() {
  const { activeGroupId, activeGroupName, setActiveGroup } = useGroupStore()
  const { data: groups = [] } = useGroups()
  
  const [isOpen, setIsOpen] = useState(false)
  const menuRef = useRef<HTMLDivElement>(null)

  // Close dropdown when clicking outside
  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
        setIsOpen(false)
      }
    }
    if (isOpen) {
      document.addEventListener('mousedown', handleClickOutside)
    }
    return () => document.removeEventListener('mousedown', handleClickOutside)
  }, [isOpen])

  const handleSelect = (id: number | null, name: string) => {
    setActiveGroup(id, name)
    setIsOpen(false)
  }

  return (
    <div className={styles.wrapper} ref={menuRef}>
      <button 
        className={styles.trigger}
        onClick={() => setIsOpen(!isOpen)}
        aria-haspopup="listbox"
        aria-expanded={isOpen}
      >
        <span className={styles.triggerIcon}>
          {activeGroupId === null ? <IconUser /> : <IconUsers />}
        </span>
        <span className={styles.triggerName}>{activeGroupName}</span>
        <IconChevron open={isOpen} />
      </button>

      {isOpen && (
        <div className={styles.dropdown} role="listbox">
          <button
            className={`${styles.option} ${activeGroupId === null ? styles.optionActive : ''}`}
            onClick={() => handleSelect(null, 'My Personal Sets')}
            role="option"
            aria-selected={activeGroupId === null}
          >
            <span className={styles.optionIcon}><IconUser /></span>
            My Personal Sets
          </button>

          {groups.length > 0 && <div className={styles.divider} />}

          {groups.map(group => (
            <button
              key={group.id}
              className={`${styles.option} ${activeGroupId === group.id ? styles.optionActive : ''}`}
              onClick={() => handleSelect(group.id, group.name)}
              role="option"
              aria-selected={activeGroupId === group.id}
            >
              <span className={styles.optionIcon}><IconUsers /></span>
              {group.name}
            </button>
          ))}
        </div>
      )}
    </div>
  )
}
