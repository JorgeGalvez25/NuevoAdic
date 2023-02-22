/* Mangueras */
(function(w) {
    "use strict";

    function getColorGas(data){
        var result = 'label-default';
        if (data.Combustible.match(/^premium/gi) != null) {
            result = 'label-danger';
        } else if (data.Combustible.match(/^magna/gi) != null) {
            result = 'label-primary';
        } else if (data.Combustible.match(/^diesel/gi) != null) {
            result = 'label-warning';
        }
        return result;
    }

    function getColorCombustibleTipo(pTipo, tDefault){
        var result = tDefault;
        switch(pTipo)
        {
            case 3:
            case 4:
                result = 'btn-success';
                break;
            default:
                result = tDefault;
                break;
        }
        return result;
    }

    function getColorCombustible(data, pTipo){
        var result = 'btn-default';
        switch(data){
            case 1:
                result = getColorCombustibleTipo(pTipo, 'btn-primary');
                break;
            case 2:
                result = getColorCombustibleTipo(pTipo, 'btn-danger');
                break;
            case 3:
                result = 'btn-warning';
                break;
        }
        return result;
    }

    function loadOperation(thread) {
        if (thread && !thread.IsBusy()) {
            thread.Start();
        }
    }

    function configuraHilo(opc) {
        return new Adicional.Threading.WorkerThread({
        	'id' : 'thrDispensario_' + opc.id,
            'dest': '/WebService.asmx/' +  opc.Async.url,
            'd': { 'd': opc.Async.param },
            'ok': function(a, b, c) {
                $('#updating_' + w.format(opc.posicion, '00')).hide();
                if (opc.Async.Data) {
                    if(a.IsFaulted) {
                        console.log(a.ExceptionMessage);
                        $('#icon_' + opc.id).show();
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
		                    action: function(dialogRef) {
		                    	if(a.Message == 'No cuenta con sesión activa' && a.Result) {
		                    		window.location = a.Result;
                				}
		                        dialogRef.close();
		                    }
		                }]);
				        window.Adicional.Dialog.open();
                    } else {
                        opc.Async.Data(this, { 'result': a.Result, 'text': b, 'obj': c });
                    }
                }
            },
            'err': function(a) {
                $('#warnSinConexion').show();
            },
            'always':function(a,b,c){
                $('#updating_' + w.format(opc.posicion, '00')).hide();
            },
            'async': true
        });
    }

    function createEvents(current) {
        if (current.opc.Async && current.opc.Async.Data && current.opc.Async.Timer > 0) {
            w.setInterval(function() {
                $('#updating_' + w.format(current.opc.posicion, '00')).show();
                loadOperation(current._thread);
            }, current.opc.Async.Timer * 1000);
        }
    }

    function crearControl(opc){
        var cols = (opc.Data.length * 2) + 1;
        var strPosicion = '';
        switch(opc.tipo) {
            case 1:
            case 2:
            case 4:
                strPosicion = 'Posici&oacute;n';
                break;
            case 3:
                strPosicion = 'Dispensario';
                break;
            case 5:
                strPosicion = 'CPU';
                break;
        }

        var $col = $('<div>').attr({ 'class' : 'col-xs-12 col-sm-12 col-md-' + cols + ' col-lg-' + cols })
                             .append($('<div>').attr({ 'id' : 'posicion_' + opc.id, 
                                                       'class' : 'panel panel-default box-shadow'
                                               })
                                               .append($('<div>').attr({ 'class' : 'panel-heading' })
                                                                 .append($('<h1>').attr({ 'class': 'panel-title' })
                                                             				      .append($('<i>').attr({ 'id' : 'icon_' + opc.id, 'class': 'fa fa-exclamation-triangle' }).hide())
                                                                                  .append($('<strong>').html(strPosicion + '&nbsp;'))
                                                                                  .append($('<span>').attr({ 'id' :'lblPosicion', 'class' : 'label label-success' })
                                                                                                     .html(w.format(opc.posicion,'00')))
                                                                                  .append($('<span>').html('&nbsp;'))
                                                                                  .append($('<i>').attr({ 'id': 'updating_' + w.format(opc.posicion, '00'), 
                                                                                                          'class' : 'fa fa-refresh' }))))
                                               .append($('<div>').attr({ 'class' : 'panel-body' })
                                                                 .append(getMangueras(opc.Data, strPosicion, opc.tipo))));
        return $col;
    }

    function getMangueras(data, pTitle, pTipo){
        var $div = $('<div>').attr({ 'class' : 'row' });
        var num = parseInt(12 / data.length);
        for(var i = 0; i < data.length; i++){
            $div.append($('<div>').attr({ 'class' : 'col-xs-' + num + ' col-sm-' + num + ' col-md-' + num + ' col-lg-' + num,
                                          'id' : 'manguera_' + data[i].nombre
                                  })
                                  .append($('<h3>').attr({ 'class' : 'no-margins text-center' })
                                  				   .append($('<span id="value">').html(getFormatNumericSmall(data[i].valor)))
                                                   .append($('<br/>')))
                                  .append($('<div>').addClass('text-center').html(data[i].showTitleId ? 'Manguera ' + w.format(data[i].id, '00') : '&nbsp;'))
                                  .append($('<button>').attr({
                                                   'class' : 'btn ' + getColorCombustible(data[i].combustible, pTipo) + ' btn-block',//btn-success btn-block',
                                                   'type' : 'button',
                                                   'data-toggle' : 'modal',
                                                   'data-target' : '#dlgModal',
                                                   'data-titulo' : data[i].valor,
                                                   'data-estacion' : data[i].estacion,
                                                   'data-combustible' : data[i].combustible,
                                                   'data-manguera' : data[i].id,
                                                   'data-posicion' : pTitle + ' ' + data[i].posicion
                                            	}).html(data[i].nombre + '&nbsp;<small><i class="fa fa-pencil-square-o"></i></small>')));
        }
        return $div;
    }

    function getFormatNumericSmall(num) {
        var iX = parseInt(num); // Enteros
        var dY = num - iX; // Decimales
        return iX.toString() + '<small>' + format(dY, '#.00') + '%&nbsp;</small>';
    }

    w.Adicional.Dispensarios = {
        'Configs' : {
            'DefaultOpc' : {
                'id': 0,
                'posicion': 0,
                'parent': $(this),
                'tipo' : 0,
                'Data': [{
                    'id': 0,
                    'estacion': 0,
                    'dispensario': 0,
                    'posicion' : 0,
                    'nombre': '',
                    'valor': 0,
                    'showTitleId' : false
                }],
                'Async': {
                    'url': '',
                    'param': '',
                    'Data': void 0,
                    'Timer': 0
                }
            },
            'Dispensario': {}
        }
    };

    var Panel = Class({
        initialize: function(a) {
            this.opc = $.extend(true, {}, w.Adicional.Dispensarios.Configs.DefaultOpc, a[0]);
        },
        Create: function() {
            this.Recreate();
            if (!this._thread) {
                this._thread = configuraHilo(this.opc);
            }
            this._timer = createEvents(this);
            loadOperation(this._thread);
        },
        Recreate: function(){
            $(this.opc.parent).hide().empty().append(crearControl(this.opc)).show();
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
        'Editar' : function(value){
        	var hilo = new Adicional.Threading.WorkerThread({
	            'dest': '/WebService.asmx/' +  opc.Async.url,
	            'd': { 'd': value },
	            'ok': function(a, b, c) {
                    $('#updating_' + w.format(opc.posicion, '00')).hide();
	                if (opc.Async.Data) {
	                    if(a.IsFaulted){
	                        console.log(a.ExceptionMessage);
	                        $('#icon_' + opc.id).show();
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
	                    }
	                }
	            },
	            'err': function(a) {
	                $('#warnSinConexion').show();
	                var msj = 'Probablemente no se haya podido aplicar el cambio. Verifique que se aplicó correctamente.';
	                window.Adicional.Dialog.setTitle('Advertencia');
		        	window.Adicional.Dialog.setMessage();
		        	window.Adicional.Dialog.setClosable(false);
		        	window.Adicional.Dialog.setType(BootstrapDialog.TYPE_WARNING);
		        	window.Adicional.Dialog.setSize(Adicional.Dialogs.GetSize(msj))
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
	            },
                'always':function(a,b,c){
                    $('#updating_' + w.format(opc.posicion, '00')).hide();
                },
	            'async': true
	        });
	        hilo.Start();
        },
        'Modificar': function(opcs) {
            this.opc.Data = opcs;
            this.Recreate();
            if ($(this.opc.parent).is(':visible')) { $('#pgLoader').find('[data-dismiss="alert"]').each(function() { $(this).click(); }); }
        }
    });
    w.Adicional.Dispensarios.Dispensario = Panel;
})(window);

/* Porcentaje Global */
(function(w){
    function crearControl(opc){
        var $col = $('<div>').attr({ 'class': 'col-xs-12 col-sm-4 col-md-3 col-lg-3' })
                             .append($('<div>').attr({ 'class' : 'panel panel-default box-shadow' })
                             				   .append($('<div>').attr({ 'class' :'panel-heading' })
                             				   					 .append($('<strong>').attr({ 'class' : 'panel-title' })
                             				   					 				      .html('Porcentaje Global'))
                                                                 .append($('<span>').html('&nbsp;'))
                                                                 .append($('<i>').attr({ 'id': 'updating_global', 
                                                                                         'class' : 'fa fa-refresh' }))
                                                                  .append($('</br>'))
                                                                  .append($('<small>').html('Permite cambiar todos los porcentajes a la vez.')))
                                               .append($('<div>').attr({ 'class' : 'panel-body' })
                                                                 .append($('<button>').attr({ 'class' : 'btn btn-success',
                                                                							  'type' : 'button',
                                                                                              'data-toggle' : 'modal',
                                                                                              'data-target' : '#dlgModal',
                                                                                              'data-titulo' : '0',
                                                                                              'data-estacion' : opc.estacion,
                                                                                              'data-posicion':'Porcentaje Global' })
                                                                					  .html('Cambiar...'))));
        return $col;
    }

    function getFormatNumericSmall(num) {
        var iX = parseInt(num); // Enteros
        var dY = num - iX; // Decimales
        return iX.toString() + '<small>' + format(dY, '#.00') + '%&nbsp;</small>';
    }

    function loadOperation(thread) {
        if (thread && !thread.IsBusy()) {
            thread.Start();
        }
    }

    function configuraHilo(opc) {
        return new Adicional.Threading.WorkerThread({
        	'id' : 'thrGlobal_' + opc.id,
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
		                    'id': 'btn-ok',   
		                    'label': 'Aceptar',
		                    'cssClass': 'btn btn-success', 
		                    'autospin': false,
		                    'action': function(dialogRef){
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
            'always': function(a) {
                $('#updating_global').hide();
            },
            'async': true
        });
    }

    function createEvents(current) {
        if (current.opc.Async && current.opc.Async.Data && current.opc.Async.Timer > 0) {
            w.setInterval(function() {
                $('#updating_global').show();
                loadOperation(current._thread);
            }, current.opc.Async.Timer * 1000);
        }
    }

    w.Adicional.Dispensarios.Configs = $.extend(true, w.Adicional.Dispensarios.Configs, {
        'DefaultGlobalOpc' : {
            'id': '',
            'parent': $(this),
            'valor': 0,
            'estacion': 0,
            'Async': {
                'url': '',
                'param': '',
                'Data': void 0,
                'Timer': 0
            }
        }
    });

    var Panel = Class({
        'initialize': function(a) {
            this.opc = $.extend(true, {}, w.Adicional.Dispensarios.Configs.DefaultGlobalOpc, a[0]);
        },
        'Create': function() {
            this.Recreate();
            if (this.opc.Async && this.opc.Async.Data && this.opc.Async.Timer > 0) {
	            if (!this._thread) {
	                this._thread = configuraHilo(this.opc);
	            }
	            this._timer = createEvents(this);
	            loadOperation(this._thread);
	        }
            $('#updating_global').hide();
        },
        'Recreate': function(){
           $(this.opc.parent).empty().append(crearControl(this.opc));
        },
        'dispose': function() {
            if (this.opc) {
                for (var i in this.opc) {
                    delete this.opc[i];
                }

                if(this._timer){
                    clearInterval(this._timer);
                    this._thread.dispose();
                }

                this.opc = undefined;
            }
        },
        'Modificar': function(opcs) {
            this.opc = $.extend(true, this.opc, opcs);
            this.Recreate();
            if ($(this.opc.Parent).is(':visible')) { $('#pgLoader').find('[data-dismiss="alert"]').each(function() { $(this).click(); }); }
        }
    });
    w.Adicional.Dispensarios.Global = Panel;
})(window);

(function(w) {
	function crearHilo(opc){
		return new Adicional.Threading.WorkerThread({
            'dest': '/WebService.asmx/' + opc.url,
            'd': { 'd': opc.param },
            'async': opc.async,
            'ok': function(a, b, c) {
                if(a.IsFaulted){
                	if(a.Message == 'No cuenta con sesión activa' && a.Result) {
                		window.location = a.Result;
                	} else {
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
				    }
                } else {
                    if(opc.Data) { opc.Data(this, { 'result': a.Result, 'text': b, 'obj': c }); }
                }
            },
            'err': function(a) {
                $('#warnSinConexion').show();
            },
            'async': true
        });
	}

	Adicional = $.extend(true, Adicional, {
		'setGlobal': function(valor, fn){
			var params = {
				'url' : 'SetGlobal',
				'param' : valor,
				'Data' : fn,
				'async': false
			};

			var hilo = crearHilo(params);
			hilo.Start();
		},
		'setPorcentaje': function(valor, fn) {
			var params = {
				'url' : 'SetPorcentaje',
				'param' : valor,
				'Data' : fn,
				'async': false
			};

			var hilo = crearHilo(params);
			hilo.Start();
		}
	});
})(window);

/* Cambio Flujo */
(function(w) {
    function crearControl(opc){
        var $div = $('<div>').attr({ 'id' : opc.id,
                                     'class' : 'widget red-bg p-lg text-center',
                                     'data-std' : (opc.valor == 'Estandar') ? 'true' : 'false'
                                   })
                             .append($('<div>').attr({ 'class' : 'm-b-md' })
                                               .append($('<i>').attr({ 'class' : 'fa fa-4x ' + ((opc.valor == 'Estandar') ? 'fa fa-arrow-circle-o-up' : 'fa-arrow-circle-o-down') }))
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
        if (current.opc.Async && current.opc.Async.Data && current.opc.Async.Timer > 0) {
            w.setInterval(function() { loadOperation(current._thread); }, current.opc.Async.Timer * 1000);

            $('#' + current.opc.id).on('click', function(e) {
                 if (current._threader && !current._threader.IsBusy()) {
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
                                current._threader.options.dest = '/WebService.asmx/SetEstatus';
                                current._threader.options.d.d = $.extend(true, current._threader.options.d.d, { 'flujo' : !flgFlujo });
                                current._threader.options.ok = function(a, b, c) {
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
            		                        if(current.opc.Async.Data) { current.opc.Async.Data(this, { 'result': a.Result, 'text': b, 'obj': c }); }
            		                    }
            		                }
            		            };
            		            current._threader.options.err = function(a) { $('#warnSinConexion').show(); };
                                current._threader.Start();
                                dialogRef.close();
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
            if(!this._threader){
                this._threader = configuraHilo(this.opc);
            }
            this._timer = createEvents(this);
            loadOperation(this._thread);
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
                if ($self.is('.red-bg')){
                   $self.removeClass('red-bg')
                        .addClass('navy-bg');
                }

                $self.find('i').each(function() {
                    if ($(this).is('.fa-arrow-circle-o-down')){
                        $(this).removeClass('fa-arrow-circle-o-down')
                               .addClass('fa-arrow-circle-o-up')
                               .parent().find('h3').html('Estandar');
                    }
                });
            } else {
                $self.data('std', false);
                if ($self.is('.navy-bg')){
                   $self.removeClass('navy-bg')
                        .addClass('red-bg');
                }

                $self.find('i').each(function() {
                    if ($(this).is('.fa-arrow-circle-o-up')){
                        $(this).removeClass('fa-arrow-circle-o-up')
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