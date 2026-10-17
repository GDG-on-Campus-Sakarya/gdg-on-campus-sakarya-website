window.gdgNavigation = window.gdgNavigation || {};

window.gdgNavigation.scrollToTop = function () {
    const root = document.documentElement;
    const body = document.body;

    // CSS'teki smooth scrolling ayarının anlık kaydırmayı
    // engellemesini önle.
    const previousBehavior = root.style.scrollBehavior;
    root.style.scrollBehavior = "auto";

    window.scrollTo(0, 0);

    root.scrollTop = 0;

    if (body) {
        body.scrollTop = 0;
    }

    requestAnimationFrame(() => {
        root.style.scrollBehavior = previousBehavior;
    });
};