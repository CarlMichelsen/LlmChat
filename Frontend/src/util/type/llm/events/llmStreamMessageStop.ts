import { LlmStreamEvent } from "./llmStreamEvent";
import { LlmStreamEventType } from "./llmStreamEventType";

export type LlmStreamMessageStop = Omit<LlmStreamEvent, 'type'> & {
    type: LlmStreamEventType.messageStop;
    stopReason: string;
}