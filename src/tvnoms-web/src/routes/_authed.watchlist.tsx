import { createFileRoute, Link } from '@tanstack/react-router'
import { getWatchlist } from '#/server/fns/watchlist-fns'
import { fetchMediaDetail } from '#/server/fns/tmdb-fns'
import { MediaCard } from '#/components/media/media-card'
import { Button } from '#/components/ui/button'
import type { WatchedItem } from '#/db/schema'
import type { TmdbMediaItem } from '#/lib/tmdb'

export const Route = createFileRoute('/_authed/watchlist')({
  loader: async () => {
    const items = await getWatchlist()
    const details = await Promise.all(
      items.map(async (item: WatchedItem) => {
        try {
          const detail = await fetchMediaDetail({
            data: { type: item.mediaType, id: String(item.tmdbId) },
          })
          return {
            ...detail,
            id: item.tmdbId,
            media_type: item.mediaType,
          } as TmdbMediaItem
        } catch {
          return null
        }
      }),
    )
    return {
      items,
      details: details.filter(Boolean) as TmdbMediaItem[],
      watchedIds: new Set(items.map((i: WatchedItem) => i.tmdbId)),
    }
  },
  component: WatchlistPage,
})

function WatchlistPage() {
  const { details, watchedIds } = Route.useLoaderData()

  if (details.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center py-20 text-center gap-4">
        <h2 className="text-2xl font-bold">Your watchlist is empty</h2>
        <p className="text-muted-foreground">
          Browse trending shows and movies to start tracking what you've watched.
        </p>
        <Button asChild>
          <Link to="/">Explore Trending</Link>
        </Button>
      </div>
    )
  }

  const movies = details.filter((d) => d.media_type === 'movie')
  const tv = details.filter((d) => d.media_type === 'tv')

  return (
    <div className="space-y-10">
      <h1 className="text-3xl font-bold">My Watchlist</h1>

      {movies.length > 0 && (
        <section>
          <h2 className="text-xl font-semibold mb-4">Movies ({movies.length})</h2>
          <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 gap-4">
            {movies.map((item) => (
              <MediaCard
                key={item.id}
                item={item}
                mediaType="movie"
                isWatched={watchedIds.has(item.id)}
              />
            ))}
          </div>
        </section>
      )}

      {tv.length > 0 && (
        <section>
          <h2 className="text-xl font-semibold mb-4">TV Shows ({tv.length})</h2>
          <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 gap-4">
            {tv.map((item) => (
              <MediaCard
                key={item.id}
                item={item}
                mediaType="tv"
                isWatched={watchedIds.has(item.id)}
              />
            ))}
          </div>
        </section>
      )}
    </div>
  )
}
