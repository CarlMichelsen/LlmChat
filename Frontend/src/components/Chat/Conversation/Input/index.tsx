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

    const getEditReplyToMessage = () => {
        return input?.editing?.previousMessageId ?? null;
    }

    const setReady = (ready: boolean) => {
        store.dispatch(setInputReady({ conversationId: selectedConversationId ?? "none", ready } ));
    }

    const setText = (text: string) => {
        if (getTextReadyValue()) {
            store.dispatch(setInputText({ conversationId: selectedConversationId ?? "none", input: text}))
        }
    }

    const sendActualMessage = async () => {
        const payload: NewMessage = {
            conversationId: selectedConversationId,
            responseToMessageId: getEditReplyToMessage() ?? getLatestMessageId(conversationState.conversation) ?? null,
            content: [{ contentType: "Text", content: getTextAreaValue() }],
            modelIdentifier: "ffa85f64-5717-4aaa-b3fc-2c963f66afa7"
        };

        setReady(false);
        new MessageStreamHandler(
            payload,
            () => {
                setText("");
                setReady(true);
            },
            () => {
            setReady(true);
        });
    }

    const handleSendOnEnter = (keyEvent: React.KeyboardEvent<HTMLTextAreaElement>) => {
        if (!keyEvent.shiftKey && keyEvent.key === "Enter") {
            keyEvent.preventDefault();

            if (!messageStream?.streaming) {
                sendActualMessage();
            }
        }
    }

    return (
        <div className="sticky bottom-0 chat-width h-36">
            <textarea 
                className={
                    `block w-full h-32 mx-auto resize-none ${messageStream?.streaming === true && "bg-blue-400"} ${!getTextReadyValue() === true && "border-none"} border border-black rounded-md disabled:bg-zinc-600 p-1 md:p-2 focus:outline-none`}
                onKeyDown={handleSendOnEnter}
                value={getTextAreaValue()}
                onChange={(e) => setText(e.target.value)}
                name="conversation_chatbox"
                id="chatbox"></textarea>
        </div>
    );
}

export default Input;