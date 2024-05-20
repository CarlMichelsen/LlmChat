import { LlmContent } from "../content/llmContent";
import { LlmStreamEvent } from "./llmStreamEvent";
import { LlmStreamEventType } from "./llmStreamEventType";

export type LlmStreamContentDelta = Omit<LlmStreamEvent, 'type'> & {
    type: LlmStreamEventType.contentDelta;
    index: number;
    delta: LlmContent;
}