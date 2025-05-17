// Fixed Item Create functionality with correct endpoints
$(document).ready(function() {
    console.log("=== ITEM CREATE FIXED INITIALIZATION ===");
    
    // Variables for tracking newly created items
    let ultimaCategoriaCreada = null;
    let ultimaMarcaCreada = null;
    
    // Initialize everything
    setTimeout(function() {
        generateItemCode();
        initCategorySelect2();
        initBrandSelect2();
        initTaxSelect2();
        initCategoryInheritance();
        initBarcodeGeneration();
        initImageUpload();
    }, 300);
    
    // Generate automatic item code
    function generateItemCode() {
        var $codeField = $('#Codigo');
        if ($codeField.length === 0) return;
        
        // Make field readonly
        $codeField.prop('readonly', true).css('background-color', '#f8f9fa');
        
        // Generate code if empty
        if (!$codeField.val()) {
            $.ajax({
                url: '/Item/GenerarCodigoAutomatico',
                type: 'GET',
                success: function(response) {
                    if (response.success) {
                        $codeField.val(response.codigo);
                        console.log("Code generated:", response.codigo);
                    }
                },
                error: function(xhr) {
                    console.error('Error generating code:', xhr.responseText);
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
                url: '/Item/ObtenerDatosCategoria',
                type: 'GET',
                data: { id: categoriaId },
                success: function(response) {
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
                },
                error: function(xhr) {
                    console.error('Error getting category data:', xhr.responseText);
                    Swal.close();
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
                if ($field.length > 0) {
                    // For Select2 with AJAX, we need to add the option first
                    $.ajax({
                        url: '/Item/BuscarCuentasContables',
                        type: 'GET',
                        data: { term: value, exactId: true },
                        success: function(data) {
                            if (data.results && data.results.length > 0) {
                                var result = data.results[0];
                                var option = new Option(result.text, result.id, true, true);
                                $field.append(option).trigger('change');
                            }
                        }
                    });
                }
            }
        });
    }
    
    // Barcode generation
    function initBarcodeGeneration() {
        $('#generarCodigoBarras').on('click', function() {
            $.ajax({
                url: '/Item/GenerarCodigoBarras',
                type: 'POST',
                success: function(response) {
                    if (response.success) {
                        $('#CodigoBarras').val(response.codigoBarras);
                        generateBarcodeVisualization(response.codigoBarras);
                    } else {
                        Swal.fire('Error', response.message || 'No se pudo generar el código de barras', 'error');
                    }
                },
                error: function(xhr) {
                    console.error('Error generating barcode:', xhr.responseText);
                    Swal.fire('Error', 'Error al generar el código de barras', 'error');
                }
            });
        });
        
        $('#imprimirCodigoBarras').on('click', function(e) {
            e.preventDefault();
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
    
    function printBarcode(codigo) {
        var win = window.open('', '_blank');
        var html = `
            <html>
            <head>
                <title>Código de Barras</title>
                <script src="https://cdn.jsdelivr.net/npm/jsbarcode@3.11.5/dist/JsBarcode.all.min.js"></script>
                <style>
                    body { text-align: center; margin: 20px; }
                    #barcode { margin: 20px auto; }
                </style>
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
                    setTimeout(function() { window.print(); }, 500);
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
        
        var formData = $('#formCategoria').serialize();
        
        $.ajax({
            url: '/Categoria/Create',
            type: 'POST',
            data: formData,
            success: function(response) {
                if (response.success) {
                    // Add new option to select
                    var newOption = new Option(response.nombre, response.id, true, true);
                    $('select[name="CategoriaId"]').append(newOption).trigger('change');
                    
                    ultimaCategoriaCreada = { id: response.id, nombre: response.nombre };
                    
                    // Close offcanvas
                    var offcanvasEl = document.getElementById('offcanvasCategoria');
                    var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                    offcanvas.hide();
                    
                    Swal.fire('Éxito', 'Categoría creada correctamente', 'success');
                } else {
                    Swal.fire('Error', response.message || 'Error al crear la categoría', 'error');
                }
            },
            error: function(xhr) {
                console.error('Error creating category:', xhr.responseText);
                Swal.fire('Error', 'Error al crear la categoría', 'error');
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
            url: '/Marca/Create',
            type: 'POST',
            data: { Name: nombre },
            success: function(response) {
                if (response.success) {
                    // Add new option to select
                    var newOption = new Option(response.name, response.id, true, true);
                    $('select[name="MarcaId"]').append(newOption).trigger('change');
                    
                    ultimaMarcaCreada = { id: response.id, name: response.name };
                    
                    // Close modal
                    var modalEl = document.getElementById('modalNuevaMarca');
                    var modal = bootstrap.Modal.getInstance(modalEl);
                    modal.hide();
                    
                    Swal.fire('Éxito', 'Marca creada correctamente', 'success');
                } else {
                    Swal.fire('Error', response.message || 'Error al crear la marca', 'error');
                }
            },
            error: function(xhr) {
                console.error('Error creating brand:', xhr.responseText);
                Swal.fire('Error', 'Error al crear la marca', 'error');
            }
        });
    });
    
    // Edit category handler
    $(document).on('click', '.edit-category', function(e) {
        e.preventDefault();
        e.stopPropagation();
        var id = $(this).data('id');
        var name = $(this).data('name');
        
        // TODO: Load category data and open offcanvas for editing
        openCategoryOffcanvas(name);
        $('#categoriaId').val(id);
    });
});