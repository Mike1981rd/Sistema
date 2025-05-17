// Item Edit Simple - Mínima inicialización
console.log('Item Edit Simple - Iniciando');

// Esperar a que todo esté completamente cargado
window.addEventListener('load', function() {
    console.log('Window load event');
    
    // Esperar más para asegurar que otros scripts terminen
    setTimeout(function() {
        if (typeof jQuery !== 'undefined' && typeof jQuery.fn.select2 !== 'undefined') {
            inicializarSelect2Simple();
        } else {
            console.error('jQuery o Select2 no están disponibles');
        }
    }, 1000);
});

function inicializarSelect2Simple() {
    console.log('Inicializando Select2 de forma simple');
    
    // Usar jQuery global
    var $ = jQuery;
    
    // Verificar que los elementos existan
    if (!$('#CategoriaId').length) {
        console.error('No se encontró el select de Categoría');
        return;
    }
    
    try {
        // Inicializar cada select de forma muy simple
        $('#CategoriaId').select2({theme: 'bootstrap-5'});
        console.log('Categoría inicializada');
        
        $('#MarcaId').select2({theme: 'bootstrap-5'});
        console.log('Marca inicializada');
        
        $('#ImpuestoId').select2({theme: 'bootstrap-5'});
        console.log('Impuesto inicializado');
        
        // Mostrar valores actuales
        console.log('Valores:', {
            categoria: $('#CategoriaId').val(),
            marca: $('#MarcaId').val(),
            impuesto: $('#ImpuestoId').val()
        });
        
    } catch (error) {
        console.error('Error durante inicialización:', error);
    }
}