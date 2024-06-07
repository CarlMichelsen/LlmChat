import { marked } from "marked";

const htmlEscapes: { [key: string]: string } = {
    '&': '&amp;',
    '<': '&lt;',
    '>': '&gt;',
    '"': '&quot;',
    "'": '&#039;'
};

const escapeHtml = (unsafe: string): string => {
    return unsafe.replace(/[&<>"']/g, (match) => htmlEscapes[match] || match);
};

const renderer = new marked.Renderer();
renderer.code = (code: string, infostring: string | undefined, escaped: boolean): string => {
    const languageClass = infostring ? `language-${infostring}` : '';
    return `<pre class="my-1 overflow-auto bg-black text-white rounded-sm pb-4 p-0.5"><code class="${escapeHtml(languageClass)}">${escaped ? code : escapeHtml(code)}</code></pre>`;
}

marked.use({ renderer: renderer });

export const render = (content: string): string => {
    return marked.parse(content.replace(/^[\u200B\u200C\u200D\u200E\u200F\uFEFF]/,"")) as string;
}