import {ApiConfig} from "@/services/api/types";
import {env} from "@/env";

export const apiConfig = {
  baseURL: env.NEXT_PUBLIC_API_URL,
  withCredentials: true
} as ApiConfig;
