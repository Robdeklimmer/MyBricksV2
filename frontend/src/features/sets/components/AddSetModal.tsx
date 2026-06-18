import { useState } from 'react'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { setsApi } from '../../../api/sets.api'
import { useGroupStore } from '../../../store/groupStore'
import styles from './AddSetModal.module.css'

interface AddSetModalProps {
  isOpen: boolean
  onClose: () => void
}

export function AddSetModal({ isOpen, onClose }: AddSetModalProps) {
  const { activeGroupId } = useGroupStore()
  const queryClient = useQueryClient()
  const [setNum, setSetNum] = useState('')

  const mutation = useMutation({
    mutationFn: (num: string) => setsApi.addSet({ 
      rebrickableSetNum: num, 
      familyGroupId: activeGroupId ?? undefined 
    }),
    onSuccess: () => {
      // Refresh the specific list we are currently viewing
      queryClient.invalidateQueries({ queryKey: ['sets', activeGroupId] })
      onClose()
      setSetNum('')
    },
  })

  if (!isOpen) return null

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    if (!setNum.trim()) return
    mutation.mutate(setNum.trim())
  }

  const handleClose = () => {
    if (mutation.isPending) return
    onClose()
    setSetNum('')
    mutation.reset()
  }

  return (
    <div className={styles.overlay} onMouseDown={handleClose}>
      <div className={styles.modal} onMouseDown={e => e.stopPropagation()}>
        <button className={styles.closeBtn} onClick={handleClose} disabled={mutation.isPending}>
          <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
            <line x1="18" y1="6" x2="6" y2="18"></line>
            <line x1="6" y1="6" x2="18" y2="18"></line>
          </svg>
        </button>

        <h2 className={styles.title}>Add LEGO Set</h2>
        <p className={styles.description}>
          Enter the set number to import it from Rebrickable. We will automatically fetch all parts and theme information.
        </p>

        <form onSubmit={handleSubmit}>
          <div className={styles.inputGroup}>
            <label htmlFor="setNum">Set Number (e.g., 10188)</label>
            <input
              id="setNum"
              type="text"
              value={setNum}
              onChange={e => setSetNum(e.target.value)}
              placeholder="10188"
              disabled={mutation.isPending}
              autoFocus
            />
          </div>

          {mutation.isError && (
            <div className={styles.error}>
              Failed to find or add set. Please check the number.
            </div>
          )}

          <div className={styles.actions}>
            <button type="button" className={styles.cancelBtn} onClick={handleClose} disabled={mutation.isPending}>
              Cancel
            </button>
            <button type="submit" className={styles.submitBtn} disabled={mutation.isPending || !setNum.trim()}>
              {mutation.isPending ? 'Fetching parts...' : 'Add Set'}
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}
