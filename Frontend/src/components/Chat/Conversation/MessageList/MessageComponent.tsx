import { useSelector } from "react-redux";
import { RootApplicationState } from "../../../../store";
import { DialogSlice } from "../../../../util/type/llmChat/conversation/dialogSlice";
import robotIcon from "../../../../assets/robot-icon.svg";
import { render } from "../../../../util/markup/renderer";
import { Content } from "../../../../util/type/llmChat/conversation/content";

type MessageComponentProps = {
    dialogSlice: DialogSlice;
}

const MessageComponent: React.FC<MessageComponentProps> = ({ dialogSlice }) => {
    const userState = useSelector((state: RootApplicationState) => state.user);
    const msg = dialogSlice.messages[dialogSlice.selectedIndex];

    const imgUrl = msg.prompt ? robotIcon : userState.user!.avatarUrl;

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
            <div className="grid grid-cols-[50px_1fr]">
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
        </li>
    );
}

export default MessageComponent;