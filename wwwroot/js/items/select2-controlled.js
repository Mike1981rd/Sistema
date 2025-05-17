// Controlled Select2 initialization that runs after other scripts
$(document).ready(function() {
    // Wait longer for contenedores.js to finish
    setTimeout(function() {
        console.log("=== CONTROLLED SELECT2 INITIALIZATION ===");
        
        // Function to safely initialize or reinitialize Select2
        function safeSelect2Init(selector, options) {
            var $element = $(selector);
            if ($element.length === 0) {
                console.warn("Element not found:", selector);
                return;
            }
            
            // Check if already initialized
            if ($element.hasClass('select2-hidden-accessible')) {
                console.log("Already initialized, skipping:", selector);
                return;
            }
            
            try {
                $element.select2(options);
                console.log("✓ Initialized:", selector);
            } catch(e) {
                console.error("✗ Error initializing", selector, ":", e);
            }
        }
        
        // Initialize main selects
        safeSelect2Init('select[name="CategoriaId"]', {
            theme: 'bootstrap-5',
            width: '100%',
            placeholder: 'Seleccione una categoría',
            allowClear: false
        });
        
        safeSelect2Init('select[name="MarcaId"]', {
            theme: 'bootstrap-5',
            width: '100%',
            placeholder: 'Seleccione una marca',
            allowClear: true
        });
        
        safeSelect2Init('select[name="ImpuestoId"]', {
            theme: 'bootstrap-5',
            width: '100%',
            placeholder: 'Seleccione un impuesto',
            allowClear: true
        });
        
        safeSelect2Init('select[name="UnidadMedidaInventarioId"]', {
            theme: 'bootstrap-5',
            width: '100%',
            placeholder: 'Seleccione una unidad',
            allowClear: false
        });
        
        // Initialize accounting accounts
        var accountSelectors = [
            'select[name="CuentaVentasId"]',
            'select[name="CuentaComprasInventariosId"]',
            'select[name="CuentaCostoVentasGastosId"]',
            'select[name="CuentaDescuentosId"]',
            'select[name="CuentaDevolucionesId"]',
            'select[name="CuentaAjustesId"]',
            'select[name="CuentaCostoMateriaPrimaId"]'
        ];
        
        accountSelectors.forEach(function(selector) {
            safeSelect2Init(selector, {
                theme: 'bootstrap-5',
                width: '100%',
                placeholder: 'Buscar cuenta contable...',
                allowClear: true,
                minimumInputLength: 1,
                ajax: {
                    url: '/Item/BuscarCuentasContables',
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
                templateResult: function(cuenta) {
                    if (!cuenta.id) return cuenta.text;
                    
                    return $('<div class="select2-result">' +
                        '<span class="badge bg-secondary me-2">' + (cuenta.codigo || '') + '</span>' +
                        '<span>' + (cuenta.nombre || cuenta.text) + '</span>' +
                    '</div>');
                },
                templateSelection: function(cuenta) {
                    return cuenta.text || '';
                }
            });
        });
        
        // Final status report
        console.log("\n=== STATUS REPORT ===");
        $('select').each(function(index) {
            var $el = $(this);
            console.log("Select " + index + ":", {
                name: $el.attr('name'),
                id: $el.attr('id'),
                class: $el.attr('class'),
                isSelect2: $el.hasClass('select2-hidden-accessible'),
                visible: $el.is(':visible')
            });
        });
        
    }, 2000); // Wait 2 seconds for other scripts
});

// Initialize image upload functionality
function initImageUpload() {
    $('#browseButton').on('click', function() {
        $('input[name="ItemImage"]').click();
    });
    
    $('input[name="ItemImage"]').on('change', function(e) {
        const file = e.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function(e) {
                $('#preview').attr('src', e.target.result);
                $('#imagePreview').show();
                $('#imageDefault').hide();
            };
            reader.readAsDataURL(file);
        }
    });
}

// Initialize image upload when ready
$(document).ready(function() {
    setTimeout(initImageUpload, 1000);
});