import { describe, expectTypeOf, it } from "vitest";
import type { PathKeys } from "./pathkeys";

describe("PathKeys", () => {
  it("creates dot-notation paths to leaf properties", () => {
    type Example = {
      name: string;
      address: {
        city: string;
        zipCode: string;
      };
    };

    expectTypeOf<PathKeys<Example>>().toEqualTypeOf<
      "name" | "address.city" | "address.zipCode"
    >();
  });

  it("handles deeply nested objects", () => {
  type Example = {
    a: {
      b: string;
      c: {
        d: number;
      };
    };
  };

  expectTypeOf<PathKeys<Example>>().toEqualTypeOf<
    "a.b" | "a.c.d"
  >();
});


});