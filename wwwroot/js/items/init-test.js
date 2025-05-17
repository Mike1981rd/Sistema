// Temporary file for debugging Select2 initialization

$(document).ready(function() {
    console.log("=== SELECT2 INITIALIZATION DEBUG ===");
    
    // Check for jQuery
    console.log("jQuery version:", $.fn.jquery);
    
    // Check for Select2
    console.log("Select2 available:", typeof $.fn.select2 !== 'undefined');
    
    // Check for required elements
    console.log("Elements found:");
    console.log("- .select2-categoria:", $('.select2-categoria').length);
    console.log("- .select2-marca:", $('.select2-marca').length);
    console.log("- .select2-cuenta:", $('.select2-cuenta').length);
    console.log("- .select2-proveedor:", $('.select2-proveedor').length);
    
    // Try basic initialization
    setTimeout(function() {
        console.log("Attempting basic Select2 initialization...");
        
        // Category
        try {
            $('.select2-categoria').select2({
                theme: 'bootstrap-5',
                width: '100%'
            });
            console.log("✓ Category Select2 initialized");
        } catch(e) {
            console.error("✗ Category initialization error:", e);
        }
        
        // Brand
        try {
            $('.select2-marca').select2({
                theme: 'bootstrap-5',
                width: '100%'
            });
            console.log("✓ Brand Select2 initialized");
        } catch(e) {
            console.error("✗ Brand initialization error:", e);
        }
        
        // Accounting accounts
        try {
            $('.select2-cuenta').each(function() {
                $(this).select2({
                    theme: 'bootstrap-5',
                    width: '100%',
                    placeholder: 'Buscar cuenta...'
                });
            });
            console.log("✓ Accounting accounts Select2 initialized");
        } catch(e) {
            console.error("✗ Accounting accounts initialization error:", e);
        }
        
    }, 500);
});