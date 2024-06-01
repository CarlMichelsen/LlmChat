import { useSelector } from "react-redux";
import store, { RootApplicationState } from "../../../../store";
import { setInputReady, setInputText } from "../../../../store/inputSlice";
import { getLatestMessageId } from "../../../../util/helpers/getLatestMessageId";
import { NewMessage } from "../../../../util/type/llmChat/newMessage";
import { MessageStreamHandler } from "../../../../util/handler/messageStreamHandler";

type InputProps = {
    selectedConversationId?: string;
}

const Input: React.FC<InputProps> = ({ selectedConversationId }) => {
    const messageStreamState = useSelector((state: RootApplicationState) => state.messageStream);
    const messageStream = messageStreamState[selectedConversationId ?? "none"];

    const inputState = useSelector((state: RootApplicationState) => state.input);
    const input = inputState.inputField[selectedConversationId ?? "none"]

    const conversationState = useSelector((state: RootApplicationState) => state.conversation);
    

    const getTextAreaValue = () => {
        return input?.text ?? "";
    }

    const getTextReadyValue = () => {
        return input?.ready ?? true;
    }

    const setReady = (ready: boolean) => {
        store.dispatch(setInputReady({ conversationId: selectedConversationId ?? "none", ready } ));
    }

    const setText = (text: string) => {
        store.dispatch(setInputText({ conversationId: selectedConversationId ?? "none", input: text}))
    } 

    const sendActualMessage = async () => {
        const payload: NewMessage = {
            conversationId: selectedConversationId,
            responseToMessageId: getLatestMessageId(conversationState.conversation) ?? undefined,
            content: [{ contentType: "Text", content: getTextAreaValue() }],
            modelIdentifier: "6be7353f-2447-4f6a-93d3-160bca5783ee"
        };

        setReady(false);
        new MessageStreamHandler(payload, () => {
            setText("");
            setReady(true);
        });
    }

    const handleSendOnEnter = (keyEvent: React.KeyboardEvent<HTMLTextAreaElement>) => {
        if (!keyEvent.shiftKey && keyEvent.key === "Enter") {
            keyEvent.preventDefault();
            sendActualMessage();
        }
    }

    return (
        <div className="sticky bottom-0 w-full h-36">
            <textarea 
                disabled={messageStream?.streaming || !getTextReadyValue()}
                className="block w-full lg:w-[750px] md:w-[500px] h-32 mx-auto resize-none bg-zinc-400 disabled:bg-zinc-600 p-1 md:p-2"
                onKeyDown={handleSendOnEnter}
                value={getTextAreaValue()}
                onChange={(e) => setText(e.target.value)}
                name="conversation_chatbox"
                id="chatbox"></textarea>
        </div>
    );
}

export default Input;