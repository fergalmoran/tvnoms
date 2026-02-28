import { useState } from 'react'
import { Check, Plus } from 'lucide-react'
import { toast } from 'sonner'
import { Button } from '#/components/ui/button'
import { addToWatchlist, removeFromWatchlist } from '#/server/fns/watchlist-fns'
import type { MediaType } from '#/db/schema'

interface WatchButtonProps {
  tmdbId: number
  mediaType: MediaType
  isWatched: boolean
  onToggle?: (watched: boolean) => void
  variant?: 'default' | 'overlay'
}

export function WatchButton({
  tmdbId,
  mediaType,
  isWatched,
  onToggle,
  variant = 'default',
}: WatchButtonProps) {
  const [pending, setPending] = useState(false)

  async function handleClick(e: React.MouseEvent) {
    e.preventDefault()
    e.stopPropagation()
    setPending(true)
    try {
      if (isWatched) {
        await removeFromWatchlist({ data: { tmdbId, mediaType } })
        toast.success('Removed from watchlist')
        onToggle?.(false)
      } else {
        await addToWatchlist({ data: { tmdbId, mediaType } })
        toast.success('Added to watchlist')
        onToggle?.(true)
      }
    } catch {
      toast.error('Sign in to manage your watchlist')
    } finally {
      setPending(false)
    }
  }

  if (variant === 'overlay') {
    return (
      <button
        onClick={handleClick}
        disabled={pending}
        className={`h-7 w-7 rounded-full flex items-center justify-center transition-colors ${
          isWatched
            ? 'bg-primary text-primary-foreground'
            : 'bg-background/80 hover:bg-background text-foreground'
        }`}
        aria-label={isWatched ? 'Remove from watchlist' : 'Add to watchlist'}
      >
        {isWatched ? (
          <Check className="h-4 w-4" />
        ) : (
          <Plus className="h-4 w-4" />
        )}
      </button>
    )
  }

  return (
    <Button
      variant={isWatched ? 'default' : 'outline'}
      size="sm"
      onClick={handleClick}
      disabled={pending}
    >
      {isWatched ? (
        <>
          <Check className="h-4 w-4 mr-1" /> Watched
        </>
      ) : (
        <>
          <Plus className="h-4 w-4 mr-1" /> Add to Watchlist
        </>
      )}
    </Button>
  )
}
