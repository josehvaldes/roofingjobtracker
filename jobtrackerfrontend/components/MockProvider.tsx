"use client";

import { useEffect } from "react";

export function MockProvider({
    children,
}: {
    children: React.ReactNode;
}) {

    useEffect(() => {

        async function start() {

            if (process.env.NEXT_PUBLIC_ENABLE_MOCKS !== "true")
                return;

            const { worker } = await import("@/mocks/browser");

            await worker.start({
                onUnhandledRequest: "bypass",
            });
        }

        start();

    }, []);

    return children;
}