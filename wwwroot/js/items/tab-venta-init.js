// Initialization script for Product Sale tab
$(document).ready(function() {
    console.log("=== PRODUCT SALE TAB INITIALIZATION ===");
    
    // Initialize only when Product Sale tab is active
    function initProductSaleTab() {
        // Initialize container select for product sale
        if ($('#ProductoVenta_ItemContenedorId').length > 0 && !$('#ProductoVenta_ItemContenedorId').hasClass('select2-hidden-accessible')) {
            $('#ProductoVenta_ItemContenedorId').select2({
                theme: 'bootstrap-5',
                width: '100%',
                placeholder: 'Seleccione un contenedor',
                allowClear: false
            });
            console.log("✓ Product container Select2 initialized");
        }
        
        // Initialize tax select for product sale
        if ($('#ProductoVenta_ImpuestoId').length > 0 && !$('#ProductoVenta_ImpuestoId').hasClass('select2-hidden-accessible')) {
            $('#ProductoVenta_ImpuestoId').select2({
                theme: 'bootstrap-5',
                width: '100%',
                placeholder: 'Seleccione un impuesto',
                allowClear: true
            });
            console.log("✓ Product tax Select2 initialized");
        }
    }
    
    // Initialize on page load if Product Sale tab is active
    if ($('#tab-venta').hasClass('active')) {
        initProductSaleTab();
    }
    
    // Reinitialize when Product Sale tab is shown
    $('a[data-bs-toggle="tab"][href="#tab-venta"]').on('shown.bs.tab', function() {
        console.log("Product Sale tab shown - initializing...");
        setTimeout(initProductSaleTab, 100);
    });
});