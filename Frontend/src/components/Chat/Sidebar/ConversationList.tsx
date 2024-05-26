import { useSelector } from "react-redux";
import store, { RootApplicationState } from "../../../store";
import { openDesktopConversationList } from "../../../store/conversationListSlice";
import ConversationOption from "./ConversationOption"

const ConversationList: React.FC<{ conversations: string[] }> = ({ conversations }) => {
    const conversationListState = useSelector((state: RootApplicationState) => state.conversationList);

    // openDesktopConversationList
    return (
        <div id="desktop-conversation-list">
            <div className={`relative h-[50px] grid ${conversationListState.desktopIsOpen ? "grid-cols-[1fr_50px]" : "grid-cols-1"}`}>
                <div className={conversationListState.desktopIsOpen ? "block" : "hidden"}>
                    <p>meme</p>
                </div>
                <button
                    className={`absolute ${conversationListState.desktopIsOpen ? "right-0" : "-right-[50px]"} h-full w-[50px] text-2xl pb-1 hover:text-green-400 transition-all`}
                    onClick={() => store.dispatch(openDesktopConversationList(!conversationListState.desktopIsOpen))}
                    >{conversationListState.desktopIsOpen ? "←" : "→"}</button>
            </div>

            <ol className={conversationListState.desktopIsOpen ? "block" : "hidden"}>
                {conversations.map(s => (<ConversationOption key={s} summary={s} />))}
            </ol>
        </div>
    )
}

export default ConversationList;