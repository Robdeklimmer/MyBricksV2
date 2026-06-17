import { create } from 'zustand'

interface GroupState {
  activeGroupId: number | null
  setActiveGroup: (id: number | null) => void
}

export const useGroupStore = create<GroupState>()((set) => ({
  activeGroupId: null,
  setActiveGroup: (id) => set({ activeGroupId: id }),
}))
