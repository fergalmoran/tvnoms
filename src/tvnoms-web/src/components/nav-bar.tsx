import { Link } from '@tanstack/react-router'
import { Tv } from 'lucide-react'
import { authClient } from '#/lib/auth-client'
import { Button } from '#/components/ui/button'
import { Avatar, AvatarFallback, AvatarImage } from '#/components/ui/avatar'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from '#/components/ui/dropdown-menu'
import { ThemeToggle } from '#/components/theme-toggle'

export function NavBar() {
  const { data: session } = authClient.useSession()

  return (
    <header className="sticky top-0 z-50 w-full border-b bg-background/80 backdrop-blur-sm">
      <div className="container mx-auto flex h-14 items-center gap-4 px-4">
        <Link to="/" className="flex items-center gap-2 font-bold text-lg">
          <Tv className="h-5 w-5" />
          TvNoms
        </Link>

        <nav className="flex items-center gap-1 ml-2">
          <Link
            to="/"
            className="text-sm font-medium px-3 py-1.5 rounded-md hover:bg-accent transition-colors"
            activeProps={{ className: 'text-sm font-medium px-3 py-1.5 rounded-md bg-accent' }}
            activeOptions={{ exact: true }}
          >
            Trending
          </Link>
          <Link
            to="/search"
            className="text-sm font-medium px-3 py-1.5 rounded-md hover:bg-accent transition-colors"
            activeProps={{ className: 'text-sm font-medium px-3 py-1.5 rounded-md bg-accent' }}
          >
            Search
          </Link>
          {session?.user && (
            <Link
              to="/watchlist"
              className="text-sm font-medium px-3 py-1.5 rounded-md hover:bg-accent transition-colors"
              activeProps={{ className: 'text-sm font-medium px-3 py-1.5 rounded-md bg-accent' }}
            >
              Watchlist
            </Link>
          )}
        </nav>

        <div className="ml-auto flex items-center gap-2">
          <ThemeToggle />
          {session?.user ? (
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button variant="ghost" className="h-8 w-8 p-0 rounded-full">
                  <Avatar className="h-8 w-8">
                    <AvatarImage src={session.user.image ?? undefined} />
                    <AvatarFallback>
                      {session.user.name?.charAt(0).toUpperCase() ?? 'U'}
                    </AvatarFallback>
                  </Avatar>
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end">
                <DropdownMenuItem className="text-sm font-medium" disabled>
                  {session.user.name}
                </DropdownMenuItem>
                <DropdownMenuItem
                  onClick={() => authClient.signOut()}
                  className="cursor-pointer"
                >
                  Sign out
                </DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          ) : (
            <div className="flex items-center gap-2">
              <Button variant="ghost" size="sm" asChild>
                <Link to="/sign-in">Sign in</Link>
              </Button>
              <Button size="sm" asChild>
                <Link to="/sign-up">Sign up</Link>
              </Button>
            </div>
          )}
        </div>
      </div>
    </header>
  )
}
