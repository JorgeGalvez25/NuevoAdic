var links = {
    login: 'Validate'
}

function getAsync(options) {
    var opc = $.extend({
        dest: '',
        d: {},
        ok: void 0,
        err: void 0,
        noIfy: void 0,
        always: void 0,
        async: !0
    }, options);
    var thread = new Thread(opc);
    thread.OnError(function(xhr, t, m) {
        if (m.toLowerCase() != 'abort') {
            if (options.err) { options.err(xhr, t, m); }
            logFailure(this, xhr, t, m);
        }
    });
    thread.OnSuccess(function(a) {
        console.log(a);
        alert(a);
    });
    thread.OnFinsh(function() {
        $('body').css({ 'cursor': 'default' });
        if (options.always) { options.always(); }
    });
    $('body').css({ 'cursor': 'progress' });
    thread.Start();
}