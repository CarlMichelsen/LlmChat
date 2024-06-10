import { useSelector } from "react-redux";
import store, { RootApplicationState } from "../../../store";
import { openDesktopConversationList, selectConversation } from "../../../store/conversationListSlice";
import ConversationOptionComponent from "./ConversationOptionComponent"
import { ConversationOption } from "../../../util/type/conversationOption";

const ConversationList: React.FC<{ conversations?: ConversationOption[] }> = ({ conversations }) => {
    const conversationListState = useSelector((state: RootApplicationState) => state.conversationList);

    const handleSelectConversation = (conversationId: string) => {
        store.dispatch(selectConversation(conversationId));
    }

    return (
        <div id="desktop-conversation-list" className="h-full grid grid-rows-[50px_1fr]">
            <div className={`relative h-[50px] grid ${conversationListState.desktopIsOpen ? "grid-cols-[1fr_50px]" : "grid-cols-1"}`}>
                <div className={conversationListState.desktopIsOpen ? "block" : "hidden"}>
                    <button
                        className="w-full h-full bg-blue-500 text-white hover:underline overflow-hidden break-keep"
                        onMouseDown={() => store.dispatch(selectConversation(null))}>
                        <p className="mx-auto w-48">Create new conversation</p>
                    </button>
                </div>
                <button
                    className={`absolute ${conversationListState.desktopIsOpen ? "right-0" : "-right-[50px]"} h-full w-[50px] text-2xl pb-1 hover:text-green-400 transition-all z-10`}
                    onClick={() => store.dispatch(openDesktopConversationList(!conversationListState.desktopIsOpen))}
                    >{conversationListState.desktopIsOpen ? "←" : "→"}</button>
            </div>

            <ol className={conversationListState.desktopIsOpen ? "block space-y-2 overflow-y-scroll h-full pt-2" : "hidden"}>
                {conversations?.map(s => (<ConversationOptionComponent key={s.id} option={s} selected={conversationListState.selectedConversationId == s.id} selectConversation={handleSelectConversation} />))}
            </ol>
        </div>
    )
}

export default ConversationList;