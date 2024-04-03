/* eslint-disable @next/next/no-img-element */
import React from "react";
import { Show } from ".";
import Image from "next/image";

type ShowListItemProps = {
  show: Show;
  index: number;
};

const ShowListItem: React.FC<ShowListItemProps> = ({ show, index }) => {
  return (
    <a
      href="#"
      key={show.id}
      className={`m-4 group relative flex h-48 items-end overflow-hidden rounded-lg bg-gray-100 shadow-lg md:h-80 ${
        index % 4 == 0 ? "col-span-2" : "col-span-1"
      }`}
    >
      <Image
        width={620}
        height={400}
        src={show.backdropImage}
        loading="lazy"
        alt="Photo by Minh Pham"
        className="absolute inset-0 h-full w-full object-cover object-center transition duration-200 group-hover:scale-110"
      />

      <div className="pointer-events-none absolute inset-0 bg-gradient-to-t from-gray-800 via-transparent to-transparent opacity-50"></div>

      <span className="relative ml-4 mb-3 inline-block text-sm text-white md:ml-5 md:text-lg">
        {`${show.title}`}
      </span>
    </a>
  );
};

export default ShowListItem;
