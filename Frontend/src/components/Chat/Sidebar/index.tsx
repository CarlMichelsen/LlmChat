import { useSelector } from "react-redux";
import store, { RootApplicationState } from "../../../store";
import { logoutRequest } from "../../../util/client/loginClient";
import { logout } from "../../../store/userSlice";
import Conversations from "./Conversations";
import { ConversationOption } from "../../../util/type/optionDateCollection";
import { getConversationOptions } from "../../../util/client/conversation";
import { useQuery } from "react-query";
import { useEffect } from "react";
import { setConversationOptions } from "../../../store/conversationListSlice";

const Sidebar: React.FC = () => {
    const userState = useSelector((state: RootApplicationState) => state.user);
    const conversationListState = useSelector((state: RootApplicationState) => state.conversationList);

    const fetchConversationOptions = async (): Promise<ConversationOption[]> => {
        const res = await getConversationOptions();
        if (res.ok) {
            return res.data!;
        } else {
            throw new Error("Failed to fetch conversation options");
        }
    }

    const { data, status } = useQuery<ConversationOption[], Error>(
        'conversationList',
        fetchConversationOptions,
        { staleTime: 1000 * 60 * 15 });

    useEffect(() => {
        if (status === "success") {
            store.dispatch(setConversationOptions(data));
        }
    }, [data, status]);

    const handleLogout = async () => {
        const res = await logoutRequest();
        if (res.ok && res.data) {
            store.dispatch(logout());
        }
    }

    return (
        <div className={`grid grid-rows-[1fr_50px] md:grid-rows-1 md:grid-cols-1 grid-cols-[250px_1fr] h-screen ${conversationListState.desktopIsOpen && "overflow-x-hidden"}`}>
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