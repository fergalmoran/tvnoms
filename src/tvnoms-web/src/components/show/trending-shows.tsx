"use client";
import * as React from "react";
import Image from "next/image";
import { Show } from "@/components/show";
import { useApi } from "@/services/api/client";
import { useCallback } from "react";
import { PagedResult } from "@/services/api/paged-results";
import ShowListItem from "./show-list-item";

const TrendingShows = () => {
  const api = useApi();
  const [shows, setShows] = React.useState<PagedResult<Show>>();
  const fetchData = useCallback(async () => {
    const results = await api.get<PagedResult<Show>>(`/shows/tv/trending`);
    setShows(results.data);
    console.log("trending-shows", "results", results.data);
  }, [api]);

  React.useEffect(() => {
    fetchData();
  }, [fetchData]);
  return (
    <div className="columns-1 gap-5 sm:columns-2 sm:gap-8 md:columns-3 lg:columns-4 [&>img:not(:first-child)]:mt-8">
      {shows &&
        shows.items.map((show, index) => (
          <ShowListItem key={index} index={index} show={show} />
        ))}
    </div>
  );
};
export default TrendingShows;
