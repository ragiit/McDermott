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

//Download File Education Awarenes
function openAndCloseTab(fileUrl) {
    // Open a new tab
    const newTab = window.open(fileUrl, "_blank");

    // Check if the tab opened successfully
    if (newTab) {
        // Close the tab after a short delay
        setTimeout(() => {
            newTab.close();
        }, 1000); // Adjust the delay as needed
    } else {
        alert("Unable to open a new tab. Please check your browser settings.");
    }
}

// ini Kepae
function saveFileExcellExporrt(fileName, streamReference) {
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

window.BlazorDownloadFileExample = (fileName) => {
    const anchor = document.createElement('a');
    anchor.setAttribute('download', fileName);
    anchor.setAttribute('href', URL.createObjectURL(new Blob([], { type: 'application/octet-stream' })));
    anchor.style.display = 'none';
    document.body.appendChild(anchor);
    anchor.click();
    document.body.removeChild(anchor);
};

function saveExportFile(filename, bytesBase64) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + bytesBase64;
    document.body.appendChild(link); // Needed for Firefox
    link.click();
    document.body.removeChild(link);
}

function getCanvasDataUrl(canvasId) {
    var canvas = document.getElementById(canvasId);
    console.log(canvas.toDataURL('image/png'));
    return canvas.toDataURL('image/png'); // atau format lain seperti 'image/jpeg'
}

//function getCompressedCanvasDataUrl(canvasId, quality = 0.8) {
//    return new Promise((resolve, reject) => {
//        var canvas = document.getElementById(canvasId);
//        var compressedCanvas = document.createElement('canvas');
//        compressedCanvas.width = canvas.width / 2; // Mengurangi ukuran gambar
//        compressedCanvas.height = canvas.height / 2;
//        var ctx = compressedCanvas.getContext('2d');

//        // Menggambar ulang dengan ukuran lebih kecil
//        ctx.drawImage(canvas, 0, 0, compressedCanvas.width, compressedCanvas.height);

//        compressedCanvas.toBlob((blob) => {
//            if (blob) {
//                var reader = new FileReader();
//                reader.onloadend = () => {
//                    resolve(reader.result);
//                };
//                reader.onerror = reject;
//                reader.readAsDataURL(blob);
//            } else {
//                reject('Blob creation failed');
//            }
//        }, 'image/jpeg', quality);
//    });
//}

window.BlazorDownloadFile = async (fileName) => {
    const response = await fetch(`/${fileName}`);
    const blob = await response.blob();
    const url = window.URL.createObjectURL(blob);
    const anchor = document.createElement('a');
    anchor.href = url;
    anchor.download = fileName;
    anchor.style.display = 'none';
    document.body.appendChild(anchor);
    anchor.click();
    window.URL.revokeObjectURL(url);
    document.body.removeChild(anchor);
};

// wwwroot/js/fileUtils.js
window.downloadFile = ({ fileName, content }) => {
    const byteCharacters = atob(content);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);

    const blob = new Blob([byteArray]);

    const link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    link.download = fileName;

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

window.hideSideBarr = function () {
    var l = document.getElementById("hal");
    l.click();
}

window.setCookie = function (name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + value + expires + "; path=/";
};

window.clearAllCookies = function () {
    var cookies = document.cookie.split(";");

    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i];
        var eqPos = cookie.indexOf("=");
        var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
        document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
    }
};

window.getCookie = function (name) {
    var nameEQ = name + "=";
    var cookies = document.cookie.split(';');
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i];
        while (cookie.charAt(0) == ' ') {
            cookie = cookie.substring(1, cookie.length);
        }
        if (cookie.indexOf(nameEQ) == 0) {
            return cookie.substring(nameEQ.length, cookie.length);
        }
    }
    return null;
};

window.deleteCookie = function (name) {
    // document.cookie = name + '=; Max-Age=-99999999;';
    document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
};

window.updateCookie = function (name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + value + expires + "; path=/";
};

window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}

window.clickInputFile = function (key) {
    document.getElementById(key).click();
}

window.BlazorHelpers = {
    setTimeout: function (callback, timeout) {
        return setTimeout(callback, timeout);
    }
};

window.blazorLocalStorage = {
    getItem: function (key) {
        return localStorage.getItem(key);
    },
    setItem: function (key, value) {
        localStorage.setItem(key, value);
    },
    removeItem: function (key) {
        localStorage.removeItem(key);
    }
};

function downloadFileFromStreamApi(fileName, byteBase64) {
    var link = document.createElement('a');
    link.download = fileName;
    link.href = 'data:application/octet-stream;base64,' + byteBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

window.saveAsFile = function (fileName, byteBase64) {
    var link = document.createElement('a');
    link.download = fileName;
    link.href = 'data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,' + byteBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

//Print Kiosk
//Print Kiosk
function printJS(contentToPrint) {
    var printWindow = window.open('', '_blank');
    printWindow.document.open();
    printWindow.document.write(contentToPrint);
    printWindow.document.close();
    printWindow.focus();
    printWindow.print();
    printWindow.close();
}

function printJSX(contentToPrint) {
    var printWindow = window.open('', '_blank');
    if (!printWindow) {
        console.error('Failed to open a new window. Please allow pop-ups for this site.');
        return;
    }
    printWindow.document.open();
    setTimeout(() => {
        printWindow.document.write(contentToPrint);
        printWindow.document.close();
        printWindow.focus();
        printWindow.print();
        printWindow.close();
    }, 500); // Delay of 500ms to ensure window is fully initialized
}

function printJS2(contentToPrint) {
    // Create a Blob object with the HTML content
    var blob = new Blob([contentToPrint], { type: 'application/pdf' });

    // Create a link element
    var link = document.createElement('a');

    // Create a URL for the Blob and set it as the href attribute
    link.href = URL.createObjectURL(blob);

    // Set the download attribute with a filename
    link.download = 'document.pdf';

    // Append the link to the body (required for Firefox)
    document.body.appendChild(link);

    // Trigger a click on the link to start the download
    link.click();

    // Remove the link from the document
    document.body.removeChild(link);
}

async function generatePdf22(content) {
    const { jsPDF } = window.jspdf;

    // Buat dokumen PDF
    const doc = new jsPDF({
        orientation: 'portrait', // Anda bisa mengatur 'landscape' jika perlu
        unit: 'mm',
        format: 'a4' // Format default, akan diubah nanti
    });

    // Buat elemen sementara untuk menghitung ukuran konten
    const tempDiv = document.createElement('div');
    tempDiv.style.position = 'absolute';
    tempDiv.style.visibility = 'hidden';
    tempDiv.style.width = '100%';
    tempDiv.innerHTML = content;
    document.body.appendChild(tempDiv);

    // Hitung tinggi konten
    const contentHeight = tempDiv.offsetHeight;

    // Hapus elemen sementara
    document.body.removeChild(tempDiv);

    // Set ukuran halaman PDF berdasarkan tinggi konten
    const pageHeight = Math.max(contentHeight, 297); // Minimal A4 height in mm
    doc.internal.pageSize.height = pageHeight;

    // Tambahkan HTML ke dokumen PDF
    doc.html(tempDiv, {
        callback: function (doc) {
            doc.save("report.pdf");
        },
        x: 10,
        y: 10
    });
}

function confirmSendEmail() {
    return confirm('Apakah Anda yakin ingin mengirim email ini?');
}
function generatePdf(content) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    // Tambahkan HTML ke dokumen PDF
    doc.html(content, {
        callback: function (doc) {
            doc.save("report.pdf");
        },
        x: 10,
        y: 10
    });
}

function downloadFileNew(url, filename) {
    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function downloadBase64File(base64Data, fileName) {
    const link = document.createElement('a');
    link.href = base64Data;
    link.download = fileName;
    link.click();
}

function printDocument(base64String) {
    const byteCharacters = atob(base64String);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    const blob = new Blob([byteArray], { type: 'application/octet-stream' });
    const url = URL.createObjectURL(blob);

    const iframe = document.createElement('iframe');
    iframe.style.display = 'none';
    document.body.appendChild(iframe);
    iframe.contentWindow.location.href = url;

    // Tunggu sebentar agar dokumen selesai dimuat
    setTimeout(() => {
        iframe.contentWindow.print();
        document.body.removeChild(iframe);
        URL.revokeObjectURL(url);
    }, 1000); // Sesuaikan waktu tunggu jika diperlukan
}