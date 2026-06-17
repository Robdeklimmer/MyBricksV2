import { apiClient } from './client'
import type { MissingPart, FlagMissingPartRequest } from '../types/part.types'

export const partsApi = {
  getMissingParts: (userSetId: number) =>
    apiClient
      .get<MissingPart[]>(`/sets/${userSetId}/parts/missing`)
      .then((r) => r.data),

  flagMissing: (body: FlagMissingPartRequest) =>
    apiClient.post<MissingPart>('/parts/flag', body).then((r) => r.data),

  markFound: (missingPartId: number) =>
    apiClient.patch(`/parts/${missingPartId}/resolve`),
}
