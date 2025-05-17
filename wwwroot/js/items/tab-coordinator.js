// Tab coordination script to manage communication between tabs
$(document).ready(function() {
    // Si ya está inicializado por otro script, detener
    if (window.itemCreateInitialized) {
        console.log("Item create already initialized by another script - skipping tab coordinator initialization");
        return;
    }
    
    console.log("=== TAB COORDINATOR INITIALIZED ===");
    
    // Track which tabs have been initialized
    var initializedTabs = {};
    
    // Prevent multiple initializations
    window.tabInitTracker = {
        isInitialized: function(tabName) {
            return initializedTabs[tabName] === true;
        },
        markInitialized: function(tabName) {
            initializedTabs[tabName] = true;
            console.log("Tab marked as initialized:", tabName);
        },
        reset: function(tabName) {
            initializedTabs[tabName] = false;
            console.log("Tab reset:", tabName);
        }
    };
    
    // Event listener for successful category creation
    $(document).on('categoria:created', function(e, newCategoria) {
        console.log("Category created:", newCategoria);
        
        // Use the global refresh function
        if (window.refreshCategorySelect2) {
            window.refreshCategorySelect2(newCategoria.id, newCategoria.nombre);
        }
        
        // If the category has accounting data, trigger update
        if (newCategoria.cuentaVentasId || newCategoria.cuentaComprasInventariosId) {
            setTimeout(function() {
                $(document).trigger('categoria:changed', [{
                    success: true,
                    cuentaVentasId: newCategoria.cuentaVentasId,
                    cuentaComprasInventariosId: newCategoria.cuentaComprasInventariosId,
                    cuentaCostoVentasGastosId: newCategoria.cuentaCostoVentasGastosId,
                    cuentaDescuentosId: newCategoria.cuentaDescuentosId,
                    cuentaDevolucionesId: newCategoria.cuentaDevolucionesId,
                    cuentaAjustesId: newCategoria.cuentaAjustesId,
                    cuentaCostoMateriaPrimaId: newCategoria.cuentaCostoMateriaPrimaId,
                    impuestoId: newCategoria.impuestoId
                }]);
            }, 300);
        }
    });
    
    // Event listener for successful marca creation
    $(document).on('marca:created', function(e, newMarca) {
        console.log("Marca created:", newMarca);
        
        // Update the marca select2 with the new option
        const $marcaSelect = $('#MarcaId');
        
        // Add the new option
        const newOption = new Option(newMarca.nombre, newMarca.id, true, true);
        $marcaSelect.append(newOption);
        $marcaSelect.val(newMarca.id).trigger('change');
    });
    
    // Handle button click for category save
    $(document).on('click', '#btnGuardarCategoria', function(e) {
        e.preventDefault();
        $(this).closest('form').submit();
    });
    
    // Handle form submit in category offcanvas (both create and edit)
    $(document).on('submit', '#formCrearCategoria, #formEditarCategoria', function(e) {
        e.preventDefault();
        
        const formData = new FormData(this);
        const isEdit = $(this).attr('id') === 'formEditarCategoria';
        
        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function(response) {
                if (response.success) {
                    // Close the offcanvas
                    const offcanvasElement = document.getElementById('offcanvasCategoria');
                    const offcanvasBS = bootstrap.Offcanvas.getInstance(offcanvasElement);
                    offcanvasBS.hide();
                    
                    if (isEdit) {
                        // For edit, update the existing option
                        if (response.categoria) {
                            const $categoriaSelect = $('#CategoriaId');
                            const currentValue = $categoriaSelect.val();
                            
                            // Use the global refresh function  
                            if (window.refreshCategorySelect2) {
                                window.refreshCategorySelect2(response.categoria.id, response.categoria.nombre);
                            }
                            
                            // Trigger the category change event to update accounting fields
                            setTimeout(function() {
                                $(document).trigger('categoria:changed', [{
                                    success: true,
                                    cuentaVentasId: response.categoria.cuentaVentasId,
                                    cuentaComprasInventariosId: response.categoria.cuentaComprasInventariosId,
                                    cuentaCostoVentasGastosId: response.categoria.cuentaCostoVentasGastosId,
                                    cuentaDescuentosId: response.categoria.cuentaDescuentosId,
                                    cuentaDevolucionesId: response.categoria.cuentaDevolucionesId,
                                    cuentaAjustesId: response.categoria.cuentaAjustesId,
                                    cuentaCostoMateriaPrimaId: response.categoria.cuentaCostoMateriaPrimaId,
                                    impuestoId: response.categoria.impuestoId
                                }]);
                            }, 300);
                            
                            // Trigger update event
                            $(document).trigger('categoria:updated', [response.categoria]);
                        }
                    } else {
                        // For create, trigger created event
                        $(document).trigger('categoria:created', [response.categoria]);
                    }
                    
                    // Show success message
                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'top-end',
                        showConfirmButton: false,
                        timer: 3000,
                        timerProgressBar: true
                    });
                    
                    Toast.fire({
                        icon: 'success',
                        title: isEdit ? 'Categoría actualizada exitosamente' : 'Categoría creada exitosamente'
                    });
                } else {
                    Swal.fire('Error', response.message || 'Error al procesar la categoría', 'error');
                }
            },
            error: function() {
                Swal.fire('Error', 'Error al procesar la solicitud', 'error');
            }
        });
    });
    
    // Handle form submit in marca offcanvas
    $(document).on('submit', '#formMarca', function(e) {
        e.preventDefault();
        
        const formData = new FormData(this);
        
        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function(response) {
                if (response.success) {
                    // Close the offcanvas
                    const offcanvasElement = document.getElementById('offcanvasMarca');
                    const offcanvasBS = bootstrap.Offcanvas.getInstance(offcanvasElement);
                    offcanvasBS.hide();
                    
                    // Trigger the created event
                    $(document).trigger('marca:created', [response.marca]);
                    
                    // Show success message
                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'top-end',
                        showConfirmButton: false,
                        timer: 3000,
                        timerProgressBar: true
                    });
                    
                    Toast.fire({
                        icon: 'success',
                        title: 'Marca creada exitosamente'
                    });
                } else {
                    Swal.fire('Error', response.message || 'Error al crear la marca', 'error');
                }
            },
            error: function() {
                Swal.fire('Error', 'Error al procesar la solicitud', 'error');
            }
        });
    });
    
    // Monitor tab changes
    $('a[data-bs-toggle="tab"]').on('shown.bs.tab', function(e) {
        var target = $(e.target).attr('href');
        console.log("Tab changed to:", target);
        
        // Force re-initialization of Select2 elements in the shown tab
        setTimeout(function() {
            $(target).find('.select2').each(function() {
                if (!$(this).hasClass('select2-hidden-accessible')) {
                    console.log(`Re-initializing select2: ${$(this).attr('id') || $(this).attr('name')}`);
                }
            });
        }, 100);
    });
    
    // Force initial tab initialization
    setTimeout(function() {
        // Si ya está inicializado por otro script, detener
        if (window.itemCreateInitialized) {
            return;
        }
        
        var activeTab = $('.nav-tabs .nav-link.active').attr('href');
        console.log("Active tab on load:", activeTab);
        $(activeTab).trigger('shown.bs.tab');
    }, 500);
    
    // Initialize select2 in edit partial form
    window.initializeCategoryEditForm = function() {
        console.log("Initializing category edit form");
        
        // Initialize select2 for account fields
        $('.select2-cuenta').each(function() {
            const $select = $(this);
            const currentValue = $select.val();
            
            $select.select2({
                theme: 'bootstrap-5',
                width: '100%',
                placeholder: 'Buscar cuenta contable...',
                allowClear: true,
                minimumInputLength: 1,
                ajax: {
                    url: '/api/CuentasContables/Buscar',
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
                }
            });
            
            // Pre-load current value if exists
            if (currentValue) {
                loadAccountOption($select, currentValue);
            }
        });
        
        // Initialize other selects
        $('.form-select:not(.select2-cuenta)').select2({
            theme: 'bootstrap-5',
            width: '100%'
        });
    };
    
    function loadAccountOption($select, accountId) {
        if (!accountId || accountId === '0') return;
        
        // Load account data
        $.ajax({
            url: '/api/CuentasContables/Buscar',
            type: 'GET',
            data: { term: accountId },
            success: function(data) {
                if (data.results && data.results.length > 0) {
                    const account = data.results.find(a => a.id == accountId);
                    if (account) {
                        // Create and add the option
                        const option = new Option(account.text, account.id, true, true);
                        $select.append(option).trigger('change');
                    }
                }
            }
        });
    }
    
    // Event listener for offcanvas shown
    $(document).on('shown.bs.offcanvas', '#offcanvasCategoria', function () {
        console.log("Category offcanvas shown");
        // Always initialize forms when offcanvas is shown
        setTimeout(function() {
            // Initialize form if it's the edit form
            if ($('#formEditarCategoria').length > 0) {
                initializeCategoryEditForm();
            }
            // Initialize form if it's the create form
            else if ($('#formCrearCategoria').length > 0) {
                // Initialize create form selects
                $('.form-select').select2({
                    theme: 'bootstrap-5',
                    width: '100%'
                });
            }
        }, 100);
    });
    
    // Debug helper
    window.debugItemForm = function() {
        console.log("=== ITEM FORM DEBUG ===");
        console.log("Category:", $('#CategoriaId').val());
        console.log("Brand:", $('#MarcaId').val());
        console.log("Tax:", $('#ImpuestoId').val());
        
        // Check accounting fields
        const accountingFields = [
            '#CuentaVentasId',
            '#CuentaComprasInventariosId',
            '#CuentaCostoVentasGastosId',
            '#CuentaDescuentosId',
            '#CuentaDevolucionesId',
            '#CuentaAjustesId'
        ];
        
        accountingFields.forEach(field => {
            const $field = $(field);
            if ($field.length) {
                console.log(`${field}: ${$field.val()}`);
            }
        });
    };
});