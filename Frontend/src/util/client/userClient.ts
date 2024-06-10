import { OAuthUser } from "../../store/userSlice/oAuthUser";
import { serviceRequest } from "./serviceRequest";

export const getUser = async () => {
    return await serviceRequest<OAuthUser>("GET", "/api/v1/session");
}