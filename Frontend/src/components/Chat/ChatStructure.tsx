import { ReactElement } from "react";

type ChatStructureProps = {
    sidebar: ReactElement;
    content: ReactElement;
}

const ChatStructure: React.FC<ChatStructureProps> = ({sidebar, content}) => {
    return (
        <main className="grid grid-cols-1 grid-rows-[50px_1fr] md:grid-cols-[250px_1fr] md:grid-rows-1 h-screen">
            <aside>{sidebar}</aside>
            <div>{content}</div>
        </main>
    );
}

export default ChatStructure;