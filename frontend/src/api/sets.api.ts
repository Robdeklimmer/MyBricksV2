import { apiClient } from './client'
import type { UserSet, AddSetRequest } from '../types/set.types'

export const setsApi = {
  getSets: (familyGroupId?: number) =>
    apiClient
      .get<UserSet[]>('/sets', { params: { familyGroupId } })
      .then((r) => r.data),

  addSet: (body: AddSetRequest) =>
    apiClient.post<UserSet>('/sets', body).then((r) => r.data),

  removeSet: (userSetId: number) =>
    apiClient.delete(`/sets/${userSetId}`),
}
