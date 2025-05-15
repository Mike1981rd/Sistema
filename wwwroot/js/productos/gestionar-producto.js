// gestionar-producto.js - Version limpia
// Clonado exactamente del módulo Items

$(document).ready(function() {
    // Solo el código del Select2 de categorías clonado de Items
    initCategoriasSelect2();
    
    // Inicializar Select2 para impuestos
    initImpuestosSelect2();
    
    // Inicializar Select2 para rutas de impresión
    initRutasImpresionSelect2();
    
    // Evento para el botón de edición en la selección (después de seleccionar)
    $(document).on('click', '.edit-categoria', function(e) {
        e.preventDefault();
        e.stopPropagation();
        var id = $(this).data('id');
        var nombre = $(this).data('name');
        editCategoria(id, nombre);
    });
});

function initCategoriasSelect2() {
    // Código clonado exactamente de form.js del módulo Items
    var $categoriaSelect = $('#categoriaId');
    var categoriaInicial = $categoriaSelect.val();
    var categoriaTextoInicial = $categoriaSelect.find('option:selected').text();
    
    $categoriaSelect.select2({
        theme: 'bootstrap-5',
        placeholder: 'Seleccione o cree una categoría',
        allowClear: true,
        width: '100%',
        dropdownParent: $('body'),
        ajax: {
            url: window.location.origin + '/Categoria/Buscar',
            dataType: 'json',
            delay: 250,
            data: function(params) {
                console.log("Enviando búsqueda categoría:", params.term);
                return {
                    term: params.term || ''
                };
            },
            processResults: function(data, params) {
                console.log("Resultados categorías recibidos:", data);
                var results = data.results || [];
                
                if (results.length == 0 && params.term && params.term.trim() !== '') {
                    console.log("Agregando opción para crear nueva categoría:", params.term);
                    results.push({
                        id: 'new',
                        text: 'Crear categoría: "' + params.term + '"',
                        term: params.term,
                        _isNew: true  // Marca para identificar elemento nuevo
                    });
                }
                
                return {
                    results: results
                };
            },
            error: function(xhr, status, error) {
                console.error("Error en la búsqueda de categorías:", error);
                console.error("Estado:", status);
                console.error("Respuesta:", xhr.responseText);
            },
            cache: true
        },
        templateResult: formatCategoriaResult,
        templateSelection: formatCategoriaSelection
    }).on('select2:select', function(e) {
        var data = e.params.data;
        console.log("Categoría seleccionada:", data);
        
        if (data.id === 'new') {
            $(this).val(null).trigger('change');
            console.log("Creando categoría:", data.term);
            abrirOffcanvasCategoria(data.term);
        }
    });
    
    // Si hay un valor inicial, cargarlo en Select2
    if (categoriaInicial && categoriaTextoInicial && categoriaTextoInicial !== 'Seleccione una categoría') {
        console.log('Cargando categoría inicial:', categoriaInicial, categoriaTextoInicial);
        var newOption = new Option(categoriaTextoInicial, categoriaInicial, true, true);
        $categoriaSelect.append(newOption).trigger('change');
    }
}

// Funciones para formatear resultados (clonadas de Items)
function formatCategoriaResult(categoria) {
    if (categoria.loading) {
        return categoria.text;
    }
    
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
    if (!categoria.id || categoria.id === 'new') {
        return categoria.text;
    }
    
    // Añadir iconos de edición junto al nombre
    var $container = $('<div class="d-flex align-items-center justify-content-between w-100"></div>');
    var $categoriaNombre = $('<div>' + categoria.text + '</div>');
    var $actions = $('<div class="categoria-actions ms-2"></div>');
    
    var $editBtn = $('<button type="button" class="btn btn-link btn-sm p-0 me-2 edit-categoria" ' +
                    'data-id="' + categoria.id + '" data-name="' + categoria.text + '">' +
                    '<i class="fas fa-pencil-alt text-primary"></i></button>');
    
    $actions.append($editBtn);
    $container.append($categoriaNombre);
    $container.append($actions);
    
    // Ocultar el botón "x" del Select2 (Clear)
    setTimeout(function() {
        $('.select2-selection__clear').hide();
    }, 100);
    
    // Asignar manejadores de eventos inmediatamente
    $editBtn.on('click', function(e) {
        e.preventDefault();
        e.stopPropagation();
        editCategoria(categoria.id, categoria.text);
        return false;
    });
    
    return $container;
}

// Función para abrir el offcanvas de categoría (clonada de Items)
function abrirOffcanvasCategoria(nombreCategoria) {
    const offcanvasElement = document.getElementById('offcanvasCategoria');
    if (!offcanvasElement) {
        console.error("No se encontró el offcanvas de categoría");
        return;
    }
    
    // Los estilos se aplican mediante CSS, no inline
    offcanvasElement.style.width = '600px';
    const offcanvasBS = bootstrap.Offcanvas.getOrCreateInstance(offcanvasElement);
    offcanvasBS.show();
    
    const formContainer = document.getElementById('formCategoriaContainer');
    if (formContainer) {
        formContainer.innerHTML = '<div class="text-center"><div class="spinner-border text-primary" role="status"></div></div>';
        
        // Cargar el formulario de creación
        $.ajax({
            url: '/Categoria/CreatePartial',
            type: 'GET',
            success: function(response) {
                formContainer.innerHTML = response;
                
                // Inicializar Select2 para familia, impuesto y cuentas
                if ($.fn.select2) {
                    $('#formCategoriaContainer .select2-familia, #formCategoriaContainer .select2-impuesto, #formCategoriaContainer .select2-cuenta').select2({
                        theme: 'bootstrap-5',
                        width: '100%',
                        dropdownParent: $('#offcanvasCategoria')
                    });
                }
                
                // Prellenar el nombre si se proporcionó
                if (nombreCategoria) {
                    setTimeout(function() {
                        const nombreInput = formContainer.querySelector('#CategoriaNombre') || 
                                          formContainer.querySelector('[name="Nombre"]');
                        if (nombreInput) {
                            nombreInput.value = nombreCategoria;
                        }
                    }, 100);
                }
            },
            error: function(xhr, status, error) {
                console.error("Error al cargar el formulario:", error);
                formContainer.innerHTML = '<div class="alert alert-danger">Error al cargar el formulario: ' + error + '</div>';
            }
        });
    }
}

// Función para editar categoría (clonada de Items)
function editCategoria(id, nombre) {
    $('#categoriaId').select2('close');
    console.log("Editando categoría:", id, nombre);
    
    // Validar que el ID sea válido
    if (!id || id === 'new' || isNaN(parseInt(id))) {
        console.error("ID de categoría inválido:", id);
        Swal.fire({ 
            icon: 'warning', 
            title: 'Aviso', 
            text: 'No se puede editar la categoría en este momento. Intente más tarde.',
            footer: 'Sugerencia: Recargue la página si acaba de crear esta categoría.'
        });
        return;
    }
    
    // Mostrar indicador de carga
    Swal.fire({
        title: 'Cargando...',
        text: 'Obteniendo datos de la categoría',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
    
    // Retrasar un poco la carga para asegurar que el elemento esté guardado en el servidor
    setTimeout(() => {
        // Siempre cargar los datos por AJAX antes de abrir el offcanvas
        $.ajax({
            url: `/Categoria/Obtener/${id}`,
            type: 'GET',
            success: function(data) {
                Swal.close();
                const offcanvasElement = document.getElementById('offcanvasCategoria');
                if (!offcanvasElement) {
                    console.error("No se encontró el offcanvas");
                    return false;
                }
                // Los estilos se aplican mediante CSS, no inline
                offcanvasElement.style.width = '600px';
                const offcanvasBS = bootstrap.Offcanvas.getOrCreateInstance(offcanvasElement);
                offcanvasBS.show();
                const formContainer = document.getElementById('formCategoriaContainer');
                if (formContainer) {
                    formContainer.innerHTML = '<div class="text-center"><div class="spinner-border text-primary" role="status"></div></div>';
                    // Cargar el formulario de edición con los datos recibidos
                    $.ajax({
                        url: `/Categoria/EditPartial/${id}`,
                        type: 'GET',
                        success: function(response) {
                            formContainer.innerHTML = response;
                            
                            // Inicializar Select2 para familia, impuesto y cuentas
                            if ($.fn.select2) {
                                $('#formCategoriaContainer .select2-familia, #formCategoriaContainer .select2-impuesto, #formCategoriaContainer .select2-cuenta').select2({
                                    theme: 'bootstrap-5',
                                    width: '100%',
                                    dropdownParent: $('#offcanvasCategoria')
                                });
                            }
                            
                            // Prellenar campos si es necesario
                            setTimeout(function() {
                                let nombreInput = formContainer.querySelector('[name="Nombre"]') || formContainer.querySelector('#Nombre') || formContainer.querySelector('#CategoriaNombre');
                                if (nombreInput && data.nombre) {
                                    nombreInput.value = data.nombre;
                                }
                            }, 100);
                        },
                        error: function(xhr, status, error) {
                            console.error("Error al cargar el formulario de edición:", error, xhr.status);
                            let mensaje = "Error al cargar el formulario de edición.";
                            if (xhr.status === 404) {
                                mensaje += " Es posible que la categoría no exista o esté siendo procesada.";
                            }
                            formContainer.innerHTML = `<div class="alert alert-danger">${mensaje}<br><small>Por favor, recargue la página e intente nuevamente.</small></div>`;
                        }
                    });
                }
            },
            error: function(xhr, status, error) {
                Swal.close();
                console.error("Error al obtener categoría:", error, xhr.status);
                let mensaje = "No se pudo cargar la categoría para editar.";
                if (xhr.status === 404) {
                    mensaje += " Es posible que la categoría no exista o acabe de ser creada.";
                }
                Swal.fire({ 
                    icon: 'error', 
                    title: 'Error', 
                    text: mensaje,
                    footer: 'Por favor, recargue la página e intente nuevamente.'
                });
            }
        });
    }, 500); // Retraso de 500ms para asegurar que el elemento esté guardado
}

// Función para inicializar Select2 de impuestos
function initImpuestosSelect2() {
    var $impuestoSelect = $('#impuestoId');
    
    $impuestoSelect.select2({
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
        language: {
            noResults: function() {
                return "No se encontraron resultados";
            },
            searching: function() {
                return "Buscando...";
            }
        }
    });
}

// Función para inicializar Select2 de rutas de impresión
function initRutasImpresionSelect2() {
    var $rutaSelect = $('#rutaImpresoraId');
    
    $rutaSelect.select2({
        theme: 'bootstrap-5',
        placeholder: 'Seleccione una ruta de impresión',
        allowClear: true,
        width: '100%',
        language: {
            noResults: function() {
                return "No se encontraron rutas de impresión";
            }
        }
    });
}

// IMPORTANTE: Exponer las funciones globalmente para que funcionen los eventos onclick
window.editCategoria = editCategoria;
window.abrirOffcanvasCategoria = abrirOffcanvasCategoria;