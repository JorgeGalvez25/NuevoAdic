$(document).ready(function() {
    $('#btnAcceder').on('click', function() {
        getAsync({
            dest: 'Login.aspx/' + links.login,
            d: { parameters: '' },
            ok: function() { },
            err: void 0,
            noIfy: void 0,
            always: void 0,
            async: !0
        });
    });
});