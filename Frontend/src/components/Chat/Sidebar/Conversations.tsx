const testConversationSummaries = [
    "Doing weird shit",
    "Did the math",
    "No. Dolphins can't fly",
    "Found non-viable solution"
];

const ConversationOption: React.FC<{summary: string}> = ({ summary }) => {
    return (
        <li className="grid grid-cols-[225px_25px]">
            <p>{summary}</p>
            <button>X</button>
        </li>
    );
}

const Conversations: React.FC = () => {

    return (
        <>
            <div className="md:hidden block" id="top-sidebar">
                <ol>
                    {testConversationSummaries.map(s => (<ConversationOption key={s} summary={s} />))}
                </ol>
            </div>

            <ol className="hidden md:block" id="large-sidebar">
                {testConversationSummaries.map(s => (<ConversationOption key={s} summary={(s)} />))}
            </ol>
        </>
    );
}

export default Conversations;