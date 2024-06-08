export type LargeLanguageModelDto = {
    id: string;
    providerName: string;
    price: LargeLanguageModelPriceDto;
    modelDisplayName: string;
    modelDescription: string;
    modelIdentifierName: string;
    maxResponseTokenCount: number;
    maxContextTokenCount: number;
    imageSupport: boolean;
    videoSupport: boolean;
    jsonResponseOptimized: boolean;
}

export type LargeLanguageModelPriceDto = {
    millionInputTokenPrice: number;
    millionOutputTokenPrice: number;
}