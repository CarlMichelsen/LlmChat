const ConversationOption: React.FC<{summary: string}> = ({ summary }) => {
    return (
        <li className="grid grid-cols-[1fr_25px] h-6 overflow-hidden">
            <p>{summary}</p>
            <button>X</button>
        </li>
    );
}

export default ConversationOption;