import store from "../../../store";
import { deleteConversationOption } from "../../../store/conversationListSlice";
import { deleteFullConversation } from "../../../util/client/conversation";

type SidebarContextMenuProps = {
    conversationId: string;
}

const SidebarContextMenu: React.FC<SidebarContextMenuProps> = ({ conversationId }) => {
    const deleteConversation = async (convId: string) => {
        const res = await deleteFullConversation(convId);
        if (!res.ok) {
            throw new Error("Failed to complete delete request");
        }

        if (res.data) {
            store.dispatch(deleteConversationOption(convId));
        }
    }

    return (
        <div className="grid grid-cols-[1fr_17px]">
            <button
                onMouseDown={() => deleteConversation(conversationId)}
                className="w-full bg-red-600 text-white rounded-sm mt-2 hover:bg-red-800 hover:underline">Delete</button>
            <div></div>
        </div>
    );
}

export default SidebarContextMenu;