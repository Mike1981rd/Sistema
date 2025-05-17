// Item Edit Fixed v2 - Complete rewrite with proper data loading
// Version 2.0 - Fixed all selects and data loading issues

$(document).ready(function() {
    console.log('Item Edit Fixed v2 - Starting initialization');
    
    // Track initialization state
    let initState = {
        basicSelects: false,
        containers: false,
        suppliers: false,
        accounts: false,
        venta: false
    };
    
    // Variable para saber si ya se inicializaron las pestañas
    let tabsInitialized = {
        compras: false,
        contabilidad: false,
        taras: false,
        venta: false
    };

    // 1. Initialize Select2 for Category
    function initializeCategorySelect() {
        const $categorySelect = $('#CategoriaId');
        const currentValue = window.modelData?.categoriaId || $categorySelect.val();
        let currentText = window.modelData?.categoriaNombre || $categorySelect.find('option:selected').text();
        
        console.log('Category - Current Value:', currentValue, 'Current Text:', currentText);
        
        // Si no tenemos texto pero tenemos valor, obtenerlo del select
        if (currentValue && (!currentText || currentText.trim() === '')) {
            currentText = $categorySelect.find(`option[value="${currentValue}"]`).text();
        }
        
        // Primero verificar si el select ya se ha inicializado
        if ($categorySelect.hasClass('select2-hidden-accessible')) {
            console.log('Category select already initialized, destroying and recreating');
            $categorySelect.select2('destroy');
        }
        
        $categorySelect.select2({
            theme: 'bootstrap-5',
            placeholder: 'Seleccione una categoría',
            width: '100%',
            ajax: {
                url: '/Categoria/Buscar',
                dataType: 'json',
                delay: 250,
                data: function(params) {
                    return {
                        term: params.term || '',
                        page: params.page || 1
                    };
                },
                processResults: function(data, params) {
                    params.page = params.page || 1;
                    return {
                        results: data.results || [],
                        pagination: {
                            more: data.pagination?.more || false
                        }
                    };
                },
                cache: true
            },
            minimumInputLength: 0,
            templateResult: formatCategoriaResult,
            templateSelection: formatCategoriaSelection
        });
        
        // Load initial value if exists - simplificado
        if (currentValue && currentValue > 0) {
            // Si ya está en el select, usar esa opción
            if ($categorySelect.find(`option[value="${currentValue}"]`).length > 0) {
                $categorySelect.val(currentValue).trigger('change');
            } else if (currentText) {
                // Si no está, crear nueva opción
                const option = new Option(currentText, currentValue, true, true);
                $categorySelect.append(option).trigger('change');
            }
        }
    }

    // 2. Initialize Select2 for Brand
    function initializeBrandSelect() {
        const $brandSelect = $('#MarcaId');
        const currentValue = window.modelData?.marcaId || $brandSelect.val();
        let currentText = window.modelData?.marcaNombre || $brandSelect.find('option:selected').text();
        
        console.log('Brand - Current Value:', currentValue, 'Current Text:', currentText);
        
        // Si no tenemos texto pero tenemos valor, obtenerlo del select
        if (currentValue && (!currentText || currentText.trim() === '')) {
            currentText = $brandSelect.find(`option[value="${currentValue}"]`).text();
        }
        
        // Primero verificar si el select ya se ha inicializado
        if ($brandSelect.hasClass('select2-hidden-accessible')) {
            console.log('Brand select already initialized, destroying and recreating');
            $brandSelect.select2('destroy');
        }
        
        $brandSelect.select2({
            theme: 'bootstrap-5',
            placeholder: 'Genérica',
            allowClear: true,
            width: '100%',
            ajax: {
                url: '/Marca/Buscar',
                dataType: 'json',
                delay: 250,
                data: function(params) {
                    return {
                        term: params.term || ''
                    };
                },
                processResults: function(data, params) {
                    var results = data.results || [];
                    
                    // Solo agregar opción de crear si hay término de búsqueda
                    if (results.length === 0 && params.term && params.term.trim() !== '') {
                        results.push({
                            id: 'new',
                            text: 'Crear marca: "' + params.term + '"',
                            term: params.term
                        });
                    }
                    
                    return {
                        results: results
                    };
                },
                cache: true
            },
            minimumInputLength: 0,
            language: {
                searching: function() {
                    return "Buscando marcas...";
                }
            },
            matcher: function(params, data) {
                // Si no hay término de búsqueda, mostrar todos
                if (!params.term) {
                    return data;
                }
                
                // Búsqueda insensible a mayúsculas/minúsculas
                if (data.text.toLowerCase().indexOf(params.term.toLowerCase()) > -1) {
                    return data;
                }
                
                return null;
            },
            templateResult: formatMarcaResult,
            templateSelection: formatMarcaSelection
        });
        
        // Load initial value if exists - simplificado
        if (currentValue && currentValue > 0) {
            // Si ya está en el select, usar esa opción
            if ($brandSelect.find(`option[value="${currentValue}"]`).length > 0) {
                $brandSelect.val(currentValue).trigger('change');
            } else if (currentText) {
                // Si no está, crear nueva opción
                const option = new Option(currentText, currentValue, true, true);
                $brandSelect.append(option).trigger('change');
            }
        }
    }

    // 3. Initialize Select2 for Tax
    function initializeTaxSelect() {
        const $taxSelect = $('#ImpuestoId');
        const currentValue = window.modelData?.impuestoId || $taxSelect.val();
        let currentText = window.modelData?.impuestoNombre || $taxSelect.find('option:selected').text();
        
        console.log('Tax - Current Value:', currentValue, 'Current Text:', currentText);
        
        // Si no tenemos texto pero tenemos valor, obtenerlo del select
        if (currentValue && (!currentText || currentText.trim() === '')) {
            currentText = $taxSelect.find(`option[value="${currentValue}"]`).text();
        }
        
        // Primero verificar si el select ya se ha inicializado
        if ($taxSelect.hasClass('select2-hidden-accessible')) {
            console.log('Tax select already initialized, destroying and recreating');
            $taxSelect.select2('destroy');
        }
        
        $taxSelect.select2({
            theme: 'bootstrap-5',
            placeholder: 'Seleccione un impuesto',
            allowClear: true,
            width: '100%',
            ajax: {
                url: '/Impuestos/Buscar',
                dataType: 'json',
                delay: 250,
                data: function(params) {
                    return {
                        term: params.term
                    };
                },
                processResults: function(data) {
                    return {
                        results: data.results
                    };
                },
                cache: true
            },
            minimumInputLength: 0,
            templateResult: function(data) {
                if (data.loading) return data.text;
                return data.text;
            },
            templateSelection: function(data) {
                return data.text;
            }
        });
        
        // Load initial value if exists - simplificado
        if (currentValue && currentValue > 0) {
            // Si ya está en el select, usar esa opción
            if ($taxSelect.find(`option[value="${currentValue}"]`).length > 0) {
                $taxSelect.val(currentValue).trigger('change');
            } else if (currentText) {
                // Si no está, crear nueva opción
                const option = new Option(currentText, currentValue, true, true);
                $taxSelect.append(option).trigger('change');
            }
        }
    }

    // 4. Initialize Select2 for Accounting Accounts
    function initializeAccountSelects() {
        console.log('Initializing accounting accounts...');
        
        // IMPORTANTE: Solo buscar dentro de la pestaña de contabilidad
        // Primero, si hay select2-cuenta fuera de la pestaña de contabilidad, ocultarlos
        $('.select2-cuenta').not('#tab-contabilidad .select2-cuenta').closest('.mb-3, .row.mb-3').hide();
        
        // Luego inicializar solo los selects dentro de la pestaña de contabilidad
        $('#tab-contabilidad .select2-cuenta').each(function() {
            const $select = $(this);
            const selectId = $select.attr('id');
            const currentValue = $select.val();
            let currentText = '';
            
            // Intentar obtener el texto actual de diferentes maneras
            if ($select.find('option:selected').length > 0) {
                currentText = $select.find('option:selected').text();
            }
            
            console.log('Account Select:', selectId, 'Value:', currentValue, 'Text:', currentText);
            
            // Primero verificar si el select ya se ha inicializado
            if ($select.hasClass('select2-hidden-accessible')) {
                console.log('Account select already initialized, destroying and recreating:', selectId);
                $select.select2('destroy');
            }
            
            // Configuración específica para cuentas contables
            $select.select2({
                theme: 'bootstrap-5',
                placeholder: 'Seleccione una cuenta',
                allowClear: true,
                width: '100%',
                dropdownParent: $('#tab-contabilidad'), // Este es un cambio importante
                ajax: {
                    url: '/api/CuentasContables/buscar',
                    dataType: 'json',
                    delay: 250,
                    data: function(params) {
                        return {
                            q: params.term || ''
                        };
                    },
                    processResults: function(data) {
                        return {
                            results: data.map(function(item) {
                                return {
                                    id: item.id,
                                    text: item.codigo + ' - ' + item.nombre
                                };
                            })
                        };
                    },
                    cache: true
                },
                minimumInputLength: 0
            });
            
            // Load initial value if exists - now improved to handle undefined text
            if (currentValue && currentValue > 0) {
                // Si ya está en el select, usar esa opción
                if ($select.find(`option[value="${currentValue}"]`).length > 0) {
                    $select.val(currentValue).trigger('change');
                    console.log('Value set from existing option:', currentValue);
                } else if (currentText && currentText !== 'Seleccione una cuenta') {
                    // Si no está pero tenemos el texto, crear nueva opción
                    console.log('Creating new option with text:', currentText);
                    const option = new Option(currentText, currentValue, true, true);
                    $select.append(option).trigger('change');
                } else {
                    // Si no tenemos el texto, cargar mediante AJAX
                    console.log('Fetching account info via AJAX for ID:', currentValue);
                    $.ajax({
                        url: '/api/CuentasContables/' + currentValue,
                        type: 'GET',
                        async: false, // Para asegurar que se completa antes de continuar
                        success: function(data) {
                            if (data) {
                                const textValue = data.codigo + ' - ' + data.nombre;
                                const option = new Option(textValue, currentValue, true, true);
                                $select.append(option).trigger('change');
                                console.log('Account data loaded via AJAX:', textValue);
                            }
                        }
                    });
                }
            }
        });
    }

    // 5. Load Container Data
    function loadContainerData() {
        console.log('Loading container data...');
        
        if (window.contenedoresData && window.contenedoresData.length > 0) {
            console.log('Container data found:', window.contenedoresData);
            
            // Use the existing function from contenedores.js
            if (typeof window.cargarContenedoresExistentes === 'function') {
                window.cargarContenedoresExistentes(window.contenedoresData);
                initState.containers = true;
            } else {
                console.error('cargarContenedoresExistentes function not found, waiting...');
                setTimeout(function() {
                    if (typeof window.cargarContenedoresExistentes === 'function') {
                        window.cargarContenedoresExistentes(window.contenedoresData);
                        initState.containers = true;
                    }
                }, 1000);
            }
        } else {
            console.log('No container data found, adding empty row');
            // Add one empty row if no data
            setTimeout(function() {
                $('#btnAgregarContenedor').click();
            }, 500);
        }
    }

    // 6. Load Supplier Data
    function loadSupplierData() {
        console.log('Loading supplier data...');
        
        if (window.proveedoresData && window.proveedoresData.length > 0) {
            console.log('Supplier data found:', window.proveedoresData);
            
            // Wait for suppliers function to be ready
            setTimeout(function() {
                if (typeof window.cargarProveedoresExistentes === 'function') {
                    window.cargarProveedoresExistentes(window.proveedoresData);
                } else {
                    console.error('cargarProveedoresExistentes function not found');
                }
            }, 1000);
            
            initState.suppliers = true;
        }
    }


    // Format functions for Select2
    function formatCategoriaResult(categoria) {
        if (categoria.loading) return categoria.text;
        if (categoria.id === 'new') {
            return $('<div class="select2-result-categoria">' +
                '<div class="select2-result-categoria__action"><i class="fas fa-plus-circle text-success me-1"></i> ' +
                categoria.text + '</div>' +
                '</div>');
        }
        return $('<div class="select2-result-categoria">' +
            '<div class="select2-result-categoria__name">' + categoria.text + '</div>' +
            '</div>');
    }

    function formatCategoriaSelection(categoria) {
        if (!categoria.id || categoria.id === 'new') return categoria.text;
        
        var $container = $('<div class="d-flex align-items-center justify-content-between w-100"></div>');
        var $categoriaNombre = $('<div>' + categoria.text + '</div>');
        var $actions = $('<div class="categoria-actions ms-2"></div>');
        
        var $editBtn = $('<button type="button" class="btn btn-link btn-sm p-0 me-2 edit-categoria" ' +
            'data-id="' + categoria.id + '" data-name="' + categoria.text + '">' +
            '<i class="fas fa-pencil-alt text-dark"></i></button>');
        
        $actions.append($editBtn);
        $container.append($categoriaNombre);
        $container.append($actions);
        
        return $container;
    }

    function formatMarcaResult(marca) {
        if (marca.loading) return marca.text;
        if (marca.id === 'new') {
            return $('<div class="select2-result-marca">' +
                '<div class="select2-result-marca__action"><i class="fas fa-plus-circle text-success me-1"></i> ' +
                marca.text + '</div>' +
                '</div>');
        }
        return $('<div class="select2-result-marca">' +
            '<div class="select2-result-marca__name">' + marca.text + '</div>' +
            '</div>');
    }

    function formatMarcaSelection(marca) {
        if (!marca.id || marca.id === 'new') return marca.text;
        
        var $container = $('<div class="d-flex align-items-center justify-content-between w-100"></div>');
        var $marcaNombre = $('<div>' + marca.text + '</div>');
        var $actions = $('<div class="marca-actions ms-2"></div>');
        
        var $editBtn = $('<button type="button" class="btn btn-link btn-sm p-0 me-2 edit-marca" ' +
            'data-id="' + marca.id + '" data-name="' + marca.text + '">' +
            '<i class="fas fa-pencil-alt text-dark"></i></button>');
        
        $actions.append($editBtn);
        $container.append($marcaNombre);
        $container.append($actions);
        
        return $container;
    }

    // Initialize Everything
    function initializeAll() {
        console.log('Initializing all components...');
        
        // Basic selects
        initializeCategorySelect();
        initializeBrandSelect();
        initializeTaxSelect();
        initState.basicSelects = true;
        
        // NO inicializar cuentas contables aquí - se hará cuando se abra la pestaña
        // Commenting out: setTimeout(initializeAccountSelects, 500);
        
        // Container and supplier data
        setTimeout(function() {
            loadContainerData();
            loadSupplierData();
        }, 750);
        
        // Debug function
        window.debugEditState = function() {
            console.log('=== EDIT STATE DEBUG ===');
            console.log('Init State:', initState);
            console.log('Tabs Initialized:', tabsInitialized);
            console.log('Category:', $('#CategoriaId').val());
            console.log('Brand:', $('#MarcaId').val());
            console.log('Tax:', $('#ImpuestoId').val());
            console.log('Containers:', $('#contenedores-body tr').length);
            console.log('Suppliers:', $('#proveedores-body tr').length);
            
            // Solo hacer debug de cuentas si la pestaña está activa
            if (initState.accounts) {
                $('#tab-contabilidad .select2-cuenta').each(function() {
                    console.log('Account:', $(this).attr('id'), 'Value:', $(this).val());
                });
            } else {
                console.log('Accounts: Not initialized yet');
            }
        };
    }

    // Start initialization
    initializeAll();
    
    // Tab event handling
    $(document).on('shown.bs.tab', 'a[data-bs-toggle="tab"]', function(e) {
        const tabId = $(e.target).attr('href');
        console.log('Tab activated:', tabId);
        
        // Initialize specific tab content when displayed
        switch(tabId) {
            case '#tab-contabilidad':
                if (!tabsInitialized.contabilidad) {
                    console.log('Inicializando tab contabilidad...');
                    setTimeout(function() {
                        initializeAccountSelects();
                        initState.accounts = true;
                        tabsInitialized.contabilidad = true;
                    }, 300); // Dar tiempo para que el DOM se actualice completamente
                } else {
                    console.log('Tab contabilidad ya inicializado, refrescando selects...');
                    // Refrescar los select2 por si acaso
                    initializeAccountSelects();
                }
                break;
                
            case '#tab-compras':
                // Existing container and supplier initialization
                if (!tabsInitialized.compras) {
                    loadContainerData();
                    loadSupplierData();
                    tabsInitialized.compras = true;
                    initState.containers = true;
                    initState.suppliers = true;
                }
                break;
                
            case '#tab-venta':
                // También inicializar la pestaña de venta si es necesario
                if (!tabsInitialized.venta) {
                    console.log('Inicializando tab venta desde event handler...');
                    setTimeout(function() {
                        if (window.cargarProductoVentaExistente && window.productoVentaData) {
                            window.cargarProductoVentaExistente(window.productoVentaData);
                        }
                        tabsInitialized.venta = true;
                        initState.venta = true;
                    }, 300);
                }
                break;
        }
    });
    
    // Anulamos cualquier otra inicialización que pudiera estar ocurriendo en los scripts inline
    // Lo hacemos definiendo una variable global que otros scripts puedan verificar
    window.selectsAlreadyInitialized = true;
    
    // Debug after everything loads
    setTimeout(function() {
        console.log('Final debug check...');
        window.debugEditState();
    }, 3000);
});