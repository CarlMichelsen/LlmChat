import { useState } from "react";
import { Message } from "../../../../util/type/llmChat/conversation/message";
import Dialog from "../../../Dialog";
import SystemMessageDialog from "./SystemMessageDialog";

type TextFieldProps = {
    ready: boolean;
    setReady: (ready: boolean) => void;
    text: string;
    setText: (text: string) => void;
    editingMessage?: Message;
    cancelEdit: () => void;
    streaming: boolean;
    sendMessage: () => void;
    placeholder?: string;
}

const TextField: React.FC<TextFieldProps> = ({ ready, streaming, sendMessage, text, setText, editingMessage, cancelEdit, placeholder }) => {
    const [systemMessageDialog, setSystemMessageDialog] = useState<boolean>(false);

    const handleSendOnEnter = (keyEvent: React.KeyboardEvent<HTMLTextAreaElement>) => {
        if (!keyEvent.shiftKey && keyEvent.key === "Enter") {
            keyEvent.preventDefault();

            if (!streaming) {
                sendMessage();
            }
        }
    }

    return (
        <div className="px-2 grid grid-cols-[1fr_80px] gap-2">
            <label htmlFor="chatbox" className="sr-only">Type message here and press enter to send it</label>
            <textarea
                placeholder={placeholder}
                className={
                    `block w-full h-32 mx-auto resize-none ${streaming ? "bg-white text-black dark:bg-zinc-300 dark:text-black" : "dark:bg-black dark:text-white bg-zinc-300 text-black"} ${!ready && "dark:border-white border-black"} border border-black rounded-md disabled:bg-zinc-600 p-1 md:p-2 focus:outline-none`}
                onKeyDown={handleSendOnEnter}
                value={text}
                onChange={(e) => setText(e.target.value)}
                name="conversation-chatbox"
                id="chatbox"></textarea>

            {editingMessage
                ? <button className="text-xs h-full w-full border border-black rounded-md bg-red-600 hover:bg-black text-white" onMouseDown={() => cancelEdit()}>Stop Editing</button>
                : <button className="text-xs h-full w-full border border-black rounded-md hover:bg-black dark:bg-black bg-white hover:text-white" onMouseDown={() => setSystemMessageDialog(true)}>Edit System Message</button>}

            {systemMessageDialog && <Dialog isOpen={systemMessageDialog} onClose={() => setSystemMessageDialog(false)} children={<SystemMessageDialog closeDialog={() => setSystemMessageDialog(false)} />} />}
        </div>
    );
}

export default TextField;