import { OptionDateCollection } from "../../util/type/optionDateCollection";

export const deleteConversationFromList = (conversationId: string, list: OptionDateCollection[]) => {
    for (const coll of list) {
        const idx = coll.options.findIndex(o => o.id === conversationId);
        if (idx !== -1) {
            coll.options.splice(idx, 1);
            break;
        }
    }
}