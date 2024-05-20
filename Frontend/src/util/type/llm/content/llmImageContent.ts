import { LlmContent } from "./llmContent";

export type LlmImageContent = LlmContent & {
    format: "base64";
    mediaType: string;
    data: string;
}