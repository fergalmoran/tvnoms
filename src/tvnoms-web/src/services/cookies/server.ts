import type {ResponseCookie} from "next/dist/compiled/@edge-runtime/cookies";
import {cookies} from "next/headers";

import {CookieAttributes, Cookies} from "./types";

export const getCookies = (): Cookies => {
  const org = cookies();

  return {
    get: (name?: string) => (name == null ? Object.fromEntries(org.getAll().map((c) => [c.name, c.value])) : org.get(name)?.value) as never,

    set: (name, value, options) => {
      const pre = org.get(name)?.value;
      try {
        org.set(name, value, options && convertCookieAttributes(options));
      } catch {
        //Cookies can only be modified in a Server Action or Route Handler. Read more: https://nextjs.org/docs/app/api-reference/functions/cookies#cookiessetname-value-options
      }
      return pre;
    },

    remove: (name) => org.delete(name)
  };
};

const convertCookieAttributes = (options: CookieAttributes): Partial<ResponseCookie> => ({
  expires: typeof options.expires === "number" ? options.expires * 864e5 : options.expires,
  path: options.path,
  domain: options.domain,
  secure: options.secure,
  sameSite: options.sameSite?.toLowerCase() as "strict" | "lax" | "none" | undefined
});
