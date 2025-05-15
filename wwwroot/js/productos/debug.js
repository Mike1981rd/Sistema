// Debug script para el módulo de productos
$(document).ready(function() {
    console.log("=== Debug Productos - Inicialización ===");
    console.log("jQuery cargado:", typeof jQuery !== 'undefined');
    console.log("Select2 cargado:", typeof $.fn.select2 !== 'undefined');
    console.log("Bootstrap cargado:", typeof bootstrap !== 'undefined');
    
    // Verificar elementos del DOM
    console.log("Elemento categoriaId encontrado:", $('#categoriaId').length > 0);
    console.log("Clases del select de categoría:", $('#categoriaId').attr('class'));
    
    // Verificar configuración de Select2
    setTimeout(function() {
        console.log("El select de categoría es un Select2:", $('#categoriaId').hasClass('select2-hidden-accessible'));
        
        if ($('#categoriaId').hasClass('select2-hidden-accessible')) {
            console.log("Select2 inicializado correctamente");
            
            // Intentar obtener configuración
            try {
                var select2Data = $('#categoriaId').data('select2');
                console.log("Datos de Select2:", select2Data);
            } catch (e) {
                console.error("Error al obtener datos de Select2:", e);
            }
        } else {
            console.error("Select2 NO se inicializó correctamente en el elemento de categoría");
        }
        
        // Verificar otros elementos
        console.log("Offcanvas de categoría encontrado:", $('#offcanvasCategoria').length > 0);
        console.log("Contenedor del formulario encontrado:", $('#formCategoriaContainer').length > 0);
    }, 1000);
    
    // Interceptar llamadas AJAX
    $(document).ajaxSend(function(event, jqxhr, settings) {
        if (settings.url.includes('Categoria')) {
            console.log("Llamada AJAX a categorías:", settings.url);
            console.log("Datos enviados:", settings.data);
        }
    });
    
    $(document).ajaxComplete(function(event, jqxhr, settings) {
        if (settings.url.includes('Categoria')) {
            console.log("Respuesta AJAX de categorías:", jqxhr.responseText);
        }
    });
    
    // Monitorear eventos de Select2
    $('#categoriaId').on('select2:open', function() {
        console.log("Select2 abierto");
    });
    
    $('#categoriaId').on('select2:opening', function() {
        console.log("Select2 abriéndose");
    });
    
    $('#categoriaId').on('select2:close', function() {
        console.log("Select2 cerrado");
    });
    
    // Verificar la configuración del Select2
    setTimeout(function() {
        var data = $('#categoriaId').data('select2');
        if (data) {
            console.log("Configuración de Select2:", data.options.options);
        }
    }, 2000);
});