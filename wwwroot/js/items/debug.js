// Debug script para el formulario de creación de items
$(document).ready(function() {
    console.log('=== DEBUG: Formulario de creación de item ===');
    console.log('Formulario encontrado:', $('#itemForm').length > 0);
    console.log('Acción del formulario:', $('#itemForm').attr('action'));
    console.log('Método del formulario:', $('#itemForm').attr('method'));
    
    // Verificar manejadores de eventos existentes
    try {
        var events = $._data($('#itemForm')[0], 'events');
        if (events && events.submit) {
            console.log('Manejadores de submit encontrados:', events.submit.length);
            events.submit.forEach(function(handler, index) {
                console.log(`Handler ${index}:`, handler.handler.toString().substring(0, 100) + '...');
            });
        }
    } catch (e) {
        console.log('No se pudieron obtener los manejadores de eventos');
    }
    
    // Agregar depuración al submit
    $('#itemForm').on('submit', function(e) {
        console.log('=== SUBMIT FORM DEBUG ===');
        console.log('Evento submit disparado');
        console.log('Validación jQuery activa:', typeof $.validator !== 'undefined');
        
        // Listar todos los campos con required
        console.log('Campos marcados como required:');
        $(this).find('[required]').each(function() {
            var field = {
                name: $(this).attr('name'),
                id: $(this).attr('id'),
                value: $(this).val(),
                type: $(this).attr('type'),
                visible: $(this).is(':visible')
            };
            console.log(field);
        });
        
        // Verificar validación
        if ($.validator) {
            var isValid = $(this).valid();
            console.log('¿Formulario válido?:', isValid);
            
            if (!isValid) {
                console.log('=== ERRORES DE VALIDACIÓN ===');
                var validator = $(this).validate();
                console.log('Número de errores:', validator.errorList.length);
                
                validator.errorList.forEach(function(error, index) {
                    console.log(`Error ${index + 1}:`);
                    console.log('  Campo:', error.element.name);
                    console.log('  ID:', error.element.id);
                    console.log('  Mensaje:', error.message);
                    console.log('  Valor actual:', error.element.value);
                    console.log('  Visible:', $(error.element).is(':visible'));
                    console.log('  Pestaña:', $(error.element).closest('.tab-pane').attr('id'));
                });
                
                // Mostrar resumen de errores por pestaña
                var errorsByTab = {};
                validator.errorList.forEach(function(error) {
                    var tabId = $(error.element).closest('.tab-pane').attr('id') || 'sin-pestaña';
                    if (!errorsByTab[tabId]) {
                        errorsByTab[tabId] = [];
                    }
                    errorsByTab[tabId].push(error.element.name);
                });
                
                console.log('=== ERRORES POR PESTAÑA ===');
                for (var tab in errorsByTab) {
                    console.log(`${tab}:`, errorsByTab[tab]);
                }
            }
        }
        
        // NO prevenir el envío para permitir ver qué pasa
        console.log('Permitiendo envío del formulario...');
    });
    
    // Log de campos
    console.log('=== CAMPOS DEL FORMULARIO ===');
    $('#itemForm').find('input, select, textarea').each(function() {
        if ($(this).attr('name')) {
            console.log({
                name: $(this).attr('name'),
                type: $(this).attr('type'),
                required: $(this).attr('required') !== undefined,
                value: $(this).val()
            });
        }
    });
});