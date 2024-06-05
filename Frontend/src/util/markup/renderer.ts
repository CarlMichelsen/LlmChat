import { marked } from "marked";

const languages = new Set([
    'html', 'css', 'javascript', 'jsx', 'ts', 'typescript', 'json', 'xml',
    'sql', 'bash', 'sh', 'shell', 'python', 'py', 'csharp', 'cs', 'cpp',
    'cpp', 'go', 'swift', 'php', 'ruby', 'rb', 'perl', 'r', 'rust', 'rs', 'kotlin',
    'kt', 'dart', 'scala', 'sbt', 'haskell', 'hs', 'elixir', 'elm', 'lua', 'groovy',
    'powershell', 'ps1', 'psm1']);

const startsWithSet = (input: string): number => {
    for (let i = 1; i < 10; i++) {
        if (input.length < i) {
            break;
        }

        if (languages.has(input.substring(0, i))) {
            return i;
        }
    }

    return -1;
}

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
    return `<pre class="my-1 overflow-auto bg-zinc-300 p-0.5"><code class="hljs ${language}">${escaped ? code : escapeHtml(code)}</code></pre>`;
}

marked.use({
    renderer: renderer
});

export const render = (content: string): string => {
    return marked.parse(content) as string;
}