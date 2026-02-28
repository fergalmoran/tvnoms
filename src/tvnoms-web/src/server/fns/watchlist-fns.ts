import { createServerFn } from '@tanstack/react-start'
import { z } from 'zod'
import { eq, and } from 'drizzle-orm'
import { db } from '#/db'
import { watchedItems } from '#/db/schema'
import { requireSession } from './auth-fns'

export const getWatchlist = createServerFn({ method: 'GET' }).handler(
  async () => {
    const session = await requireSession()
    return db
      .select()
      .from(watchedItems)
      .where(eq(watchedItems.userId, session.user.id))
  },
)

const watchItemSchema = z.object({
  tmdbId: z.number(),
  mediaType: z.enum(['movie', 'tv']),
})

export const addToWatchlist = createServerFn({ method: 'POST' })
  .inputValidator(watchItemSchema)
  .handler(async ({ data }) => {
    const session = await requireSession()
    await db
      .insert(watchedItems)
      .values({ userId: session.user.id, ...data })
      .onConflictDoNothing()
  })

export const removeFromWatchlist = createServerFn({ method: 'POST' })
  .inputValidator(watchItemSchema)
  .handler(async ({ data }) => {
    const session = await requireSession()
    await db.delete(watchedItems).where(
      and(
        eq(watchedItems.userId, session.user.id),
        eq(watchedItems.tmdbId, data.tmdbId),
        eq(watchedItems.mediaType, data.mediaType),
      ),
    )
  })
