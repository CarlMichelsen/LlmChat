import { LlmStreamEvent } from "./llmStreamEvent";
import { LlmStreamEventType } from "./llmStreamEventType";

export type LlmStreamContentStop = Omit<LlmStreamEvent, 'type'> & {
    type: LlmStreamEventType.contentStop;
    index: number;
}