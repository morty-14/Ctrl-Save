document.addEventListener('DOMContentLoaded', function () {
    const imgWrap = document.querySelector('.cs-detail-img-wrap');
    if (!imgWrap) return;

    const img = imgWrap.querySelector('img');
    if (!img) return;

    // Inject overlay HTML directly into the page
    const overlayHtml = `
        <div id="cs-zoom-overlay" style="
            display:none;
            position:fixed;
            top:0;left:0;
            width:100%;height:100%;
            background:rgba(0,0,0,0.85);
            z-index:99999;
            justify-content:center;
            align-items:center;
            cursor:zoom-out;">
            <button id="cs-zoom-close" style="
                position:fixed;
                top:1rem;right:1.25rem;
                background:#fff;
                border:none;
                border-radius:50%;
                width:36px;height:36px;
                font-size:1.3rem;
                cursor:pointer;
                z-index:100000;
                display:flex;
                align-items:center;
                justify-content:center;">&#x2715;</button>
            <img id="cs-zoom-img" style="
                max-width:90vw;
                max-height:90vh;
                object-fit:contain;
                border-radius:6px;" />
        </div>`;

    document.body.insertAdjacentHTML('beforeend', overlayHtml);

    const overlay = document.getElementById('cs-zoom-overlay');
    const overlayImg = document.getElementById('cs-zoom-img');
    const closeBtn = document.getElementById('cs-zoom-close');

    // Open on image click
    imgWrap.addEventListener('click', function () {
        overlayImg.src = img.src;
        overlay.style.display = 'flex';
        document.body.style.overflow = 'hidden';
    });

    // Close on overlay background click
    overlay.addEventListener('click', function (e) {
        if (e.target === overlay) closeLightbox();
    });

    // Close on close button
    closeBtn.addEventListener('click', function () {
        closeLightbox();
    });

    // Close on Escape key
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') closeLightbox();
    });

    function closeLightbox() {
        overlay.style.display = 'none';
        document.body.style.overflow = '';
    }
});
