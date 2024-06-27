import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import store, { RootApplicationState } from "../../../../store";
import { setConversationSystemMessage } from "../../../../util/client/conversation";
import { setSystemMessage } from "../../../../store/conversationSlice";
import RichTextArea from "../../../RichTextArea";
import { SystemMessage } from "../../../../util/type/systemMessage/systemMessage";
import { getSystemMessageList } from "../../../../util/client/systemMessage";
import { useQuery } from "react-query";

type SystemMessageDialogProps = {
    closeDialog: () => void;
}

type LocalSystemMessage = {
    id: string|null;
    name: string|null;
    content: string
}

const SystemMessageDialog: React.FC<SystemMessageDialogProps> = ({ closeDialog }) => {
    const conversationState = useSelector((state: RootApplicationState) => state.conversation);
    const [systemMessageList, setSystemMessageList] = useState<SystemMessage[]|null>(null);
    const [localSystemMessage, setLocalSystemMessage] = useState<LocalSystemMessage|null>(
        { id: null, name: null, content: conversationState.conversation?.systemMessage ?? "" });

    // SystemMessage
    const fetchSystemMessages = async () => {
        const res = await getSystemMessageList();
        if (res.ok) {
            return res.data!;
        } else {
            throw new Error("Failed to fetch system message list");
        }
    }

    const { data, status } = useQuery<SystemMessage[], Error>(
        'systemMessageList',
        fetchSystemMessages, {
        staleTime: 1000 * 60 * 2
    });

    useEffect(() => {
        if (status === "success") {
            setSystemMessageList(data);
        }
    }, [data, status]);

    const applySystemMessage = async () => {
        if (!conversationState.conversation || !localSystemMessage) {
            return;
        }
        const prev = localSystemMessage;
        setLocalSystemMessage(null);

        try {
            const systemMessageResponse = await setConversationSystemMessage(conversationState.conversation.id, prev.content);
            if (systemMessageResponse.ok && systemMessageResponse.data) {
                store.dispatch(setSystemMessage(systemMessageResponse.data));
                setLocalSystemMessage({
                    ...prev,
                    content: systemMessageResponse.data,
                });
                closeDialog();
            } else {
                throw new Error(systemMessageResponse.errors.join("\n"));
            }
        } catch (error) {
            const naiveError = error as Error;
            setLocalSystemMessage(prev);
            alert(naiveError.message);
        }
    }

    const onNameChange = (text: string|null) => {
        if (localSystemMessage) {
            setLocalSystemMessage({ ...localSystemMessage, name: text });
        } else {
            setLocalSystemMessage({ id: null, name: text, content: "" });
        }
    }

    return (
        <div className="grid grid-rows-[50px_50px_1fr_200px_40px] h-full">
            <div>
                <h1 className="text-xl">Edit System message for this conversation</h1>
                <p className="text-xs ml-2">- "{conversationState.conversation?.summary ?? "New conversation"}"</p>
            </div>
            
            <div className="my-1 grid grid-cols-[70px_1fr]">
                <div className="h-full">
                    <label htmlFor="system-message-name-input" className="block mt-1.5 text-xl">Name:</label>
                </div>
                <input
                    type="text"
                    name="system-message-name-input"
                    id="system-message-name-input"
                    onChange={(e) => onNameChange(e.target.value ?? null)}
                    className="focus:outline-none w-full border border-black rounded-sm p-1 dark:bg-zinc-800 bg-white disabled:bg-zinc-400 disabled:border-none text-xl" />
            </div>

            <RichTextArea
                name="conversation-system-message-textbox"
                id="conversation-system-input"
                text={localSystemMessage?.content ?? ""}
                setText={(text) => setLocalSystemMessage(localSystemMessage ? { ...localSystemMessage, content: text } : { id: null, name: null, content: text })}
                disabled={!localSystemMessage} />
            
            <ol>
                {systemMessageList?.map(sm => (
                    <li key={sm.id}>
                        <button>{sm.name}</button>
                    </li>
                ))}
            </ol>
            
            <div className="w-full h-full grid grid-cols-[1fr_400px_1fr]">
                <button
                    disabled={(conversationState.conversation?.systemMessage ?? null) === localSystemMessage}
                    className="w-full h-full rounded-md bg-green-500 disabled:bg-zinc-500 hover:underline disabled:hover:no-underline"
                    onMouseDown={() => applySystemMessage()}>Save And {localSystemMessage?.id ? "Overwrite" : "Apply"}</button>
                <div></div>
                <button
                    className="w-full h-full rounded-md bg-red-500 hover:underline"
                    onMouseDown={() => closeDialog()}>Close</button>
            </div>
        </div>
    );
}

export default SystemMessageDialog;