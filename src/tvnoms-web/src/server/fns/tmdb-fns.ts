import { createServerFn } from '@tanstack/react-start'
import { z } from 'zod'
import { searchMulti, getMovieDetail, getTvDetail } from '#/lib/tmdb'
import {
  getTrendingFromCache,
  startTrendingCron,
} from '#/server/trending-cache'

export const fetchTrending = createServerFn({ method: 'GET' }).handler(
  async () => {
    startTrendingCron()
    return getTrendingFromCache()
  },
)

export const fetchSearch = createServerFn({ method: 'GET' })
  .inputValidator(z.object({ query: z.string().min(1), page: z.number().default(1) }))
  .handler(async ({ data }) => {
    return searchMulti(data.query, data.page)
  })

export const fetchMediaDetail = createServerFn({ method: 'GET' })
  .inputValidator(z.object({ type: z.enum(['movie', 'tv']), id: z.string() }))
  .handler(async ({ data }) => {
    return data.type === 'movie'
      ? getMovieDetail(data.id)
      : getTvDetail(data.id)
  })
