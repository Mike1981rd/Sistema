// Initialization script for Accounting tab only
$(document).ready(function() {
    console.log("=== ACCOUNTING TAB INITIALIZATION ===");
    
    // Initialize only when Accounting tab is active
    function initAccountingTab() {
        // Initialize all accounting account Select2s
        $('.select2-cuenta').each(function() {
            var $this = $(this);
            
            // Skip if already initialized
            if ($this.hasClass('select2-hidden-accessible')) {
                return;
            }
            
            $this.select2({
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
                },
                templateResult: function(cuenta) {
                    if (!cuenta.id) return cuenta.text;
                    
                    return $('<div class="select2-result">' +
                        '<span class="badge bg-secondary me-2">' + (cuenta.codigo || '') + '</span>' +
                        '<span>' + (cuenta.nombre || cuenta.text) + '</span>' +
                    '</div>');
                },
                templateSelection: function(cuenta) {
                    return cuenta.text || '';
                }
            });
            
            console.log("✓ Accounting Select2 initialized:", $this.attr('id') || $this.attr('name'));
        });
        
        // Verificar si hay una categoría seleccionada y cargar sus datos automáticamente
        cargarDatosCategoriaActual();
    }
    
    // Initialize on page load if Accounting tab is active
    if ($('#tab-contabilidad').hasClass('active')) {
        initAccountingTab();
    }
    
    // Reinitialize when Accounting tab is shown
    $('a[data-bs-toggle="tab"][href="#tab-contabilidad"]').on('shown.bs.tab', function() {
        console.log("Accounting tab shown - initializing...");
        setTimeout(initAccountingTab, 100);
    });
    
    // Función para cargar datos de la categoría actual
    function cargarDatosCategoriaActual() {
        const categoriaId = $('#CategoriaId').val();
        
        if (!categoriaId || categoriaId === 'new' || categoriaId === '') {
            console.log("No hay categoría seleccionada para cargar datos contables");
            return;
        }
        
        console.log("Cargando datos contables para categoría ID:", categoriaId);
        
        // Mostrar indicador de carga 
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true
        });
        
        Toast.fire({
            icon: 'info',
            title: 'Cargando datos contables...'
        });
        
        // Obtener datos de la categoría seleccionada
        $.ajax({
            url: `/Categoria/ObtenerDetalle/${categoriaId}`,
            type: 'GET',
            success: function(response) {
                if (response && response.success && response.categoria) {
                    const cat = response.categoria;
                    
                    // Mapear la respuesta al formato esperado por el evento categoria:changed
                    const mappedResponse = {
                        success: true,
                        cuentaVentasId: cat.cuentaVentasId,
                        cuentaComprasInventariosId: cat.cuentaComprasInventariosId,
                        cuentaCostoVentasGastosId: cat.cuentaCostoVentasGastosId,
                        cuentaDescuentosId: cat.cuentaDescuentosId,
                        cuentaDevolucionesId: cat.cuentaDevolucionesId,
                        cuentaAjustesId: cat.cuentaAjustesId,
                        cuentaCostoMateriaPrimaId: cat.cuentaCostoMateriaPrimaId,
                        impuestoId: cat.impuestoId
                    };
                    
                    console.log("Datos contables cargados para la categoría:", mappedResponse);
                    
                    // Actualizar campos contables usando la función existente
                    if (mappedResponse.cuentaVentasId) {
                        updateAccountingSelectWithLoad('#CuentaVentasId', mappedResponse.cuentaVentasId);
                    }
                    if (mappedResponse.cuentaComprasInventariosId) {
                        updateAccountingSelectWithLoad('#CuentaComprasInventariosId', mappedResponse.cuentaComprasInventariosId);
                    }
                    if (mappedResponse.cuentaCostoVentasGastosId) {
                        updateAccountingSelectWithLoad('#CuentaCostoVentasGastosId', mappedResponse.cuentaCostoVentasGastosId);
                    }
                    if (mappedResponse.cuentaDescuentosId) {
                        updateAccountingSelectWithLoad('#CuentaDescuentosId', mappedResponse.cuentaDescuentosId);
                    }
                    if (mappedResponse.cuentaDevolucionesId) {
                        updateAccountingSelectWithLoad('#CuentaDevolucionesId', mappedResponse.cuentaDevolucionesId);
                    }
                    if (mappedResponse.cuentaAjustesId) {
                        updateAccountingSelectWithLoad('#CuentaAjustesId', mappedResponse.cuentaAjustesId);
                    }
                    if (mappedResponse.cuentaCostoMateriaPrimaId) {
                        updateAccountingSelectWithLoad('#CuentaCostoMateriaPrimaId', mappedResponse.cuentaCostoMateriaPrimaId);
                    }
                    
                    // Mostrar mensaje de éxito
                    Toast.fire({
                        icon: 'success',
                        title: 'Datos contables aplicados'
                    });
                } else {
                    console.error("Error al obtener datos de la categoría:", response);
                    Toast.fire({
                        icon: 'error',
                        title: 'Error al cargar datos contables'
                    });
                }
            },
            error: function(xhr, status, error) {
                console.error("Error en la solicitud de datos contables:", error);
                Toast.fire({
                    icon: 'error',
                    title: 'Error al cargar datos contables'
                });
            }
        });
    }
});

// Listen for category change to inherit accounting values
$(document).ready(function() {
    $(document).on('categoria:changed', function(e, response) {
        console.log("Category changed - updating accounting fields...", response);
        
        if (response && response.success) {
            // Update accounting fields
            if (response.cuentaVentasId) {
                updateAccountingSelectWithLoad('#CuentaVentasId', response.cuentaVentasId);
            }
            if (response.cuentaComprasInventariosId) {
                updateAccountingSelectWithLoad('#CuentaComprasInventariosId', response.cuentaComprasInventariosId);
            }
            if (response.cuentaCostoVentasGastosId) {
                updateAccountingSelectWithLoad('#CuentaCostoVentasGastosId', response.cuentaCostoVentasGastosId);
            }
            if (response.cuentaDescuentosId) {
                updateAccountingSelectWithLoad('#CuentaDescuentosId', response.cuentaDescuentosId);
            }
            if (response.cuentaDevolucionesId) {
                updateAccountingSelectWithLoad('#CuentaDevolucionesId', response.cuentaDevolucionesId);
            }
            if (response.cuentaAjustesId) {
                updateAccountingSelectWithLoad('#CuentaAjustesId', response.cuentaAjustesId);
            }
            if (response.cuentaCostoMateriaPrimaId) {
                updateAccountingSelectWithLoad('#CuentaCostoMateriaPrimaId', response.cuentaCostoMateriaPrimaId);
            }
        }
    });
    
    // También escuchar 'categoria:seleccionada' para capturar los eventos del item-create-simple.js
    $(document).on('categoria:seleccionada', function(e, categoriaId) {
        console.log("Categoría seleccionada ID:", categoriaId);
        
        if (!categoriaId || categoriaId === 'new') return;
        
        // Cargar datos de la categoría
        $.ajax({
            url: `/Categoria/ObtenerDetalle/${categoriaId}`,
            type: 'GET',
            success: function(response) {
                if (response && response.success && response.categoria) {
                    // Disparar evento categoria:changed con los datos cargados
                    const mappedResponse = {
                        success: true,
                        cuentaVentasId: response.categoria.cuentaVentasId,
                        cuentaComprasInventariosId: response.categoria.cuentaComprasInventariosId,
                        cuentaCostoVentasGastosId: response.categoria.cuentaCostoVentasGastosId,
                        cuentaDescuentosId: response.categoria.cuentaDescuentosId,
                        cuentaDevolucionesId: response.categoria.cuentaDevolucionesId,
                        cuentaAjustesId: response.categoria.cuentaAjustesId,
                        cuentaCostoMateriaPrimaId: response.categoria.cuentaCostoMateriaPrimaId,
                        impuestoId: response.categoria.impuestoId
                    };
                    
                    $(document).trigger('categoria:changed', [mappedResponse]);
                }
            }
        });
    });
});

function updateAccountingSelect(selector, value, text) {
    var $select = $(selector);
    if ($select.length > 0) {
        // Add option if it doesn't exist
        if ($select.find('option[value="' + value + '"]').length === 0) {
            $select.append(new Option(text, value, true, true));
        }
        // Set value
        $select.val(value).trigger('change');
    }
}

// Function to update accounting select with data loading
function updateAccountingSelectWithLoad(selector, value) {
    var $select = $(selector);
    if ($select.length > 0 && value) {
        console.log(`Updating ${selector} with value ${value}`);
        
        // First, add a loading option
        const loadingOption = new Option('Cargando...', value, true, true);
        $select.append(loadingOption);
        $select.val(value).trigger('change');
        
        // Then load the actual data
        $.ajax({
            url: '/api/CuentasContables/Buscar',
            type: 'GET',
            data: { term: value },
            success: function(data) {
                console.log(`Account data loaded for ${value}:`, data);
                
                // Remove the loading option
                $select.find(`option[value="${value}"]`).remove();
                
                // Find the correct account data
                let accountData = null;
                if (data.results && data.results.length > 0) {
                    accountData = data.results.find(item => item.id == value);
                }
                
                if (accountData) {
                    // Add the real option with proper text
                    const newOption = new Option(accountData.text, accountData.id, true, true);
                    $select.append(newOption);
                    $select.val(value).trigger('change');
                    console.log(`Updated ${selector} with: ${accountData.text}`);
                } else {
                    console.log(`No data found for account ID ${value}`);
                }
            },
            error: function(xhr, status, error) {
                console.error(`Error loading account data for ${value}:`, error);
                console.error('Response:', xhr.responseText);
                
                // Update the option to show error
                $select.find(`option[value="${value}"]`).text(`Error cargando cuenta ID: ${value}`);
            }
        });
    }
}