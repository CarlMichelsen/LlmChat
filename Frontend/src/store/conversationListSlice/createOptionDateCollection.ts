import { ConversationOption, OptionDateCollection } from "../../util/type/optionDateCollection";

type OptionDate = {
    title: string;
    date: number;
    index: number;
}

const oneDaySeconds = 3600 * 24;
const today = new Date();
today.setHours(0,0,0);
const nowSeconds = today.getTime()/1000;

const groups: OptionDate[] = [
    { title: "Today", date: Math.round(nowSeconds), index: 0 },
    { title: "Yesterday", date: Math.round(nowSeconds - oneDaySeconds * 1), index: 1 },
    { title: "Three days ago", date: Math.round(nowSeconds - oneDaySeconds * 3), index: 2 },
    { title: "A week ago", date: Math.round(nowSeconds - oneDaySeconds * 7), index: 3 },
    { title: "A month ago", date: Math.round(nowSeconds - oneDaySeconds * 31), index: 4 },
    { title: "A year ago", date: Math.round(nowSeconds - oneDaySeconds * 365), index: 5},
]

export const createOptionDateCollection = (options: ConversationOption[]): OptionDateCollection[] => {
    const collectionList = groups.map((g): OptionDateCollection => {
        return {
            index: g.index,
            htmlId: `collection-${g.index}`,
            dateString: g.title,
            date: g.date,
            options: [],
        };
    });

    const sorted = [ ...options.sort((a, b) => b.lastAppendedEpoch - a.lastAppendedEpoch) ];
    let current: ConversationOption|undefined;
    do
    {
        current = sorted.shift();
        if (!current) continue;
        

        for (let i = 0; i < collectionList.length; i++) {
            const collection = collectionList[i];
            
            if (collection.date <= current.lastAppendedEpoch) {
                collection.options.push(current);
                break;
            }
        }
    }
    while (current);

    return collectionList;
}