import { createFileRoute } from '@tanstack/react-router'
import { refreshTrendingCache } from '#/server/trending-cache'

export const Route = createFileRoute('/api/trending/refresh')({
  server: {
    handlers: {
      POST: async ({ request }) => {
        const secret = request.headers.get('x-refresh-secret')
        if (secret !== process.env.REFRESH_SECRET) {
          return new Response('Unauthorized', { status: 401 })
        }

        const start = Date.now()
        await refreshTrendingCache()
        const ms = Date.now() - start

        return Response.json({ ok: true, ms })
      },
    },
  },
})
