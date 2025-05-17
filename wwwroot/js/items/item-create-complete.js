// Complete Item Create functionality
$(document).ready(function() {
    console.log("=== ITEM CREATE COMPLETE INITIALIZATION ===");
    
    // Variable to track newly created items
    let ultimaCategoriaCreada = null;
    let ultimaMarcaCreada = null;
    
    // Initialize everything with proper timing
    setTimeout(function() {
        // 1. Generate automatic code
        generateItemCode();
        
        // 2. Initialize Select2 controls
        initCategorySelect2();
        initBrandSelect2();
        initTaxSelect2();
        
        // 3. Setup inheritance from category
        initCategoryInheritance();
        
        // 4. Initialize barcode functionality
        initBarcodeGeneration();
        
        // 5. Initialize image upload
        initImageUpload();
        
        console.log("All initializations complete");
    }, 500);
    
    // Generate automatic item code
    function generateItemCode() {
        var $codeField = $('#Codigo');
        if ($codeField.length === 0) {
            console.warn("Code field not found");
            return;
        }
        
        // Make field readonly
        $codeField.prop('readonly', true).css('background-color', '#f8f9fa');
        
        // Generate code if empty
        if (!$codeField.val()) {
            $.ajax({
                url: '/api/Item/GenerarCodigo',
                type: 'GET',
                success: function(response) {
                    if (response && response.codigo) {
                        $codeField.val(response.codigo);
                        console.log("Code generated:", response.codigo);
                    }
                },
                error: function(xhr) {
                    console.error('Error generating code:', xhr.responseText);
                    // Try alternative endpoint
                    $.get('/Item/GenerarCodigo', function(data) {
                        if (data.success) {
                            $codeField.val(data.codigo);
                        }
                    });
                }
            });
        }
    }
    
    // Initialize Category Select2 with create/edit
    function initCategorySelect2() {
        var $categorySelect = $('select[name="CategoriaId"]');
        if ($categorySelect.length === 0) return;
        
        $categorySelect.select2({
            theme: 'bootstrap-5',
            placeholder: 'Seleccione o cree una categoría',
            allowClear: false,
            width: '100%',
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
                    
                    // Add "Create new" option
                    if (params.term && params.term.trim() !== '') {
                        results.push({
                            id: 'new',
                            text: 'Crear categoría: "' + params.term + '"',
                            term: params.term,
                            _isNew: true
                        });
                    }
                    
                    return { results: results };
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
    
    // Initialize Brand Select2 with create
    function initBrandSelect2() {
        var $brandSelect = $('select[name="MarcaId"]');
        if ($brandSelect.length === 0) return;
        
        $brandSelect.select2({
            theme: 'bootstrap-5',
            placeholder: 'Seleccione o cree una marca',
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
                    
                    // Add "Create new" option
                    if (params.term && params.term.trim() !== '') {
                        results.push({
                            id: 'new',
                            text: 'Crear marca: "' + params.term + '"',
                            term: params.term,
                            _isNew: true
                        });
                    }
                    
                    return { results: results };
                },
                cache: true
            },
            templateResult: formatBrandResult
        }).on('select2:select', function(e) {
            var data = e.params.data;
            
            if (data.id === 'new') {
                $(this).val(null).trigger('change');
                openBrandModal(data.term);
            }
        });
    }
    
    // Initialize Tax Select2
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
    
    // Category inheritance
    function initCategoryInheritance() {
        $('select[name="CategoriaId"]').on('change', function() {
            var categoriaId = $(this).val();
            if (!categoriaId) return;
            
            // Skip newly created categories
            if (ultimaCategoriaCreada && ultimaCategoriaCreada.id == categoriaId) {
                return;
            }
            
            // Show loading
            Swal.fire({
                title: 'Procesando...',
                text: 'Obteniendo datos de la categoría',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });
            
            // Get category data
            $.ajax({
                url: '/api/Categorias/ObtenerDatos/' + categoriaId,
                type: 'GET',
                success: function(response) {
                    if (response) {
                        // Update tax
                        if (response.impuestoId) {
                            $('select[name="ImpuestoId"]').val(response.impuestoId).trigger('change');
                        }
                        
                        // Update accounting accounts
                        updateAccountingAccounts(response);
                        
                        Swal.close();
                        
                        // Show success notification
                        Swal.fire({
                            icon: 'success',
                            title: 'Datos heredados',
                            text: 'Se han actualizado los campos con los datos de la categoría',
                            timer: 2000,
                            showConfirmButton: false
                        });
                    }
                },
                error: function() {
                    // Try alternative endpoint
                    $.get('/Item/ObtenerDatosCategoria', { categoriaId: categoriaId }, function(data) {
                        if (data.success) {
                            updateFromCategory(data.categoria);
                        }
                        Swal.close();
                    });
                }
            });
        });
    }
    
    // Update accounting accounts
    function updateAccountingAccounts(categoryData) {
        var accounts = {
            'CuentaVentasId': categoryData.cuentaVentasId,
            'CuentaComprasInventariosId': categoryData.cuentaComprasInventariosId,
            'CuentaCostoVentasGastosId': categoryData.cuentaCostoVentasGastosId,
            'CuentaDescuentosId': categoryData.cuentaDescuentosId,
            'CuentaDevolucionesId': categoryData.cuentaDevolucionesId,
            'CuentaAjustesId': categoryData.cuentaAjustesId,
            'CuentaCostoMateriaPrimaId': categoryData.cuentaCostoMateriaPrimaId
        };
        
        $.each(accounts, function(fieldId, value) {
            if (value) {
                var $field = $('#' + fieldId);
                if ($field.length > 0 && $field.hasClass('select2-hidden-accessible')) {
                    // Load account data and set value
                    $.get('/api/CuentasContables/Obtener/' + value, function(account) {
                        var option = new Option(account.nombre + ' (' + account.codigo + ')', account.id, true, true);
                        $field.append(option).trigger('change');
                    });
                }
            }
        });
    }
    
    // Alternative update method
    function updateFromCategory(categoryData) {
        // Update tax
        if (categoryData.impuestoId) {
            $('select[name="ImpuestoId"]').val(categoryData.impuestoId).trigger('change');
        }
        
        // Update accounts
        var accountMapping = {
            'CuentaVentasId': categoryData.cuentaVentasId,
            'CuentaComprasInventariosId': categoryData.cuentaComprasInventariosId,
            'CuentaCostoVentasGastosId': categoryData.cuentaCostoVentasGastosId,
            'CuentaDescuentosId': categoryData.cuentaDescuentosId,
            'CuentaDevolucionesId': categoryData.cuentaDevolucionesId,
            'CuentaAjustesId': categoryData.cuentaAjustesId,
            'CuentaCostoMateriaPrimaId': categoryData.cuentaCostoMateriaPrimaId
        };
        
        $.each(accountMapping, function(fieldId, accountId) {
            if (accountId) {
                $('#' + fieldId).val(accountId).trigger('change');
            }
        });
    }
    
    // Barcode generation
    function initBarcodeGeneration() {
        $('#generarCodigoBarras').on('click', function() {
            $.ajax({
                url: '/api/Item/GenerarCodigoBarras',
                type: 'GET',
                success: function(response) {
                    if (response && response.codigo) {
                        $('#CodigoBarras').val(response.codigo);
                        generateBarcodeVisualization(response.codigo);
                    }
                },
                error: function() {
                    // Try alternative endpoint
                    $.get('/Item/GenerarCodigoBarras', function(data) {
                        if (data.success) {
                            $('#CodigoBarras').val(data.codigo);
                            generateBarcodeVisualization(data.codigo);
                        }
                    });
                }
            });
        });
        
        $('#imprimirCodigoBarras').on('click', function() {
            var codigo = $('#CodigoBarras').val();
            if (!codigo) {
                Swal.fire('Error', 'Debe generar un código de barras primero', 'warning');
                return;
            }
            printBarcode(codigo);
        });
        
        // Check for existing barcode
        var existingCode = $('#CodigoBarras').val();
        if (existingCode) {
            generateBarcodeVisualization(existingCode);
        }
    }
    
    function generateBarcodeVisualization(codigo) {
        if (typeof JsBarcode !== 'undefined') {
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
    
    function printBarcode(codigo) {
        var win = window.open('', '_blank');
        var html = `
            <html>
            <head>
                <title>Código de Barras</title>
                <script src="https://cdn.jsdelivr.net/npm/jsbarcode@3.11.5/dist/JsBarcode.all.min.js"></script>
            </head>
            <body>
                <svg id="barcode"></svg>
                <script>
                    JsBarcode("#barcode", "${codigo}", {
                        format: "CODE128",
                        lineColor: "#000",
                        width: 2,
                        height: 100,
                        displayValue: true
                    });
                    window.print();
                </script>
            </body>
            </html>`;
        win.document.write(html);
        win.document.close();
    }
    
    // Image upload
    function initImageUpload() {
        $('#browseButton').on('click', function() {
            $('input[name="ItemImage"]').click();
        });
        
        $('input[name="ItemImage"]').on('change', function(e) {
            var file = e.target.files[0];
            if (file) {
                var reader = new FileReader();
                reader.onload = function(e) {
                    $('#preview').attr('src', e.target.result);
                    $('#imagePreview').show();
                    $('#imageDefault').hide();
                };
                reader.readAsDataURL(file);
            }
        });
    }
    
    // Format functions
    function formatCategoryResult(category) {
        if (category.loading) return category.text;
        
        if (category.id === 'new') {
            return $('<div><i class="fas fa-plus-circle text-success"></i> ' + category.text + '</div>');
        }
        
        return $('<div>' + category.text + '</div>');
    }
    
    function formatCategorySelection(category) {
        if (!category.id || category.id === 'new') {
            return category.text;
        }
        
        // Add edit button
        return $('<div class="d-flex justify-content-between align-items-center w-100">' +
                 '<span>' + category.text + '</span>' +
                 '<button type="button" class="btn btn-link btn-sm p-0 edit-category" data-id="' + category.id + '" data-name="' + category.text + '">' +
                 '<i class="fas fa-pencil-alt"></i></button>' +
                 '</div>');
    }
    
    function formatBrandResult(brand) {
        if (brand.loading) return brand.text;
        
        if (brand.id === 'new') {
            return $('<div><i class="fas fa-plus-circle text-success"></i> ' + brand.text + '</div>');
        }
        
        return $('<div>' + brand.text + '</div>');
    }
    
    // Open Category Offcanvas
    function openCategoryOffcanvas(name) {
        $('#offcanvasCategoriaLabel').text(name ? 'Nueva Categoría' : 'Editar Categoría');
        $('#categoriaNombre').val(name || '');
        
        var offcanvas = new bootstrap.Offcanvas(document.getElementById('offcanvasCategoria'));
        offcanvas.show();
    }
    
    // Open Brand Modal
    function openBrandModal(name) {
        $('#nuevaMarcaName').val(name || '');
        var modal = new bootstrap.Modal(document.getElementById('modalNuevaMarca'));
        modal.show();
    }
    
    // Category save handler
    $('#btnGuardarCategoria').on('click', function() {
        var nombre = $('#categoriaNombre').val();
        if (!nombre) {
            Swal.fire('Error', 'El nombre es requerido', 'error');
            return;
        }
        
        var data = {
            Nombre: nombre,
            // Add other fields as needed
        };
        
        $.ajax({
            url: '/api/Categorias/Crear',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function(response) {
                if (response.id) {
                    var newOption = new Option(response.nombre, response.id, true, true);
                    $('select[name="CategoriaId"]').append(newOption).trigger('change');
                    
                    ultimaCategoriaCreada = response;
                    
                    var offcanvasEl = document.getElementById('offcanvasCategoria');
                    var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                    offcanvas.hide();
                    
                    Swal.fire('Éxito', 'Categoría creada correctamente', 'success');
                }
            },
            error: function() {
                // Alternative endpoint
                $.post('/Categoria/Create', data, function(result) {
                    if (result.success) {
                        location.reload();
                    }
                });
            }
        });
    });
    
    // Brand save handler
    $('#guardarNuevaMarca').on('click', function() {
        var nombre = $('#nuevaMarcaName').val();
        if (!nombre) {
            Swal.fire('Error', 'El nombre es requerido', 'error');
            return;
        }
        
        $.ajax({
            url: '/api/Marcas/Crear',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ Name: nombre }),
            success: function(response) {
                if (response.id) {
                    var newOption = new Option(response.name, response.id, true, true);
                    $('select[name="MarcaId"]').append(newOption).trigger('change');
                    
                    ultimaMarcaCreada = response;
                    
                    var modalEl = document.getElementById('modalNuevaMarca');
                    var modal = bootstrap.Modal.getInstance(modalEl);
                    modal.hide();
                    
                    Swal.fire('Éxito', 'Marca creada correctamente', 'success');
                }
            },
            error: function() {
                // Alternative endpoint
                $.post('/Marca/Create', { Name: nombre }, function(result) {
                    if (result.success) {
                        location.reload();
                    }
                });
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