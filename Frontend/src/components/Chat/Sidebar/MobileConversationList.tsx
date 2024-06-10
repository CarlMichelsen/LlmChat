import { useSelector } from "react-redux";
import ConversationOptionComponent from "./ConversationOptionComponent"
import store, { RootApplicationState } from "../../../store";
import { openMobileConversationList, selectConversation } from "../../../store/conversationListSlice";
import { ConversationOption } from "../../../util/type/conversationOption";

const MobileConversationList: React.FC<{ conversations?: ConversationOption[] }> = ({ conversations }) => {
    const conversationListState = useSelector((state: RootApplicationState) => state.conversationList);

    const handleSelectConversation = (conversationId: string) => {
        store.dispatch(selectConversation(conversationId));
        store.dispatch(openMobileConversationList(false));
    }

    return (
        <div id="mobile-conversation-list">
            <div className="grid grid-cols-[60px_1fr]">
                <button
                    className="hover:underline p-1"
                    onMouseDown={() => store.dispatch(openMobileConversationList(!conversationListState.mobileIsOpen))}>
                    {conversationListState.mobileIsOpen ? "Close" : "Open"}
                </button>

                <button
                    className="hover:underline"
                    onMouseDown={() => store.dispatch(selectConversation(null))}>Create new conversation</button>
            </div>

            {conversationListState.mobileIsOpen && (
                <div className="absolute left-0 top-[50px] w-full z-20">
                    <ol className="mx-2 px-6 bg-white max-h-72 overflow-y-scroll space-y-1 border-b border-l border-r border-black rounded-b-md">
                        {conversations?.map(s => (<ConversationOptionComponent key={s.id} option={s} selected={conversationListState.selectedConversationId == s.id} selectConversation={handleSelectConversation} />))}
                    </ol>
                </div>
            )}
        </div>
    )
}

export default MobileConversationList;