/**
 * Sets a query parameter.
 * @param key Query parameter key.
 * @param value Query parameter value.
 */
export const setQueryParam = (key: string, value: string | null): void => {
    const url = new URL(window.location.href);

    if (value === null) {
        url.searchParams.delete(key);
    } else {
        url.searchParams.set(key, value);
    }
    
    if (!url.searchParams.toString()) {
        window.history.replaceState({}, '', url.pathname);
    } else {
        window.history.replaceState({}, '', url.toString());
    }
}

/**
 * Gets a query parameter value.
 * @param key Query parameter key.
 * @returns Query parameter value or null if not present.
 */
export const getQueryParam = (key: string): string | null => {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(key);
}