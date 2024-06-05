import { useSelector } from "react-redux";
import { RootApplicationState } from "../../../store";
import FetchLogicComponent from "./FetchLogicComponent";
import Input from "./Input";
import MessageList from "./MessageList";
import { useEffect } from "react";
import { scrollStickToBottom } from "../../../util/helpers/scrollStickToBottom";

const Conversation: React.FC = () => {
    const conversationState = useSelector((state: RootApplicationState) => state.conversation);
    useEffect(() => {
        if (conversationState.conversation != null) {
            scrollStickToBottom(true);
        }
    }, [conversationState.conversation]);

    return (
        <>
        <FetchLogicComponent />
        <div className="h-full mx-auto container overflow-y-scroll relative" id="conversation-scroll-chat">
            {conversationState.conversation
                ? <MessageList conversationId={conversationState.conversation?.id} dialogSlices={conversationState.conversation.dialogSlices} />
                : <div>No conversation selected</div>}

            <Input selectedConversationId={conversationState.conversation?.id} />
        </div>
        </>
    );
}

export default Conversation;