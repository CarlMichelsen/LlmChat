import { Content } from "./conversation/content";

export type NewMessage = {
    conversationId?: string;
    responseToMessageId: string|null;
    content: Content[];
    modelIdentifier: string;
}