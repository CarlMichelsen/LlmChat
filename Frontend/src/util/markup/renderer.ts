import { marked } from "marked";

const htmlEscapes: { [key: string]: string } = {
    '&': '&amp;',
    '<': '&lt;',
    '>': '&gt;',
    '"': '\"',
};

const escapeHtml = (unsafe: string): string => {
    return unsafe.replace(/[&<>"']/g, (match) => htmlEscapes[match] || match);
};

const renderer = new marked.Renderer();
renderer.code = (code: string, infostring: string | undefined, _: boolean): string => {
    const languageClass = infostring ? `language-${infostring}` : '';
    return `<pre class="my-1 overflow-auto bg-black text-white rounded-sm pb-4 p-0.5 w-[490px] lg:container"><code class="${languageClass}">${code}</code></pre>`;
}

marked.use({ renderer: renderer });

export const render = (content: string): string => {
    return marked.parse(escapeHtml(content).replace(/^[\u200B\u200C\u200D\u200E\u200F\uFEFF]/,"")) as string;
}