import { useSelector } from "react-redux";
import store, { RootApplicationState } from "../../../store";
import FetchLogicComponent from "./FetchLogicComponent";
import Input from "./Input";
import MessageList from "./MessageList";
import { useEffect } from "react";
import { scrollStickToBottom } from "../../../util/helpers/scrollStickToBottom";
import ModelSelector from "./ModelSelector";
import Dialog from "../../Dialog";
import { setDialogOpen } from "../../../store/modelSlice";
import ModelSelectorWindow from "./ModelSelector/ModelSelectorWindow";

/*
isOpen: boolean;
onClose: () => void;
children: React.ReactNode;
*/

const Conversation: React.FC = () => {
    const modelState = useSelector((state: RootApplicationState) => state.models);
    const conversationState = useSelector((state: RootApplicationState) => state.conversation);
    useEffect(() => {
        if (conversationState.conversation != null) {
            scrollStickToBottom(true);
        }
    }, [conversationState.conversation]);

    return (
        <>
        <FetchLogicComponent />
        <div className="absolute z-10 right-0">
            <ModelSelector />
        </div>
        <Dialog isOpen={modelState.dialogOpen} onClose={() => store.dispatch(setDialogOpen(false))} children={(<ModelSelectorWindow/>)} />
        <div className="h-full overflow-y-scroll" id="conversation-scroll-chat">
            {conversationState.conversation
                ? (
                <div className="chat-width relative">
                    <MessageList conversationId={conversationState.conversation?.id} dialogSlices={conversationState.conversation.dialogSlices} />
                </div>
                )
                : <div>No conversation selected</div>}

            <Input selectedConversationId={conversationState.conversation?.id} />
        </div>
        </>
    );
}

export default Conversation;