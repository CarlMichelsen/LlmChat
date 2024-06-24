import { useSelector } from "react-redux";
import store, { RootApplicationState } from "../../../store";
import { openMobileConversationList, selectConversation } from "../../../store/conversationListSlice";
import { OptionDateCollection } from "../../../util/type/optionDateCollection";
import OptionDateCollectionComponent from "./OptionDateCollectionComponent";

const MobileConversationList: React.FC<{ conversations?: OptionDateCollection[] }> = ({ conversations }) => {
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

            <div className="absolute left-0 top-[50px] w-full z-20">
                <ol className="mx-2 px-6 bg-white dark:bg-black max-h-72 overflow-y-scroll space-y-2 border-b border-l border-r border-black rounded-b-md">
                    {conversationListState.mobileIsOpen && conversations?.map(coll => (
                        <OptionDateCollectionComponent
                            key={coll.htmlId}
                            collection={coll}
                            selectedConversationId={conversationListState.selectedConversationId}
                            selectConversation={handleSelectConversation} />
                    ))}
                </ol>
            </div>
        </div>
    )
}

export default MobileConversationList;