CREATE SCHEMA "cache";
--> statement-breakpoint
CREATE TABLE "cache"."trending_movies" (
	"tmdb_id" integer PRIMARY KEY NOT NULL,
	"title" text NOT NULL,
	"poster_path" text,
	"backdrop_path" text,
	"overview" text DEFAULT '' NOT NULL,
	"vote_average" double precision DEFAULT 0 NOT NULL,
	"release_date" text,
	"refreshed_at" timestamp DEFAULT now() NOT NULL
);
--> statement-breakpoint
CREATE TABLE "cache"."trending_tv" (
	"tmdb_id" integer PRIMARY KEY NOT NULL,
	"name" text NOT NULL,
	"poster_path" text,
	"backdrop_path" text,
	"overview" text DEFAULT '' NOT NULL,
	"vote_average" double precision DEFAULT 0 NOT NULL,
	"first_air_date" text,
	"refreshed_at" timestamp DEFAULT now() NOT NULL
);
--> statement-breakpoint
