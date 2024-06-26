import { Conversation } from "../type/llmChat/conversation";
import { ServiceResponse } from "../type/serviceResponse";
import { ConversationOption } from "../type/optionDateCollection";
import { serviceRequest } from "./serviceRequest";

export const getConversation = async (conversationId: string): Promise<ServiceResponse<Conversation>> => {
    return await serviceRequest<Conversation>("GET", `/api/v1/conversation/${conversationId}`);
}

export const deleteFullConversation = async (conversationId: string): Promise<ServiceResponse<boolean>> => {
    return await serviceRequest<boolean>("DELETE", `/api/v1/conversation/${conversationId}`);
}

export const getConversationOptions = async (): Promise<ServiceResponse<ConversationOption[]>> => {
    return await serviceRequest<ConversationOption[]>("GET", "/api/v1/conversation/list");
}

export const setConversationSystemMessage = async (conversationId: string, systemMessage: string): Promise<ServiceResponse<string>> => {
    return await serviceRequest<string>("POST", `/api/v1/conversation/${conversationId}/system`, systemMessage);
}