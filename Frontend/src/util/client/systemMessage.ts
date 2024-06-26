import { EditSystemMessageDto } from "../type/systemMessage/editSystemMessage";
import { SystemMessage } from "../type/systemMessage/systemMessage";
import { serviceRequest } from "./serviceRequest";

export const getSystemMessage = async (systemMessageId: string) => {
    return await serviceRequest<SystemMessage>("GET", `/api/v1/systemMessage/${systemMessageId}`);
}

export const addSystemMessage = async (addSystemMessage: EditSystemMessageDto) => {
    return await serviceRequest<SystemMessage>("POST", "/api/v1/systemMessage", addSystemMessage);
}

export const editSystemMessage = async (systemMessageId: string, editSystemMessage: EditSystemMessageDto) => {
    return await serviceRequest<SystemMessage>("PUT", `/api/v1/systemMessage/${systemMessageId}`, editSystemMessage);
}

export const softDeleteSystemMessage = async (systemMessageId: string) => {
    return await serviceRequest<SystemMessage>("DELETE", `/api/v1/systemMessage/${systemMessageId}`);
}

export const getSystemMessageList = async () => {
    return await serviceRequest<SystemMessage>("GET", "/api/v1/systemMessage");
}