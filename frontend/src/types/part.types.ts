// ── Part types ────────────────────────────────────────────────

export type PartCondition = 'Missing' | 'Broken'

export interface Part {
  id: number
  rebrickablePartNum: string
  name: string
  color: string
  category: string
  imageUrl: string | null
}

export interface MissingPart {
  id: number
  part: Part
  quantityMissing: number
  condition: PartCondition
  note: string | null
  flaggedAt: string
  resolvedAt: string | null
}

export interface FlagMissingPartRequest {
  userSetId: number
  partId: number
  quantityMissing: number
  condition: PartCondition
  note?: string
}

// ── Shopping list ─────────────────────────────────────────────

export interface ShoppingListItem {
  part: Part
  totalQuantityNeeded: number
  estimatedPriceEur: number | null
}
