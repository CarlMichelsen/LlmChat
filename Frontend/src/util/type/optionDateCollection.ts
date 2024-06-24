export type ConversationOption = {
    id: string;
    summary?: string;
    lastAppendedEpoch: number;
    createdEpoch: number;
}

export type OptionDateCollection = {
    index: number;
    htmlId: string;
    dateString: string;
    date: number;
    options: ConversationOption[];
}