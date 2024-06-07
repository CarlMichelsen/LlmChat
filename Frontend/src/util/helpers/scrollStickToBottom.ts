export const scrollStickToBottom = (forceScroll: boolean = false) => {
    const scrollElementId = "conversation-scroll-chat";
    const element = document.getElementById(scrollElementId);
    if (element == null) {
        throw new Error(`Element with id "${scrollElementId}" was not found when attempting to scroll to bottom`);
    }

    const threshold = 300;
    const isNearBottom = element.scrollHeight - element.scrollTop <= element.clientHeight + threshold;

    if (isNearBottom || forceScroll) {
        element.scrollTop = element.scrollHeight;
    }
}