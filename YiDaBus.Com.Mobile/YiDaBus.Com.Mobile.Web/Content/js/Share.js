var _system = {
    $: function (id) { return document.getElementById(id); },
    _client: function () {
        return { w: document.documentElement.scrollWidth, h: document.documentElement.scrollHeight, bw: document.documentElement.clientWidth, bh: document.documentElement.clientHeight };
    },
    _scroll: function () {
        return { x: document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft, y: document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop };
    },
    _cover: function (show) {
        if (show) {
            this.$("cover").style.display = "block";
            this.$("cover").style.width = (this._client().bw > this._client().w ? this._client().bw : this._client().w) + "px";
            this.$("cover").style.height = (this._client().bh > this._client().h ? this._client().bh : this._client().h) + "px";
        } else {
            this.$("cover").style.display = "none";
        }
    },
    _guide: function (click) {
        this._cover(true);
        this.$("guide").style.display = "block";
        //this.$("guide").style.top = (_system._scroll().y + 5) + "px";
        window.onresize = function () { _system._cover(true); _system.$("guide").style.top = (_system._scroll().y + 5) + "px"; };
        if (click) {
            _system.$("cover").onclick = function () {
                _system._cover();
                _system.$("guide").style.display = "none";
                _system.$("cover").onclick = null;
                window.onresize = null;
            };
        }
    },
    _zero: function (n) {
        return n < 0 ? 0 : n;
    }
}
$(function () {
    $('body').append('<div id="cover" style="display: none;position: absolute;left: 0;top: -45px;z-index: 18888;background-color: #000000;opacity: 0.7;"></div><div id="guide" style="display: none;position: absolute;right: 10px;top: -40px;z-index: 19999;"><img style="width: 260px;height: 180px;position: fixed;top: 10px;right: 0px;" src="/Content/img/share.png" /></div>');
});