import { useSelector } from "react-redux";
import store, { RootApplicationState } from "../../../../store";
import { DialogSlice } from "../../../../util/type/llmChat/conversation/dialogSlice";
import robotIcon from "../../../../assets/robot-icon.svg";
import { selectMessage } from "../../../../store/conversationSlice";
import { editMessage } from "../../../../store/inputSlice";
import DisplayMessageComponent from "./DisplayMessageComponent";

type MessageComponentProps = {
    conversationId: string;
    dialogSlice: DialogSlice;
}

const MessageComponent: React.FC<MessageComponentProps> = ({ dialogSlice, conversationId }) => {
    const modelState = useSelector((state: RootApplicationState) => state.models);
    const userState = useSelector((state: RootApplicationState) => state.user);
    const msg = dialogSlice.messages[dialogSlice.selectedIndex];
    const imgUrl = msg.prompt ? robotIcon : userState.user!.avatarUrl;

    const incrementSelectedMessageSliceId = (increment: number) => {
        const messageId = dialogSlice.messages[dialogSlice.selectedIndex + increment]?.id;
        if (messageId == null) {
            return;
        }

        store.dispatch(selectMessage({ conversationId, messageId: messageId }));
    }

    const getModelDisplayName = (modelId?: string): string|null => {
        if (!modelId) {
            return null;
        }

        if (!modelState.models) {
            return null;
        }

        const model = modelState.models.find(m => m.id === modelId);
        if (!model) {
            return null;
        }

        return model.modelDisplayName;
    }

    return (
        <li>
            <DisplayMessageComponent
                id={msg.id}
                isUser={!msg.prompt}
                displayName={getModelDisplayName(msg.prompt?.modelId) ?? msg.prompt?.modelName ?? userState.user!.name}
                imageUrl={imgUrl}
                inputTokens={msg.prompt?.inputTokens ?? 0}
                outputTokens={msg.prompt?.outputTokens ?? 0}
                content={msg.content}
                userMessageSubheader={!msg.prompt ? (
                    <div className={`grid md:grid-cols-[100px_250px_60px_1fr] grid-cols-[100px_170px_60px_1fr] h-6`}>
                        <button
                            className="hover:underline bg-zinc-400 dark:bg-zinc-700 hover:bg-zinc-700 dark:hover:bg-zinc-200 hover:text-white dark:hover:text-black rounded-md text-xs"
                            onMouseDown={() => store.dispatch(editMessage({ conversationId, editing: msg }))}>Edit message</button>

                        {import.meta.env.MODE == "development" ? <p className="text-xs ml-2 mt-1">{msg.id}</p> : <label className="sr-only" htmlFor={msg.id}>User Message</label>}
                        
                        {dialogSlice.messages.length > 1 && (
                            <div className="grid grid-cols-[15px_30px_15px]">
                                <button
                                    disabled={dialogSlice.selectedIndex <= 0}
                                    className="hover:underline disabled:text-zinc-400 disabled:hover:no-underline"
                                    onMouseDown={() => incrementSelectedMessageSliceId(-1)}>&lt;</button>

                                <p className="text-center my-auto">{dialogSlice.selectedIndex + 1}/{dialogSlice.messages.length}</p>

                                <button
                                    disabled={dialogSlice.selectedIndex >= dialogSlice.messages.length - 1}
                                    className="hover:underline disabled:text-zinc-400 disabled:hover:no-underline"
                                    onMouseDown={() => incrementSelectedMessageSliceId(1)}>&gt;</button>
                            </div>
                        )}
                    </div>
                ) : (import.meta.env.MODE == "development" ? <p className="text-xs ml-2">{msg.id}</p> : <label className="sr-only" htmlFor={msg.id}>Assistant Message</label>)} />
        </li>
    );
}

export default MessageComponent;