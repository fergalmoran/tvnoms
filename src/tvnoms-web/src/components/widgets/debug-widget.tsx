"use client"
import {useUser} from "@/services/api/client";

export const DebugWidget: React.FC = () => {
  const currentUser = useUser()
  return (<div>
    <h1>I AM HERE</h1>
    <p>{currentUser?.email}</p>
  </div>)
}
