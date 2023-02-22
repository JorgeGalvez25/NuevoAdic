/* Cambio Flujo */
(function(w) {
    function crearControl(opc){
        var $div = $('<div>').attr({ 'id' : opc.id,
                                     'class' : 'widget gray-bg p-lg text-center',
                                     'data-std' : (opc.valor == 'Estandar') ? 'true' : 'false'
                                   })
                             .append($('<div>').attr({ 'class' : 'm-b-md' })
                                               .append($('<i>').attr({ 'class' : 'fa fa-4x ' + ((opc.valor == 'Inactivo') ? 'fa-hourglass' : ((opc.valor == 'Estandar') ? 'fa-arrow-circle-o-up' : 'fa-arrow-circle-o-down')) }))
                                               .append($('<h1>').attr({ 'class' : 'm-xs' }).html('Estatus') )
                                               .append($('<h3>').attr({ 'class' : 'font-bold no-margins' }).html((opc.valor == 'Inactivo') ? 'Consultando' : ((opc.valor == 'Estandar') ? 'Estandar' : 'M&iacute;nimo'))));
        return $div;
    }

    function loadOperation(thread) {
        if (thread && !thread.IsBusy()) {
            thread.Start();
        }
    }

    function configuraHilo(opc) {
        return new Adicional.Threading.WorkerThread({
            'id' : 'thrFlujo_' + opc.id,
            'dest': '/WebService.asmx/' + opc.Async.url,
            'd': { 'd': opc.Async.param },
            'ok': function(a, b, c) {
                if (opc.Async.Data) {
                    if(a.IsFaulted) {
                        console.log(a.ExceptionMessage);
                        window.Adicional.Dialog.setTitle('Error');
                        window.Adicional.Dialog.setMessage(a.Message);
                        window.Adicional.Dialog.setClosable(false);
                        window.Adicional.Dialog.setType(BootstrapDialog.TYPE_DANGER);
                        window.Adicional.Dialog.setSize(Adicional.Dialogs.GetSize(a.Message))
                        window.Adicional.Dialog.setButtons([{
                            id: 'btn-ok',   
                            label: 'Aceptar',
                            cssClass: 'btn btn-success', 
                            autospin: false,
                            action: function(dialogRef){    
                                if(a.Message == 'No cuenta con sesión activa' && a.Result) {
                                    window.location = a.Result;
                                }
                                dialogRef.close();
                            }
                        }]);
                        window.Adicional.Dialog.open();
                    } else {
                        if(opc.Async.Data) { opc.Async.Data(this, { 'result': a.Result, 'text': b, 'obj': c }); }
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
        $('a[name="itemMenu"]').off('click')
              .on('click', function(){
                current.opc.Async.Timer = 0;
                current._thread.Abort();
                clearInterval(this.IntervalId);
              });
        if (current._thread.IsEnabled() && current.opc.Async && current.opc.Async.Data && current.opc.Async.Timer > 0) {
            this.IntervalId = w.setInterval(function() { loadOperation(current._thread); }, current.opc.Async.Timer * 1000);

            $('#' + current.opc.id).on('click', function(e) {
                 if (current._thread && !current._thread.IsBusy()) {
                    var strMessage = '¿Desea aplicar el cambio de estatus?';
                    var flgFlujo = $(this).data('std');
                    BootstrapDialog.show({
                        type: BootstrapDialog.TYPE_WARNING,
                        size: Adicional.Dialogs.GetSize(strMessage),
                        title: 'Confirmación',
                        message: strMessage,
                        buttons: [{
                            label: 'Aceptar',
                            cssClass: 'btn-success',
                            action: function(dialogRef){
                                var _inner = configuraHilo(current.opc);
                                _inner.options.dest = '/WebService.asmx/SetEstatus';
                                _inner.options.d.d = $.extend(true, current._thread.options.d.d, { 'noEst' : current.opc.Async.param.noEst, 'flujo' : !flgFlujo });
                                _inner.options.ok = function(a, b, c) {
                                    if (current.opc.Async.Data) {
                                        if(a.IsFaulted){
                                            console.log(a.ExceptionMessage);
                                            window.Adicional.Dialog.setTitle('Error');
                                            window.Adicional.Dialog.setMessage(a.Message);
                                            window.Adicional.Dialog.setClosable(false);
                                            window.Adicional.Dialog.setType(BootstrapDialog.TYPE_DANGER);
                                            window.Adicional.Dialog.setSize(Adicional.Dialogs.GetSize(a.Message))
                                            window.Adicional.Dialog.setButtons([{
                                                id: 'btn-ok',   
                                                label: 'Aceptar',
                                                cssClass: 'btn btn-success', 
                                                autospin: false,
                                                action: function(dialogRef){    
                                                    dialogRef.close();
                                                }
                                            }]);
                                            window.Adicional.Dialog.open();
                                        } else {
                                            current._trdUpdate.options.err = function(a) {
                                                dialogRef.close();
                                                $('#warnSinConexion').show();
                                            };
                                            current._trdUpdate.options.ok = function(){
                                                if(current.opc.Async.Data) { current.opc.Async.Data(this, { 'result': a.Result, 'text': b, 'obj': c }); }
                                            };
                                            current._trdUpdate.options.always = function(a) {
                                                dialogRef.close();
                                            };
                                            current._trdUpdate.Start();
                                        }
                                    }
                                };
                                _inner.options.err = function(a) { 
                                    dialogRef.close();
                                    $('#warnSinConexion').show();
                                };
                                _inner.Start();
                            }
                        }, {
                            label: 'Cancelar',
                            cssClass: 'btn-link',
                            action: function(dialogRef){
                                dialogRef.close();
                            }
                        }]
                    });
                }
                e.preventDefault();
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
        'initialize': function(a) {
            this.opc = $.extend(true, w.Adicional.Flujos.Configs.DefaultGlobalOpc, a[0]);
        },
        'Create': function() {
            this.Recreate();
            if (!this._thread) {
                this._thread = configuraHilo(this.opc);
            }
            if (!this._trdUpdate) {
                this._trdUpdate = configuraHilo(this.opc);
            }
            this._timer = createEvents(this);
            loadOperation(this._thread);
        },
        'Update': function(async, err, always) {
            this._trdUpdate.options.async = (typeof(async) != 'undefined') ? async : true;
            this._trdUpdate.options.err = err;
            this._trdUpdate.options.always = always;
            loadOperation(this._trdUpdate);
        },
        'Recreate': function(){
           $(this.opc.parent).empty().append(crearControl(this.opc));
        },
        'dispose': function() {
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
        'Modificar': function(opcs) {
            this.opc = $.extend(true, this.opc, opcs);
            //this.Recreate();
            var $self = $('#' + this.opc.id);
            if(this.opc.valor == 'Estandar') {
                $self.data('std', true);
                if ($self.is('.red-bg') || $self.is('.gray-bg')){
                   $self.removeClass('gray-bg')
                        .removeClass('red-bg')
                        .addClass('navy-bg');
                }

                $self.find('i').each(function() {
                    if ($(this).is('.fa-arrow-circle-o-down') || $(this).is('.fa-hourglass')){
                        $(this).removeClass('fa-hourglass')
                               .removeClass('fa-arrow-circle-o-down')
                               .addClass('fa-arrow-circle-o-up')
                               .parent().find('h3').html('Estandar');
                    }
                });
            } else {
                $self.data('std', false);
                if ($self.is('.navy-bg') || $self.is('.gray-bg')){
                   $self.removeClass('gray-bg')
                        .removeClass('navy-bg')
                        .addClass('red-bg');
                }

                $self.find('i').each(function() {
                    if ($(this).is('.fa-arrow-circle-o-up') || $(this).is('.fa-hourglass')){
                        $(this).removeClass('fa-hourglass')
                               .removeClass('fa-arrow-circle-o-up')
                               .addClass('fa-arrow-circle-o-down')
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
})(window);