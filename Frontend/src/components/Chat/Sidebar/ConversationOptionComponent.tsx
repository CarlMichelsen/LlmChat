import { useSelector } from "react-redux";
import { ConversationOption } from "../../../util/type/optionDateCollection";
import store, { RootApplicationState } from "../../../store";
import SidebarContextMenu from "./SidebarContextMenu";
import { setContextMenu } from "../../../store/conversationListSlice";

type ConversationOptionComponentProps = {
    option: ConversationOption;
    selected: boolean;
    selectConversation: (conversationId: string) => void;
}

const ConversationOptionComponent: React.FC<ConversationOptionComponentProps> = ({ option, selected, selectConversation }) => {
    const conversationListState = useSelector((state: RootApplicationState) => state.conversationList);

    return (
        <li onBlur={() => {
            if (conversationListState.contextMenuOption == option.id) {
                store.dispatch(setContextMenu(undefined));
            }
        }}>
            <div className="grid grid-cols-[1fr_15px] h-6 overflow-hidden px-0.5">
                <button
                    className={`text-xs text-left hover:underline hover:bg-zinc-400 dark:hover:bg-zinc-800 rounded-sm px-0.5 ${selected && "bg-zinc-400 dark:bg-zinc-700"} whitespace-nowrap overflow-hidden text-ellipsis`}
                    onMouseDown={() => selectConversation(option.id)}>
                    {option.summary ?? "New conversation"}
                </button>

                <button
                    onMouseDown={() => {
                        store.dispatch(setContextMenu(option.id));
                    }}
                    aria-label="options"
                    className="grid grid-cols-1 grid-rows-3 rounded-sm text-xs ml-0.5 py-1.5 hover:bg-zinc-400 dark:hover:bg-zinc-800">
                    <div className="mx-auto rounded-full w-1 h-1 bg-zinc-600"></div>
                    <div className="mx-auto rounded-full w-1 h-1 bg-zinc-600"></div>
                    <div className="mx-auto rounded-full w-1 h-1 bg-zinc-600"></div>
                </button>
            </div>
            

            {conversationListState.contextMenuOption == option.id ? (
                <SidebarContextMenu conversationId={option.id} />
            ) : null}
        </li>
    );
}

export default ConversationOptionComponent;