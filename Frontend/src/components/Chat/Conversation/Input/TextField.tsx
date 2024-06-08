import { Message } from "../../../../util/type/llmChat/conversation/message";

type TextFieldProps = {
    ready: boolean;
    setReady: (ready: boolean) => void;
    text: string;
    setText: (text: string) => void;
    editingMessage?: Message;
    cancelEdit: () => void;
    streaming: boolean;
    sendMessage: () => void;
}

const TextField: React.FC<TextFieldProps> = ({ ready, streaming, sendMessage, text, setText, editingMessage, cancelEdit }) => {
    const handleSendOnEnter = (keyEvent: React.KeyboardEvent<HTMLTextAreaElement>) => {
        if (!keyEvent.shiftKey && keyEvent.key === "Enter") {
            keyEvent.preventDefault();

            if (!streaming) {
                sendMessage();
            }
        }
    }

    return (
        <div className="px-2 grid grid-cols-[1fr_50px]">
            <label htmlFor="chatbox" className="sr-only">Send message here</label>
            <textarea 
                className={
                    `block w-full h-32 mx-auto resize-none ${streaming && "bg-blue-400"} ${!ready && "border-none"} border border-black rounded-md disabled:bg-zinc-600 p-1 md:p-2 focus:outline-none ${!editingMessage && "col-span-2"}`}
                onKeyDown={handleSendOnEnter}
                value={text}
                onChange={(e) => setText(e.target.value)}
                name="conversation-chatbox"
                id="chatbox"></textarea>

            {editingMessage && (<button className="h-full w-full" onMouseDown={() => cancelEdit()}>Stop Editing</button>)}
        </div>
    );
}

export default TextField;