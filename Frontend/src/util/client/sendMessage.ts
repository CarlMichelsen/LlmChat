import { ContentDelta } from "../type/llmChat/contentDelta";
import { NewMessage } from "../type/llmChat/newMessage";
import { rootUrl } from "./endpoints";

export const sendMessage = async (message: NewMessage, eventHandler?: (streamEvent: ContentDelta) => void): Promise<void> => {
    const response = await fetch(
        `${rootUrl()}/api/v1/chat`,
    {
        method: 'POST',
        credentials: 'include',
        body: JSON.stringify(message),
        headers: {
            "Content-Type": "application/json",
        },
    });

    if (!response.ok) throw new Error('Network response was not ok.');

    const reader = response.body!.getReader();
    const decoder = new TextDecoder('utf-8');

    let partialText = '';
    reader.read().then(async function processStream({ done, value }): Promise<ReadableStreamReadResult<Uint8Array>|void> {
        if (done) {
            console.log('Stream complete');
            return; 
        }
    
        partialText += decoder.decode(value);
    
        let eolIndex;
        while ((eolIndex = partialText.indexOf('\n')) >= 0) {
            const line = partialText.slice(0, eolIndex);
            partialText = partialText.slice(eolIndex + 1);
            if (line == "") continue;

            try {
                const streamEvent = JSON.parse(line) as ContentDelta;
                eventHandler && eventHandler(streamEvent);
            } catch (error) {
                
            }
        }
    
        return reader.read().then(processStream);
    });
}