const replaceSubdomain = (newSubdomain: string, overrideUrl?: string): string => {
    // get current URL
    let url = overrideUrl ?? window.location.href;
    
    // create an URL object
    let urlObj = new URL(url);
  
    // split hostname by dot
    let parts = urlObj.hostname.split('.');
  
    // if there's a subdomain
    if(parts.length > 2) {
      // replace the subdomain
      parts[0] = newSubdomain;
    }
  
    // join the parts back together
    urlObj.hostname = parts.join('.');

    const finalUrl = urlObj.href;
    return finalUrl.endsWith('/')
        ? finalUrl.slice(0, -1)
        : finalUrl;
}

export const loginUrl = (): string => {
    return import.meta.env.MODE == "development"
        ? "http://localhost:5197"
        : replaceSubdomain("login");
}

export const rootUrl = (): string => import.meta.env.VITE_APP_ENV === 'development'
    ? "http://localhost:5286"
    : "";