function RegisterWatermark(imageUrl, css, text) {
    if (/BugxTestHide=1/.test(document.cookie)) {
        return;
    }
    function $createText(text, dest) {
        var result = document.createTextNode(text || '');
        if (dest) {
            dest.appendChild(result);
        }
        return result;
    }
    function $create(e, attribs, styles) {
        var result = document.createElement(e);
        if (attribs) {
            for (var attrib in attribs) {
                result[attrib] = attribs[attrib];
            }
        }
        if (styles) {
            for (var style in styles) {
                result.style[style] = styles[style];
            }
        }
        result.injectInside = $injectInside;
        return result;
    }
    function $injectInside(e) {
        e.appendChild(this);
        return this;
    }
    function $ready(callback) {
        if (document.attachEvent && document.readyState != "complete") {
            document.attachEvent("onreadystatechange", function() {
                if (document.readyState == "complete") {
                    callback();
                }
            });
        } else {
            callback();
        }
    }
    function init() {
        imageUrl = 'url(' + imageUrl + ')';

        document.getElementsByTagName("head")[0].appendChild($create('link', { rel: 'stylesheet', type: 'text/css', href: css }));
        text = text || "Pre-production Environment";
        document.title = text + ": " + document.title;
        var host = $create("div", { className: 'Bugx_Test' }).injectInside(document.body);
        if (document.attachEvent) {
            //Disable IE text selection
            host.onselectstart = function() { return false; };
        }
        var bar = $create("div").injectInside(host);
        $create("b", null, { backgroundImage: imageUrl }).injectInside(bar);
        $create("a", { href: "#" }, { backgroundImage: imageUrl }).injectInside(bar).onclick = function() {
            host.style.display = 'none';
            document.cookie = "BugxTestHide=1";
            return false;
        };
        $createText(document.title, bar);
    }
    $ready(init);
}