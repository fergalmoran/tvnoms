import {
  pgTable,
  pgSchema,
  text,
  timestamp,
  integer,
  primaryKey,
  pgEnum,
  boolean,
  doublePrecision,
} from 'drizzle-orm/pg-core'

// better-auth tables
export const users = pgTable('users', {
  id: text('id').primaryKey(),
  name: text('name').notNull(),
  email: text('email').notNull().unique(),
  emailVerified: boolean('email_verified').notNull().default(false),
  image: text('image'),
  createdAt: timestamp('created_at').defaultNow().notNull(),
  updatedAt: timestamp('updated_at').defaultNow().notNull(),
})

export const sessions = pgTable('sessions', {
  id: text('id').primaryKey(),
  expiresAt: timestamp('expires_at').notNull(),
  token: text('token').notNull().unique(),
  userId: text('user_id')
    .notNull()
    .references(() => users.id, { onDelete: 'cascade' }),
  ipAddress: text('ip_address'),
  userAgent: text('user_agent'),
  createdAt: timestamp('created_at').defaultNow().notNull(),
  updatedAt: timestamp('updated_at').defaultNow().notNull(),
})

export const accounts = pgTable('accounts', {
  id: text('id').primaryKey(),
  accountId: text('account_id').notNull(),
  providerId: text('provider_id').notNull(),
  userId: text('user_id')
    .notNull()
    .references(() => users.id, { onDelete: 'cascade' }),
  accessToken: text('access_token'),
  refreshToken: text('refresh_token'),
  idToken: text('id_token'),
  expiresAt: timestamp('expires_at'),
  accessTokenExpiresAt: timestamp('access_token_expires_at'),
  refreshTokenExpiresAt: timestamp('refresh_token_expires_at'),
  scope: text('scope'),
  password: text('password'),
  createdAt: timestamp('created_at').defaultNow().notNull(),
  updatedAt: timestamp('updated_at').defaultNow().notNull(),
})

export const verifications = pgTable('verifications', {
  id: text('id').primaryKey(),
  identifier: text('identifier').notNull(),
  value: text('value').notNull(),
  expiresAt: timestamp('expires_at').notNull(),
  createdAt: timestamp('created_at').defaultNow().notNull(),
  updatedAt: timestamp('updated_at').defaultNow().notNull(),
})

// App tables
export const mediaTypeEnum = pgEnum('media_type', ['movie', 'tv'])

export const watchedItems = pgTable(
  'watched_items',
  {
    userId: text('user_id')
      .notNull()
      .references(() => users.id, { onDelete: 'cascade' }),
    tmdbId: integer('tmdb_id').notNull(),
    mediaType: mediaTypeEnum('media_type').notNull(),
    watchedAt: timestamp('watched_at').defaultNow().notNull(),
  },
  (t) => [primaryKey({ columns: [t.userId, t.tmdbId, t.mediaType] })],
)

export type WatchedItem = typeof watchedItems.$inferSelect
export type MediaType = 'movie' | 'tv'

// Cache schema
export const cacheSchema = pgSchema('cache')

export const trendingMovies = cacheSchema.table('trending_movies', {
  tmdbId: integer('tmdb_id').primaryKey(),
  title: text('title').notNull(),
  posterPath: text('poster_path'),
  backdropPath: text('backdrop_path'),
  overview: text('overview').notNull().default(''),
  voteAverage: doublePrecision('vote_average').notNull().default(0),
  releaseDate: text('release_date'),
  refreshedAt: timestamp('refreshed_at').defaultNow().notNull(),
})

export const trendingTv = cacheSchema.table('trending_tv', {
  tmdbId: integer('tmdb_id').primaryKey(),
  name: text('name').notNull(),
  posterPath: text('poster_path'),
  backdropPath: text('backdrop_path'),
  overview: text('overview').notNull().default(''),
  voteAverage: doublePrecision('vote_average').notNull().default(0),
  firstAirDate: text('first_air_date'),
  refreshedAt: timestamp('refreshed_at').defaultNow().notNull(),
})

export type TrendingMovie = typeof trendingMovies.$inferSelect
export type TrendingTv = typeof trendingTv.$inferSelect
