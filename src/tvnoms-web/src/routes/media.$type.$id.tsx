import { createFileRoute, notFound } from '@tanstack/react-router'
import { Star, Clock, CalendarDays, ImageOff } from 'lucide-react'
import { fetchMediaDetail } from '#/server/fns/tmdb-fns'
import { TMDB_IMAGE_ORIGINAL } from '#/lib/tmdb'
import { Badge } from '#/components/ui/badge'
import { Separator } from '#/components/ui/separator'
import { WatchButton } from '#/components/media/watch-button'
import { useState } from 'react'
import type { TmdbMovieDetail, TmdbTvDetail } from '#/lib/tmdb'

export const Route = createFileRoute('/media/$type/$id')({
  loader: async ({ params }) => {
    const type = params.type as 'movie' | 'tv'
    if (type !== 'movie' && type !== 'tv') throw notFound()
    const detail = await fetchMediaDetail({ data: { type, id: params.id } })
    if (!detail || 'success' in detail) throw notFound()
    return { detail, type }
  },
  component: MediaDetailPage,
})

function MediaDetailPage() {
  const { detail, type } = Route.useLoaderData()
  const { id: tmdbId } = Route.useParams()
  const [watched, setWatched] = useState(false)

  const title = 'title' in detail ? detail.title : (detail as TmdbTvDetail).name
  const date =
    'release_date' in detail
      ? (detail as TmdbMovieDetail).release_date
      : (detail as TmdbTvDetail).first_air_date
  const year = date?.slice(0, 4)

  return (
    <div className="space-y-6 max-w-4xl">
      <div className="flex flex-col sm:flex-row gap-6">
        <div className="w-full sm:w-48 shrink-0">
          {detail.poster_path ? (
            <img
              src={`${TMDB_IMAGE_ORIGINAL}${detail.poster_path}`}
              alt={title}
              className="w-full rounded-lg shadow-md"
            />
          ) : (
            <div className="w-full aspect-[2/3] rounded-lg bg-muted flex items-center justify-center">
              <ImageOff className="h-12 w-12 text-muted-foreground" />
            </div>
          )}
        </div>

        <div className="flex-1 space-y-4">
          <div>
            <h1 className="text-3xl font-bold">
              {title}
              {year && (
                <span className="text-muted-foreground font-normal ml-2">
                  ({year})
                </span>
              )}
            </h1>
            {detail.tagline && (
              <p className="text-muted-foreground italic mt-1">{detail.tagline}</p>
            )}
          </div>

          <div className="flex flex-wrap gap-2 items-center">
            <Badge variant="secondary" className="gap-1">
              <Star className="h-3 w-3" />
              {detail.vote_average.toFixed(1)}
            </Badge>
            {'runtime' in detail && (detail as TmdbMovieDetail).runtime && (
              <Badge variant="outline" className="gap-1">
                <Clock className="h-3 w-3" />
                {(detail as TmdbMovieDetail).runtime}m
              </Badge>
            )}
            {'number_of_seasons' in detail && (
              <Badge variant="outline" className="gap-1">
                <CalendarDays className="h-3 w-3" />
                {(detail as TmdbTvDetail).number_of_seasons} season
                {(detail as TmdbTvDetail).number_of_seasons !== 1 ? 's' : ''}
              </Badge>
            )}
            {detail.genres.map((g) => (
              <Badge key={g.id} variant="outline">
                {g.name}
              </Badge>
            ))}
          </div>

          <p className="text-muted-foreground leading-relaxed">{detail.overview}</p>

          <WatchButton
            tmdbId={Number(tmdbId)}
            mediaType={type}
            isWatched={watched}
            onToggle={setWatched}
          />
        </div>
      </div>

      <Separator />

      <div className="grid grid-cols-2 sm:grid-cols-3 gap-4 text-sm">
        <div>
          <p className="font-medium">Status</p>
          <p className="text-muted-foreground">{detail.status}</p>
        </div>
        {'number_of_episodes' in detail && (
          <div>
            <p className="font-medium">Episodes</p>
            <p className="text-muted-foreground">
              {(detail as TmdbTvDetail).number_of_episodes}
            </p>
          </div>
        )}
        {date && (
          <div>
            <p className="font-medium">
              {'release_date' in detail ? 'Release Date' : 'First Aired'}
            </p>
            <p className="text-muted-foreground">{date}</p>
          </div>
        )}
      </div>
    </div>
  )
}
