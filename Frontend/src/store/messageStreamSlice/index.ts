import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { StreamContent } from '../../util/type/llmChat/conversation/streamContent';

type ConversationMessageStream = {
    streaming: boolean;
    messageContent: string[];
}

type MessageStreamState = {
    [conversationId: string]: ConversationMessageStream;
}

const initialState: MessageStreamState = {};

const messageStreamSlice = createSlice({
    name: 'messageStream',
    initialState,
    reducers: {
        appendStream: (state, action: PayloadAction<{conversationId: string, content: StreamContent }>) => {
            const id = action.payload.conversationId;
            const content = action.payload.content;

            if (!state[id]) {
                state[id] = {
                    streaming: true,
                    messageContent: [],
                };
            }

            if (state[id].messageContent[content.index]) {
                state[id].messageContent[content.index] += content.content;
            } else {
                state[id].messageContent[content.index] = content.content;
            }
        },
        clearStream: (state, action: PayloadAction<string>) => {
            delete state[action.payload];
        }
    },
});

export const {
    appendStream,
    clearStream,
} = messageStreamSlice.actions;

export default messageStreamSlice.reducer;