"use client"

import React from 'react'
import {Api} from "@/services/api/core";
import {ApiConfig, User} from "@/services/api/types";
import {useCookies} from "@/services/cookies/client";

const ApiContext = React.createContext<Api>(undefined!);

export const useApi = (): Api => {
  const api = React.useContext(ApiContext);
  if (api === undefined) {
    throw new Error("useApi must be used within ApiProvider");
  }
  return api;
};
export const useUser = (): User | null | undefined => {
  const api = React.useContext(ApiContext);
  if (api === undefined) {
    throw new Error("useUser must be used within ApiProvider");
  }

  const [user, setUser] = React.useState<User | null | undefined>(api.user.value);

  React.useEffect(() => {
    const subscription = api.user.subscribe((currentUser) => {
      setUser(currentUser);
    });

    return () => {
      subscription.unsubscribe();
    };
  }, []);

  return user;
};
export const ApiProvider: React.FC<{ config: ApiConfig; children: React.ReactNode }> = ({config, children}) => {
  const cookies = useCookies();
  const api = React.useRef(new Api({...config, store: cookies})).current;
  return <ApiContext.Provider value={api}> {children} </ApiContext.Provider>;
};
