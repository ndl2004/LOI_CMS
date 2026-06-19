const decodeHtml = (value) => {
    if (typeof document === "undefined") {
        return value;
    }

    const textarea = document.createElement("textarea");
    textarea.innerHTML = value;
    return textarea.value;
};

export const getPlainTextFromHtml = (content, fallback) => {
    if (!content) return fallback;

    let text = String(content);

    for (let i = 0; i < 2; i += 1) {
        text = decodeHtml(text)
            .replace(/<script[\s\S]*?<\/script>/gi, " ")
            .replace(/<style[\s\S]*?<\/style>/gi, " ")
            .replace(/<[^>]+>/g, " ");
    }

    text = decodeHtml(text)
        .replace(/\s+/g, " ")
        .trim();

    return text || fallback;
};

export const getShortPlainText = (content, maxLength, fallback) => {
    const text = getPlainTextFromHtml(content, fallback);

    return text.length > maxLength
        ? `${text.substring(0, maxLength).trim()}...`
        : text;
};
