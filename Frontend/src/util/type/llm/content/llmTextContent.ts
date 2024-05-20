import { LlmContent } from "./llmContent";

export type LlmImageContent = LlmContent & {
    text: string;
}