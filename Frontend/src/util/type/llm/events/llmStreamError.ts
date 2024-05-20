import { LlmStreamEvent } from "./llmStreamEvent";
import { LlmStreamEventType } from "./llmStreamEventType";

export type LlmStreamError = Omit<LlmStreamEvent, 'type'> & {
    type: LlmStreamEventType.error;
    message: string;
}