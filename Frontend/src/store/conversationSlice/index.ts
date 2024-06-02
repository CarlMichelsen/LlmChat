import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { Conversation } from '../../util/type/llmChat/conversation';
import { appendMessageAction, AppendMessagePayload } from './appendMessage';

export type ConversationState = {
    conversation?: Conversation;
}

const initialState: ConversationState = {
};

const conversationSlice = createSlice({
    name: 'conversation',
    initialState,
    reducers: {
        setConversation: (state, action: PayloadAction<Conversation | undefined | null>) => {
            state.conversation = action.payload ?? undefined;
        },
        appendMessage: (state, action: PayloadAction<AppendMessagePayload>) => appendMessageAction(state, action)
    },
});

export const {
    setConversation,
    appendMessage,
} = conversationSlice.actions;

export default conversationSlice.reducer;