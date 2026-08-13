type EventHandler<T> = (payload: T) => void;

type EventHandlers<Events extends Record<string, unknown>> = {
  [K in keyof Events]?: Set<EventHandler<Events[K]>>;
};

export function createTypedEventEmitter<
  Events extends Record<string, unknown>,
>() {
  const handlers: EventHandlers<Events> = {};

  return {
    on<K extends keyof Events>(
      event: K,
      handler: EventHandler<Events[K]>,
    ) {
      const eventHandlers = handlers[event];

      if (eventHandlers) {
        eventHandlers.add(handler);
      } else {
        handlers[event] = new Set([handler]);
      }
    },

    off<K extends keyof Events>(
      event: K,
      handler: EventHandler<Events[K]>,
    ) {
      handlers[event]?.delete(handler);
    },

    emit<K extends keyof Events>(
      event: K,
      payload: Events[K],
    ) {
      handlers[event]?.forEach((handler) => {
        handler(payload);
      });
    },
  };
}