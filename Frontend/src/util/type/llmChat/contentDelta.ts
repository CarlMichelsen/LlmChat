import { Concluded } from "./concluded";
import { StreamContent } from "./conversation/streamContent";

export type ContentDelta = {
    conversationId?: string;
    userMessageId?: string;
    content?: StreamContent;
    concluded?: Concluded;
    error?: string;
}