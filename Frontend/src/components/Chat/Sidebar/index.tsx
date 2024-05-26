import { useSelector } from "react-redux";
import store, { RootApplicationState } from "../../../store";
import { logoutRequest } from "../../../util/client/loginClient";
import { logout } from "../../../store/userSlice";
import Conversations from "./Conversations";

const Sidebar: React.FC = () => {
    const userState = useSelector((state: RootApplicationState) => state.user);
    const conversationListState = useSelector((state: RootApplicationState) => state.conversationList);

    const handleLogout = async () => {
        const res = await logoutRequest();
        if (res.ok && res.data) {
            store.dispatch(logout());
        }
    }

    return (
        <div className={`grid grid-rows-[1fr_50px] md:grid-rows-1 md:grid-cols-1 grid-cols-[250px_1fr] h-full ${conversationListState.desktopIsOpen && "overflow-x-hidden"}`}>
            <div className="order-2 md:order-1">
                <Conversations />
            </div>

            <div className={`order-1 md:order-2 relative grid grid-cols-[50px_200px] ${!conversationListState.desktopIsOpen && "md:hidden"}`}>
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
            </div>
        </div>
    );
}

export default Sidebar;