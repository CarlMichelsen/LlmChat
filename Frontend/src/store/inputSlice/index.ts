import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { Message } from '../../util/type/llmChat/conversation/message';

type InputFieldState = {
    ready: boolean;
    text: string;
    editing?: Message;
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
        editMessage: (state, action: PayloadAction<{ conversationId: string, editing?: Message }>) => {
            if (!state.inputField[action.payload.conversationId]) {
                state.inputField[action.payload.conversationId] = { ...initialInputFieldState };
            }

            if (!state.inputField[action.payload.conversationId].ready) {
                return;
            }

            state.inputField[action.payload.conversationId] = {
                ready: true,
                text: action.payload.editing?.content.filter(c => c.contentType === "Text")[0].content ?? "",
                editing: action.payload.editing,
            }
        },
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
    editMessage,
    setInputText,
    setInputReady,
} = inputSlice.actions;

export default inputSlice.reducer;