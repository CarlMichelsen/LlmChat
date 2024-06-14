import { ServiceResponse } from "../type/serviceResponse";
import { rootUrl } from "./endpoints";

const getFullDestinationFromPath = (path: string) => {
    if (path.startsWith('/')) {
        path = path.substring(1);
    }
    return `${rootUrl()}/${path}`;
}

export const serviceRequest = async <T>(method: "GET"|"POST"|"DELETE", path: string, body?: any): Promise<ServiceResponse<T>> => {
    try {
        const fullDestination = getFullDestinationFromPath(path);
        const init: RequestInit = {
            method: method,
            credentials: 'include',
            body: JSON.stringify(body),
            headers: { "Content-Type": "application/json" },
        };
        const response = await fetch(fullDestination, init);

        // Check if the request was successful
        if (response.ok) {
            const data = await response.json() as ServiceResponse<T>;
            return data;
        } else {
            throw new Error(`${method} "${fullDestination}" request failed with status: ${response.status}`);
        }
    } catch (error) {
        console.error(`Fetch error: ${error}`);
        throw error;  
    }
}