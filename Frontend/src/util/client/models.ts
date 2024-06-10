import { LargeLanguageModelDto } from "../type/llmChat/model";
import { serviceRequest } from "./serviceRequest";

export const getAvailableModels = async () => {
    return await serviceRequest<LargeLanguageModelDto[]>("GET", "/api/v1/model/all");
}