import { Conversation } from "../type/llmChat/conversation";
import { DialogSlice } from "../type/llmChat/conversation/dialogSlice";

const getLatestVisibleAssistantDialogSlice = (conv: Conversation): DialogSlice|null => {
    let highestIndex: number|null = null;

    for (let i = 0; i < conv.dialogSlices.length; i++) {
        const dialogSlice = conv.dialogSlices[i];
        if (!dialogSlice.visible) {
            continue;
        }

        if (dialogSlice.messages[dialogSlice.selectedIndex].isUserMessage) {
            continue;
        }

        highestIndex = i;
    }

    return highestIndex != null ? conv.dialogSlices[highestIndex] : null;
}

export const getLatestMessageId = (conv?: Conversation): string|null => {
    if (!conv) {
        return null;
    }

    const lastDialogSlice = getLatestVisibleAssistantDialogSlice(conv);
    if (lastDialogSlice == null) {
        return null;
    }
    
    return lastDialogSlice.messages[lastDialogSlice.selectedIndex]?.id ?? null;
}