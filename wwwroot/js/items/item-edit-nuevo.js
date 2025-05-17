// Item Edit Nuevo - Versión simplificada para editar items
// Versión 2 - Corregido para manejar mejor la inicialización del DOM

console.log('Item Edit Nuevo - Cargando script');

// Función para verificar que el elemento existe y es visible
function elementoListo(selector) {
    const elemento = document.querySelector(selector);
    return elemento && elemento.offsetParent !== null;
}

// Esperar a que jQuery y el DOM estén completamente listos
function esperarParaInicializar() {
    if (typeof jQuery === 'undefined' || !elementoListo('#CategoriaId')) {
        console.log('Esperando jQuery o elementos del DOM...');
        setTimeout(esperarParaInicializar, 100);
        return;
    }
    
    // jQuery y elementos listos
    jQuery(function($) {
        console.log('jQuery listo, DOM listo, iniciando...');
        
        // Esperar un momento más para asegurar que todo esté cargado
        setTimeout(function() {
            inicializarSelectsBasicos();
        }, 300);
    });
}

// Función principal de inicialización
function inicializarSelectsBasicos() {
    console.log('Inicializando Select2 básicos...');
    
    // 1. Categoría
    try {
        const $categoria = jQuery('#CategoriaId');
        if ($categoria.length && !$categoria.hasClass('select2-hidden-accessible')) {
            console.log('Inicializando Select2 para Categoría');
            $categoria.select2({
                theme: 'bootstrap-5',
                placeholder: 'Seleccione una categoría',
                width: '100%',
                debug: true
            });
            console.log('Categoría inicializada. Valor actual:', $categoria.val());
        }
    } catch (error) {
        console.error('Error al inicializar Categoría:', error);
    }
    
    // 2. Marca
    try {
        const $marca = jQuery('#MarcaId');
        if ($marca.length && !$marca.hasClass('select2-hidden-accessible')) {
            console.log('Inicializando Select2 para Marca');
            $marca.select2({
                theme: 'bootstrap-5',
                placeholder: 'Genérica',
                allowClear: true,
                width: '100%',
                debug: true
            });
            console.log('Marca inicializada. Valor actual:', $marca.val());
        }
    } catch (error) {
        console.error('Error al inicializar Marca:', error);
    }
    
    // 3. Impuesto
    try {
        const $impuesto = jQuery('#ImpuestoId');
        if ($impuesto.length && !$impuesto.hasClass('select2-hidden-accessible')) {
            console.log('Inicializando Select2 para Impuesto');
            $impuesto.select2({
                theme: 'bootstrap-5',
                placeholder: 'Seleccione un impuesto',
                allowClear: true,
                width: '100%',
                debug: true
            });
            console.log('Impuesto inicializado. Valor actual:', $impuesto.val());
        }
    } catch (error) {
        console.error('Error al inicializar Impuesto:', error);
    }
    
    // Log final de valores
    console.log('Valores finales:', {
        categoriaId: jQuery('#CategoriaId').val(),
        marcaId: jQuery('#MarcaId').val(),
        impuestoId: jQuery('#ImpuestoId').val()
    });
}

// Iniciar el proceso
esperarParaInicializar();