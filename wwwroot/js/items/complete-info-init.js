// Complete initialization for Information tab with all functionality
$(document).ready(function() {
    console.log("=== COMPLETE INFO TAB INITIALIZATION ===");
    
    // Variables for tracking newly created items
    let ultimaCategoriaCreada = null;
    let ultimaMarcaCreada = null;
    
    // Initialize all on page load
    setTimeout(function() {
        initCategorySelect2();
        initBrandSelect2();
        initTaxSelect2();
        initImageUpload();
        initBarcodeGeneration();
        initCategoryInheritance();
        generateItemCode();
    }, 300);
    
    // Image upload functionality
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
    
    // Category Select2 with Create/Edit functionality
    function initCategorySelect2() {
        var $categorySelect = $('select[name="CategoriaId"]');
        if ($categorySelect.length === 0) return;
        
        $categorySelect.select2({
            theme: 'bootstrap-5',
            placeholder: 'Seleccione o cree una categoría',
            allowClear: false,
            width: '100%',
            dropdownParent: $('body'),
            ajax: {
                url: '/Categoria/Buscar',
                dataType: 'json',
                delay: 250,
                data: function(params) {
                    return {
                        term: params.term || ''
                    };
                },
                processResults: function(data, params) {
                    var results = data.results || [];
                    
                    // Add "Create new" option if search term exists
                    if (params.term && params.term.trim() !== '' && results.length === 0) {
                        results.push({
                            id: 'new',
                            text: 'Crear categoría: "' + params.term + '"',
                            term: params.term,
                            _isNew: true
                        });
                    }
                    
                    return {
                        results: results
                    };
                },
                cache: true
            },
            templateResult: formatCategoryResult,
            templateSelection: formatCategorySelection
        }).on('select2:select', function(e) {
            var data = e.params.data;
            
            if (data.id === 'new') {
                $(this).val(null).trigger('change');
                openCategoryOffcanvas(data.term);
            }
        });
    }
    
    // Brand Select2 with Create/Edit functionality
    function initBrandSelect2() {
        var $brandSelect = $('select[name="MarcaId"]');
        if ($brandSelect.length === 0) return;
        
        $brandSelect.select2({
            theme: 'bootstrap-5',
            placeholder: 'Seleccione o cree una marca',
            allowClear: true,
            width: '100%',
            dropdownParent: $('body'),
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
                    
                    // Add "Create new" option if search term exists
                    if (params.term && params.term.trim() !== '' && results.length === 0) {
                        results.push({
                            id: 'new',
                            text: 'Crear marca: "' + params.term + '"',
                            term: params.term,
                            _isNew: true
                        });
                    }
                    
                    return {
                        results: results
                    };
                },
                cache: true
            },
            templateResult: formatBrandResult,
            templateSelection: formatBrandSelection
        }).on('select2:select', function(e) {
            var data = e.params.data;
            
            if (data.id === 'new') {
                $(this).val(null).trigger('change');
                openBrandModal(data.term);
            }
        });
    }
    
    // Tax Select2 - simple initialization
    function initTaxSelect2() {
        var $taxSelect = $('select[name="ImpuestoId"]');
        if ($taxSelect.length === 0) return;
        
        $taxSelect.select2({
            theme: 'bootstrap-5',
            width: '100%',
            placeholder: 'Seleccione un impuesto',
            allowClear: true
        });
    }
    
    // Category inheritance functionality
    function initCategoryInheritance() {
        $('select[name="CategoriaId"]').on('change', function() {
            var categoriaId = $(this).val();
            if (!categoriaId) return;
            
            // Skip if it's a newly created category
            if (ultimaCategoriaCreada && ultimaCategoriaCreada.id == categoriaId) {
                console.log('Skipping processing for newly created category');
                return;
            }
            
            // Get category data via AJAX
            $.ajax({
                url: '/Item/ObtenerDatosCategoria',
                type: 'GET',
                data: { categoriaId: categoriaId },
                success: function(response) {
                    if (response.success) {
                        var data = response.categoria;
                        
                        // Update tax if inherited
                        if (data.impuestoId) {
                            $('select[name="ImpuestoId"]').val(data.impuestoId).trigger('change');
                        }
                        
                        // Update accounting accounts
                        updateAccountingAccounts(data);
                        
                        // Show success message
                        Swal.fire({
                            icon: 'success',
                            title: 'Datos heredados',
                            text: 'Se han heredado los datos de la categoría seleccionada',
                            timer: 2000,
                            showConfirmButton: false
                        });
                    }
                },
                error: function() {
                    console.error('Error getting category data');
                }
            });
        });
    }
    
    // Update accounting accounts from category
    function updateAccountingAccounts(categoryData) {
        // Update each accounting field
        var accountFields = {
            'CuentaVentasId': categoryData.cuentaVentasId,
            'CuentaComprasInventariosId': categoryData.cuentaComprasInventariosId,
            'CuentaCostoVentasGastosId': categoryData.cuentaCostoVentasGastosId,
            'CuentaDescuentosId': categoryData.cuentaDescuentosId,
            'CuentaDevolucionesId': categoryData.cuentaDevolucionesId,
            'CuentaAjustesId': categoryData.cuentaAjustesId,
            'CuentaCostoMateriaPrimaId': categoryData.cuentaCostoMateriaPrimaId
        };
        
        $.each(accountFields, function(fieldName, value) {
            if (value) {
                var $field = $('#' + fieldName);
                if ($field.length > 0) {
                    // For Select2 with AJAX, we need to add the option first
                    $.ajax({
                        url: '/Item/BuscarCuentasContables',
                        data: { term: value, exactId: true },
                        success: function(data) {
                            if (data.results && data.results.length > 0) {
                                var option = new Option(data.results[0].text, data.results[0].id, true, true);
                                $field.append(option).trigger('change');
                            }
                        }
                    });
                }
            }
        });
    }
    
    // Generate automatic item code
    function generateItemCode() {
        var $codeField = $('#Codigo');
        if ($codeField.length === 0) return;
        
        // Make field readonly
        $codeField.prop('readonly', true);
        
        // Generate code if empty
        if (!$codeField.val()) {
            $.ajax({
                url: '/Item/GenerarCodigo',
                type: 'GET',
                success: function(response) {
                    if (response.success) {
                        $codeField.val(response.codigo);
                    }
                },
                error: function() {
                    console.error('Error generating item code');
                }
            });
        }
    }
    
    // Barcode generation
    function initBarcodeGeneration() {
        // Generate barcode button
        $('#generarCodigoBarras').on('click', function() {
            $.ajax({
                url: '/Item/GenerarCodigoBarras',
                type: 'GET',
                success: function(response) {
                    if (response.success) {
                        $('#CodigoBarras').val(response.codigo);
                        generateBarcodeVisualization(response.codigo);
                    }
                },
                error: function() {
                    Swal.fire('Error', 'No se pudo generar el código de barras', 'error');
                }
            });
        });
        
        // Print barcode button
        $('#imprimirCodigoBarras').on('click', function() {
            var codigoBarras = $('#CodigoBarras').val();
            if (!codigoBarras) {
                Swal.fire('Advertencia', 'Debe generar un código de barras primero', 'warning');
                return;
            }
            printBarcode(codigoBarras);
        });
    }
    
    // Generate barcode visualization
    function generateBarcodeVisualization(codigo) {
        if (typeof JsBarcode !== 'undefined' && codigo) {
            JsBarcode("#barcode", codigo, {
                format: "CODE128",
                lineColor: "#000",
                width: 2,
                height: 100,
                displayValue: true
            });
            $('#codigoBarrasPreview').show();
            $('#imprimirCodigoBarras').prop('disabled', false);
        }
    }
    
    // Print barcode
    function printBarcode(codigo) {
        var win = window.open('', '_blank');
        var barcodeHtml = $('#codigoBarrasPreview').html();
        var printHtml = '<html><head><title>Código de Barras</title></head><body>' + barcodeHtml + '</body></html>';
        win.document.write(printHtml);
        win.document.close();
        win.print();
    }
    
    // Format results for Select2
    function formatCategoryResult(category) {
        if (category.loading) return category.text;
        
        if (category.id === 'new') {
            return $('<div class="select2-result-category">' +
                     '<div class="select2-result-category__action"><i class="fas fa-plus-circle text-success me-1"></i> ' + 
                     category.text + '</div>' +
                     '</div>');
        }
        
        return $('<div class="select2-result-category">' +
                 '<div class="select2-result-category__name">' + category.text + '</div>' +
                 '</div>');
    }
    
    function formatCategorySelection(category) {
        if (!category.id || category.id === 'new') {
            return category.text;
        }
        
        // Add edit button to selection
        var $container = $('<div class="d-flex align-items-center justify-content-between w-100"></div>');
        var $categoryName = $('<div>' + category.text + '</div>');
        var $editBtn = $('<button type="button" class="btn btn-link btn-sm p-0 edit-category" ' +
                        'data-id="' + category.id + '" data-name="' + category.text + '">' +
                        '<i class="fas fa-pencil-alt text-primary"></i></button>');
        
        $container.append($categoryName).append($editBtn);
        return $container;
    }
    
    function formatBrandResult(brand) {
        if (brand.loading) return brand.text;
        
        if (brand.id === 'new') {
            return $('<div class="select2-result-brand">' +
                     '<div class="select2-result-brand__action"><i class="fas fa-plus-circle text-success me-1"></i> ' + 
                     brand.text + '</div>' +
                     '</div>');
        }
        
        return $('<div class="select2-result-brand">' +
                 '<div class="select2-result-brand__name">' + brand.text + '</div>' +
                 '</div>');
    }
    
    function formatBrandSelection(brand) {
        if (!brand.id || brand.id === 'new') {
            return brand.text;
        }
        return brand.text;
    }
    
    // Open category offcanvas
    function openCategoryOffcanvas(categoryName) {
        $('#offcanvasCategoriaLabel').text(categoryName ? 'Editar Categoría' : 'Nueva Categoría');
        $('#categoriaNombre').val(categoryName || '');
        $('#categoriaId').val('');
        
        var offcanvas = new bootstrap.Offcanvas(document.getElementById('offcanvasCategoria'));
        offcanvas.show();
    }
    
    // Open brand modal
    function openBrandModal(brandName) {
        $('#modalNuevaMarcaLabel').text('Nueva Marca');
        $('#nuevaMarcaName').val(brandName || '');
        
        var modal = new bootstrap.Modal(document.getElementById('modalNuevaMarca'));
        modal.show();
    }
    
    // Handle category save
    $('#btnGuardarCategoria').on('click', function() {
        var nombre = $('#categoriaNombre').val();
        if (!nombre) {
            Swal.fire('Error', 'El nombre es requerido', 'error');
            return;
        }
        
        $.ajax({
            url: '/Categoria/Create',
            type: 'POST',
            data: { Nombre: nombre },
            success: function(response) {
                if (response.success) {
                    // Add new option to select and select it
                    var newOption = new Option(response.categoria.nombre, response.categoria.id, true, true);
                    $('select[name="CategoriaId"]').append(newOption).trigger('change');
                    
                    // Track as newly created
                    ultimaCategoriaCreada = response.categoria;
                    
                    // Close offcanvas
                    bootstrap.Offcanvas.getInstance(document.getElementById('offcanvasCategoria')).hide();
                    
                    Swal.fire('Éxito', 'Categoría creada correctamente', 'success');
                }
            }
        });
    });
    
    // Handle brand save
    $('#guardarNuevaMarca').on('click', function() {
        var nombre = $('#nuevaMarcaName').val();
        if (!nombre) {
            Swal.fire('Error', 'El nombre es requerido', 'error');
            return;
        }
        
        $.ajax({
            url: '/Marca/Create',
            type: 'POST',
            data: { Name: nombre },
            success: function(response) {
                if (response.success) {
                    // Add new option to select and select it
                    var newOption = new Option(response.marca.name, response.marca.id, true, true);
                    $('select[name="MarcaId"]').append(newOption).trigger('change');
                    
                    // Track as newly created
                    ultimaMarcaCreada = response.marca;
                    
                    // Close modal
                    bootstrap.Modal.getInstance(document.getElementById('modalNuevaMarca')).hide();
                    
                    Swal.fire('Éxito', 'Marca creada correctamente', 'success');
                }
            }
        });
    });
    
    // Edit category handler
    $(document).on('click', '.edit-category', function(e) {
        e.preventDefault();
        e.stopPropagation();
        var id = $(this).data('id');
        var name = $(this).data('name');
        openCategoryOffcanvas(name);
        $('#categoriaId').val(id);
    });
});