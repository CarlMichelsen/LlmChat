import { useState } from "react";
import { useSelector } from "react-redux";
import store, { RootApplicationState } from "../../../../store";
import { setConversationSystemMessage } from "../../../../util/client/conversation";
import { setSystemMessage } from "../../../../store/conversationSlice";
import RichTextArea from "../../../RichTextArea";

const ConversationSystemMessageDialog: React.FC = () => {
    const conversationState = useSelector((state: RootApplicationState) => state.conversation);
    const [localSystemMessage, setLocalSystemMessage] = useState<string|null>(conversationState.conversation?.systemMessage ?? null);

    const applySystemMessage = async () => {
        if (!conversationState.conversation || !localSystemMessage) {
            return;
        }
        const prev = localSystemMessage;
        setLocalSystemMessage(null);

        try {
            const systemMessageResponse = await setConversationSystemMessage(conversationState.conversation.id, prev);
            if (systemMessageResponse.ok) {
                store.dispatch(setSystemMessage(systemMessageResponse.data));
                setLocalSystemMessage(systemMessageResponse.data)
            } else {
                throw new Error(systemMessageResponse.errors.join("\n"));
            }
        } catch (error) {
            const naiveError = error as Error;
            setLocalSystemMessage(prev);
            alert(naiveError.message);
        }
    }

    return (
        <div className="grid grid-rows-[50px_1fr] h-full">
            <div>
                <h1 className="text-xl">Edit System message for this conversation</h1>
                <p className="text-xs">{conversationState.conversation?.summary ?? "New conversation"}</p>
            </div>
            <RichTextArea
                name="conversation-system-message-textbox"
                id="conversation-system/input"
                text={localSystemMessage ?? ""}
                setText={setLocalSystemMessage}
                disabled={localSystemMessage === null}
                onReturn={applySystemMessage} />
        </div>
    );
}

export default ConversationSystemMessageDialog;