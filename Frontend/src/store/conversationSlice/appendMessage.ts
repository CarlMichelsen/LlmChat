import { PayloadAction } from "@reduxjs/toolkit";
import { Message } from "../../util/type/llmChat/conversation/message";
import { ConversationState } from ".";
import { Conversation } from "../../util/type/llmChat/conversation";
import { DialogSlice } from "../../util/type/llmChat/conversation/dialogSlice";

export type AppendMessagePayload = {
    conversationId: string,
    message: Message
};

const createSliceFromMessage = (message: Message): DialogSlice => {
    return {
        messages: [message],
        selectedIndex: 0,
        visible: true,
    } satisfies DialogSlice;
}

const createOrUpdateSlice = (conversation: Conversation, message: Message, prevMessageId: string|null): DialogSlice|null => {
    const indexOfPrevSlice = conversation.dialogSlices
        .findIndex(slice => slice.messages.find(m => m.id == prevMessageId));
    
    if (indexOfPrevSlice == -1 && conversation.dialogSlices.length === 0) {
        const newSlice = createSliceFromMessage(message);
        return newSlice;
    }
    
    let currentSlice: DialogSlice|null = conversation.dialogSlices[indexOfPrevSlice + 1] ?? null;
    if (currentSlice == null)
    {
        return createSliceFromMessage(message);
    }
    
    currentSlice.visible = true;
    currentSlice.messages.push(message);
    currentSlice.selectedIndex = currentSlice.messages.length - 1;
    return null;
}

export const appendMessageAction = (state: ConversationState, action: PayloadAction<AppendMessagePayload>) => {
    if (state.conversation == null || action.payload.conversationId != state.conversation?.id) {
        return;
    }

    const newSlice = createOrUpdateSlice(
        state.conversation,
        action.payload.message,
        action.payload.message.previousMessageId);
    
    if (newSlice != null)
    {
        state.conversation.dialogSlices.push(newSlice);
    }
}