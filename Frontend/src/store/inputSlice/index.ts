import { createSlice, PayloadAction } from '@reduxjs/toolkit';

type InputFieldState = {
    ready: boolean;
    text: string;
    replyToMessageId?: string;
}

const initialInputFieldState: InputFieldState = {
    ready: true,
    text: "",
}

type InputState = {
    inputField: { [conversationId: string]: InputFieldState },
}

const initialState: InputState = {
    inputField: {},
};

const inputSlice = createSlice({
    name: 'input',
    initialState,
    reducers: {
        setInputText: (state, action: PayloadAction<{ conversationId: string, input: string }>) => {
            if (!state.inputField[action.payload.conversationId]) {
                state.inputField[action.payload.conversationId] = { ...initialInputFieldState };
            }

            state.inputField[action.payload.conversationId].text = action.payload.input;
        },
        setInputReady: (state, action: PayloadAction<{ conversationId: string, ready: boolean }>) => {
            if (!state.inputField[action.payload.conversationId]) {
                state.inputField[action.payload.conversationId] = { ...initialInputFieldState };
            }

            state.inputField[action.payload.conversationId].ready = action.payload.ready;
        }
    },
});

export const {
    setInputText,
    setInputReady,
} = inputSlice.actions;

export default inputSlice.reducer;