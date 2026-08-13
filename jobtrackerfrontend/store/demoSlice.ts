import { createSlice, PayloadAction } from "@reduxjs/toolkit";

interface DemoState {
  value: number;
}

const initialState: DemoState = {
  value: 0,
};

const demoSlice = createSlice({
  name: "demo",
  initialState,
  reducers: {
    increment(state) {
      state.value += 1;
    },
    decrement(state) {
      state.value -= 1;
    },
    incrementByAmount(state, action: PayloadAction<number>) {
      state.value += action.payload;
    },
  },
});

export const { increment, decrement, incrementByAmount } = demoSlice.actions;
export default demoSlice.reducer;