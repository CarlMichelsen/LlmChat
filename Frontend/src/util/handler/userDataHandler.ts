import store from "../../store";
import { login, logout } from "../../store/userSlice";
import { OAuthUser } from "../../store/userSlice/oAuthUser";
import { ServiceResponse } from "../type/serviceResponse";

export const handleUserDataState = (isLoading: boolean, isError: boolean, serviceResponse?: ServiceResponse<OAuthUser>): void => {
	if (isLoading) {
		console.log("fetching user login state...");
	} else if (serviceResponse != null) {
		serviceResponse.ok
			? store.dispatch(login(serviceResponse.data))
			: store.dispatch(logout());
	} else if (isError) {
		console.error("ERROR");
	} else {
		console.error("BAD ERROR");
	}
}