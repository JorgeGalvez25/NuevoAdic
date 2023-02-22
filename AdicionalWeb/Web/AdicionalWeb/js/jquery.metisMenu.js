﻿; (function($, window, document, undefined) {
    var pluginName = "metisMenu", defaults = { toggle: true, doubleTapToGo: false }; function Plugin(element, options) { this.element = $(element); this.settings = $.extend({}, defaults, options); this._defaults = defaults; this._name = pluginName; this.init(); }
    Plugin.prototype = { init: function() {
        var $this = this.element, $toggle = this.settings.toggle, obj = this; if (this.isIE() <= 9) { $this.find("li.active").has("ul").children("ul").collapse("show"); $this.find("li").not(".active").has("ul").children("ul").collapse("hide"); } else { $this.find("li.active").has("ul").children("ul").addClass("collapse in"); $this.find("li").not(".active").has("ul").children("ul").addClass("collapse"); }
        if (obj.settings.doubleTapToGo) { $this.find("li.active").has("ul").children("a").addClass("doubleTapToGo"); }
        $this.find("li").has("ul").children("a").on("click" + "." + pluginName, function(e) {
            e.preventDefault(); if (obj.settings.doubleTapToGo) { if (obj.doubleTapToGo($(this)) && $(this).attr("href") !== "#" && $(this).attr("href") !== "") { e.stopPropagation(); document.location = $(this).attr("href"); return; } }
            $(this).parent("li").toggleClass("active").children("ul").collapse("toggle"); if ($toggle) { $(this).parent("li").siblings().removeClass("active").children("ul.in").collapse("hide"); } 
        });
    }, isIE: function() { var undef, v = 3, div = document.createElement("div"), all = div.getElementsByTagName("i"); while (div.innerHTML = "<!--[if gt IE " + (++v) + "]><i></i><![endif]-->", all[0]) { return v > 4 ? v : undef; } }, doubleTapToGo: function(elem) {
        var $this = this.element; if (elem.hasClass("doubleTapToGo")) { elem.removeClass("doubleTapToGo"); return true; }
        if (elem.parent().children("ul").length) { $this.find(".doubleTapToGo").removeClass("doubleTapToGo"); elem.addClass("doubleTapToGo"); return false; } 
    }, remove: function() { this.element.off("." + pluginName); this.element.removeData(pluginName); } 
    }; $.fn[pluginName] = function(options) {
        this.each(function() {
            var el = $(this); if (el.data(pluginName)) { el.data(pluginName).remove(); }
            el.data(pluginName, new Plugin(this, options));
        }); return this;
    };
})(jQuery, window, document);