import { DialogSlice } from "./dialogSlice";

export type Conversation = {
    id: string;
    summary?: string;
    dialogSlices: DialogSlice[];
    lastUpdatedUtc: Date;
    createdUtc: Date;
}