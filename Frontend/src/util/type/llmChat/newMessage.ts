import { Content } from "./conversation/content";

export type NewMessage = {
    conversationId?: string;
    responseToMessageId?: string;
    content: Content[];
    modelIdentifier: string;
}