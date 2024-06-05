import { useSelector } from "react-redux";
import store, { RootApplicationState } from "../../../../store";
import { DialogSlice } from "../../../../util/type/llmChat/conversation/dialogSlice";
import robotIcon from "../../../../assets/robot-icon.svg";
import { render } from "../../../../util/markup/renderer";
import { Content } from "../../../../util/type/llmChat/conversation/content";
import { selectMessage } from "../../../../store/conversationSlice";
import { editMessage } from "../../../../store/inputSlice";

type MessageComponentProps = {
    conversationId: string;
    dialogSlice: DialogSlice;
}

const MessageComponent: React.FC<MessageComponentProps> = ({ dialogSlice, conversationId }) => {
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

    const renderContent = (content: Content, index: number) => {
        if (content.contentType != "Text") {
            throw new Error("Only Text content supported right now");
        }

        if (msg.prompt) {
            const trusted = { __html: render(content.content) }
            return <div key={index} dangerouslySetInnerHTML={trusted}></div>
        }

        return (
            <div key={index}>
                <p>{content.content}</p>
            </div>
        );
    }

    return (
        <li>
            <div className={`grid ${msg.prompt ? "grid-cols-[50px_1fr_20px]" : "grid-cols-[50px_1fr]"}`}>
                <img className="rounded-lg bg-blue-300 h-[50px] w-[50px]" src={imgUrl} alt="profile" />
                <div className="ml-4">
                    {msg.prompt ? (
                        <>
                            <p>{msg.prompt.modelName}</p>
                            <p className="text-xs text-gray-500">{msg.prompt.inputTokens} → {msg.prompt.outputTokens}</p>
                        </>
                    ) : (
                        <p>{userState.user!.name}</p>
                    )}
                </div>
            </div>

            <div className="md:ml-5 mt-2">
                {msg.content.map(renderContent)}
            </div>

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
                        className="ml-4 hover:underline"
                        onMouseDown={() => store.dispatch(editMessage({ conversationId, editing: msg }))}>edit</button>
                </div>
            </div>}
        </li>
    );
}

export default MessageComponent;