type RichTextAreaProps = {
    name: string;
    id: string;
    text: string;
    setText: (text: string) => void;
    disabled?: boolean;
    onReturn?: () => void;
}

const RichTextArea: React.FC<RichTextAreaProps> = ({ name, id, text, setText, disabled, onReturn }) => {
    const handleEnter = (keyEvent: React.KeyboardEvent<HTMLTextAreaElement>) => {
        if (!keyEvent.shiftKey && keyEvent.key === "Enter") {
            keyEvent.preventDefault();
            onReturn && onReturn();
        }
    }

    return (
        <textarea
            name={name}
            id={id}
            value={text}
            onChange={(e) => setText(e.target.value)}
            disabled={disabled ?? false}
            onKeyDown={handleEnter}
            className="focus:outline-none resize-none w-full h-full border border-black rounded-sm p-1 dark:bg-zinc-800 bg-white disabled:bg-zinc-400 disabled:border-none"></textarea>
    );
}

export default RichTextArea;