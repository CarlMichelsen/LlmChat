import { LlmResponse } from "../llmResponse";
import { LlmStreamEvent } from "./llmStreamEvent";
import { LlmStreamEventType } from "./llmStreamEventType";

export type LlmStreamMessageStart = Omit<LlmStreamEvent, 'type'> & {
    type: LlmStreamEventType.messageStart;
    message: LlmResponse;
}