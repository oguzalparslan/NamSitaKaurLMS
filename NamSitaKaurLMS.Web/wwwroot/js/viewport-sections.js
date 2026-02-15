(function () {
    function setVhVar() {
        const vh = window.innerHeight * 0.01;
        document.documentElement.style.setProperty("--vh", `${vh}px`);
    }

    function setLayoutHeights() {
        const header = document.querySelector("header.app-navbar");
        const toolbar = document.querySelector(".app-toolbar");
        const bottomNav = document.querySelector(".bottom-nav");

        const headerH = header ? header.offsetHeight : 0;
        const toolbarH = toolbar ? toolbar.offsetHeight : 0;

        // bottom-nav mobilde fixed, desktop’ta yok gibi; gene de ölçüp veriyoruz
        const bottomH = bottomNav ? bottomNav.offsetHeight : 0;

        document.documentElement.style.setProperty("--app-header-h", `${headerH}px`);
        document.documentElement.style.setProperty("--app-toolbar-h", `${toolbarH}px`);
        document.documentElement.style.setProperty("--app-bottomnav-h", `${bottomH}px`);

        // scroll-padding için tek değişkende toplamak istersen:
        document.documentElement.style.setProperty("--app-sticky-offset", `${headerH + toolbarH}px`);
    }

    function refresh() {
        setVhVar();
        setLayoutHeights();
    }

    window.addEventListener("resize", refresh);
    window.addEventListener("orientationchange", refresh);
    document.addEventListener("DOMContentLoaded", refresh);
})();
