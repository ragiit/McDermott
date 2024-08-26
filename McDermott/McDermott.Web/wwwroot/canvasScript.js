////window.initializeCanvas = (canvasId) => {
////    const canvas = document.getElementById(canvasId);
////    if (!canvas) {
////        console.error("Canvas element not found.");
////        return;
////    }
////    const ctx = canvas.getContext('2d');
////    let markerCount = 0;
////    let markers = [];

////    const img = new Image();
////    img.src = 'image/aciddent.png'; // Replace with the correct image path
////    img.onload = () => {
////        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
////        redrawMarkers();
////    }

////    canvas.addEventListener('click', (e) => {
////        const x = e.offsetX;
////        const y = e.offsetY;

////        const clickedMarkerIndex = markers.findIndex(marker => {
////            const dx = marker.x - x;
////            const dy = marker.y - y;
////            return Math.sqrt(dx * dx + dy * dy) < 10; // Adjust radius as needed
////        });

////        if (clickedMarkerIndex !== -1) {
////            markers.splice(clickedMarkerIndex, 1);
////        } else {
////            markerCount++;
////            markers.push({ number: markerCount, x, y });
////        }
////        redrawMarkers();
////    });

////    window.resetMarkers = () => {
////        markerCount = 0;
////        markers = [];
////        redrawMarkers();
////    }

////    window.getCanvasImageData = (canvasId) => {
////        const canvas = document.getElementById(canvasId);
////        if (!canvas) {
////            console.error("Canvas element not found.");
////            return null;
////        }
////        console.log(canvas.toDataURL("image/png"));
////        return canvas.toDataURL("image/png");
////    }

////    function redrawMarkers() {
////        ctx.clearRect(0, 0, canvas.width, canvas.height);
////        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
////        markers.forEach(marker => {
////            ctx.fillStyle = 'purple';
////            ctx.beginPath();
////            ctx.arc(marker.x, marker.y, 12, 0, 2 * Math.PI);
////            ctx.fill();
////            ctx.fillStyle = 'white';
////            ctx.font = 'bold 12px Arial';
////            ctx.fillText(marker.number, marker.x - 5, marker.y + 5);
////        });
////    }
////};

window.initializeCanvas = (canvasId) => {
    const canvas = document.getElementById(canvasId);
    if (!canvas) {
        console.error("Canvas element not found.");
        return;
    }
    const ctx = canvas.getContext('2d');
    let markerCount = 0;
    let markers = [];

    // Menghapus event listener sebelumnya jika ada
    canvas.removeEventListener('click', handleClick);
    canvas.addEventListener('click', handleClick);

    const img = new Image();
    img.crossOrigin = "anonymous"; // Pastikan gambar dapat diakses lintas domain jika perlu
    img.src = 'image/aciddent.png'; // Ganti dengan path gambar yang benar
    img.onload = () => {
        // Kompres gambar terlebih dahulu
        const compressedCanvas = document.createElement('canvas');
        const compressedCtx = compressedCanvas.getContext('2d');
        compressedCanvas.width = img.width / 2; // Mengurangi ukuran gambar
        compressedCanvas.height = img.height / 2;

        compressedCtx.drawImage(img, 0, 0, compressedCanvas.width, compressedCanvas.height);

        // Menggambar gambar terkompresi ke canvas utama
        ctx.drawImage(compressedCanvas, 0, 0, canvas.width, canvas.height);
        redrawMarkers();
    }

    function handleClick(e) {
        const x = e.offsetX;
        const y = e.offsetY;

        const clickedMarkerIndex = markers.findIndex(marker => {
            const dx = marker.x - x;
            const dy = marker.y - y;
            return Math.sqrt(dx * dx + dy * dy) < 10; // Adjust radius as needed
        });

        if (clickedMarkerIndex !== -1) {
            markers.splice(clickedMarkerIndex, 1);
        } else {
            markerCount++;
            markers.push({ number: markerCount, x, y });
        }
        redrawMarkers();
    }

    window.resetMarkers = () => {
        markerCount = 0;
        markers = [];
        redrawMarkers();
    }

    window.getCanvasImageData = (canvasId) => {
        const canvas = document.getElementById(canvasId);
        if (!canvas) {
            console.error("Canvas element not found.");
            return null;
        }
        console.log(canvas.toDataURL("image/png"));
        return canvas.toDataURL("image/png");
    }

    function redrawMarkers() {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
        markers.forEach(marker => {
            ctx.fillStyle = 'purple';
            ctx.beginPath();
            ctx.arc(marker.x, marker.y, 12, 0, 2 * Math.PI);
            ctx.fill();
            ctx.fillStyle = 'white';
            ctx.font = 'bold 12px Arial';
            ctx.fillText(marker.number, marker.x - 5, marker.y + 5);
        });
    }
};