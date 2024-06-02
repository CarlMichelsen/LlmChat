import { Prompt } from "./conversation/prompt";

export type Concluded = Prompt & {
    messageId: string;
};