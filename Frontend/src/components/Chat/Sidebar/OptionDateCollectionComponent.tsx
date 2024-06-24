import { OptionDateCollection } from "../../../util/type/optionDateCollection";
import ConversationOptionComponent from "./ConversationOptionComponent";

type OptionDateCollectionComponentProps = {
    selectedConversationId?: string;
    selectConversation: (conversationId: string) => void;
    collection: OptionDateCollection;
}

const OptionDateCollectionComponent: React.FC<OptionDateCollectionComponentProps> = ({ collection, selectedConversationId, selectConversation }) => {
    return collection.options.length !== 0 ? (
        <li>
            <label className="text-xs font-bold underline" htmlFor={collection.htmlId}>{collection.dateString}</label>
            <ol className="pl-1 max-h-72 overflow-y-scroll space-y-1" id={collection.htmlId}>
                {collection.options.map(s => (<ConversationOptionComponent key={s.id} option={s} selected={selectedConversationId == s.id} selectConversation={selectConversation} />))}
            </ol>
        </li>
    ) : null;
}

export default OptionDateCollectionComponent;