

export type DeepReadonly<T> =
  T extends Map<infer K, infer V>
    ? ReadonlyMap<K, DeepReadonly<V>>
    : T extends Set<infer V>
      ? ReadonlySet<DeepReadonly<V>>
      : T extends Array<infer U>
        ? ReadonlyArray<DeepReadonly<U>>
        : T extends object
          ? { readonly [K in keyof T]: DeepReadonly<T[K]> }
          : T;


