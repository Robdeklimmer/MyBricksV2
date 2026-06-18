import { create } from 'zustand'
import { persist } from 'zustand/middleware'

interface GroupState {
  activeGroupId: number | null
  activeGroupName: string

  setActiveGroup: (id: number | null, name: string) => void
}

export const useGroupStore = create<GroupState>()(
  persist(
    (set) => ({
      activeGroupId: null,
      activeGroupName: 'My Personal Sets',

      setActiveGroup: (id, name) => set({ activeGroupId: id, activeGroupName: name }),
    }),
    {
      name: 'mybricks-group-store',
    }
  )
)
