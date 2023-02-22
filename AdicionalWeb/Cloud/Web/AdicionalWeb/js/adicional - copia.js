/**  Consola  **/
(function() { window.console || (window.console = {}); for (var b = "log info warn error debug trace dir group groupCollapsed groupEnd time timeEnd profile profileEnd dirxml assert count markTimeline timeStamp clear".split(" "), a = 0; a < b.length; a++) window.console[b[a]] || (window.console[b[a]] = function() { }) })();
/** Formato de numeros **/
if (!window.format) { (function(a) { a.format = function(v, u) { if (!u || isNaN(+v)) { return v } var v = u.charAt(0) == "-" ? -v : +v, m = v < 0 ? v = -v : 0, r = u.match(/[^\d\-\+#]/g), o = r && r[r.length - 1] || ".", r = r && r[1] && r[0] || ",", u = u.split(o), v = v.toFixed(u[1] && u[1].length), v = +v + "", s = u[1] && u[1].lastIndexOf("0"), t = v.split("."); if (!t[1] || t[1] && t[1].length <= s) { v = (+v).toFixed(s + 1) } s = u[0].split(r); u[0] = s.join(""); var q = u[0] && u[0].indexOf("0"); if (q > -1) { for (; t[0].length < u[0].length - q; ) { t[0] = "0" + t[0] } } else { +t[0] == 0 && (t[0] = "") } v = v.split("."); v[0] = t[0]; if (t = s[1] && s[s.length - 1].length) { for (var s = v[0], q = "", l = s.length % t, p = 0, n = s.length; p < n; p++) { q += s.charAt(p), !((p - l + 1) % t) && p < n - t && (q += r) } v[0] = q } v[1] = u[1] && v[1] ? o + v[1] : ""; return (m ? "-" : "") + v[0] + v[1] } })(window) }
(function(c) { var b = /["\\\x00-\x1f\x7f-\x9f]/g, d = { "\b": "\\b", "\t": "\\t", "\n": "\\n", "\f": "\\f", "\r": "\\r", '"': '\\"', "\\": "\\\\" }, a = Object.prototype.hasOwnProperty; c.stringify = typeof JSON === "object" && JSON.stringify ? JSON.stringify : function(g) { if (g === null) { return "null" } var f, j, e, h, p = c.type(g); if (p === "undefined") { return undefined } if (p === "number" || p === "boolean") { return String(g) } if (p === "string") { return c.quoteString(g) } if (typeof g.stringify === "function") { return c.stringify(g.stringify()) } if (p === "date") { var m = g.getUTCMonth() + 1, q = g.getUTCDate(), n = g.getUTCFullYear(), r = g.getUTCHours(), i = g.getUTCMinutes(), s = g.getUTCSeconds(), l = g.getUTCMilliseconds(); if (m < 10) { m = "0" + m } if (q < 10) { q = "0" + q } if (r < 10) { r = "0" + r } if (i < 10) { i = "0" + i } if (s < 10) { s = "0" + s } if (l < 100) { l = "0" + l } if (l < 10) { l = "0" + l } return '"' + n + "-" + m + "-" + q + "T" + r + ":" + i + ":" + s + "." + l + 'Z"' } f = []; if (c.isArray(g)) { for (j = 0; j < g.length; j++) { f.push(c.toJSON(g[j]) || "null") } return "[" + f.join(",") + "]" } if (typeof g === "object") { for (j in g) { if (a.call(g, j)) { p = typeof j; if (p === "number") { e = '"' + j + '"' } else { if (p === "string") { e = c.quoteString(j) } else { continue } } p = typeof g[j]; if (p !== "function" && p !== "undefined") { h = c.stringify(g[j]); f.push(e + ":" + h) } } } return "{" + f.join(",") + "}" } }; c.quoteString = function(e) { if (e.match(b)) { return '"' + e.replace(b, function(f) { var g = d[f]; if (typeof g === "string") { return g } g = f.charCodeAt(); return "\\u00" + Math.floor(g / 16).toString(16) + (g % 16).toString(16) }) + '"' } return '"' + e + '"' } } (jQuery));
// Clases Adicional Javascript v1.0
(function(w)  { 
    function getNavigator(idx){
        var ua=navigator.userAgent,tem,M=ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];                                                                                                                         
        if(/trident/i.test(M[1])){
            tem=/\brv[ :]+(\d+)/g.exec(ua) || [];
            return 'IE '+(tem[1]||'');
            }
        if(M[1]==='Chrome'){
            tem=ua.match(/\bOPR\/(\d+)/)
            if(tem!=null)   {return 'Opera '+tem[1];}
            }   
        M=M[2]? [M[1], M[2]]: [navigator.appName, navigator.appVersion, '-?'];
        if((tem=ua.match(/version\/(\d+)/i))!=null) {M.splice(1,1,tem[1]);}
        return M[idx];
    }
    w.Adicional = { 
        'browser': { 
            'isIE' : navigator.userAgent.match(/msie/i), 
            'name': (function(){return getNavigator(0);})(),
            'version' : (function(){return getNavigator(1);})(),
        }, 'OnLoad': void 0 
    }; 
    if (!w.Class) { w.Class = function(a) { var b = function() { if (this.initialize) this.initialize(arguments) }, c; for (c in a) { b.prototype[c] = a[c] } b.prototype.initialize || (b.prototype.initialize = function() { }); b.prototype.dispose || (b.prototype.dispose = function() { }); return b } } })(window);

(function(w){
     w.Adicional = $.extend(true, w.Adicional, {
        'Compressors':{
            'LZW':{
                'compress':function(f){var a,c={},d,e,b="",g=[],h=256;for(a=0;256>a;a+=1){c[String.fromCharCode(a)]=a;}for(a=0;a<f.length;a+=1){d=f.charAt(a),e=b+d,c.hasOwnProperty(e)?b=e:(g.push(c[b]),c[e]=h++,b=String(d));}""!==b&&g.push(c[b]);return g;},
                'decompress':function(f){var a,c=[],d,e,b;b="";var g=256;for(a=0;256>a;a+=1){c[a]=String.fromCharCode(a);}e=d=String.fromCharCode(f[0]);for(a=1;a<f.length;a+=1){b=f[a];if(c[b]){b=c[b];}else{if(b===g){b=d+d.charAt(0);}else{return null;}}e+=b;c[g++]=d+b.charAt(0);d=b;}return e;}
            }
        }
    });
})(window);

// Threading
(function(w){
    "use strict";
    function loadXMLDoc(current)
    {
        return ((window.XMLHttpRequest) ? new XMLHttpRequest() : 
                                          new ActiveXObject('Microsoft.XMLHTTP'));
    }

    function createEvents(param){
        param.current.onreadystatechange = function(e, f, g, h) {
            try {
                if (param.current.readyState == param.current.DONE){
                    if(param.current.status == 200){
                        if (param.options.ok) {
                            var response = param.current.response ? param.current.response : param.current.responseText;
                            if(Adicional.browser.isIE) {
                                if(response && response != ''){ 
                                    var aux = $.parseJSON(response);
                                    param.options.ok($.parseJSON(w.Adicional.Compressors.LZW.decompress(aux.d)));
                                }
                            } else {
                                param.options.ok($.parseJSON(w.Adicional.Compressors.LZW.decompress(param.current.response.d)), param);
                            }
                        }
                    } else {
                        if (param.options.err) { param.options.err(param); }
                    }
                }
            } finally {
                if (param.current.readyState == param.current.DONE){
                    param._flgIsBusy = false;
                    if (param.options.always) { param.options.always(param); }
                    console.log('[' + param.options.id + ']: ' + param.current.statusText);
                }
            }
        }
    }

    function getParams(params) {
        var result = {};
        for (var i in params) {
            result[i] = Adicional.Compressors.LZW.compress($.stringify(params[i]));
        }
        return result;
    }

    var _Thread = Class({
        initialize: function(a) {
            this.options = $.extend(true, {}, w.Adicional.Threading.Configs.DefaultOpc, a[0]);
            this.current = loadXMLDoc(this);
            //createEvents(this);
            this._flgIsBusy = false;
        },
        dispose: function() {
            if(this.current){
                this.current.abort();
                delete this.current;
            }
            if(this.options){
                for(var i in this.options){
                    delete this.options[i];
                }
            }
        },
        OnError: function(a) {
            a && (this.options.err = a)
        },
        OnSuccess: function(a) {
            a && (this.options.ok = a)
        },
        OnFinsh: function(a) {
            a && (this.options.always = a)
        },
        IsBusy: function() {
            return this._flgIsBusy;
        },
        /* readyState : [this.current.UNSENT | DONE | HEADERS_RECEIVED | LOADING | OPENED] */
        Start: function(params){
            if(this.IsBusy()){ console.info('[thread is busy]'); return; }
            this._flgIsBusy = true;
            this.options = $.extend(true, {}, this.options, params);
            var c = this.options;
            var okFn = this.options.ok;            

            if(this.current.readyState != this.current.OPENED) {
                var config = w.Adicional.Threading.ThreadConfig;
                createEvents(this);
                this.current.open(this.options.type, config.toRequest + this.options.dest, this.options.async);
                this.current.setRequestHeader('Content-type', config.contentType);
                if (this.current.overrideMimeType) { this.current.overrideMimeType(config.mimeType); }
                this.current.responseType = config.responseType;
            }
            try{this.current.send($.stringify(getParams(c.d)));}
            catch(err){
                console.info('readyState - ' + this.current.readyState);
                console.error('[' + this.options.id + ']: ' + err.code);
                console.debug(err);
            }
        }
    });
    w.Adicional = $.extend(true, w.Adicional,{
        'Threading' : {
            'WorkerThread': _Thread,
            'Configs': {
                'DefaultOpc' : {
                    'dest': '',
                    'd': {},
                    'ok': void 0,
                    'err': void 0,
                    'always': void 0,
                    'noIfy': void 0,
                    'async': !0,
                    'type': 'POST',
                    'id': 'thread'
                }
            },
            'ThreadConfig' : {
                'responseType' : 'json',
                'mimetype' : 'application/json',
                'contentType' : 'application/json; charset=utf-8',
                'toRequest' :  w.location.protocol + '//' + w.location.host
            }
        }
    });
})(window);

(function(w){
    w.Adicional = $.extend(true, w.Adicional, { 
        'Cookies' : {
            'setCookie': function(cname, cvalue, exdays) {
                var d = new Date();
                d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
                var expires = "expires=" + d.toUTCString();
                document.cookie = cname + "=" + cvalue + "; " + expires;
            },
            'getCookie': function(cname) {
                var name = cname + "=";
                var ca = document.cookie.split(';');
                for (var i = 0; i < ca.length; i++) {
                    var c = ca[i];
                    while (c.charAt(0) == ' ') c = c.substring(1);
                    if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
                }
                return "";
            },
            'deleteCookie': function(cname) {
                document.cookie = cname + '=;expires=Wed; 01 Jan 1970';
            }        
        }
    });

    w.Adicional.Cookies = {
        'setCookie': function(cname, cvalue, exdays) {
            var d = new Date();
            d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
            var expires = "expires=" + d.toUTCString();
            document.cookie = cname + "=" + cvalue + "; " + expires;
        },
        'getCookie': function(cname) {
            var name = cname + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') c = c.substring(1);
                if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
            }
            return "";
        },
        'deleteCookie': function(cname) {
            document.cookie = cname + '=;expires=Wed; 01 Jan 1970';
        }
    };
})(window);

(function(w){
    function createBase(opc){
        $_base = $('<div>').attr({ 'id' : 'msjDialog',
                                   'class':'modal fade',
                                   'tabindex' : '-1',
                                   'role': 'dialog',
                                   'aria-labelledby': 'msjModalLabel',
                                   'aria-hidden':'true'
                                 }).append($('<div>').attr({ 'class': 'modal-dialog modal-sm' })
                                                     .append($('<div>').attr({ 'class':'modal-content' })
                                                                       .append($('<div>').attr({ 'class' : 'modal-header' })
                                                                                         .append($('<button>').attr({ 
                                                                                                                    'type' : 'button',
                                                                                                                    'class' : 'close',
                                                                                                                    'data-dismiss':'modal'
                                                                                                                }).append($('<span>').attr({ 
                                                                                                                                            'aria-hidden': 'true'
                                                                                                                                        }).html('&times;'))
                                                                                                                  .append($('<span>').attr({ 'class' : 'sr-only' })
                                                                                                                                     .html('Cerrar')))
                                                                                         .append($('<h4>').attr({ 
                                                                                                            'class' : 'modal-title',
                                                                                                            'id' : 'modalLabel'
                                                                                                          }).html(opc.Title)))
                                                                       .append($('<div>').attr({ 'class' : 'modal-body' })
                                                                                         .append($('<span>').html(opc.Message)))
                                                                       .append($('<div>').attr({ 'class' : 'modal-footer' })
                                                                                         .append($('<button>').attr({
                                                                                                                'type':'button',
                                                                                                                'class': 'btn btn-default',
                                                                                                                'data-dismiss': 'modal'
                                                                                                               }).html('Aceptar')))));
        return $_base;
    }

    w.Adicional.Dialogs = {
        'Options' : {
            Title: '',
            Message: '',
            show: false,
            parent: $(this)
        }
    };

    var mbox = Class({
        initialize: function(a){
            this.opc= $.extend({}, w.Adicional.Dialogs.Options, a[0]);
            $(this.opc.parent).append(createBase(this.opc));
            this._mbox = $('#msjDialog');
            this.queued = [];
        },
        dispose:function(){},
        SetTitle: function(title){
            if(this._mbox){
                this._mbox.modal('hide');
                this.opc.Title = title;
                this._mbox.find('.modal-title').html(title);
            }
        },
        SetMessage: function(msj){
            if(this._mbox){
                this._mbox.modal('hide');
                this.opc.Message = msj;
                this._mbox.find('.modal-body span').html(msj)
            }
        },
        Show: function(opc){
            if(this._mbox && !this._mbox.hasClass('in')){
                if(opc){
                    this.opc = $.extend({}, this.opc, opc);
                }
                if(this._mbox){
                    this._mbox.modal('hide');
                    this.SetTitle(this.opc.Title);
                    this.SetMessage(this.opc.Message);
                }
                this._mbox.modal('show');
            }

            if(this._mbox.attr('aria-hidden') == 'false'){
                this._mbox.css({ 'display' : 'block' });
            }
        },
        Hidde: function(){
            if(this._mbox){
                this._mbox.modal('hide');
            }
        }
    });
    w.Adicional.MessageBox = mbox;
})(window);

Adicional.OnLoad = function(fn) {
    var floatInfoShow = 0;
    function loadAnimation() {
        var items = $('.alert-danger');
        if (items.length > 0 && floatInfoShow <= 0) {
            floatInfoShow++;
            items.each(function(current) {
                $.gritter.add({
                    title: '<h3>Alerta</h3>',
                    text: $(this).find('.lblValor').html() + ' esta en ' + $(this).find('.stat-percent').html(),
                    time: 4000
                });
            });
        }
        items.removeClass('flash animated')
             .addClass('flash animated')
             .one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function() {
                 $(this).removeClass('flash animated');
             });
    }
    $(function() {
        var nua = navigator.userAgent
        var isAndroid = (nua.indexOf('Mozilla/5.0') > -1 && nua.indexOf('Android ') > -1 && nua.indexOf('AppleWebKit') > -1 && nua.indexOf('Chrome') === -1)
        if (isAndroid) {
            $('select.form-control').removeClass('form-control').css('width', '100%')
        }
        $('.collapse-link').click(function() {
            var ibox = $(this).closest('div.ibox');
            var button = $(this).find('i');
            var content = ibox.find('div.ibox-content');
            content.slideToggle(200);
            button.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
            ibox.toggleClass('').toggleClass('border-bottom');
            setTimeout(function() {
                ibox.resize();
                ibox.find('[id^=map-]').resize();
            }, 50);
        });
        $('.badge').hide();
        window.setTimeout(function() {
            $('.badge').removeClass('bounceIn animated')
            		     .addClass('bounceIn animated')
           		     .one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function() {
           		         $(this).removeClass('bounceIn animated');
           		     })
                  .show();
        }, 1000);
        loadAnimation();
        window.setInterval(loadAnimation, 5000);
    });
    function disposeControles() {
        if (Adicional && Adicional.Controles) {
            for (var i in Adicional.Controles) {
                Adicional.Controles[i].dispose();
                delete Adicional.Controles[i];
                if (Adicional.Controles[i]) {
                    Adicional.Controles[i] = undefined;
                }
            }
        }
    }
    $(window).on('unload', function() {
        disposeControles();
    });
    $(document).off('keydown')
               .on('keydown', function(e) {
                    var code = (e.keyCode ? e.keyCode : e.which);
                    if ((code === 116) || ((e.ctlkey || e.shiftkey) && code === 116)) {
                        disposeControles();
                    }
               });
    function fix_height() {
        var h = $("#tray").height();
        $("#preview").attr("height", (($(window).height()) - h) + "px");
    }
    $(window).resize(function() { fix_height(); }).resize();
    $(document).ready(function() {
        try {
            $('form').attr({ 'accept-charset': 'UTF-8' });
            $body = $('body');/**
            window.Adicional.Dialog = new Adicional.MessageBox({
                Title :'Modal',
                Message: '-',
                parent: $('#msjContainer')
            });/**/
            $('[data-toggle="tooltip"]').tooltip();
            $('#side-menu').metisMenu();

            if (fn) {
                fn();
            }
            window.setInterval(loadAnimation, 5000);
        } finally {
            if(!$body.is(':visible')) { $body.show(); }
        }
    });
}