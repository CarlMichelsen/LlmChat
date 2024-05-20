import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { OAuthUser } from './oAuthUser';

type UserState = {
    authorized: "pending" | "logged-in" | "logged-out",
    user?: OAuthUser,
}

const initialState: UserState = {
    authorized: "pending",
};

const userSlice = createSlice({
  name: 'user',
  initialState,
  reducers: {
    login: (state, action: PayloadAction<OAuthUser>) => {
        state.authorized = "logged-in"
        state.user = action.payload
    },
    logout: (state) => {
        state.authorized = "logged-out"
        state.user = undefined
    },
  },
});

export const { login, logout } = userSlice.actions;

export default userSlice.reducer;