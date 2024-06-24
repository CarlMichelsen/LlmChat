import { ConversationOption, OptionDateCollection } from "../../util/type/optionDateCollection";
import { deleteConversationFromList } from "./deleteConversationFromList";

export const addConversationOptionToList = (option: ConversationOption, list: OptionDateCollection[]) => {
    // First, remove it if it already exsists.
    deleteConversationFromList(option.id, list);
    
    list[0].options.unshift(option);
}