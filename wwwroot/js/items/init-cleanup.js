// Final cleanup and initialization script
$(document).ready(function() {
    console.log("=== FINAL CLEANUP AND INIT ===");
    
    // Don't do extensive cleanup - instead ensure proper initialization
    setTimeout(function() {
        // Check initialization status
        var initStatus = {
            category: $('select[name="CategoriaId"]').hasClass('select2-hidden-accessible'),
            brand: $('select[name="MarcaId"]').hasClass('select2-hidden-accessible'),
            tax: $('select[name="ImpuestoId"]').hasClass('select2-hidden-accessible'),
            code: $('#Codigo').val() ? true : false,
            barcode: $('#codigoBarrasPreview').is(':visible')
        };
        
        console.log("Initialization Status:", initStatus);
        
        // Log any errors or warnings
        if (!initStatus.category) console.warn("Category Select2 not initialized");
        if (!initStatus.brand) console.warn("Brand Select2 not initialized");
        if (!initStatus.tax) console.warn("Tax Select2 not initialized");
        if (!initStatus.code) console.warn("Item code not generated");
        
        // Check for Select2 containers
        console.log("Select2 containers found:", $('.select2-container').length);
        
    }, 3000); // Wait 3 seconds for all scripts
});