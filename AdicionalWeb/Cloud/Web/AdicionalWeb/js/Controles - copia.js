(function(w) {
    function crearControl(opc){
        var $div = $('<div>').attr({ 'id' : opc.id,
                                     'class' : 'widget red-bg p-lg text-center',
                                     'data-std' : (opc.valor == 'Estandar') ? 'true' : 'false'
                                   })
                             .append($('<div>').attr({ 'class' : 'm-b-md' })
                                               .append($('<i>').attr({ 'class' : 'fa fa-4x ' + ((opc.valor == 'Estandar') ? 'fa-times-circle-o' : 'fa-check-circle-o') }))
                                               .append($('<h1>').attr({ 'class' : 'm-xs' }).html('Estatus') )
                                               .append($('<h3>').attr({ 'class' : 'font-bold no-margins' }).html(((opc.valor == 'Estandar') ? 'Estandar' : 'M&iacute;nimo'))));
        return $div;
    }

    function loadOperation(thread) {
        if (thread && !thread.IsBusy()) {
            thread.Start();
        }
    }

    function configuraHilo(opc) {
        return new Adicional.Threading.WorkerThread({
            'dest': opc.Async.url,
            'd': { 'd': opc.Async.param },
            'ok': function(a, b, c) {
                if (opc.Async.Data) {
                    if(a.IsFaulted){
                        console.log(a.ExceptionMessage);
                        Adicional.Dialog.Show({ 'Title' : 'Error', 'Message': a.Message });
                    } else {
                        opc.Async.Data(this, { 'result': a.Result, 'text': b, 'obj': c });
                    }
                }
            },
            'err': function(a) {
                $('#warnSinConexion').show();
            },
            'async': true
        });
    }

    function createEvents(current) {
        if (current.opc.Async && current.opc.Async.Data && current.opc.Async.Timer > 0) {
            w.setInterval(function() { loadOperation(current._thread); }, current.opc.Async.Timer * 1000);

            $('#' + current.opc.id).on('click', function() {
                 if (current._threader && !current._threader.IsBusy()) {
                    current._threader.options.dest = '/WebService.asmx/SetEstatus';
                    current._threader.options.d.d = $.extend(true, current._threader.options.d.d, { 'flujo' : !$(this).data('std') });
                    current._threader.Start();
                }
            });
        }
    }

    w.Adicional = $.extend(true, {}, w.Adicional, {
        'Flujos':{
            'Configs':{
                'DefaultGlobalOpc' : {
                    'id': '',
                    'parent': $(this),
                    'valor': '',
                    'Async': {
                        'url': '',
                        'param': '',
                        'Data': void 0,
                        'Timer': 0
                    }
                }
            }
        }
    });

    var Panel = Class({
        initialize: function(a) {
            this.opc = $.extend({}, w.Adicional.Flujos.Configs.DefaultGlobalOpc);
            $.extend(this.opc, a[0]);
        },
        Create: function() {
            this.Recreate();
            if (!this._thread) {
                this._thread = configuraHilo(this.opc);
            }
            if(!this._threader){
                this._threader = configuraHilo(this.opc);
            }
            this._timer = createEvents(this);
            loadOperation(this._thread);
        },
        Recreate: function(){
           $(this.opc.parent).empty().append(crearControl(this.opc));
        },
        dispose: function() {
            if (this.opc) {
                for (var i in this.opc) {
                    delete this.opc[i];
                };

                if(this._timer){
                    clearInterval(this._timer);
                    this._thread.dispose();
                }

                this.opc = undefined;
            }
        },
        Modificar: function(opcs) {
            this.opc = $.extend(this.opc, opcs);
            //this.Recreate();
            var $self = $('#'+ this.opc.id);
            if(this.opc.valor == 'Estandar') {
                $('#' + this.opc.id).attr({ 'data-std' : 'true' });
                if ($self.is('.red-bg')){
                   $self.removeClass('red-bg')
                        .addClass('navy-bg');
                }

                $self.find('i').each(function() {
                    if ($(this).is('.fa-check-circle-o')){
                        $(this).removeClass('fa-check-circle-o')
                               .addClass('fa-times-circle-o')
                               .parent().find('h3').html('Estandar');
                    }
                });
            } else {
                $('#' + this.opc.id).attr({ 'data-std' : 'false' });
                if ($self.is('.navy-bg')){
                   $self.removeClass('navy-bg')
                        .addClass('red-bg');
                }

                $self.find('i').each(function() {
                    if ($(this).is('.fa-times-circle-o')){
                        $(this).removeClass('fa-times-circle-o')
                               .addClass('fa-check-circle-o')
                               .parent().find('h3').html('M&iacute;nimo');
                    }
                })
            }

            if ($(this.opc.Parent).is(':visible')) { $('#pgLoader').find('[data-dismiss="alert"]').each(function() { $(this).click(); }); }
        }
    });
    w.Adicional = $.extend(true, {}, w.Adicional, {
        'Flujos': {
            'Flujo' : Panel
        }
    });
})(window)