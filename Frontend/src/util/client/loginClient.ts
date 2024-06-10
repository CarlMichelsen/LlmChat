import { loginUrl } from "./endpoints";
import { serviceRequest } from "./serviceRequest";

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
    return await serviceRequest<boolean>("DELETE", "/api/v1/session");
}