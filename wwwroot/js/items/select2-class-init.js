// Select2 initialization using CSS classes instead of IDs
$(document).ready(function() {
    console.log("=== SELECT2 CLASS-BASED INITIALIZATION ===");
    
    // Wait for DOM to be fully ready
    setTimeout(function() {
        
        // 1. Category Select2 (by class)
        var $category = $('.select2-categoria');
        if ($category.length > 0) {
            try {
                // Remove any existing Select2
                if ($category.hasClass('select2-hidden-accessible')) {
                    $category.select2('destroy');
                }
                
                $category.select2({
                    theme: 'bootstrap-5',
                    width: '100%',
                    placeholder: 'Seleccione una categoría',
                    allowClear: false
                });
                console.log("✓ Category Select2 initialized - Found " + $category.length + " elements");
            } catch(e) {
                console.error("✗ Category initialization error:", e);
            }
        } else {
            console.warn("Category select not found! Looking for .select2-categoria");
        }
        
        // 2. Brand Select2 (by class)
        var $brand = $('.select2-marca');
        if ($brand.length > 0) {
            try {
                // Remove any existing Select2
                if ($brand.hasClass('select2-hidden-accessible')) {
                    $brand.select2('destroy');
                }
                
                $brand.select2({
                    theme: 'bootstrap-5',
                    width: '100%',
                    placeholder: 'Seleccione una marca',
                    allowClear: true
                });
                console.log("✓ Brand Select2 initialized - Found " + $brand.length + " elements");
            } catch(e) {
                console.error("✗ Brand initialization error:", e);
            }
        } else {
            console.warn("Brand select not found! Looking for .select2-marca");
        }
        
        // 3. Tax Select2 (by class)
        var $tax = $('.select2').not('.select2-categoria, .select2-marca, .select2-cuenta, .select2-proveedor');
        if ($tax.length > 0) {
            try {
                $tax.each(function() {
                    var $this = $(this);
                    
                    // Remove any existing Select2
                    if ($this.hasClass('select2-hidden-accessible')) {
                        $this.select2('destroy');
                    }
                    
                    $this.select2({
                        theme: 'bootstrap-5',
                        width: '100%',
                        placeholder: 'Seleccione...',
                        allowClear: true
                    });
                });
                console.log("✓ Tax/General Select2 initialized - Found " + $tax.length + " elements");
            } catch(e) {
                console.error("✗ Tax initialization error:", e);
            }
        } else {
            console.warn("Tax/General select not found! Looking for .select2");
        }
        
        // 4. Accounting accounts with AJAX (by class)
        var $accounts = $('.select2-cuenta');
        if ($accounts.length > 0) {
            try {
                $accounts.each(function() {
                    var $this = $(this);
                    
                    // Remove any existing Select2
                    if ($this.hasClass('select2-hidden-accessible')) {
                        $this.select2('destroy');
                    }
                    
                    $this.select2({
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
                console.log("✓ Account Select2 initialized - Found " + $accounts.length + " elements");
            } catch(e) {
                console.error("✗ Account initialization error:", e);
            }
        } else {
            console.warn("Account selects not found! Looking for .select2-cuenta");
        }
        
        // Final status check
        console.log("\n=== FINAL STATUS ===");
        console.log("Total selects found:", $('select').length);
        console.log("Initialized Select2s:", $('.select2-hidden-accessible').length);
        
        // List all selects and their status
        $('select').each(function(index) {
            var $el = $(this);
            console.log("Select " + index + ":", {
                id: $el.attr('id') || 'no-id',
                class: $el.attr('class'),
                name: $el.attr('name'),
                initialized: $el.hasClass('select2-hidden-accessible')
            });
        });
        
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