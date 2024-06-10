import { serviceRequest } from "./serviceRequest";

export const setDefaultSystemMessage = async (defaultSystemMessage: string) => {
    return await serviceRequest<string>("POST", `/api/v1/profile/system`, defaultSystemMessage);
}