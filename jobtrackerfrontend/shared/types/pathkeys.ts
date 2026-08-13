export type PathKeys<T> = {
  [K in keyof T & string]:
    T[K] extends object
      ? `${K}.${PathKeys<T[K]>}`
      : K
}[keyof T & string];