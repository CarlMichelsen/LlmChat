import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { getQueryParam } from '../../util/helpers/queryParameter';
import { ConversationOption, OptionDateCollection } from '../../util/type/optionDateCollection';
import { createOptionDateCollection } from './createOptionDateCollection';
import { addConversationOptionToList } from './addConversationOptionToList';

type ConversationListState = {
    selectedConversationId?: string;
    mobileIsOpen: boolean;
    desktopIsOpen: boolean;
    list?: OptionDateCollection[];
}

const initialState: ConversationListState = {
    selectedConversationId: getQueryParam("c") ?? undefined,
    mobileIsOpen: false,
    desktopIsOpen: true,
};

const conversationListSlice = createSlice({
    name: 'conversationList',
    initialState,
    reducers: {
        openMobileConversationList: (state, action: PayloadAction<boolean>) => {
            state.mobileIsOpen = action.payload
        },
        openDesktopConversationList: (state, action: PayloadAction<boolean>) => {
            state.desktopIsOpen = action.payload;
        },
        setConversationOptions: (state, action: PayloadAction<ConversationOption[]>) => {
            state.list = createOptionDateCollection(action.payload);
        },
        addConversationOption: (state, action: PayloadAction<ConversationOption>) => {
            if (state.list) {
                addConversationOptionToList(action.payload, state.list);
            }
        },
        promoteConversation: (state, action: PayloadAction<string>) => {
            const option = state.list?.flatMap(coll => coll.options).find(o => o.id === action.payload);
            if (option && state.list) {
                addConversationOptionToList(option, state.list);
            }
        },
        addConversationOptionSummary: (state, action: PayloadAction<{conversationId: string, summary: string|null}>) => {
            const conversationOption = state.list?.find(odc => odc.options.find(co => co.id === action.payload.conversationId))
                ?.options.find(co => co.id === action.payload.conversationId) ?? null;
            if (conversationOption != null) {
                conversationOption.summary = action.payload.summary ?? undefined;
            }
        },
        selectConversation: (state, action: PayloadAction<string | null>) => {
            state.selectedConversationId = action.payload ?? undefined;
        },
    },
});

export const {
    openMobileConversationList,
    openDesktopConversationList,
    setConversationOptions,
    addConversationOption,
    promoteConversation,
    addConversationOptionSummary,
    selectConversation
} = conversationListSlice.actions;

export default conversationListSlice.reducer;