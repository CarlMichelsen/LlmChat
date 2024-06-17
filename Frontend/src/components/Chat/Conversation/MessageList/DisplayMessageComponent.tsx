import { render } from "../../../../util/markup/renderer";
import { Content } from "../../../../util/type/llmChat/conversation/content";

type DisplayMessageComponentProps = {
    id: string;
    isUser: boolean;
    displayName: string;
    imageUrl: string;
    inputTokens: number;
    outputTokens: number;
    content: Content[]
    userMessageSubheader?: React.ReactNode;
}

const DisplayMessageComponent: React.FC<DisplayMessageComponentProps> = (
    { id, isUser, displayName, imageUrl, inputTokens, outputTokens, content, userMessageSubheader }) => {
    const renderContent = (content: Content, index: number) => {
        if (!content) {
            return null;
        }

        if (content.contentType != "Text") {
            throw new Error("Only Text content supported right now");
        }

        if (!isUser) {
            const trusted = { __html: render(content.content) }
            return <div key={index} dangerouslySetInnerHTML={trusted}></div>
        }

        return (
            <div key={index}>
                <pre className="font-sans w-full overflow-auto whitespace-break-spaces">{content.content}</pre>
            </div>
        );
    }


    return (
        <>
            <div className="grid grid-cols-[50px_1fr]">
                <img className="rounded-lg bg-blue-300 h-[50px] w-[50px]" src={imageUrl} alt="profile" />
                <div className="ml-4">
                    {isUser ? (
                        <>
                            <p>{displayName}</p>
                            {userMessageSubheader}
                        </>
                    ) : (
                        <>
                            <p>{displayName}</p>
                            <div className="grid grid-cols-[70px_1fr]">
                                <p className="text-xs text-gray-500">{inputTokens} → {outputTokens}</p>
                                {userMessageSubheader}
                            </div>
                            
                        </>
                    )}
                </div>
            </div>

            <div id={id} className="md:ml-5 mt-2">
                {content.map(renderContent)}
            </div>
        </>
    );
}

export default DisplayMessageComponent;