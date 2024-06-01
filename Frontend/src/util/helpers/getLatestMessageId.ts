import { Conversation } from "../type/llmChat/conversation";

export const getLatestMessageId = (conv?: Conversation): string|null => {
    if (!conv) {
        return null;
    }

    const lastDialogSlice = conv.dialogSlices[conv.dialogSlices.length - 1] ?? null;
    if (lastDialogSlice == null) {
        return null;
    }
    
    return lastDialogSlice.messages[lastDialogSlice.selectedIndex]?.id ?? null;
}