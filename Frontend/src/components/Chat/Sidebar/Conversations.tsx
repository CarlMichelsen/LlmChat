import ConversationList from "./ConversationList";
import MobileConversationList from "./MobileConversationList";

const testConversationSummaries = [
    "Doing weird shit",
    "Did the math",
    "No. Dolphins can't fly",
    "Found non-viable solution"
];

const Conversations: React.FC = () => {

    return (
        <>
            <div className="md:hidden block">
                <MobileConversationList conversations={testConversationSummaries} />
            </div>

            <div className="hidden md:block">
                <ConversationList conversations={testConversationSummaries} />
            </div>
        </>
    );
}

export default Conversations;