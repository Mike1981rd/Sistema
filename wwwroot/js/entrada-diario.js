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
    $("form").submit(function(e) {
        // Verificar balance
        var totalDebito = calcularTotalDebito();
        var totalCredito = calcularTotalCredito();
        
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
                $("#numeracionSelect").append(new Option(result.nombre, result.id, true, true));
                
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
            '<span class="' + tipoClass + ' text-white rounded-1 me-2 p-1 d-inline-block" style="width: 24px; text-align: center;">' + tipoLabel + '</span>' +
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
            '<span class="' + tipoClass + ' text-white rounded-1 me-2 p-1 d-inline-block" style="width: 20px; text-align: center; font-size: 0.8em;">' + tipoLabel + '</span>' +
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
    // Configuración de formato de moneda con coma como separador decimal
    $('.monto-debito, .monto-credito').each(function() {
        // Quitar el valor inicial de "0.00"
        $(this).val('');
        
        // Crear instancia de Cleave.js con configuración fija para coma decimal
        new Cleave(this, {
            numeral: true,
            numeralThousandsGroupStyle: 'thousand',
            numeralDecimalMark: ',',
            numeralDecimalScale: 2,
            delimiter: '.',
            numeralPositiveOnly: true
        });
    });
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
    
    // Quitar el valor inicial de "0.00"
    nuevoDebitoInput.val('');
    nuevoCreditoInput.val('');
    
    // Inicializar Cleave.js para los nuevos campos con configuración fija para coma decimal
    new Cleave(nuevoDebitoInput[0], {
        numeral: true,
        numeralThousandsGroupStyle: 'thousand',
        numeralDecimalMark: ',',
        numeralDecimalScale: 2,
        delimiter: '.',
        numeralPositiveOnly: true
    });
    
    new Cleave(nuevoCreditoInput[0], {
        numeral: true,
        numeralThousandsGroupStyle: 'thousand',
        numeralDecimalMark: ',',
        numeralDecimalScale: 2,
        delimiter: '.',
        numeralPositiveOnly: true
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
            // Reemplazar puntos de miles por nada y coma decimal por punto para parsear
            input = input.replace(/\./g, '').replace(',', '.');
            valor = parseFloat(input) || 0;
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
            // Reemplazar puntos de miles por nada y coma decimal por punto para parsear
            input = input.replace(/\./g, '').replace(',', '.');
            valor = parseFloat(input) || 0;
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
    
    // Formatear y mostrar totales
    $("#totalDebito").text('$' + totalDebito.toFixed(2));
    $("#totalCredito").text('$' + totalCredito.toFixed(2));
    
    // Calcular diferencia
    var diferencia = totalDebito - totalCredito;
    $("#diferencia").text('$' + Math.abs(diferencia).toFixed(2));
    
    // Cambiar color según el balance
    if (Math.abs(diferencia) < 0.001) {
        $("#diferencia").removeClass('text-danger').addClass('text-success');
    } else {
        $("#diferencia").removeClass('text-success').addClass('text-danger');
    }
} 