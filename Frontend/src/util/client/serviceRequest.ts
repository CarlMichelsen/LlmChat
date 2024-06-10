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
        const isString = typeof(body) === "string";
        const fullDestination = getFullDestinationFromPath(path);
        const response = await fetch(
            fullDestination,
        {
            method: method,
            credentials: 'include',
            body: isString ? body : JSON.stringify(body),
            headers: isString ? {"Content-Type": "text/plain"} : { "Content-Type": "application/json" },
        });

        // Check if the request was successful
        if (response.ok) {
            const data = await response.json();
            return data as ServiceResponse<T>;
        } else {
            throw new Error(`${method} "${fullDestination}" request failed with status: ${response.status}`);
        }
    } catch (error) {
        console.error(`Fetch error: ${error}`);
        throw error;  
    }
}