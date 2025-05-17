// Simple, focused Select2 initialization
$(document).ready(function() {
    console.log("=== SIMPLE SELECT2 INITIALIZATION ===");
    
    // Wait for DOM to be fully ready
    setTimeout(function() {
        
        // 1. Category Select2
        var $category = $('#CategoriaId');
        if ($category.length > 0) {
            try {
                $category.select2({
                    theme: 'bootstrap-5',
                    width: '100%',
                    placeholder: 'Seleccione una categoría',
                    allowClear: false
                });
                console.log("✓ Category Select2 initialized");
            } catch(e) {
                console.error("✗ Category initialization error:", e);
            }
        } else {
            console.warn("Category select not found!");
        }
        
        // 2. Brand Select2
        var $brand = $('#MarcaId');
        if ($brand.length > 0) {
            try {
                $brand.select2({
                    theme: 'bootstrap-5',
                    width: '100%',
                    placeholder: 'Seleccione una marca',
                    allowClear: true
                });
                console.log("✓ Brand Select2 initialized");
            } catch(e) {
                console.error("✗ Brand initialization error:", e);
            }
        } else {
            console.warn("Brand select not found!");
        }
        
        // 3. Tax Select2
        var $tax = $('#ImpuestoId');
        if ($tax.length > 0) {
            try {
                $tax.select2({
                    theme: 'bootstrap-5',
                    width: '100%',
                    placeholder: 'Seleccione un impuesto',
                    allowClear: true
                });
                console.log("✓ Tax Select2 initialized");
            } catch(e) {
                console.error("✗ Tax initialization error:", e);
            }
        } else {
            console.warn("Tax select not found!");
        }
        
        // 4. Unit of Measure Select2
        var $uom = $('#UnidadMedidaInventarioId');
        if ($uom.length > 0) {
            try {
                $uom.select2({
                    theme: 'bootstrap-5',
                    width: '100%',
                    placeholder: 'Seleccione una unidad',
                    allowClear: false
                });
                console.log("✓ Unit of Measure Select2 initialized");
            } catch(e) {
                console.error("✗ Unit of Measure initialization error:", e);
            }
        } else {
            console.warn("Unit of Measure select not found!");
        }
        
        // 5. Accounting accounts with AJAX (by ID)
        var accountIds = [
            'CuentaVentasId',
            'CuentaComprasInventariosId', 
            'CuentaCostoVentasGastosId',
            'CuentaDescuentosId',
            'CuentaDevolucionesId',
            'CuentaAjustesId',
            'CuentaCostoMateriaPrimaId'
        ];
        
        accountIds.forEach(function(id) {
            var $account = $('#' + id);
            if ($account.length > 0) {
                try {
                    $account.select2({
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
                    console.log("✓ Account Select2 initialized: " + id);
                } catch(e) {
                    console.error("✗ Account initialization error for " + id + ":", e);
                }
            } else {
                console.warn("Account select not found: " + id);
            }
        });
        
        // Final check
        console.log("\n=== FINAL STATUS ===");
        console.log("Category:", $('#CategoriaId').hasClass('select2-hidden-accessible') ? 'Initialized' : 'Not initialized');
        console.log("Brand:", $('#MarcaId').hasClass('select2-hidden-accessible') ? 'Initialized' : 'Not initialized');
        console.log("Tax:", $('#ImpuestoId').hasClass('select2-hidden-accessible') ? 'Initialized' : 'Not initialized');
        console.log("Unit of Measure:", $('#UnidadMedidaInventarioId').hasClass('select2-hidden-accessible') ? 'Initialized' : 'Not initialized');
        
    }, 1000); // Wait 1 second to ensure all elements are ready
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
    initImageUpload();
});