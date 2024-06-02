import { useSelector } from "react-redux";
import { RootApplicationState } from "../../../../store";
import robotIcon from "../../../../assets/robot-icon.svg";
import DisplayMessageComponent from "./DisplayMessageComponent";
import { Content } from "../../../../util/type/llmChat/conversation/content";

const StreamMessage: React.FC<{conversationId: string}> = ({ conversationId }) => {
    const messageStreamState = useSelector((state: RootApplicationState) => state.messageStream);
    const stream = messageStreamState[conversationId];

    const mappedContent = () => {
        return stream.messageContent.map(x => ({ contentType: "Text", content: x} satisfies Content));
    }

    return stream?.streaming ? (
        <li key="streaming">
            <DisplayMessageComponent
                isUser={false}
                displayName="Streaming"
                imageUrl={robotIcon}
                inputTokens={0}
                outputTokens={0}
                content={mappedContent()}/>
        </li>
    ) : null;
}

export default StreamMessage;