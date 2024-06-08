import { useQuery } from "react-query";
import { getAvailableModels } from "../../../../util/client/models";
import { LargeLanguageModelDto } from "../../../../util/type/llmChat/model";
import { useEffect, useState } from "react";
import store, { RootApplicationState } from "../../../../store";
import { setDialogOpen, setModels } from "../../../../store/modelSlice";
import { useSelector } from "react-redux";

const ModelSelector: React.FC = () => {
    const [model, setModel] = useState<LargeLanguageModelDto|null>(null);
    const modelState = useSelector((state: RootApplicationState) => state.models);
    
    const fetchModels = async (): Promise<LargeLanguageModelDto[]> => {
        const res = await getAvailableModels();
        if (res.ok) {
            return res.data!;
        } else {
            throw new Error("Failed to fetch conversation options");
        }
    }

    const toggleDialog = () => store.dispatch(setDialogOpen(!modelState.dialogOpen));

    const { data, status } = useQuery<LargeLanguageModelDto[], Error>(
        'models',
        fetchModels, {
        staleTime: Infinity
    });

    useEffect(() => {
        if (status === "success") {
            store.dispatch(setModels(data));
        }
    }, [data, status]);

    useEffect(() => {
        setModel(modelState.models?.find(m => m.id === modelState.selectedModelId) ?? null);
    }, [modelState.models, modelState.selectedModelId]);

    return (
        <div className="bg-white rounded-bl-md w-36">
            {model ? (
                <div className="grid grid-cols-[25px_1fr] w-full">
                    <button
                        className="rounded-bl-md w-full h-full bg-blue-600 text-white pb-1"
                        onMouseDown={() => toggleDialog()}>+</button>
                    <p className="m-2">{model?.modelDisplayName ?? "loading..."}</p>
                </div>
            ) : (
                <div className="p-2 w-full"><p>loading...</p></div>
            )}
        </div>
    );
}

export default ModelSelector;