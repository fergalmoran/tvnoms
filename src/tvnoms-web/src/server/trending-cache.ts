import cron from 'node-cron'
import { db } from '#/db'
import { trendingMovies, trendingTv } from '#/db/schema'
import { getTrendingMovies, getTrendingTv } from '#/lib/tmdb'
import type { TmdbMediaItem } from '#/lib/tmdb'

export interface TrendingData {
  movies: TmdbMediaItem[]
  tv: TmdbMediaItem[]
}

export async function refreshTrendingCache(): Promise<void> {
  const [movies, tv] = await Promise.all([
    getTrendingMovies('week'),
    getTrendingTv('week'),
  ])

  // Replace all rows atomically
  await db.transaction(async (tx) => {
    await tx.delete(trendingMovies)
    if (movies.results.length > 0) {
      await tx.insert(trendingMovies).values(
        movies.results.map((m) => ({
          tmdbId: m.id,
          title: m.title ?? m.name ?? '',
          posterPath: m.poster_path,
          backdropPath: m.backdrop_path,
          overview: m.overview,
          voteAverage: m.vote_average,
          releaseDate: m.release_date,
          refreshedAt: new Date(),
        })),
      )
    }
  })

  await db.transaction(async (tx) => {
    await tx.delete(trendingTv)
    if (tv.results.length > 0) {
      await tx.insert(trendingTv).values(
        tv.results.map((t) => ({
          tmdbId: t.id,
          name: t.name ?? t.title ?? '',
          posterPath: t.poster_path,
          backdropPath: t.backdrop_path,
          overview: t.overview,
          voteAverage: t.vote_average,
          firstAirDate: t.first_air_date,
          refreshedAt: new Date(),
        })),
      )
    }
  })
}

export async function getTrendingFromCache(): Promise<TrendingData> {
  const [movies, tv] = await Promise.all([
    db.select().from(trendingMovies),
    db.select().from(trendingTv),
  ])

  // Cache miss — fetch immediately and prime the cache
  if (movies.length === 0 || tv.length === 0) {
    await refreshTrendingCache()
    return getTrendingFromCache()
  }

  return {
    movies: movies.map((m) => ({
      id: m.tmdbId,
      title: m.title,
      poster_path: m.posterPath,
      backdrop_path: m.backdropPath,
      overview: m.overview,
      vote_average: m.voteAverage,
      release_date: m.releaseDate ?? undefined,
      media_type: 'movie',
    })) as TmdbMediaItem[],
    tv: tv.map((t) => ({
      id: t.tmdbId,
      name: t.name,
      poster_path: t.posterPath,
      backdrop_path: t.backdropPath,
      overview: t.overview,
      vote_average: t.voteAverage,
      first_air_date: t.firstAirDate ?? undefined,
      media_type: 'tv',
    })) as TmdbMediaItem[],
  }
}

// Singleton: start the cron once per process
let started = false

export function startTrendingCron(): void {
  if (started) return
  started = true

  cron.schedule('0 * * * *', () => {
    refreshTrendingCache().catch((err) =>
      console.error('[trending-cache] refresh failed:', err),
    )
  })

  console.log('[trending-cache] cron scheduled (hourly)')
}
