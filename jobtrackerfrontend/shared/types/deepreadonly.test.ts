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

  it("makes arrays readonly", () => {
    type User = {
        name: string;
        roles: string[];
    };

    type ReadonlyUser = DeepReadonly<User>;

    expectTypeOf<ReadonlyUser>().toEqualTypeOf<{
        readonly name: string;
        readonly roles: ReadonlyArray<string>;
    }>();
    });

  it("makes Maps readonly recursively", () => {
    type Data = {
        metadata: Map<string, {
        value: number;
        }>;
    };

    type ReadonlyData = DeepReadonly<Data>;

    expectTypeOf<ReadonlyData>().toEqualTypeOf<{
        readonly metadata: ReadonlyMap<string, {
        readonly value: number;
        }>;
    }>();
    });
  it("makes Sets readonly recursively", () => {
    type Data = {
        permissions: Set<{
        name: string;
        }>;
    };

    type ReadonlyData = DeepReadonly<Data>;

    expectTypeOf<ReadonlyData>().toEqualTypeOf<{
        readonly permissions: ReadonlySet<{
        readonly name: string;
        }>;
    }>();
  });

  it("does not modify primitive types", () => {
    expectTypeOf<DeepReadonly<string>>().toEqualTypeOf<string>();
    expectTypeOf<DeepReadonly<number>>().toEqualTypeOf<number>();
    expectTypeOf<DeepReadonly<boolean>>().toEqualTypeOf<boolean>();
  });

});