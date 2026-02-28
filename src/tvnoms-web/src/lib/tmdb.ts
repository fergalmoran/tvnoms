const BASE_URL = 'https://api.themoviedb.org/3'
export const TMDB_IMAGE_BASE = 'https://image.tmdb.org/t/p/w500'
export const TMDB_IMAGE_ORIGINAL = 'https://image.tmdb.org/t/p/original'

export interface TmdbMediaItem {
  id: number
  title?: string
  name?: string
  poster_path: string | null
  backdrop_path: string | null
  overview: string
  vote_average: number
  release_date?: string
  first_air_date?: string
  media_type?: string
  genre_ids?: number[]
}

export interface TmdbMovieDetail {
  id: number
  title: string
  poster_path: string | null
  backdrop_path: string | null
  overview: string
  vote_average: number
  release_date: string
  runtime: number | null
  genres: Array<{ id: number; name: string }>
  tagline: string
  status: string
}

export interface TmdbTvDetail {
  id: number
  name: string
  poster_path: string | null
  backdrop_path: string | null
  overview: string
  vote_average: number
  first_air_date: string
  number_of_seasons: number
  number_of_episodes: number
  genres: Array<{ id: number; name: string }>
  tagline: string
  status: string
}

async function tmdbFetch<T>(
  path: string,
  params?: Record<string, string>,
): Promise<T> {
  const url = new URL(`${BASE_URL}${path}`)
  if (params) {
    Object.entries(params).forEach(([k, v]) => url.searchParams.set(k, v))
  }
  const res = await fetch(url.toString(), {
    headers: {
      Authorization: `Bearer ${process.env.TMDB_API_READ_TOKEN}`,
      'Content-Type': 'application/json',
    },
  })
  if (!res.ok) throw new Error(`TMDB error: ${res.status}`)
  return res.json() as Promise<T>
}

export function getTrendingMovies(timeWindow: 'day' | 'week' = 'week') {
  return tmdbFetch<{ results: TmdbMediaItem[] }>(
    `/trending/movie/${timeWindow}`,
  )
}

export function getTrendingTv(timeWindow: 'day' | 'week' = 'week') {
  return tmdbFetch<{ results: TmdbMediaItem[] }>(`/trending/tv/${timeWindow}`)
}

export function searchMulti(query: string, page = 1) {
  return tmdbFetch<{ results: TmdbMediaItem[]; total_pages: number }>(
    '/search/multi',
    { query, page: String(page) },
  )
}

export function getMovieDetail(id: string) {
  return tmdbFetch<TmdbMovieDetail>(`/movie/${id}`)
}

export function getTvDetail(id: string) {
  return tmdbFetch<TmdbTvDetail>(`/tv/${id}`)
}

export function getMediaTitle(item: TmdbMediaItem): string {
  return item.title ?? item.name ?? 'Unknown'
}

export function getMediaYear(item: TmdbMediaItem): string {
  const date = item.release_date ?? item.first_air_date
  return date ? date.slice(0, 4) : ''
}
