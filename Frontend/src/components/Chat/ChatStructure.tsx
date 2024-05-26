import { ReactElement } from "react";
import { useSelector } from "react-redux";
import { RootApplicationState } from "../../store";

type ChatStructureProps = {
    sidebar: ReactElement;
    content: ReactElement;
}

const ChatStructure: React.FC<ChatStructureProps> = ({sidebar, content}) => {
    const conversationListState = useSelector((state: RootApplicationState) => state.conversationList);
    
    return (
        <main className={`grid grid-cols-1 grid-rows-[50px_1fr] ${conversationListState.desktopIsOpen ? "md:grid-cols-[250px_1fr]" : "md:grid-cols-[0px_1fr]"} md:grid-rows-1 h-screen transition-all`}>
            <aside>{sidebar}</aside>
            <div>{content}</div>
        </main>
    );
}

export default ChatStructure;