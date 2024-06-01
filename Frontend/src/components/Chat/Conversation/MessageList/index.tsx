import { DialogSlice } from "../../../../util/type/llmChat/conversation/dialogSlice";
import MessageComponent from "./MessageComponent";
import StreamMessage from "./StreamMessage";

type MessageListProps = {
    conversationId: string;
    dialogSlices: DialogSlice[]
}

const MessageList: React.FC<MessageListProps> = ({ conversationId, dialogSlices }) => {
    return (
        <ol className="space-y-6 px-2 pt-2 mb-6">
            {dialogSlices.map((slice, index) => (<MessageComponent key={index} dialogSlice={slice} />))}
            <StreamMessage conversationId={conversationId} />
        </ol>
    );
}

export default MessageList;