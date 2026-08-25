// Paints a version's palette onto the page.
//
// Every colour in sga.css derives from custom properties on :root, so switching
// versions is setting those properties — no stylesheet swap, no page reload, and
// no per-version CSS to maintain.

(function () {
    const loadedFonts = new Set();

    function apply(variables, googleFonts) {
        const root = document.documentElement;

        for (const [name, value] of Object.entries(variables || {})) {
            if (typeof value === "string" && value.length > 0) {
                root.style.setProperty(name, value);
            }
        }

        // The browser chrome around an installed app is themed separately.
        const primary = variables && variables["--sga-primary"];
        if (primary) {
            let meta = document.querySelector('meta[name="theme-color"]');
            if (!meta) {
                meta = document.createElement("meta");
                meta.setAttribute("name", "theme-color");
                document.head.appendChild(meta);
            }
            meta.setAttribute("content", primary);
        }

        loadFonts(googleFonts);
    }

    // Each version only pays for the faces it actually uses, and a family already
    // requested is never requested twice.
    function loadFonts(families) {
        if (!families || families.length === 0) return;

        const wanted = families.filter(f => f && !loadedFonts.has(f));
        if (wanted.length === 0) return;

        wanted.forEach(f => loadedFonts.add(f));

        const href = "https://fonts.googleapis.com/css2?" +
            wanted.map(f => "family=" + f).join("&") +
            "&display=swap";

        const link = document.createElement("link");
        link.rel = "stylesheet";
        link.href = href;
        document.head.appendChild(link);
    }

    window.sgaTheme = { apply };
})();
