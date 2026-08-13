import { describe, expectTypeOf, it } from "vitest";
import { createTypedEventEmitter } from "./eventEmitter";

type JobEvents = {
  created: {
    jobId: string;
    title: string;
  };

  completed: {
    jobId: string;
    completedAt: Date;
  };
};

describe("createTypedEventEmitter", () => {
  it("enforces event payload types", () => {
    const emitter = createTypedEventEmitter<JobEvents>();

    emitter.emit("created", {
      jobId: "123",
      title: "Roof repair",
    });

    emitter.emit("completed", {
      jobId: "123",
      completedAt: new Date(),
    });
  });

  it("infers the correct handler payload", () => {
    const emitter = createTypedEventEmitter<JobEvents>();

    emitter.on("created", (payload) => {
      expectTypeOf(payload).toEqualTypeOf<{
        jobId: string;
        title: string;
      }>();
    });

    emitter.on("completed", (payload) => {
      expectTypeOf(payload).toEqualTypeOf<{
        jobId: string;
        completedAt: Date;
      }>();
    });
  });
});