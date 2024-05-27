import { Content } from "../content";

export type Message = {
    id: string;
    content: Content[];
    completedUtc: Date;
}