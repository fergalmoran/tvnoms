import { useState } from 'react'
import { Link } from '@tanstack/react-router'
import { Star, ImageOff } from 'lucide-react'
import { Card, CardContent } from '#/components/ui/card'
import { Badge } from '#/components/ui/badge'
import { WatchButton } from '#/components/media/watch-button'
import { TMDB_IMAGE_BASE, getMediaTitle, getMediaYear } from '#/lib/tmdb'
import type { TmdbMediaItem } from '#/lib/tmdb'
import type { MediaType } from '#/db/schema'

interface MediaCardProps {
  item: TmdbMediaItem
  mediaType: MediaType
  isWatched?: boolean
}

export function MediaCard({ item, mediaType, isWatched: initialWatched = false }: MediaCardProps) {
  const [watched, setWatched] = useState(initialWatched)
  const title = getMediaTitle(item)
  const year = getMediaYear(item)

  return (
    <Link to="/media/$type/$id" params={{ type: mediaType, id: String(item.id) }}>
      <Card className="overflow-hidden hover:shadow-md transition-shadow group cursor-pointer h-full">
        <div className="relative aspect-[2/3] bg-muted">
          {item.poster_path ? (
            <img
              src={`${TMDB_IMAGE_BASE}${item.poster_path}`}
              alt={title}
              className="w-full h-full object-cover"
              loading="lazy"
            />
          ) : (
            <div className="w-full h-full flex items-center justify-center text-muted-foreground">
              <ImageOff className="h-10 w-10" />
            </div>
          )}
          <div className="absolute top-2 right-2">
            <WatchButton
              tmdbId={item.id}
              mediaType={mediaType}
              isWatched={watched}
              onToggle={setWatched}
              variant="overlay"
            />
          </div>
        </div>
        <CardContent className="p-3">
          <p className="font-medium text-sm line-clamp-1">{title}</p>
          <div className="flex items-center justify-between mt-1">
            <span className="text-xs text-muted-foreground">{year}</span>
            <Badge variant="secondary" className="text-xs gap-1 py-0 px-1.5">
              <Star className="h-3 w-3" />
              {item.vote_average.toFixed(1)}
            </Badge>
          </div>
        </CardContent>
      </Card>
    </Link>
  )
}
