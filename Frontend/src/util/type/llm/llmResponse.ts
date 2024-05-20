import { LlmPromptMessageDto } from "./llmPromptMessageDto"
import { LlmUsage } from "./llmUsage"

export type LlmResponse = {
    providerPromptIdentifier: string
    modelId: string
    modelIdentifierName: string
    message: LlmPromptMessageDto
    usage: LlmUsage
    stopReason: string
    detailedModelIdentifierName: string
}