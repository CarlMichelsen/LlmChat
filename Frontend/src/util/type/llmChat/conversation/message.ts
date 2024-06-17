import { Content } from "./content";
import { Prompt } from "./prompt";

export type Message = {
    id: string;
    isUserMessage: boolean;
    prompt?: Prompt
    content: Content[];
    completedUtc: string;
    previousMessageId: string|null;
}