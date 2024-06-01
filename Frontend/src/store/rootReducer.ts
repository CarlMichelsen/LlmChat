import { combineReducers } from '@reduxjs/toolkit';
import userReducer from "./userSlice";
import conversationListReducer from './conversationListSlice';
import conversationReducer from './conversationSlice';
import inputReducer from './inputSlice';
import messageStreamReducer from './messageStreamSlice';

const rootReducer = combineReducers({
    messageStream: messageStreamReducer,
    input: inputReducer,
    conversation: conversationReducer,
    conversationList: conversationListReducer,
    user: userReducer,
});

export default rootReducer;