import ChatStructure from "../Chat/ChatStructure";
import Conversation from "../Chat/Conversation";
import Sidebar from "../Chat/Sidebar";

const LoggedIn: React.FC = () => {
    return (
        <ChatStructure
            sidebar={<Sidebar />}
            content={<Conversation />} />
    );
}

export default LoggedIn;