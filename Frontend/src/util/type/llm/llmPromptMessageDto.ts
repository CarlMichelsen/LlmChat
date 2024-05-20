import { LlmContent } from "./content/llmContent";

export type LlmPromptMessageDto = {
    isUserMessage: boolean;
    content: LlmContent[]
}