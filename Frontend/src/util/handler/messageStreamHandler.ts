import store from "../../store";
import { selectConversation } from "../../store/conversationListSlice";
import { appendMessage } from "../../store/conversationSlice";
import { AppendMessagePayload } from "../../store/conversationSlice/appendMessage";
import { appendStream, clearStream } from "../../store/messageStreamSlice";
import { sendMessage } from "../client/sendMessage";
import { Concluded } from "../type/llmChat/concluded";
import { ContentDelta } from "../type/llmChat/contentDelta";
import { Content } from "../type/llmChat/conversation/content";
import { Message } from "../type/llmChat/conversation/message";
import { NewMessage } from "../type/llmChat/newMessage";

export class MessageStreamHandler
{
    ranOnFinally = false;
    onFinally: () => void

    conversationId?: string;
    userMessageId?: string;
    promptMessage: NewMessage;

    content: { [key: number]: Content };

    constructor(newMessage: NewMessage, onFinally: () => void) {
        this.onFinally = onFinally;
        this.content = {};
        this.handleContentDelta = this.handleContentDelta.bind(this);
        sendMessage(newMessage, this.handleContentDelta);
        this.promptMessage = newMessage;
    }

    public handleContentDelta(contentDelta: ContentDelta) {
        if (contentDelta.error) {
            alert(contentDelta.error);
            if (!this.ranOnFinally) {
                this.onFinally();
                this.ranOnFinally = true;
            }
        }

        if (!this.conversationId && contentDelta.conversationId) {
            this.conversationId = contentDelta.conversationId;
            store.dispatch(selectConversation(this.conversationId));
        }

        if (!this.userMessageId && contentDelta.userMessageId) {
            this.userMessageId = contentDelta.userMessageId;
            const payload = this.createAppendMessageModelFromUserMessage(this.userMessageId, this.promptMessage);
            store.dispatch(appendMessage(payload));
        }

        if (contentDelta.content != null) {
            let exsisting = this.content[contentDelta.content.index];
            if (!exsisting) {
                exsisting = contentDelta.content;
                this.content[contentDelta.content.index] = exsisting;
            } else {
                exsisting.content += contentDelta.content.content;
            }

            store.dispatch(appendStream({ conversationId: this.conversationId!, content: contentDelta.content }));
        }

        if (contentDelta.concluded != null) {
            store.dispatch(clearStream(this.conversationId!));
            this.concludeMessage(contentDelta.concluded);
        }
    }

    private concludeMessage(concluded: Concluded) {
        const sortedList = Object.keys(this.content)
            .map(key => Number(key))
            .map(key => [key, this.content[key]] satisfies [number, Content])
            .sort(([a], [b]) => a - b);
        
        const msg: Message = {
            id: concluded.messageId,
            prompt: concluded,
            content: sortedList.map(kv => kv[1]),
            completedUtc: (new Date()).toUTCString(),
        }

        const payload: AppendMessagePayload = {
            conversationId: this.conversationId!,
            message: msg,
            respondToMessageId: this.userMessageId!,
        };

        store.dispatch(appendMessage(payload));
        if (!this.ranOnFinally) {
            this.onFinally();
            this.ranOnFinally = true;
        }
    }

    private createAppendMessageModelFromUserMessage(userMessageId: string, newMessage: NewMessage): AppendMessagePayload {
        if (!this.conversationId) {
            throw new Error("ConversationId must be known before user message can be added");
        }

        const msg: Message = {
            id: userMessageId,
            content: newMessage.content,
            completedUtc: (new Date()).toUTCString(),
        }

        return {
            conversationId: this.conversationId,
            message: msg,
            respondToMessageId: newMessage.responseToMessageId,
        };
    }
}