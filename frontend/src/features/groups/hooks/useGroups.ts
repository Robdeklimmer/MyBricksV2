import { useQuery } from '@tanstack/react-query'
import { groupsApi } from '../../../api/groups.api'

export function useGroups() {
  return useQuery({
    queryKey: ['groups'],
    queryFn: groupsApi.getGroups,
    staleTime: 1000 * 60 * 5, // 5 minutes
  })
}
