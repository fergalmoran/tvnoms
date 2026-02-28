import { MediaCard } from '#/components/media/media-card'
import { Skeleton } from '#/components/ui/skeleton'
import type { TmdbMediaItem } from '#/lib/tmdb'
import type { MediaType } from '#/db/schema'

interface MediaGridProps {
  items: TmdbMediaItem[]
  mediaType: MediaType
  watchedIds?: Set<number>
}

export function MediaGrid({ items, mediaType, watchedIds }: MediaGridProps) {
  return (
    <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 gap-4">
      {items
        .filter((item) => item.poster_path)
        .map((item) => (
          <MediaCard
            key={item.id}
            item={item}
            mediaType={mediaType}
            isWatched={watchedIds?.has(item.id) ?? false}
          />
        ))}
    </div>
  )
}

export function MediaGridSkeleton({ count = 12 }: { count?: number }) {
  return (
    <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 gap-4">
      {Array.from({ length: count }).map((_, i) => (
        <div key={i} className="space-y-2">
          <Skeleton className="aspect-[2/3] w-full rounded-lg" />
          <Skeleton className="h-4 w-3/4" />
          <Skeleton className="h-3 w-1/2" />
        </div>
      ))}
    </div>
  )
}
