import { Conversation } from "../type/llmChat/conversation";
import { ServiceResponse } from "../type/serviceResponse";
import { ConversationOption } from "./conversationOption";
import { rootUrl } from "./endpoints";

export const getConversation = async (conversationId: string): Promise<ServiceResponse<Conversation>> => {
    try {
        const response = await fetch(
            `${rootUrl()}/api/v1/conversation/${conversationId}`,
        {
            credentials: 'include',
        });

        // Check if the request was successful
        if (response.ok) {
            const data = await response.json();
            return data as ServiceResponse<Conversation>;
        } else {
            throw new Error(`Request failed with status: ${response.status}`);
        }
    } catch (error) {
        console.error(`Fetch error: ${error}`);
        throw error;  
    }
}

export const getConversationOptions = async (): Promise<ServiceResponse<ConversationOption[]>> => {
    try {
        const response = await fetch(
            `${rootUrl()}/api/v1/conversation/list`,
        {
            credentials: 'include',
        });

        // Check if the request was successful
        if (response.ok) {
            const data = await response.json();
            return data as ServiceResponse<ConversationOption[]>;
        } else {
            throw new Error(`Request failed with status: ${response.status}`);
        }
    } catch (error) {
        console.error(`Fetch error: ${error}`);
        throw error;  
    }
}