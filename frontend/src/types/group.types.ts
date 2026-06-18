// ── Group types ───────────────────────────────────────────────

export interface FamilyGroup {
  id: number
  name: string
  joinCode: string
  adminId: number
}

export interface CreateFamilyGroupRequest {
  name: string
}

export interface JoinFamilyGroupRequest {
  joinCode: string
}
