import { LargeLanguageModelDto } from "../type/llmChat/model";
import { ServiceResponse } from "../type/serviceResponse";
import { rootUrl } from "./endpoints";

export const getAvailableModels = async () => {
    const response = await fetch(
        `${rootUrl()}/api/v1/model/all`,
    {
        method: 'GET',
        credentials: 'include',
        headers: { "Content-Type": "application/json", },
    });

    // Check if the request was successful
    if (response.ok) {
        const data = await response.json();
        return data as ServiceResponse<LargeLanguageModelDto[]>;
    } else {
        throw new Error(`Request failed with status: ${response.status}`);
    }
}