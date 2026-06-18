import { apiClient } from './client'
import type { FamilyGroup, CreateFamilyGroupRequest, JoinFamilyGroupRequest } from '../types/group.types'

export const groupsApi = {
  getGroups: () =>
    apiClient.get<FamilyGroup[]>('/groups').then((r) => r.data),

  createGroup: (body: CreateFamilyGroupRequest) =>
    apiClient.post<FamilyGroup>('/groups', body).then((r) => r.data),

  joinGroup: (body: JoinFamilyGroupRequest) =>
    apiClient.post<FamilyGroup>('/groups/join', body).then((r) => r.data),
}
