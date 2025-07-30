window.BlazorDownloadFile = (filename, contentType, content) => {
    const blob = new Blob([new Uint8Array(content)], { type: contentType });
    const link = document.createElement('a');
    link.download = filename;
    link.href = URL.createObjectURL(blob);
    link.click();
    URL.revokeObjectURL(link.href);
};