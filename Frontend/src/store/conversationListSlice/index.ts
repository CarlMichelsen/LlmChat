import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { ConversationOption } from '../../util/client/conversationOption';

type ConversationListState = {
    selectedConversationId?: string;
    mobileIsOpen: boolean;
    desktopIsOpen: boolean;
    conversationOptions?: ConversationOption[];
}

const initialState: ConversationListState = {
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
        selectConversation: (state, action: PayloadAction<string | null>) => {
            state.selectedConversationId = action.payload ?? undefined;
        },
    },
});

export const {
    openMobileConversationList,
    openDesktopConversationList,
    setConversationOptions,
    selectConversation
} = conversationListSlice.actions;

export default conversationListSlice.reducer;