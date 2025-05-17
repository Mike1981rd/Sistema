// Item Edit Loader - Specifically for loading saved data in edit mode
// This script ensures all saved data is properly loaded into the form

$(document).ready(function() {
    console.log('=== ITEM EDIT LOADER STARTED ===');
    
    // Wait for all scripts to be loaded
    let waitCount = 0;
    const maxWait = 10; // 5 seconds max wait
    
    function waitForDependencies() {
        waitCount++;
        
        const dependencies = {
            jQuery: typeof $ !== 'undefined',
            Select2: typeof $.fn.select2 !== 'undefined',
            Contenedores: typeof window.cargarContenedoresExistentes === 'function',
            Proveedores: typeof window.cargarProveedoresExistentes === 'function'
        };
        
        console.log(`Attempt ${waitCount} - Dependencies:`, dependencies);
        
        const allLoaded = Object.values(dependencies).every(d => d);
        
        if (allLoaded || waitCount >= maxWait) {
            if (allLoaded) {
                console.log('All dependencies loaded. Initializing edit mode...');
                initializeEditMode();
            } else {
                console.error('Max wait reached. Some dependencies not loaded:', dependencies);
                initializeEditMode(); // Try anyway
            }
        } else {
            setTimeout(waitForDependencies, 500);
        }
    }
    
    function initializeEditMode() {
        console.log('Starting edit mode initialization...');
        
        // 1. Initialize Select2 with saved values
        initializeSelects();
        
        // 2. Load containers
        loadContainers();
        
        // 3. Load suppliers
        setTimeout(loadSuppliers, 1000);
        
        // 4. Load barcode
        loadBarcode();
        
        // 5. Load accounting accounts
        setTimeout(loadAccountingAccounts, 1500);
    }
    
    function initializeSelects() {
        console.log('Initializing selects with saved values...');
        
        // Category
        const $categorySelect = $('#CategoriaId');
        if ($categorySelect.length > 0 && !$categorySelect.hasClass('select2-hidden-accessible')) {
            const categoryId = $categorySelect.data('selected-id') || $categorySelect.val();
            const categoryText = $categorySelect.data('selected-text') || $categorySelect.find('option:selected').text();
            
            if (categoryId) {
                console.log('Loading category:', categoryId, categoryText);
                // Initialize Select2
                $categorySelect.select2({
                    theme: 'bootstrap-5',
                    placeholder: 'Seleccione una categoría',
                    width: '100%'
                });
                
                // Set the value
                if (categoryText && categoryText !== 'Seleccione una categoría') {
                    const option = new Option(categoryText, categoryId, true, true);
                    $categorySelect.append(option).trigger('change');
                }
            }
        }
        
        // Brand
        const $brandSelect = $('#MarcaId');
        if ($brandSelect.length > 0 && !$brandSelect.hasClass('select2-hidden-accessible')) {
            const brandId = $brandSelect.data('selected-id') || $brandSelect.val();
            const brandText = $brandSelect.data('selected-text') || $brandSelect.find('option:selected').text();
            
            if (brandId) {
                console.log('Loading brand:', brandId, brandText);
                // Initialize Select2
                $brandSelect.select2({
                    theme: 'bootstrap-5',
                    placeholder: 'Genérica',
                    allowClear: true,
                    width: '100%'
                });
                
                // Set the value
                if (brandText && brandText !== 'Genérica') {
                    const option = new Option(brandText, brandId, true, true);
                    $brandSelect.append(option).trigger('change');
                }
            }
        }
        
        // Tax
        const $taxSelect = $('#ImpuestoId');
        if ($taxSelect.length > 0 && !$taxSelect.hasClass('select2-hidden-accessible')) {
            const taxId = $taxSelect.data('selected-id') || $taxSelect.val();
            const taxText = $taxSelect.data('selected-text') || $taxSelect.find('option:selected').text();
            
            if (taxId) {
                console.log('Loading tax:', taxId, taxText);
                // Initialize Select2
                $taxSelect.select2({
                    theme: 'bootstrap-5',
                    placeholder: 'Seleccione un impuesto',
                    allowClear: true,
                    width: '100%'
                });
                
                // Set the value
                if (taxText && taxText !== 'Seleccione un impuesto') {
                    const option = new Option(taxText, taxId, true, true);
                    $taxSelect.append(option).trigger('change');
                }
            }
        }
    }
    
    function loadContainers() {
        console.log('Loading containers...');
        
        // Get containers data from window if available
        if (window.contenedoresData && typeof window.cargarContenedoresExistentes === 'function') {
            console.log('Container data found:', window.contenedoresData);
            window.cargarContenedoresExistentes(window.contenedoresData);
        } else {
            console.warn('No container data or function not available');
        }
    }
    
    function loadSuppliers() {
        console.log('Loading suppliers...');
        
        // Get suppliers data from window if available
        if (window.proveedoresData && typeof window.cargarProveedoresExistentes === 'function') {
            console.log('Supplier data found:', window.proveedoresData);
            window.cargarProveedoresExistentes(window.proveedoresData);
        } else {
            console.warn('No supplier data or function not available');
        }
    }
    
    function loadBarcode() {
        const barcodeValue = $('#CodigoBarras').val();
        if (barcodeValue && typeof JsBarcode !== 'undefined') {
            console.log('Loading barcode:', barcodeValue);
            $('#codigoBarrasPreview').show();
            JsBarcode("#barcode", barcodeValue, {
                format: "CODE128",
                displayValue: true,
                fontSize: 14,
                height: 50,
                margin: 10
            });
        }
    }
    
    function loadAccountingAccounts() {
        console.log('Loading accounting accounts...');
        
        $('.select2-cuenta').each(function() {
            const $select = $(this);
            const accountId = $select.data('selected-id') || $select.val();
            const accountText = $select.data('selected-text') || $select.find('option:selected').text();
            
            if (accountId && !$select.hasClass('select2-hidden-accessible')) {
                console.log('Loading account:', $select.attr('id'), accountId, accountText);
                
                // Initialize Select2
                $select.select2({
                    theme: 'bootstrap-5',
                    placeholder: 'Seleccione una cuenta',
                    allowClear: true,
                    width: '100%'
                });
                
                // Set the value
                if (accountText && accountText !== 'Seleccione una cuenta') {
                    const option = new Option(accountText, accountId, true, true);
                    $select.append(option).trigger('change');
                }
            }
        });
    }
    
    // Global debug function
    window.debugEditLoader = function() {
        console.log('=== EDIT LOADER DEBUG ===');
        console.log('Category:', $('#CategoriaId').val());
        console.log('Brand:', $('#MarcaId').val());
        console.log('Tax:', $('#ImpuestoId').val());
        console.log('Containers:', $('#contenedores-body tr').length);
        console.log('Suppliers:', $('#proveedores-body tr').length);
        console.log('Container data:', window.contenedoresData);
        console.log('Supplier data:', window.proveedoresData);
        
        $('.select2-cuenta').each(function() {
            console.log('Account:', $(this).attr('id'), 'Value:', $(this).val());
        });
    };
    
    // Start the process
    waitForDependencies();
});