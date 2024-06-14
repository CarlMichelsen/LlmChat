import { Content } from "./conversation/content";
import { ResponseTo } from "./responseTo";

export type NewMessage = {
    responseTo?: ResponseTo;
    content: Content[];
    modelIdentifier: string;
}