// ── Set types ─────────────────────────────────────────────────

export interface LegoSet {
  id: number
  rebrickableSetNum: string
  name: string
  year: number
  theme: string
  totalParts: number
  imageUrl: string | null
  lastSyncedAt: string | null
}

export interface UserSet {
  id: number
  legoSet: LegoSet
  familyGroupId: number | null
  addedAt: string
  isComplete: boolean
}

export interface AddSetRequest {
  rebrickableSetNum: string
  familyGroupId?: number | null
}
