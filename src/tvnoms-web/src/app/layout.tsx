import type { Metadata } from "next";
import { Inter } from "next/font/google";
import { cookies as cookieStore, headers } from "next/headers";
import { ApiProvider } from "@/services/api/client";
import { CookiesProvider } from "@/services/cookies/client";
import "./globals.css";
import AppProvider from "@/components/providers/app-provider";
import { apiConfig } from "@/config/api-config";
import Navbar from "@/components/navbar";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
  title: "TV Noms",
  description: "Robot powered couch surfing",
};

const RootLayout = ({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) => {
  const headersList = headers();
  console.log("layout", "headersList", headersList.get("x-pathname"));
  if (headersList.get("x-pathname")?.startsWith("/bt")) return children;

  return (
    <html lang="en">
      <body className={`${inter.className} dark`}>
        <CookiesProvider value={cookieStore().getAll()}>
          <ApiProvider config={apiConfig}>
            <AppProvider>
              <Navbar />
              {children}
              <div id="modal-root"></div>
            </AppProvider>
          </ApiProvider>
        </CookiesProvider>
      </body>
    </html>
  );
};
export default RootLayout;
