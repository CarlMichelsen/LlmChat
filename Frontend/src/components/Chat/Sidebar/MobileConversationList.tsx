import { useSelector } from "react-redux";
import ConversationOptionComponent from "./ConversationOptionComponent"
import store, { RootApplicationState } from "../../../store";
import { openMobileConversationList, selectConversation } from "../../../store/conversationListSlice";
import { ConversationOption } from "../../../util/client/conversationOption";

const MobileConversationList: React.FC<{ conversations?: ConversationOption[] }> = ({ conversations }) => {
    const conversationListState = useSelector((state: RootApplicationState) => state.conversationList);

    const handleSelectConversation = (conversationId: string) => {
        store.dispatch(selectConversation(conversationId));
        store.dispatch(openMobileConversationList(false));
    }

    return (
        <div id="mobile-conversation-list">
            <button
                className="p-1"
                onMouseDown={() => store.dispatch(openMobileConversationList(!conversationListState.mobileIsOpen))}>
                {conversationListState.mobileIsOpen ? "Close" : "Open"}
            </button>

            {conversationListState.mobileIsOpen && (
                <div className="absolute left-0 top-[50px] w-full z-10">
                    <ol className="mx-2 px-6 bg-white max-h-72 overflow-y-scroll space-y-1">
                        {conversations?.map(s => (<ConversationOptionComponent key={s.id} option={s} selected={conversationListState.selectedConversationId == s.id} selectConversation={handleSelectConversation} />))}
                    </ol>
                </div>
            )}
        </div>
    )
}

export default MobileConversationList;