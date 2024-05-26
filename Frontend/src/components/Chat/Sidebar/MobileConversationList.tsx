import { useSelector } from "react-redux";
import ConversationOption from "./ConversationOption"
import store, { RootApplicationState } from "../../../store";
import { openMobileConversationList } from "../../../store/conversationListSlice";

const MobileConversationList: React.FC<{ conversations: string[] }> = ({ conversations }) => {
    const conversationListState = useSelector((state: RootApplicationState) => state.conversationList);

    return (
        <div id="mobile-conversation-list">
            <button
                className="p-1"
                onClick={() => store.dispatch(openMobileConversationList(!conversationListState.mobileIsOpen))}>
                {conversationListState.mobileIsOpen ? "Close" : "Open"}
            </button>

            {conversationListState.mobileIsOpen && (
                <div className="absolute left-0 top-[50px] w-full">
                    <ol className="mx-2 px-6 bg-white">
                        {conversations.map(s => (<ConversationOption key={s} summary={s} />))}
                    </ol>
                </div>
            )}
        </div>
    )
}

export default MobileConversationList;