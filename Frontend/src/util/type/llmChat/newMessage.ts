import { Content } from "./content";

export type NewMessage = {
    conversationId?: number;
    responseToMessageId?: number;
    content: Content[];
    modelIdentifier: string;
}