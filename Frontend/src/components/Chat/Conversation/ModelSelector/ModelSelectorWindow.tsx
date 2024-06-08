import { useSelector } from "react-redux";
import store, { RootApplicationState } from "../../../../store";
import { useEffect, useState } from "react";
import { LargeLanguageModelDto } from "../../../../util/type/llmChat/model";
import { setDialogOpen, setModelId } from "../../../../store/modelSlice";

type ProviderListItem = {
    providerName: string,
    models: LargeLanguageModelDto[]
}

const ModelSelectorWindow: React.FC = () => {
    const [providers, setProviders] = useState<ProviderListItem[]>([]);
    const modelState = useSelector((state: RootApplicationState) => state.models);

    useEffect(() => {
        if (!modelState.models) {
            return;
        }
        
        const nextProviders: ProviderListItem[] = [];
        modelState.models.forEach(model => {
            const exsisting = nextProviders.find(p => p.providerName === model.providerName);
            if (exsisting) {
                exsisting.models.push(model);
            } else {
                nextProviders.push({
                    providerName: model.providerName,
                    models: [model],
                });
            }
        });

        setProviders(nextProviders);
    }, [modelState.models]);

    const handleClick = (modelId: string) => {
        store.dispatch(setModelId(modelId));
        store.dispatch(setDialogOpen(false));
    }
    
    return (
        <ol>
            {providers.map(p => (
                <li>
                    <p>{p.providerName}</p>
                    <ol>
                        {p.models.map(m => (
                            <li className="ml-2">
                                <button className="hover:underline w-48 text-left px-2" onMouseDown={() => handleClick(m.id)}>{m.modelDisplayName}</button>
                            </li>
                        ))}
                    </ol>
                </li>
            ))}
        </ol>
    );
}

export default ModelSelectorWindow;