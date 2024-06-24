import { ConversationOption, OptionDateCollection } from "../../util/type/optionDateCollection";

export const addConversationOptionToList = (option: ConversationOption, list: OptionDateCollection[]) => {
    // First, remove it if it already exsists.
    for (const coll of list) {
        const idx = coll.options.findIndex(o => o.id === option.id);
        if (idx !== -1) {
            coll.options.splice(idx, 1);
            break;
        }
    }

    list[0].options.unshift(option);
}