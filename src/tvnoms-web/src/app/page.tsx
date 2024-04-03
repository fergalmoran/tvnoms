"use server";
import { getUser } from "@/services/api/server";
import TrendingShows from "@/components/show/trending-shows";

export default async function Home() {
  const currentUser = getUser();
  return (
    <main className="min-h-screen p-8">
      {currentUser ? <TrendingShows /> : <p>You need to login!</p>}
    </main>
  );
}
