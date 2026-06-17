import { apiClient } from './client'
import type { ShoppingListItem } from '../types/part.types'

export const shoppingListApi = {
  generate: (familyGroupId: number) =>
    apiClient
      .get<ShoppingListItem[]>('/shopping-list', { params: { familyGroupId } })
      .then((r) => r.data),
}
