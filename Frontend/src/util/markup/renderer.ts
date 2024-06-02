import { marked } from "marked";

const escapeHtml = (unsafe: string): string => {
    const htmlEscapes: { [key: string]: string } = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#039;'
    };

    return unsafe.replace(/[&<>"']/g, (match) => htmlEscapes[match] || match);
};

const renderer = new marked.Renderer();
renderer.code = (code: string, infostring: string | undefined, escaped: boolean): string => {
    const language = infostring ? `language-${infostring}}` : '';
    return `<pre class="my-1 overflow-auto bg-zinc-300 p-0.5"><code class="hljs ${language}"></code>${escaped ? code : escapeHtml(code)}</pre>`;
}

marked.use({
    renderer: renderer
});

export const render = (content: string): string => {
    return marked.parse(content.replace(/^[\u200B\u200C\u200D\u200E\u200F\uFEFF]/,"")) as string;
}