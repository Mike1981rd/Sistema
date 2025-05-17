// Archivo auxiliar para inicialización de Select2 que puede ser reutilizado

// Verificar si jQuery y Select2 están disponibles
function checkDependencies() {
    if (typeof jQuery === 'undefined') {
        console.error('jQuery is not loaded!');
        return false;
    }
    if (typeof jQuery.fn.select2 === 'undefined') {
        console.error('Select2 is not loaded!');
        return false;
    }
    return true;
}

// Inicializar Select2 de forma segura
function safeInitSelect2(selector, options) {
    try {
        if (checkDependencies()) {
            const $element = jQuery(selector);
            if ($element.length > 0) {
                // Destruir select2 existente si ya está inicializado
                if ($element.hasClass('select2-hidden-accessible')) {
                    $element.select2('destroy');
                }
                
                // Inicializar con las opciones proporcionadas
                $element.select2(options);
                console.log(`Select2 initialized for ${selector}`);
                return true;
            } else {
                console.warn(`No elements found for selector: ${selector}`);
            }
        }
    } catch (error) {
        console.error(`Error initializing Select2 for ${selector}:`, error);
    }
    return false;
}

// Función para verificar si un Select2 está correctamente inicializado
function isSelect2Initialized(selector) {
    const $element = jQuery(selector);
    return $element.length > 0 && $element.hasClass('select2-hidden-accessible');
}

// Reintentar inicialización con delay
function retryInitSelect2(selector, options, maxRetries = 5, delay = 100) {
    let retries = 0;
    
    function tryInit() {
        if (safeInitSelect2(selector, options)) {
            return; // Success
        }
        
        retries++;
        if (retries < maxRetries) {
            setTimeout(tryInit, delay);
        } else {
            console.error(`Failed to initialize Select2 for ${selector} after ${maxRetries} attempts`);
        }
    }
    
    tryInit();
}

// Función para depuración
function debugSelect2Status() {
    console.log('Select2 Status Report:');
    console.log('jQuery loaded:', typeof jQuery !== 'undefined');
    console.log('Select2 loaded:', typeof jQuery !== 'undefined' && typeof jQuery.fn.select2 !== 'undefined');
    
    const selectors = [
        '.select2-categoria',
        '.select2-marca',
        '.select2-cuenta',
        '.select2-proveedor',
        '.select2-contenedor',
        '.select2'
    ];
    
    selectors.forEach(selector => {
        const $elements = jQuery(selector);
        console.log(`${selector}: ${$elements.length} elements found, ${$elements.filter('.select2-hidden-accessible').length} initialized`);
    });
}