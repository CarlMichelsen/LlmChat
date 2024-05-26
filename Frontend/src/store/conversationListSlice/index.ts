import { createSlice, PayloadAction } from '@reduxjs/toolkit';

type ConversationListState = {
    mobileIsOpen: boolean;
    desktopIsOpen: boolean;
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
    }
  },
});

export const {
  openMobileConversationList,
  openDesktopConversationList
} = conversationListSlice.actions;

export default conversationListSlice.reducer;