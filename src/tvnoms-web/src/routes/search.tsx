import { createFileRoute, useNavigate } from '@tanstack/react-router'
import { z } from 'zod'
import { useState } from 'react'
import { Search } from 'lucide-react'
import { fetchSearch } from '#/server/fns/tmdb-fns'
import { MediaCard } from '#/components/media/media-card'
import { Input } from '#/components/ui/input'
import { Button } from '#/components/ui/button'
import type { TmdbMediaItem } from '#/lib/tmdb'

const searchSchema = z.object({
  q: z.string().optional().default(''),
  page: z.number().optional().default(1),
})

export const Route = createFileRoute('/search')({
  validateSearch: searchSchema,
  loaderDeps: ({ search }) => search,
  loader: async ({ deps }) => {
    if (!deps.q) return { results: [], total_pages: 0, query: '' }
    const data = await fetchSearch({ data: { query: deps.q, page: deps.page } })
    return { ...data, query: deps.q }
  },
  component: SearchPage,
})

function SearchPage() {
  const { results, total_pages, query } = Route.useLoaderData()
  const search = Route.useSearch()
  const navigate = useNavigate({ from: '/search' })
  const [inputValue, setInputValue] = useState(query)

  function handleSearch(e: React.FormEvent) {
    e.preventDefault()
    void navigate({ search: { q: inputValue, page: 1 } })
  }

  const mediaResults = results.filter(
    (r: TmdbMediaItem) =>
      (r.media_type === 'movie' || r.media_type === 'tv') && r.poster_path,
  )

  return (
    <div className="space-y-6">
      <form onSubmit={handleSearch} className="flex gap-2 max-w-xl">
        <div className="relative flex-1">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
          <Input
            className="pl-9"
            placeholder="Search movies & TV shows..."
            value={inputValue}
            onChange={(e) => setInputValue(e.target.value)}
          />
        </div>
        <Button type="submit">Search</Button>
      </form>

      {query && (
        <p className="text-sm text-muted-foreground">
          {mediaResults.length > 0
            ? `Showing results for "${query}"`
            : `No results for "${query}"`}
        </p>
      )}

      {mediaResults.length > 0 && (
        <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 gap-4">
          {mediaResults.map((item: TmdbMediaItem) => (
            <MediaCard
              key={item.id}
              item={item}
              mediaType={item.media_type as 'movie' | 'tv'}
            />
          ))}
        </div>
      )}

      {total_pages > 1 && (
        <div className="flex justify-center gap-2">
          {search.page > 1 && (
            <Button
              variant="outline"
              onClick={() =>
                void navigate({ search: { q: query, page: search.page - 1 } })
              }
            >
              Previous
            </Button>
          )}
          <span className="flex items-center text-sm text-muted-foreground">
            Page {search.page} of {total_pages}
          </span>
          {search.page < total_pages && (
            <Button
              variant="outline"
              onClick={() =>
                void navigate({ search: { q: query, page: search.page + 1 } })
              }
            >
              Next
            </Button>
          )}
        </div>
      )}
    </div>
  )
}
