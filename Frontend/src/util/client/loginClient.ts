import { ServiceResponse } from "../type/serviceResponse";
import { loginUrl, rootUrl } from "./endpoints";

export type LoginType = "Development" | "Guest" | "Github" | "Discord";

export const navigateToLoginPage = (loginType: LoginType) => {
    const endpoints: Record<LoginType, string> = {
        "Development": "/api/v1/Login/Development",
        "Guest": "/api/v1/Login/Guest",
        "Github": "/api/v1/Login/Github",
        "Discord": "/api/v1/Login/Discord"
    };

    const url = new URL(window.location.href);
    const loginPage = `${loginUrl()}${endpoints[loginType]}?redirect=${encodeURIComponent(url.origin)}`;
    window.location.replace(loginPage);
}

export const logoutRequest = async () => {
    const response = await fetch(
        `${rootUrl()}/api/v1/session`,
    {
        method: 'delete',
        credentials: 'include',
    });

    // Check if the request was successful
    if (response.ok) {
        const data = await response.json();
        return data as ServiceResponse<boolean>;
    } else {
        throw new Error(`Request failed with status: ${response.status}`);
    }
}