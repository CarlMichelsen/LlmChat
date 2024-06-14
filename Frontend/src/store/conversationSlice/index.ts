import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { Conversation } from '../../util/type/llmChat/conversation';
import { appendMessageAction, AppendMessagePayload } from './appendMessage';
import { setQueryParam } from '../../util/helpers/queryParameter';
import { selectMessageAction, SelectMessagePayload } from './selectMessage';

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
            setQueryParam("c", action.payload?.id ?? null);
            state.conversation = action.payload ?? undefined;
        },
        appendMessage: (state, action: PayloadAction<AppendMessagePayload>) => appendMessageAction(state, action),
        selectMessage: (state, action: PayloadAction<SelectMessagePayload>) => selectMessageAction(state, action),
        setSystemMessage: (state, action: PayloadAction<string>) => {
            if (!state.conversation) {
                return;
            }

            state.conversation.systemMessage = action.payload;
        },
    },
});

export const {
    setConversation,
    appendMessage,
    selectMessage,
    setSystemMessage,
} = conversationSlice.actions;

export default conversationSlice.reducer;