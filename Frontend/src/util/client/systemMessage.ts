import { SystemMessage } from "../type/systemMessage";
import { serviceRequest } from "./serviceRequest";

export const getSystemMessage = async (systemMessageId: string) => {
    return await serviceRequest<SystemMessage>("GET", `/api/v1/systemMessage/${systemMessageId}`);
}

export const editSystemMessageContent = async (systemMessageId: string, content: string) => {
    return await serviceRequest<SystemMessage>("PUT", `/api/v1/systemMessage/${systemMessageId}/content`, content);
}

export const editSystemMessageName = async (systemMessageId: string, name: string) => {
    return await serviceRequest<SystemMessage>("PUT", `/api/v1/systemMessage/${systemMessageId}/name`, name);
}

export const softDeleteSystemMessage = async (systemMessageId: string) => {
    return await serviceRequest<SystemMessage>("DELETE", `/api/v1/systemMessage/${systemMessageId}`);
}

export const getSystemMessageList = async () => {
    return await serviceRequest<SystemMessage>("GET", "/api/v1/systemMessage");
}