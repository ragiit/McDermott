// Fixed
function downloadFileFromStream(fileName, streamReference) {
    streamReference.arrayBuffer().then(function (arrayBuffer) {
        const blob = new Blob([arrayBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = URL.createObjectURL(blob);
        const anchorElement = document.createElement('a');
        anchorElement.href = url;
        anchorElement.download = fileName;
        anchorElement.click();
        URL.revokeObjectURL(url);
    });
}

window.clickInputFile = function (key) {
    document.getElementById(key).click();
}