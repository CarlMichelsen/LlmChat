import { LlmStreamEvent } from "./llmStreamEvent";
import { LlmStreamEventType } from "./llmStreamEventType";

export type LlmStreamTotalUsage = Omit<LlmStreamEvent, 'type'> & {
    type: LlmStreamEventType.totalUsage;
    providerPromptIdentifier: string
    inputTokens: number
    outputTokens: number
    stopReason: string
}