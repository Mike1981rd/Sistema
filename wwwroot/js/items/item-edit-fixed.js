// Fixed JavaScript for Item Edit
$(document).ready(function() {
    console.log('Item Edit Fixed - Starting initialization');

    // Initialize Select2 for Category and Brand
    function initializeBasicSelects() {
        $('.select2-categoria, .select2-marca').each(function() {
            if (!$(this).hasClass('select2-hidden-accessible')) {
                $(this).select2({
                    theme: 'bootstrap-5',
                    width: '100%',
                    ajax: {
                        url: function() {
                            return $(this).hasClass('select2-categoria') 
                                ? '/api/CuentasContables/GetCategorias' 
                                : '/api/CuentasContables/GetMarcas';
                        },
                        dataType: 'json',
                        processResults: function (data) {
                            return {
                                results: data.map(function(item) {
                                    return {
                                        id: item.id,
                                        text: item.nombre
                                    };
                                })
                            };
                        }
                    }
                });
            }
        });
    }

    // Initialize Select2 for tax
    function initializeTaxSelect() {
        $('.select2').not('.select2-categoria, .select2-marca').each(function() {
            if (!$(this).hasClass('select2-hidden-accessible')) {
                $(this).select2({
                    theme: 'bootstrap-5',
                    width: '100%',
                    ajax: {
                        url: '/api/Impuestos/GetImpuestos',
                        dataType: 'json',
                        processResults: function (data) {
                            return {
                                results: data.map(function(item) {
                                    return {
                                        id: item.id,
                                        text: item.nombre + ' (' + item.tasa + '%)'
                                    };
                                })
                            };
                        }
                    }
                });
            }
        });
    }

    // Initialize Select2 for accounting accounts
    function initializeAccountSelects() {
        $('.select2-cuenta').each(function() {
            const $select = $(this);
            if (!$select.hasClass('select2-hidden-accessible')) {
                let url = '/api/CuentasContables/GetCuentas';
                let tipoCuenta = null;

                if ($select.attr('id').includes('Ventas')) tipoCuenta = 'ventas';
                else if ($select.attr('id').includes('ComprasInventarios')) tipoCuenta = 'compras';
                else if ($select.attr('id').includes('CostoVentasGastos')) tipoCuenta = 'costos';
                else if ($select.attr('id').includes('Descuentos')) tipoCuenta = 'descuentos';
                else if ($select.attr('id').includes('Devoluciones')) tipoCuenta = 'devoluciones';
                else if ($select.attr('id').includes('Ajustes')) tipoCuenta = 'ajustes';
                else if ($select.attr('id').includes('CostoMateriaPrima')) tipoCuenta = 'costos';

                if (tipoCuenta) {
                    url += '?tipoCuenta=' + tipoCuenta;
                }

                $select.select2({
                    theme: 'bootstrap-5',
                    width: '100%',
                    ajax: {
                        url: url,
                        dataType: 'json',
                        processResults: function (data) {
                            return {
                                results: data.map(function(item) {
                                    return {
                                        id: item.id,
                                        text: item.codigo + ' - ' + item.nombre
                                    };
                                })
                            };
                        }
                    }
                });
            }
        });
    }

    // Initialize barcode functionality
    function initializeBarcodeHandling() {
        const $barcodeField = $('#CodigoBarras');
        const $generateBtn = $('#generarCodigoBarras');
        const $printBtn = $('#imprimirCodigoBarras');
        const $preview = $('#codigoBarrasPreview');
        const $barcodeSvg = $('#barcode');

        if ($generateBtn.length && $barcodeField.length) {
            $generateBtn.on('click', function() {
                $.ajax({
                    url: '/Item/GenerarCodigoBarras',
                    type: 'POST',
                    success: function(response) {
                        if (response.success) {
                            $barcodeField.val(response.codigo);
                            generateBarcodePreview(response.codigo);
                        } else {
                            Swal.fire('Error', response.message || 'Error al generar código de barras', 'error');
                        }
                    },
                    error: function() {
                        Swal.fire('Error', 'Error al generar código de barras', 'error');
                    }
                });
            });
        }

        function generateBarcodePreview(code) {
            if (code && $barcodeSvg.length) {
                try {
                    JsBarcode($barcodeSvg[0], code, {
                        format: "CODE128",
                        width: 2,
                        height: 100,
                        displayValue: true
                    });
                    $preview.show();
                    if ($printBtn.length) {
                        $printBtn.prop('disabled', false);
                    }
                } catch (e) {
                    console.error('Error generating barcode:', e);
                }
            }
        }

        // Generate preview for existing barcode
        const existingBarcode = $barcodeField.val();
        if (existingBarcode) {
            generateBarcodePreview(existingBarcode);
        }
    }

    // Handle category change and inheritance
    $('#CategoriaId').on('change', function() {
        const categoriaId = $(this).val();
        if (categoriaId) {
            $.ajax({
                url: '/Item/ObtenerDatosCategoria',
                type: 'GET',
                data: { id: categoriaId },
                success: function(response) {
                    if (response.success) {
                        const data = response.data;
                        
                        // Update tax if present
                        if (data.impuestoId && $('#ImpuestoId').length) {
                            $('#ImpuestoId').val(data.impuestoId).trigger('change');
                        }
                        
                        // Update accounting accounts if present
                        if (data.cuentaVentasId && $('#CuentaVentasId').length) {
                            $('#CuentaVentasId').val(data.cuentaVentasId).trigger('change');
                        }
                        if (data.cuentaComprasInventariosId && $('#CuentaComprasInventariosId').length) {
                            $('#CuentaComprasInventariosId').val(data.cuentaComprasInventariosId).trigger('change');
                        }
                        if (data.cuentaCostoVentasGastosId && $('#CuentaCostoVentasGastosId').length) {
                            $('#CuentaCostoVentasGastosId').val(data.cuentaCostoVentasGastosId).trigger('change');
                        }
                        if (data.cuentaDescuentosId && $('#CuentaDescuentosId').length) {
                            $('#CuentaDescuentosId').val(data.cuentaDescuentosId).trigger('change');
                        }
                        if (data.cuentaDevolucionesId && $('#CuentaDevolucionesId').length) {
                            $('#CuentaDevolucionesId').val(data.cuentaDevolucionesId).trigger('change');
                        }
                        if (data.cuentaAjustesId && $('#CuentaAjustesId').length) {
                            $('#CuentaAjustesId').val(data.cuentaAjustesId).trigger('change');
                        }
                        if (data.cuentaCostoMateriaPrimaId && $('#CuentaCostoMateriaPrimaId').length) {
                            $('#CuentaCostoMateriaPrimaId').val(data.cuentaCostoMateriaPrimaId).trigger('change');
                        }
                    }
                },
                error: function() {
                    console.error('Error loading category data');
                }
            });
        }
    });

    // Initialize category and brand creation buttons
    if ($('.btn-nuevo-categoria').length) {
        $('.btn-nuevo-categoria').on('click', function() {
            $('#offcanvasCategoria').offcanvas('show');
            loadCategoryForm();
        });
    }

    if ($('.btn-nuevo-marca').length) {
        $('.btn-nuevo-marca').on('click', function() {
            $('#offcanvasMarca').offcanvas('show');
            loadBrandForm();
        });
    }

    function loadCategoryForm() {
        $('#formCategoriaContainer').load('/Categoria/_CreatePartial', function() {
            initializeFormValidation('#formCategoria');
        });
    }

    function loadBrandForm() {
        $('#formMarcaContainer').load('/Marca/_Create', function() {
            initializeFormValidation('#formMarca');
        });
    }

    function initializeFormValidation(formSelector) {
        const $form = $(formSelector);
        $form.removeData("validator").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($form);
    }

    // Initialize all components
    initializeBasicSelects();
    initializeTaxSelect();
    initializeAccountSelects();
    initializeBarcodeHandling();

    // Tab change event
    $('a[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
        const target = $(e.target).attr("href");
        console.log('Tab changed to:', target);
        
        // Reinitialize Select2 if needed
        if (target === '#tab-contabilidad') {
            setTimeout(function() {
                $('.select2-cuenta').each(function() {
                    if (!$(this).hasClass('select2-hidden-accessible')) {
                        initializeAccountSelects();
                    }
                });
            }, 100);
        }
    });

    console.log('Item Edit Fixed - Initialization complete');
});