import { Concluded } from "./concluded";
import { Content } from "./content";

type ErrorContentDelta = {
    conversationId?: string;
    error: string;
};

type NonErrorContentDelta = {
    conversationId: string;
    content?: Content;
    concluded?: Concluded;
}

export type ContentDelta = NonErrorContentDelta | ErrorContentDelta;