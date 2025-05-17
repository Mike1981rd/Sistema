// Fixed initialization script for Select2 in Edit view
$(document).ready(function() {
    console.log("=== FIXED SELECT2 INITIALIZATION FOR EDIT ===");
    
    // Function to safely destroy existing Select2 instances
    function destroySelect2(selector) {
        $(selector).each(function() {
            if ($(this).hasClass('select2-hidden-accessible')) {
                try {
                    $(this).select2('destroy');
                } catch(e) {
                    console.warn("Could not destroy Select2 for:", this);
                }
            }
        });
    }
    
    // Function to initialize Select2 safely
    function initializeSelect2(selector, options) {
        var $element = $(selector);
        
        // Skip if element doesn't exist
        if ($element.length === 0) {
            return;
        }
        
        // Destroy existing instance if any
        destroySelect2(selector);
        
        try {
            $element.select2(options);
            console.log("✓ Initialized Select2 for:", selector);
        } catch(e) {
            console.error("✗ Failed to initialize Select2 for:", selector, e);
        }
    }
    
    // Wait for all scripts to load
    setTimeout(function() {
        console.log("Starting Select2 initialization for Edit...");
        
        // Clean up any existing Select2 instances
        $('.select2-hidden-accessible').each(function() {
            try {
                $(this).select2('destroy');
            } catch(e) {
                // Ignore errors
            }
        });
        
        // Initialize Category Select2
        initializeSelect2('.select2-categoria', {
            theme: 'bootstrap-5',
            width: '100%',
            placeholder: 'Seleccione una categoría',
            allowClear: false
        });
        
        // Initialize Brand Select2
        initializeSelect2('.select2-marca', {
            theme: 'bootstrap-5',
            width: '100%',
            placeholder: 'Seleccione una marca',
            allowClear: true
        });
        
        // Initialize Tax Select2
        initializeSelect2('#ImpuestoId', {
            theme: 'bootstrap-5',
            width: '100%',
            placeholder: 'Seleccione un impuesto',
            allowClear: true
        });
        
        // Initialize accounting accounts with AJAX
        $('.select2-cuenta').each(function() {
            var $this = $(this);
            var id = $this.attr('id') || 'cuenta_' + Math.random();
            
            // Check for pre-selected values
            var selectedId = $this.data('selected-id');
            var selectedText = $this.data('selected-text');
            
            // If there's a pre-selected value, add it as an option
            if (selectedId && selectedText) {
                $this.empty().append(new Option(selectedText, selectedId, true, true));
            }
            
            initializeSelect2('#' + id, {
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
        
        // Initialize supplier with AJAX
        $('.select2-proveedor').each(function() {
            var $this = $(this);
            var selectedId = $this.data('selected-id');
            var selectedText = $this.data('selected-text');
            
            // If there's a pre-selected value, add it as an option
            if (selectedId && selectedText) {
                $this.empty().append(new Option(selectedText, selectedId, true, true));
            }
            
            initializeSelect2(this, {
                theme: 'bootstrap-5',
                width: '100%',
                placeholder: 'Buscar proveedor...',
                allowClear: true,
                ajax: {
                    url: '/Clientes/Buscar',
                    dataType: 'json',
                    delay: 250,
                    data: function(params) {
                        return {
                            term: params.term,
                            esProveedor: true
                        };
                    },
                    processResults: function(data) {
                        return {
                            results: $.map(data.results || [], function(item) {
                                return {
                                    id: item.id,
                                    text: item.text
                                };
                            })
                        };
                    }
                }
            });
        });
        
        // Initialize other basic Select2 elements
        $('select.form-select').each(function() {
            // Skip if already initialized or if it's one of our special selects
            if ($(this).hasClass('select2-hidden-accessible') || 
                $(this).hasClass('select2-categoria') ||
                $(this).hasClass('select2-marca') ||
                $(this).hasClass('select2-cuenta') ||
                $(this).hasClass('select2-proveedor')) {
                return;
            }
            
            // Check if it has a specific ID that needs special handling
            var id = $(this).attr('id');
            if (id === 'ImpuestoId' || id === 'UnidadMedidaInventarioId') {
                // Already handled above or needs special config
                return;
            }
            
            // Initialize as basic Select2
            initializeSelect2(this, {
                theme: 'bootstrap-5',
                width: '100%'
            });
        });
        
        console.log("Select2 initialization complete for Edit view");
        
    }, 500); // Wait 500ms to ensure all DOM elements are ready
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
                $('#btnResetImage').show();
            };
            reader.readAsDataURL(file);
        }
    });
    
    $('#btnResetImage').on('click', function() {
        $('input[name="ItemImage"]').val('');
        $('#preview').attr('src', '');
        $('#imagePreview').hide();
        $('#imageDefault').show();
        $('#btnResetImage').hide();
    });
}

// Initialize image upload when ready
$(document).ready(function() {
    initImageUpload();
});