import { OAuthUser } from "../../store/userSlice/oAuthUser";
import { ServiceResponse } from "../type/serviceResponse";
import { rootUrl } from "./endpoints";

export const getUser = async (): Promise<ServiceResponse<OAuthUser>> => {
    try {
        const response = await fetch(
            `${rootUrl()}/api/v1/session`,
        {
            credentials: 'include',
        });

        // Check if the request was successful
        if (response.ok) {
            const data = await response.json();
            return data as ServiceResponse<OAuthUser>;
        } else {
            throw new Error(`Request failed with status: ${response.status}`);
        }
    } catch (error) {
        console.error(`Fetch error: ${error}`);
        throw error;  
    }
}