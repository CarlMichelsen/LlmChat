import ChatStructure from "../Chat/ChatStructure";
import Sidebar from "../Chat/Sidebar";

const LoggedIn: React.FC = () => {
    return (
        <ChatStructure
            sidebar={<Sidebar />}
            content={(<div className="bg-green-600 h-full">content</div>)} />
    );
}

export default LoggedIn;