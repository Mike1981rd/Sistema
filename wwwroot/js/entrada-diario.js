/**
 * EntradaDiario JS
 * Maneja la funcionalidad del formulario de entrada de diario
 */

$(document).ready(function() {
    console.log("Inicializando módulo de Entrada de Diario");
    
    // Inicializar Select2 para cuentas contables
    initCuentasContablesSelect2();
    
    // Inicializar Select2 para contactos
    initContactosSelect2();
    
    // Formatear campos de débito y crédito
    initMoneyInputs();
    
    // Evento para tipo de entrada
    $("#tipoEntradaSelect").change(function() {
        var tipoId = $(this).val();
        if (tipoId) {
            cargarNumeraciones(tipoId);
        } else {
            // Limpiar select de numeraciones
            $("#numeracionSelect").empty().append('<option value="">-- Seleccionar tipo primero --</option>');
            // Limpiar previsualización de código
            $("#codigoEntrada").val('');
        }
    });
    
    // Evento para numeración
    $("#numeracionSelect").change(function() {
        var numeracionId = $(this).val();
        if (numeracionId) {
            previsualizarCodigo(numeracionId);
        } else {
            $("#codigoEntrada").val('');
        }
    });
    
    // Botón para agregar nueva fila
    $("#btnAgregarLinea").click(function() {
        agregarNuevaFila();
    });
    
    // Evento para eliminar fila
    $(document).on('click', '.btn-remove-row', function() {
        $(this).closest('tr').remove();
        renumerarFilas();
        calcularTotales();
    });
    
    // Calcular totales al cambiar montos
    $(document).on('change keyup', '.monto-debito, .monto-credito', function() {
        calcularTotales();
    });
    
    // Inicializar tooltips
    $('[data-bs-toggle="tooltip"]').tooltip();
    
    // Calcular los totales iniciales
    calcularTotales();
    
    // Validación del formulario
    $("#formEntradaDiario").submit(function(e) {
        // Actualizar todos los campos ocultos antes de enviar
        $('.monto-debito, .monto-credito').each(function() {
            actualizarCampoOculto(this);
        });
        
        // Verificar balance
        var totalDebito = calcularTotalDebito();
        var totalCredito = calcularTotalCredito();
        
        console.log("Verificando balances antes de enviar:");
        console.log(`Total Débito: ${totalDebito}`);
        console.log(`Total Crédito: ${totalCredito}`);
        console.log(`Diferencia: ${Math.abs(totalDebito - totalCredito)}`);
        
        // Verificar que al menos una línea tenga valores
        var hayValores = false;
        $('.monto-debito, .monto-credito').each(function() {
            const valor = parseFloat($(this).val().replace(/,/g, '')) || 0;
            if (valor > 0) {
                hayValores = true;
            }
        });
        
        if (!hayValores) {
            alert("Debe ingresar al menos un valor en Débito o Crédito");
            e.preventDefault();
            return false;
        }
        
        if (Math.abs(totalDebito - totalCredito) > 0.001) {
            alert("El total de débitos debe ser igual al total de créditos");
            e.preventDefault();
            return false;
        }
        
        return true;
    });
    
    // Configuración de modales de numeraciones y tipos
    $("#btnGuardarTipo").click(function() {
        var formData = $("#formNuevoTipo").serialize();
        
        $.ajax({
            url: '/TipoEntradaDiario/Create',
            type: 'POST',
            data: formData,
            success: function(result) {
                $("#modalNuevoTipo").modal('hide');
                
                // Agregar nuevo tipo al select
                $("#tipoEntradaSelect").append(new Option(result.nombre, result.id, true, true)).trigger('change');
                
                // Actualizar select en modal de numeración
                $("#tipoEntradaIdNumeracion").append(new Option(result.nombre, result.id));
                
                // Limpiar form
                $("#formNuevoTipo")[0].reset();
            },
            error: function(error) {
                alert("Error al guardar el tipo de entrada: " + error.responseText);
            }
        });
    });
    
    $("#btnGuardarNumeracion").click(function() {
        var formData = $("#formNuevaNumeracion").serialize();
        
        $.ajax({
            url: '/NumeracionEntradaDiario/Create',
            type: 'POST',
            data: formData,
            success: function(result) {
                $("#modalNuevaNumeracion").modal('hide');
                
                // Agregar nueva numeración al select
                $("#numeracionSelect").append(new Option(result.nombre, result.id, true, true)).trigger('change');
                
                // Limpiar form
                $("#formNuevaNumeracion")[0].reset();
            },
            error: function(error) {
                alert("Error al guardar la numeración: " + error.responseText);
            }
        });
    });
    
    // Cargar tipos de entrada en modal de numeración
    $("#modalNuevaNumeracion").on('show.bs.modal', function () {
        var options = $("#tipoEntradaSelect option").clone();
        $("#tipoEntradaIdNumeracion").empty().append(options);
    });
});

/**
 * Inicializa los Select2 para cuentas contables
 */
function initCuentasContablesSelect2() {
    $('.select-cuenta').select2({
        theme: 'bootstrap-5',
        placeholder: "Buscar cuenta contable...",
        allowClear: true,
        ajax: {
            url: '/EntradaDiario/BuscarCuentasContables',
            dataType: 'json',
            delay: 250,
            data: function(params) {
                console.log("Enviando búsqueda de cuentas:", params.term);
                return {
                    term: params.term || ''
                };
            },
            processResults: function(data) {
                console.log("Respuesta cuentas contables (tipo):", typeof data);
                console.log("Respuesta cuentas contables:", data);
                console.log("Propiedades:", Object.keys(data));
                
                if (data.results) {
                    console.log("Número de resultados:", data.results.length);
                    if (data.results.length > 0) {
                        console.log("Primer resultado:", data.results[0]);
                    }
                } else {
                    console.error("No se encontró propiedad 'results' en la respuesta");
                }
                
                // SOLUCIÓN: Usar data directamente si ya viene con formato correcto
                return data;
            },
            cache: true
        },
        minimumInputLength: 1
    });
}

/**
 * Inicializa los Select2 para contactos
 */
function initContactosSelect2() {
    $('.select-contacto').select2({
        theme: 'bootstrap-5',
        placeholder: "Buscar contacto...",
        allowClear: true,
        ajax: {
            url: '/EntradaDiario/BuscarContactos',
            dataType: 'json',
            delay: 250,
            data: function(params) {
                console.log("Enviando búsqueda de contactos:", params.term);
                return {
                    term: params.term || ''
                };
            },
            processResults: function(data) {
                console.log("Respuesta contactos (tipo):", typeof data);
                console.log("Respuesta contactos:", data);
                console.log("Propiedades:", Object.keys(data));
                
                if (data.results) {
                    console.log("Número de contactos:", data.results.length);
                    if (data.results.length > 0) {
                        console.log("Primer contacto:", data.results[0]);
                    }
                } else {
                    console.error("No se encontró propiedad 'results' en la respuesta");
                }
                
                // SOLUCIÓN: Usar data directamente si ya viene con formato correcto
                return data;
            },
            cache: true
        },
        minimumInputLength: 1,
        templateResult: formatContacto,
        templateSelection: formatContactoSelection
    });
    
    // Eventos para manejar la selección de contactos
    $('.select-contacto').on('select2:select', function(e) {
        var data = e.params.data;
        var tipoContactoInput = $(this).closest('tr').find('input[name$=".TipoContacto"]');
        tipoContactoInput.val(data.tipo);
    });
    
    $('.select-contacto').on('select2:clear', function() {
        var tipoContactoInput = $(this).closest('tr').find('input[name$=".TipoContacto"]');
        tipoContactoInput.val('');
    });
}

/**
 * Formateo de contactos en el dropdown
 */
function formatContacto(contacto) {
    if (!contacto.id) {
        return contacto.text;
    }
    
    var tipoLabel = '';
    var tipoClass = '';
    
    if (contacto.tipo === 'C') {
        tipoLabel = 'C';
        tipoClass = 'bg-danger';
    } else if (contacto.tipo === 'P') {
        tipoLabel = 'P';
        tipoClass = 'bg-primary';
    }
    
    var $container = $(
        '<div class="d-flex align-items-center">' +
            '<span class="' + tipoClass + ' text-white rounded-1 me-2 p-1 d-inline-block" style="width: 18px; height: 18px; text-align: center; font-size: 0.7em; line-height: 1.1;">' + tipoLabel + '</span>' +
            '<span>' + contacto.text + '</span>' +
        '</div>'
    );
    
    return $container;
}

/**
 * Formateo de contactos seleccionados
 */
function formatContactoSelection(contacto) {
    if (!contacto.id) {
        return contacto.text;
    }
    
    var tipoLabel = '';
    var tipoClass = '';
    
    if (contacto.tipo === 'C') {
        tipoLabel = 'C';
        tipoClass = 'bg-danger';
    } else if (contacto.tipo === 'P') {
        tipoLabel = 'P';
        tipoClass = 'bg-primary';
    }
    
    var $container = $(
        '<div class="d-flex align-items-center">' +
            '<span class="' + tipoClass + ' text-white rounded-1 me-1 p-1 d-inline-block" style="width: 16px; height: 16px; text-align: center; font-size: 0.7em; line-height: 1;">' + tipoLabel + '</span>' +
            '<span>' + contacto.text + '</span>' +
        '</div>'
    );
    
    return $container;
}

/**
 * Función para cargar numeraciones según el tipo seleccionado
 */
function cargarNumeraciones(tipoId) {
    $.ajax({
        url: '/EntradaDiario/GetNumeraciones',
        type: 'GET',
        data: { tipoEntradaId: tipoId },
        success: function(data) {
            var select = $("#numeracionSelect");
            select.empty();
            select.append('<option value="">-- Seleccionar --</option>');
            
            $.each(data, function(index, item) {
                select.append('<option value="' + item.id + '">' + item.nombre + '</option>');
            });
        },
        error: function() {
            alert("Error al cargar las numeraciones.");
        }
    });
}

/**
 * Inicializa los formateadores de campos monetarios usando Cleave.js
 */
function initMoneyInputs() {
    // Obtener el separador decimal de la configuración global
    const separadorDecimal = '.'; // Forzar punto como separador decimal para ingreso
    const precisionDecimal = window.appConfig?.precisionDecimal || 2;
    
    console.log(`Inicializando campos monetarios con separador decimal '${separadorDecimal}' y precisión ${precisionDecimal}`);
    
    // Configuración de formato de moneda según la configuración de la empresa
    $('.monto-debito, .monto-credito').each(function() {
        // Quitar el valor inicial
        $(this).val('');
        
        // Determinar el delimitador de miles
        const delimiter = ','; // Usar coma como separador de miles para formato americano
        
        // Crear instancia de Cleave.js con configuración dinámica
        new Cleave(this, {
            numeral: true,
            numeralThousandsGroupStyle: 'thousand',
            numeralDecimalMark: separadorDecimal,
            numeralDecimalScale: precisionDecimal,
            delimiter: delimiter,
            numeralPositiveOnly: true,
            // Actualizar el campo oculto con el valor para el modelo
            onValueChanged: function(e) {
                actualizarCampoOculto(e.target);
            }
        });
    });
    
    // Agregar eventos change directos para asegurar la actualización
    $('.monto-debito, .monto-credito').on('blur', function() {
        actualizarCampoOculto(this);
    });
    
    // Asegurar que haya al menos una línea activa
    if ($("#tablaMovimientos tbody tr").length === 0) {
        agregarNuevaFila();
    }
}

/**
 * Función para agregar nueva fila de movimiento
 */
function agregarNuevaFila() {
    var index = $("#tablaMovimientos tbody tr").length;
    var template = $("#template-row").html();
    var newRow = template.replace(/__INDEX__/g, index).replace(/__NUMERO__/g, index + 1);
    
    $("#tablaMovimientos tbody").append(newRow);
    
    // Inicializar Select2 para la nueva fila
    var nuevaCuentaSelect = $("#tablaMovimientos tbody tr:last-child .select-cuenta");
    var nuevoContactoSelect = $("#tablaMovimientos tbody tr:last-child .select-contacto");
    
    // Usar la misma configuración de Select2 que los originales
    nuevaCuentaSelect.select2({
        theme: 'bootstrap-5',
        placeholder: "Buscar cuenta contable...",
        allowClear: true,
        ajax: {
            url: '/EntradaDiario/BuscarCuentasContables',
            dataType: 'json',
            delay: 250,
            data: function(params) {
                return {
                    term: params.term || ''
                };
            },
            processResults: function(data) {
                return data;
            },
            cache: true
        },
        minimumInputLength: 1
    });
    
    // Usar la misma configuración de Select2 que los originales
    nuevoContactoSelect.select2({
        theme: 'bootstrap-5',
        placeholder: "Buscar contacto...",
        allowClear: true,
        ajax: {
            url: '/EntradaDiario/BuscarContactos',
            dataType: 'json',
            delay: 250,
            data: function(params) {
                return {
                    term: params.term || ''
                };
            },
            processResults: function(data) {
                return data;
            },
            cache: true
        },
        minimumInputLength: 1,
        templateResult: formatContacto,
        templateSelection: formatContactoSelection
    });
    
    // Eventos para nuevo select de contacto
    nuevoContactoSelect.on('select2:select', function(e) {
        var data = e.params.data;
        var tipoContactoInput = $(this).closest('tr').find('input[name$=".TipoContacto"]');
        tipoContactoInput.val(data.tipo);
    });
    
    nuevoContactoSelect.on('select2:clear', function() {
        var tipoContactoInput = $(this).closest('tr').find('input[name$=".TipoContacto"]');
        tipoContactoInput.val('');
    });
    
    // Inicializar formateadores para los nuevos campos monetarios
    var nuevoDebitoInput = $("#tablaMovimientos tbody tr:last-child .monto-debito");
    var nuevoCreditoInput = $("#tablaMovimientos tbody tr:last-child .monto-credito");
    
    // Quitar el valor inicial
    nuevoDebitoInput.val('');
    nuevoCreditoInput.val('');
    
    // Obtener el separador decimal de la configuración global
    const separadorDecimal = window.appConfig?.separadorDecimal || ',';
    const precisionDecimal = window.appConfig?.precisionDecimal || 2;
    const delimiter = separadorDecimal === ',' ? '.' : ',';
    
    // Inicializar Cleave.js para los nuevos campos con la configuración de la empresa
    new Cleave(nuevoDebitoInput[0], {
        numeral: true,
        numeralThousandsGroupStyle: 'thousand',
        numeralDecimalMark: '.',
        numeralDecimalScale: precisionDecimal,
        delimiter: ',',
        numeralPositiveOnly: true,
        onValueChanged: function(e) {
            actualizarCampoOculto(e.target);
        }
    });
    
    new Cleave(nuevoCreditoInput[0], {
        numeral: true,
        numeralThousandsGroupStyle: 'thousand',
        numeralDecimalMark: '.',
        numeralDecimalScale: precisionDecimal,
        delimiter: ',',
        numeralPositiveOnly: true,
        onValueChanged: function(e) {
            actualizarCampoOculto(e.target);
        }
    });
    
    // Calcular totales
    calcularTotales();
}

/**
 * Función para renumerar las filas
 */
function renumerarFilas() {
    $("#tablaMovimientos tbody tr").each(function(index) {
        $(this).attr('data-row', index);
        $(this).find('.numero-fila').text(index + 1);
        
        // Actualizar nombres de campos para mantener el array indexado correctamente
        $(this).find('input, select').each(function() {
            var name = $(this).attr('name');
            if (name) {
                var newName = name.replace(/\[\d+\]/, '[' + index + ']');
                $(this).attr('name', newName);
            }
            
            var id = $(this).attr('id');
            if (id) {
                var newId = id.replace(/\_\d+\_/, '_' + index + '_');
                $(this).attr('id', newId);
            }
        });
    });
}

/**
 * Calcular el total de débitos
 */
function calcularTotalDebito() {
    var total = 0;
    
    $('.monto-debito').each(function() {
        // Obtener el valor sin formato y convertirlo correctamente
        var valor = 0;
        var input = this.value;
        if (input) {
            // Convertir de formato americano (1,234.56) a formato para cálculos (1234.56)
            const valorLimpio = input.replace(/,/g, '');
            valor = parseFloat(valorLimpio) || 0;
        }
        total += valor;
    });
    return total;
}

/**
 * Calcular el total de créditos
 */
function calcularTotalCredito() {
    var total = 0;
    
    $('.monto-credito').each(function() {
        // Obtener el valor sin formato y convertirlo correctamente
        var valor = 0;
        var input = this.value;
        if (input) {
            // Convertir de formato americano (1,234.56) a formato para cálculos (1234.56)
            const valorLimpio = input.replace(/,/g, '');
            valor = parseFloat(valorLimpio) || 0;
        }
        total += valor;
    });
    return total;
}

/**
 * Función para calcular los totales
 */
function calcularTotales() {
    var totalDebito = calcularTotalDebito();
    var totalCredito = calcularTotalCredito();
    
    const separadorDecimal = window.appConfig?.separadorDecimal || ',';
    const precisionDecimal = window.appConfig?.precisionDecimal || 2;
    
    // Formatear y mostrar totales según la configuración de la empresa
    if (separadorDecimal === ',') {
        // Formato europeo (1.234,56)
        $("#totalDebito").text(formatearNumero(totalDebito, separadorDecimal, precisionDecimal));
        $("#totalCredito").text(formatearNumero(totalCredito, separadorDecimal, precisionDecimal));
        
        // Calcular diferencia
        var diferencia = Math.abs(totalDebito - totalCredito);
        $("#diferencia").text(formatearNumero(diferencia, separadorDecimal, precisionDecimal));
    } else {
        // Formato americano (1,234.56)
        $("#totalDebito").text(formatearNumero(totalDebito, separadorDecimal, precisionDecimal));
        $("#totalCredito").text(formatearNumero(totalCredito, separadorDecimal, precisionDecimal));
        
        // Calcular diferencia
        var diferencia = Math.abs(totalDebito - totalCredito);
        $("#diferencia").text(formatearNumero(diferencia, separadorDecimal, precisionDecimal));
    }
    
    // Cambiar color según el balance
    if (Math.abs(totalDebito - totalCredito) < 0.001) {
        $("#diferencia").removeClass('text-danger').addClass('text-success');
    } else {
        $("#diferencia").removeClass('text-success').addClass('text-danger');
    }
}

/**
 * Función auxiliar para formatear números según la configuración
 */
function formatearNumero(valor, separadorDecimal, precision) {
    // Formato con el separador decimal correcto
    let resultado = '$';
    
    try {
        console.log(`Formateando número: ${valor} con separador ${separadorDecimal} y precisión ${precision}`);
        
        // Convertir a formato americano por defecto (1,234.56)
        resultado += valor.toFixed(precision)
            .replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        
        console.log(`Número formateado: ${resultado}`);
    } catch (error) {
        console.error(`Error al formatear número: ${error.message}`);
        // En caso de error, devolver un valor por defecto
        resultado += '0.00';
    }
    
    return resultado;
}

/**
 * Función para previsualizar el código que se generará
 */
function previsualizarCodigo(numeracionId) {
    $.ajax({
        url: '/EntradaDiario/ObtenerPrevisualizacionCodigo',
        type: 'GET',
        data: { numeracionId: numeracionId },
        success: function(data) {
            $("#codigoEntrada").val(data.codigo);
        },
        error: function() {
            $("#codigoEntrada").val('Error al obtener código');
        }
    });
}

// Función para actualizar el valor del campo oculto cuando se modifica el valor formateado
function actualizarCampoOculto(input) {
    const $input = $(input);
    const valorFormateado = input.value || '0';
    let valorNumerico = 0;
    
    if (valorFormateado) {
        // Convertir de formato americano a formato numérico
        const valorLimpio = valorFormateado.replace(/,/g, '');
        valorNumerico = parseFloat(valorLimpio) || 0;
    }
    
    // Buscar el campo hidden correspondiente y actualizar su valor
    const fieldName = $input.attr('name');
    const hiddenField = fieldName.replace('Str', '');
    const $hidden = $(`input[name="${hiddenField}"]`);
    
    // Actualizar el valor del campo oculto que va al servidor
    $hidden.val(valorNumerico.toString());
    console.log(`Actualizado campo oculto ${hiddenField}: ${valorNumerico}`);
    
    // Calcular totales
    calcularTotales();
} 