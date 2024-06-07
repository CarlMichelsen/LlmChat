import { PayloadAction } from "@reduxjs/toolkit";
import { ConversationState } from ".";
import { Conversation } from "../../util/type/llmChat/conversation";
import { Message } from "../../util/type/llmChat/conversation/message";

export type SelectMessagePayload = {
    conversationId: string,
    messageId: string
}

const getSliceIdAndMessageSliceId = (conversation: Conversation, messageId: string): { sliceId: number, messageSliceId: number } | null => {
    const sliceId = conversation.dialogSlices
        .findIndex(s => s.messages.find(m => m.id === messageId));
    
    if (sliceId === -1) {
        return null;
    }

    const messageSliceId = conversation.dialogSlices[sliceId].messages
        .findIndex(m => m.id === messageId);
    
    if (messageSliceId === -1) {
        throw new Error("I know this message should be present in the slice. This should never ever happen");
    }

    return { messageSliceId, sliceId };
}

export const selectMessageAction = (state: ConversationState, action: PayloadAction<SelectMessagePayload>) => {
    if (state.conversation == null) {
        return;
    }

    const ids = getSliceIdAndMessageSliceId(state.conversation, action.payload.messageId);
    if (ids == null) {
        return;
    }

    state.conversation.dialogSlices[ids.sliceId].selectedIndex = ids.messageSliceId;

    let message: Message|null = state.conversation.dialogSlices[ids.sliceId].messages[ids.messageSliceId];
    let nextSliceId = ids.sliceId;
    while (message != null) {
        nextSliceId++;
        const nextSlice = state.conversation.dialogSlices[nextSliceId];
        const indexOfNextMessage = nextSlice?.messages.findIndex(m => m.previousMessageId === message!.id) ?? -1;

        if (indexOfNextMessage === -1) {
            message = null;
            continue;
        }

        state.conversation.dialogSlices[nextSliceId].selectedIndex = indexOfNextMessage;
        message = nextSlice.messages[indexOfNextMessage];
    }

    state.conversation.dialogSlices.forEach((slice, index) => {
        slice.visible = index <= nextSliceId - 1;
    });
}