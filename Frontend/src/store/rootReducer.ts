import { combineReducers } from '@reduxjs/toolkit';
import userReducer from "./userSlice";
import conversationListReducer from './conversationListSlice';

const rootReducer = combineReducers({
    conversationList: conversationListReducer,
    user: userReducer,
});

export default rootReducer;