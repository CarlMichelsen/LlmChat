import { useSelector } from "react-redux";
import store, { RootApplicationState } from "../../../../store";
import { cancelEdit, setInputReady, setInputText } from "../../../../store/inputSlice";
import { getLatestMessageId } from "../../../../util/helpers/getLatestMessageId";
import { NewMessage } from "../../../../util/type/llmChat/newMessage";
import { MessageStreamHandler } from "../../../../util/handler/messageStreamHandler";
import TextField from "./TextField";
import { ResponseTo } from "../../../../util/type/llmChat/responseTo";

type InputProps = {
    selectedConversationId?: string;
}

const Input: React.FC<InputProps> = ({ selectedConversationId }) => {
    const messageStreamState = useSelector((state: RootApplicationState) => state.messageStream);
    const messageStream = messageStreamState[selectedConversationId ?? "none"];

    const inputState = useSelector((state: RootApplicationState) => state.input);
    const input = inputState.inputField[selectedConversationId ?? "none"]

    const modelState = useSelector((state: RootApplicationState) => state.models);

    const conversationState = useSelector((state: RootApplicationState) => state.conversation);

    const getTextAreaValue = () => {
        return input?.text ?? "";
    }

    const getTextReadyValue = () => {
        return input?.ready ?? true;
    }

    const getEditingMessage = () => {
        return input?.editing ?? null;
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
        let responseTo: ResponseTo|undefined;
        const responseToMessageId = getEditingMessage()?.previousMessageId
            ?? getLatestMessageId(conversationState.conversation)
            ?? null

        if (responseToMessageId && selectedConversationId) {
            const responseToDialogSlice = conversationState.conversation?.dialogSlices
                .find(d => d.messages.find(m => m.id === responseToMessageId));
            
            if (!responseToDialogSlice) {
                throw new Error("Unable to find dialog slice of message being responded to");
            }

            responseTo = {
                conversationId: selectedConversationId,
                responseToMessageId: responseToMessageId,
            } satisfies ResponseTo;
        }

        const payload: NewMessage = {
            responseTo: responseTo,
            content: [{ contentType: "Text", content: getTextAreaValue() }],
            modelIdentifier: modelState.selectedModelId,
        };

        setReady(false);
        new MessageStreamHandler(
            payload,
            () => {
                setText("");
                setReady(true);
                store.dispatch(cancelEdit(selectedConversationId!));
            },
            () => setReady(true));
    }

    const handleCancelEdit = () => {
        store.dispatch(cancelEdit(selectedConversationId!));
        setText("");
    }

    return (
        <div className="sticky bottom-0 chat-width h-36">
            <TextField
                ready={input?.ready ?? true}
                setReady={setReady}
                text={input?.text ?? ""}
                setText={setText}
                editingMessage={input?.editing}
                cancelEdit={handleCancelEdit}
                streaming={messageStream?.streaming === true}
                sendMessage={sendActualMessage}
                placeholder="Send message here"/>
        </div>
    );
}

export default Input;