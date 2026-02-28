import { createFileRoute } from '@tanstack/react-router'
import { fetchTrending } from '#/server/fns/tmdb-fns'
import { MediaGrid, MediaGridSkeleton } from '#/components/media/media-grid'

export const Route = createFileRoute('/')({
  loader: () => fetchTrending(),
  pendingComponent: TrendingPending,
  component: HomePage,
})

function HomePage() {
  const { movies, tv } = Route.useLoaderData()

  return (
    <div className="space-y-10">
      <section>
        <h2 className="text-2xl font-bold mb-4">Trending Movies</h2>
        <MediaGrid items={movies} mediaType="movie" />
      </section>
      <section>
        <h2 className="text-2xl font-bold mb-4">Trending TV Shows</h2>
        <MediaGrid items={tv} mediaType="tv" />
      </section>
    </div>
  )
}

function TrendingPending() {
  return (
    <div className="space-y-10">
      <section>
        <h2 className="text-2xl font-bold mb-4">Trending Movies</h2>
        <MediaGridSkeleton />
      </section>
      <section>
        <h2 className="text-2xl font-bold mb-4">Trending TV Shows</h2>
        <MediaGridSkeleton />
      </section>
    </div>
  )
}
