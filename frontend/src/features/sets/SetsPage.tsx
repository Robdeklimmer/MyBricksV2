import { useState } from 'react'
import { SetCard } from '../../components/shared/SetCard'
import { useSets } from './hooks/useSets'
import { useGroupStore } from '../../store/groupStore'
import styles from './SetsPage.module.css'

/* ── Icons ───────────────────────────────────────────────────── */
const IconPlus = () => (
  <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
    <line x1="12" y1="5" x2="12" y2="19"/>
    <line x1="5" y1="12" x2="19" y2="12"/>
  </svg>
)

const IconSearch = () => (
  <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <circle cx="11" cy="11" r="8"/>
    <line x1="21" y1="21" x2="16.65" y2="16.65"/>
  </svg>
)

const IconBox = () => (
  <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round">
    <path d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"/>
    <polyline points="3.27 6.96 12 12.01 20.73 6.96"/>
    <line x1="12" y1="22.08" x2="12" y2="12"/>
  </svg>
)

/* ── Loading skeleton card ───────────────────────────────────── */
function SkeletonCard() {
  return <div className={styles.skeleton} aria-hidden="true" />
}

/* ── Page ────────────────────────────────────────────────────── */
export function SetsPage() {
  const { activeGroupId, activeGroupName } = useGroupStore()
  const [search, setSearch] = useState('')

  const { data: sets = [], isLoading, isError } = useSets(activeGroupId)

  const filtered = sets.filter((s) =>
    s.legoSet.name.toLowerCase().includes(search.toLowerCase()) ||
    s.legoSet.rebrickableSetNum.toLowerCase().includes(search.toLowerCase()) ||
    s.legoSet.theme.toLowerCase().includes(search.toLowerCase()),
  )

  const totalParts = sets.reduce((sum, s) => sum + s.legoSet.totalParts, 0)
  const completeCount = sets.filter((s) => s.isComplete).length

  // TODO: Wire up to real remove handler
  const handleRemove = (_userSetId: number) => {
    console.log('Remove set:', _userSetId)
  }

  // TODO: Wire up to add set modal/flow
  const handleAddSet = () => {
    console.log('Open add set dialog')
  }

  return (
    <div className={styles.page}>
      {/* ── Page header ─────────────────────────────────────── */}
      <div className={styles.header}>
        <div className={styles.headerText}>
          <p className={styles.headerSub}>
            {isLoading
              ? 'Loading your collection…'
              : `${sets.length} sets · ${completeCount} complete · ${totalParts.toLocaleString()} total parts`
            }
          </p>
        </div>

        <button id="btn-add-set" className={styles.addBtn} onClick={handleAddSet}>
          <IconPlus />
          Add set
        </button>
      </div>

      {/* ── Error banner ─────────────────────────────────────── */}
      {isError && (
        <div className={styles.errorBanner}>
          Could not load your sets. Check your connection and try refreshing.
        </div>
      )}

      {/* ── Search bar ──────────────────────────────────────── */}
      <div className={styles.searchWrap}>
        <span className={styles.searchIcon}><IconSearch /></span>
        <input
          id="input-search-sets"
          className={styles.searchInput}
          type="search"
          placeholder="Search by name, number or theme…"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          disabled={isLoading}
        />
      </div>

      {/* ── Loading skeletons ────────────────────────────────── */}
      {isLoading && (
        <div className={styles.grid}>
          {Array.from({ length: 6 }).map((_, i) => (
            <SkeletonCard key={i} />
          ))}
        </div>
      )}

      {/* ── Grid ────────────────────────────────────────────── */}
      {!isLoading && filtered.length > 0 && (
        <div className={styles.grid}>
          {filtered.map((userSet) => (
            <SetCard
              key={userSet.id}
              userSet={userSet}
              onRemove={handleRemove}
            />
          ))}
        </div>
      )}

      {/* ── Empty state ─────────────────────────────────────── */}
      {!isLoading && !isError && filtered.length === 0 && (
        <div className={styles.empty}>
          <div className={styles.emptyIcon}><IconBox /></div>
          <h2 className={styles.emptyTitle}>
            {search ? 'No sets found' : 'No sets yet'}
          </h2>
          <p className={styles.emptyText}>
            {search
              ? `No sets match "${search}". Try a different search.`
              : activeGroupId === null 
                ? 'Add your first LEGO set to start tracking your collection.'
                : `No sets have been added to ${activeGroupName} yet.`}
          </p>
          {!search && (
            <button className={styles.addBtn} onClick={handleAddSet}>
              <IconPlus />
              Add your first set
            </button>
          )}
        </div>
      )}
    </div>
  )
}
