import { useSelector } from "react-redux";
import ConversationList from "./ConversationList";
import MobileConversationList from "./MobileConversationList";
import { RootApplicationState } from "../../../store";

const Conversations: React.FC = () => {
    const conversationListState = useSelector((state: RootApplicationState) => state.conversationList);

    return (
        <>
            <div className="md:hidden block">
                <MobileConversationList conversations={conversationListState.list} />
            </div>

            <div className="hidden md:block h-full">
                <ConversationList conversations={conversationListState.list} />
            </div>
        </>
    );
}

export default Conversations;