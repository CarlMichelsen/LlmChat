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
    index: number;
}

const MessageComponent: React.FC<MessageComponentProps> = ({ dialogSlice, conversationId, index }) => {
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

    return (
        <li>
            <DisplayMessageComponent
                isUser={!msg.prompt}
                displayName={msg.prompt?.modelName ?? userState.user!.name}
                imageUrl={imgUrl}
                inputTokens={msg.prompt?.inputTokens ?? 0}
                outputTokens={msg.prompt?.outputTokens ?? 0}
                content={msg.content} />

            {!msg.prompt && <div className="grid grid-cols-[15px_30px_15px_50px]">
                <button
                    disabled={dialogSlice.selectedIndex <= 0}
                    className="hover:underline disabled:text-zinc-400 disabled:hover:no-underline"
                    onMouseDown={() => incrementSelectedMessageSliceId(-1)}>&lt;</button>
                <p className="text-center">{dialogSlice.selectedIndex + 1}/{dialogSlice.messages.length}</p>
                <button
                    disabled={dialogSlice.selectedIndex >= dialogSlice.messages.length - 1}
                    className="hover:underline disabled:text-zinc-400 disabled:hover:no-underline"
                    onMouseDown={() => incrementSelectedMessageSliceId(1)}>&gt;</button>
                <div>
                    <button
                        disabled={index === 0}
                        className="ml-4 hover:underline disabled:text-zinc-400 disabled:hover:no-underline"
                        onMouseDown={() => store.dispatch(editMessage({ conversationId, editing: msg }))}>edit</button>
                </div>
            </div>}
        </li>
    );
}

export default MessageComponent;