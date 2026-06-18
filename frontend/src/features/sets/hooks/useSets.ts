import { useQuery } from '@tanstack/react-query'
import { setsApi } from '../../../api/sets.api'

/**
 * Fetches the current user's sets, optionally filtered by a family group.
 *
 * Query key includes familyGroupId so React Query caches each group separately.
 * Data is considered fresh for 2 minutes, then refetched in the background.
 */
export function useSets(familyGroupId?: number) {
  return useQuery({
    queryKey: ['sets', familyGroupId ?? null],
    queryFn: () => setsApi.getSets(familyGroupId),
    staleTime: 1000 * 60 * 2, // 2 minutes
  })
}
