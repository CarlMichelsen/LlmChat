import { DialogSlice } from "./dialogSlice";

export type Conversation = {
    id: string;
    summary?: string;
    systemMessage: string;
    dialogSlices: DialogSlice[];
    lastUpdatedUtc: Date;
    createdUtc: Date;
}