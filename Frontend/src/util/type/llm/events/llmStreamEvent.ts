import { LlmStreamEventType } from "./llmStreamEventType";

export type LlmStreamEvent = {
    typeName: string;
    type: LlmStreamEventType;
}