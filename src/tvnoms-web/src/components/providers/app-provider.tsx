"use client"
import React from "react";
import {ExternalWindow} from "@/services/exterrnal-window";
import * as zustand from "zustand";
import {useApi, useUser} from "@/services/api/client";

export interface AppProviderProps {
  children: React.ReactNode;
}

const AppProvider: React.FC<AppProviderProps> = ({children}) => {
  const [mounted, setMounted] = React.useState(false);
  const {finishLoading} = useAppStore();
  const api = useApi();
  const currentUser = useUser();

  React.useEffect(() => {
    setMounted(true);
    setTimeout(() => {
      finishLoading();
      ExternalWindow.notify();
    }, 1500);
  }, []);
  return <>{children}</>;
}

export type AppState = {
  loading: boolean;
};

export type AppActions = {
  startLoading: () => void;
  finishLoading: () => void;
};

export const useAppStore = zustand.create<AppState & AppActions>((set) => ({
  loading: true,
  startLoading: () => set((state) => ({...state, loading: true})),
  finishLoading: () => set((state) => ({...state, loading: false}))
}));
export default AppProvider;
