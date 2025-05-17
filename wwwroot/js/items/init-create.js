// Función de inicialización de imagen
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

// Funciones de formato para Select2
function formatCuenta(cuenta) {
    if (!cuenta.id) {
        return cuenta.text;
    }
    
    var $resultado = $(
        '<div class="select2-result">' +
            '<span class="badge bg-secondary me-2">' + (cuenta.codigo || '') + '</span>' +
            '<span>' + (cuenta.nombre || cuenta.text) + '</span>' +
        '</div>'
    );
    return $resultado;
}

function formatCuentaSelection(cuenta) {
    if (!cuenta.id) {
        return cuenta.text;
    }
    return cuenta.text;
}

// Inicialización de Select2
function initSelect2Controls() {
    console.log("Inicializando Select2 en Create...");
    
    // Categoría
    if ($('.select2-categoria').length > 0) {
        $('.select2-categoria').select2({
            theme: 'bootstrap-5',
            width: '100%',
            placeholder: 'Seleccione una categoría',
            allowClear: false
        });
        console.log("Select2 categoría inicializado");
    }
    
    // Marca
    if ($('.select2-marca').length > 0) {
        $('.select2-marca').select2({
            theme: 'bootstrap-5',
            width: '100%',
            placeholder: 'Seleccione una marca',
            allowClear: true
        });
        console.log("Select2 marca inicializado");
    }
    
    // Cuentas contables con AJAX
    console.log("Inicializando cuentas contables...");
    $('.select2-cuenta').each(function() {
        const $this = $(this);
        
        if (!$this.hasClass('select2-hidden-accessible')) {
            $this.select2({
                theme: 'bootstrap-5',
                width: '100%',
                placeholder: 'Buscar cuenta contable...',
                allowClear: true,
                minimumInputLength: 1,
                language: {
                    inputTooShort: function() {
                        return "Ingrese al menos 1 caracter para buscar";
                    },
                    noResults: function() {
                        return "No se encontraron resultados";
                    },
                    searching: function() {
                        return "Buscando...";
                    }
                },
                ajax: {
                    url: '/Item/BuscarCuentasContables',
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
                templateResult: formatCuenta,
                templateSelection: formatCuentaSelection
            });
            console.log(`Cuenta ${$this.attr('id')} inicializada`);
        }
    });
    
    // Proveedor con AJAX
    if ($('.select2-proveedor').length > 0) {
        $('.select2-proveedor').select2({
            theme: 'bootstrap-5',
            width: '100%',
            placeholder: 'Buscar proveedor...',
            allowClear: true,
            ajax: {
                url: '/Clientes/Buscar',
                dataType: 'json',
                delay: 250,
                data: function(params) {
                    return {
                        term: params.term,
                        esProveedor: true
                    };
                },
                processResults: function(data) {
                    return {
                        results: $.map(data.results || [], function(item) {
                            return {
                                id: item.id,
                                text: item.text
                            };
                        })
                    };
                }
            }
        });
        console.log("Select2 proveedor inicializado");
    }
    
    // Otros Select2 generales
    $('.select2').each(function() {
        if (!$(this).hasClass('select2-hidden-accessible') && 
            !$(this).hasClass('select2-cuenta') && 
            !$(this).hasClass('select2-categoria') && 
            !$(this).hasClass('select2-marca') &&
            !$(this).hasClass('select2-proveedor')) {
            
            $(this).select2({
                theme: 'bootstrap-5',
                width: '100%',
                placeholder: $(this).find('option:first').text() || 'Seleccione...',
                allowClear: true
            });
        }
    });
    console.log("Select2 generales inicializados");
}

// Inicialización al cargar la página
$(document).ready(function() {
    // Esperar un momento para que todos los scripts estén cargados
    setTimeout(function() {
        console.log("Inicializando controles de Create view...");
        
        // Inicializar Select2
        initSelect2Controls();
        
        // Inicializar función de imagen
        initImageUpload();
        
        // Debug Select2 status si está disponible
        if (typeof debugSelect2Status === 'function') {
            setTimeout(debugSelect2Status, 500);
        }
    }, 100);
});