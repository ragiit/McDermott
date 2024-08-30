window.initializeCanvas = (canvasId, existingMarkers = []) => {
    const canvas = document.getElementById(canvasId);
    if (!canvas) {
        console.error("Canvas element not found.");
        return;
    }

    const ctx = canvas.getContext('2d');
    let markers = existingMarkers || [];

    const img = new Image();
    img.src = 'image/aciddent.png'; // Replace with the correct image path
    img.onload = () => {
        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
        redrawMarkers();
    };

    const handleClick = (e) => {
        const { offsetX: x, offsetY: y } = e;
        const clickedMarkerIndex = markers.findIndex(({ x: mx, y: my }) =>
            Math.hypot(mx - x, my - y) < 10
        );

        if (clickedMarkerIndex !== -1) {
            markers.splice(clickedMarkerIndex, 1);
        } else {
            markers.push({ number: markers.length + 1, x, y });
        }
        redrawMarkers();
    };

    const redrawMarkers = () => {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
        markers.forEach(({ number, x, y }) => {
            ctx.fillStyle = 'purple';
            ctx.beginPath();
            ctx.arc(x, y, 12, 0, 2 * Math.PI);
            ctx.fill();

            ctx.fillStyle = 'white';
            ctx.font = 'bold 12px Arial';
            ctx.fillText(number, x - 5, y + 5);
        });
    };

    canvas.addEventListener('click', handleClick);

    window.resetMarkers = () => {
        markers = [];
        redrawMarkers();
    };

    window.getMarkersData = () => {
        console.log("cekfaktadata"+markers);
        JSON.stringify(markers);
    };

    
};
