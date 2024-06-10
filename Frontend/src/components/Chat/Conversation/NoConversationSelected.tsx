import { useEffect, useState } from "react";
import RichTextArea from "../../RichTextArea";
import { getDefaultSystemMessage, setDefaultSystemMessage } from "../../../util/client/profile";
import { useQuery, useQueryClient } from "react-query";

const NoConversationSelected: React.FC = () => {
    const queryClient = useQueryClient();
    const [systemMessage, setSystemMessage] = useState<string|null>(null);

    const fetchDefaultSystemMesasge = async (): Promise<string|null> => {
        const res = await getDefaultSystemMessage();
        if (res.ok) {
            return res.data;
        } else {
            throw new Error("Failed to fetch conversation");
        }
    }

    const { data, status } = useQuery<string|null, Error>(
        "default-system-message",
        fetchDefaultSystemMesasge, {
        staleTime: Infinity,
        cacheTime: Infinity
    });

    useEffect(() => {
        if (status === "success") {
            setSystemMessage(data);
        }
    }, [data, status]);

    const onEnter = async () => {
        if (!systemMessage) {
            return;
        }
        const prev = systemMessage;
        
        setSystemMessage(null);
        try {
            var res = await setDefaultSystemMessage(prev);
            if (res.ok) {
                queryClient.invalidateQueries("default-system-message");
            } else {
                throw new Error("Failed to set default system message\n" + res.errors.join("\n"));
            }
        } catch (error) {
            const naiveError = error as Error;
            setSystemMessage(prev);
            alert(naiveError.message);
        }
    }

    return (
        <div className="chat-width px-2 grid grid-rows-[1fr_30px]">
            <div>
                <h1 className="md:text-center text-xl">Start a new conversation</h1>

                <br />
                <br />

                <label htmlFor="default-system-message-textbox" className="mb-2">Set default conversation system message</label>

                <div className="h-48">
                    <RichTextArea
                        name="default-system-message"
                        id="default-system-message-textbox"
                        text={systemMessage ?? ""}
                        setText={setSystemMessage}
                        disabled={systemMessage === null}
                        onReturn={onEnter}/>
                </div>
            </div>

            <p>Start a new conversation by sending a message in the textbox here.</p>
        </div>
    );
}

export default NoConversationSelected;