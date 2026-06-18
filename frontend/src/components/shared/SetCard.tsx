import type { UserSet } from '../../types/set.types'
import styles from './SetCard.module.css'

/* ── Icons ───────────────────────────────────────────────────── */
const IconParts = () => (
  <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <rect x="2" y="8" width="20" height="12" rx="2"/>
    <path d="M6 8V6a2 2 0 0 1 2-2h8a2 2 0 0 1 2 2v2"/>
  </svg>
)

const IconCalendar = () => (
  <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <rect x="3" y="4" width="18" height="18" rx="2" ry="2"/>
    <line x1="16" y1="2" x2="16" y2="6"/>
    <line x1="8" y1="2" x2="8" y2="6"/>
    <line x1="3" y1="10" x2="21" y2="10"/>
  </svg>
)

const IconTag = () => (
  <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <path d="M20.59 13.41l-7.17 7.17a2 2 0 0 1-2.83 0L2 12V2h10l8.59 8.59a2 2 0 0 1 0 2.82z"/>
    <line x1="7" y1="7" x2="7.01" y2="7"/>
  </svg>
)

const IconTrash = () => (
  <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <polyline points="3 6 5 6 21 6"/>
    <path d="M19 6l-1 14a2 2 0 0 1-2 2H8a2 2 0 0 1-2-2L5 6"/>
    <path d="M10 11v6"/>
    <path d="M14 11v6"/>
    <path d="M9 6V4a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v2"/>
  </svg>
)

const IconCheckCircle = () => (
  <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
    <path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"/>
    <polyline points="22 4 12 14.01 9 11.01"/>
  </svg>
)

const IconAlertCircle = () => (
  <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
    <circle cx="12" cy="12" r="10"/>
    <line x1="12" y1="8" x2="12" y2="12"/>
    <line x1="12" y1="16" x2="12.01" y2="16"/>
  </svg>
)

/* ── Fallback image placeholder ─────────────────────────────── */
const ImageFallback = ({ name }: { name: string }) => (
  <div className={styles.imageFallback}>
    <svg width="40" height="40" viewBox="0 0 32 32" fill="none">
      <rect x="2" y="10" width="28" height="18" rx="3" fill="currentColor" opacity="0.15"/>
      <rect x="7" y="5" width="6" height="8" rx="2" fill="currentColor" opacity="0.25"/>
      <rect x="19" y="5" width="6" height="8" rx="2" fill="currentColor" opacity="0.25"/>
    </svg>
    <span className={styles.imageFallbackText}>{name.slice(0, 2).toUpperCase()}</span>
  </div>
)

/* ── Props ───────────────────────────────────────────────────── */
interface SetCardProps {
  userSet: UserSet
  onRemove?: (userSetId: number) => void
}

/* ── Component ───────────────────────────────────────────────── */
export function SetCard({ userSet, onRemove }: SetCardProps) {
  const { legoSet, isComplete, addedAt } = userSet
  const addedDate = new Date(addedAt).toLocaleDateString('en-GB', {
    day: 'numeric', month: 'short', year: 'numeric',
  })

  return (
    <article className={styles.card}>
      {/* Image */}
      <div className={styles.imageWrap}>
        {legoSet.imageUrl ? (
          <img
            src={legoSet.imageUrl}
            alt={legoSet.name}
            className={styles.image}
            loading="lazy"
          />
        ) : (
          <ImageFallback name={legoSet.name} />
        )}

        {/* Status badge */}
        <span className={`${styles.badge} ${isComplete ? styles.badgeComplete : styles.badgeIncomplete}`}>
          {isComplete ? <IconCheckCircle /> : <IconAlertCircle />}
          {isComplete ? 'Complete' : 'Incomplete'}
        </span>
      </div>

      {/* Body */}
      <div className={styles.body}>
        <p className={styles.setNum}>{legoSet.rebrickableSetNum}</p>
        <h2 className={styles.name} title={legoSet.name}>{legoSet.name}</h2>

        {/* Meta chips */}
        <div className={styles.meta}>
          <span className={styles.chip}>
            <IconCalendar />
            {legoSet.year}
          </span>
          <span className={styles.chip}>
            <IconTag />
            {legoSet.theme}
          </span>
          <span className={styles.chip}>
            <IconParts />
            {legoSet.totalParts.toLocaleString()} parts
          </span>
        </div>
      </div>

      {/* Footer */}
      <div className={styles.footer}>
        <span className={styles.addedDate}>Added {addedDate}</span>
        {onRemove && (
          <button
            className={styles.removeBtn}
            onClick={() => onRemove(userSet.id)}
            title="Remove set"
            aria-label={`Remove ${legoSet.name}`}
          >
            <IconTrash />
          </button>
        )}
      </div>
    </article>
  )
}
