import { expectTypeOf, describe, it } from "vitest";
import type {DeepReadonly} from "./deepreadonly";

describe("DeepReadonly", () => {
  it("makes nested properties readonly", () => {
    type User = {
      name: string;
      address: {
        city: string;
      };
    };

    type ReadonlyUser = DeepReadonly<User>;

    expectTypeOf<ReadonlyUser>().toEqualTypeOf<{
      readonly name: string;
      readonly address: {
        readonly city: string;
      };
    }>();
  });
});