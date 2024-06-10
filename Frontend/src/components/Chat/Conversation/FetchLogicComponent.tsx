import { useSelector } from "react-redux";
import store, { RootApplicationState } from "../../../store";
import { useEffect } from "react";
import { setConversation } from "../../../store/conversationSlice";
import { Conversation } from "../../../util/type/llmChat/conversation";
import { getConversation } from "../../../util/client/conversation";
import { useQuery, useQueryClient } from "react-query";
import { scrollStickToBottom } from "../../../util/helpers/scrollStickToBottom";

const FetchLogicComponent: React.FC = () => {
    const queryClient = useQueryClient();
    const conversationListState = useSelector((state: RootApplicationState) => state.conversationList);

    const fetchConversation = async (): Promise<Conversation|null> => {
        if (!conversationListState.selectedConversationId) {
            return null;
        }

        const res = await getConversation(conversationListState.selectedConversationId);
        if (res.ok) {
            return res.data;
        } else {
            throw new Error("Failed to fetch conversation");
        }
    }

    const { data, status } = useQuery<Conversation|null, Error>(
        ["conversation", conversationListState.selectedConversationId],
        fetchConversation, {
        staleTime: Infinity,
        cacheTime: Infinity
    });

    useEffect(() => {
        if (status === "success") {
            store.dispatch(setConversation(data));
            setTimeout(() => scrollStickToBottom(true), 0);
        }
    }, [data, status]);

    useEffect(() => {
        queryClient.invalidateQueries(["conversation", conversationListState.selectedConversationId]);
    }, [conversationListState.selectedConversationId]);

    return null;
}

export default FetchLogicComponent;