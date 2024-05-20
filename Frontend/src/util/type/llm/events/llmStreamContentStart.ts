import { LlmStreamEvent } from "./llmStreamEvent";
import { LlmStreamEventType } from "./llmStreamEventType";

export type LlmStreamContentStart = Omit<LlmStreamEvent, 'type'> & {
    type: LlmStreamEventType.contentStart;
    index: number;
}