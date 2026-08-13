import { defineConfig } from "vitest/config";
import path from "path";

export default defineConfig({
    test: {
        globals: true,
        environment: "jsdom",
        setupFiles: "./tests/setupTests.ts",
        css: true,
        coverage: {
            provider: "v8",
            reporter: [
                "text",
                "html"
            ]
        }
    },

    resolve: {
        alias: {
            "@": path.resolve(__dirname, "./src")
        }
    }
});