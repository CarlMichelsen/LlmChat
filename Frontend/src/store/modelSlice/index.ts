import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { LargeLanguageModelDto } from '../../util/type/llmChat/model';

const localStorageIdentifier = "modelId";

type ModelState = {
    selectedModelId: string;
    dialogOpen: boolean;
    models?: LargeLanguageModelDto[];
}

const initialState: ModelState = {
    selectedModelId: localStorage.getItem(localStorageIdentifier) ?? "2370891f-9593-4ba6-be41-56e47fa6083f",
    dialogOpen: false,
};

const modelSlice = createSlice({
    name: 'models',
    initialState,
    reducers: {
        setModels: (state, action: PayloadAction<LargeLanguageModelDto[]>) => {
            state.models = action.payload;
        },
        setModelId: (state, action: PayloadAction<string>) => {
            if (!state.models) {
                return;
            }

            const exsistingModel = state.models.find(m => m.id === action.payload);
            if (!exsistingModel) {
                return;
            }

            localStorage.setItem(localStorageIdentifier, exsistingModel.id);
            state.selectedModelId = exsistingModel.id;
        },
        setDialogOpen: (state, action: PayloadAction<boolean>) => {
            state.dialogOpen = action.payload;
        }
    },
});

export const {
    setModels,
    setModelId,
    setDialogOpen,
} = modelSlice.actions;

export default modelSlice.reducer;