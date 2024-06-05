import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { ConversationOption } from '../../util/client/conversationOption';
import { getQueryParam } from '../../util/helpers/queryParameter';

type ConversationListState = {
    selectedConversationId?: string;
    mobileIsOpen: boolean;
    desktopIsOpen: boolean;
    conversationOptions?: ConversationOption[];
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
            state.conversationOptions = action.payload;
        },
        addConversationOption: (state, action: PayloadAction<ConversationOption>) => {
            state.conversationOptions?.push(action.payload);
        },
        addConversationOptionSummary: (state, action: PayloadAction<{conversationId: string, summary: string|null}>) => {
            const conversationOption = state.conversationOptions?.find(co => co.id === action.payload.conversationId);
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
    addConversationOptionSummary,
    selectConversation
} = conversationListSlice.actions;

export default conversationListSlice.reducer;