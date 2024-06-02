import { Message } from "./message";

export type DialogSlice = {
    messages: Message[];
    selectedIndex: number;
}