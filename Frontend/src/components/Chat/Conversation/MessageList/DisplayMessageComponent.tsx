import { render } from "../../../../util/markup/renderer";
import { Content } from "../../../../util/type/llmChat/conversation/content";

type DisplayMessageComponentProps = {
    isUser: boolean;
    displayName: string;
    imageUrl: string;
    inputTokens: number;
    outputTokens: number;
    content: Content[]
}

const DisplayMessageComponent: React.FC<DisplayMessageComponentProps> = (
    { isUser, displayName, imageUrl, inputTokens, outputTokens, content }) => {
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
                        <p>{displayName}</p>
                    ) : (
                        <>
                            <p>{displayName}</p>
                            <p className="text-xs text-gray-500">{inputTokens} → {outputTokens}</p>
                        </>
                    )}
                </div>
            </div>

            <div className="md:ml-5 mt-2">
                {content.map(renderContent)}
            </div>
        </>
    );
}

export default DisplayMessageComponent;