import { useSelector } from "react-redux";
import store, { RootApplicationState } from "../../../store";
import { logoutRequest } from "../../../util/client/loginClient";
import { logout } from "../../../store/userSlice";
import Conversations from "./Conversations";

const Sidebar: React.FC = () => {
    const userState = useSelector((state: RootApplicationState) => state.user);

    const handleLogout = async () => {
        const res = await logoutRequest();
        if (res.ok && res.data) {
            store.dispatch(logout());
        }
    }

    return (
        <div className="grid grid-cols-[50px_200px_1fr] md:grid-cols-[50px_1fr]">
            <img
                src={userState.user!.avatarUrl}
                alt="profile"
                width="50px"
                height="50px" />
            
            <div className="ml-2">
                <p>{userState.user!.name}</p>
                <button
                    onClick={() => handleLogout()}
                    className="bg-red-400 hover:bg-red-600 hover:text-white py-px w-full">Log out</button>
            </div>

            <Conversations />
        </div>
    );
}

export default Sidebar;