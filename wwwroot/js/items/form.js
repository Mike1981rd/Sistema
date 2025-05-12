/**
 * Items Form JS
 * Maneja la funcionalidad del formulario de items, incluyendo:
 * - Select2 para categorías, marcas, proveedores, etc.
 * - Manejo de conversiones entre unidades (Contenedores)
 * - Gestión de proveedores
 * - Distribución en almacenes
 * - Configuración de taras
 * - Funcionalidad de producto de venta
 * - Offcanvas para categorías y proveedores
 * - Código de barras y imagen
 */

/**
 * Función para diagnosticar problemas de conexión con el API
 */
function diagnosticarConexion() {
    console.log('Diagnóstico de conexión iniciado...');
    $.get('../api/status')
        .done(function(data) {
            console.log('Conexión exitosa al API:', data);
        })
        .fail(function(xhr, status, error) {
            console.error('Error de conexión al API:', status, error);
            console.log('Respuesta del servidor:', xhr.responseText);
            console.log('URL actual:', window.location.href);
            console.log('Ruta base:', window.location.origin);
        });
        
    // Diagnosticar endpoints específicos
    console.log('Diagnosticando endpoints...');
    
    // Verificar endpoint Categoria/ObtenerTodas
    $.get('../Categoria/ObtenerTodas')
        .done(function(data) {
            console.log('Endpoint Categoria/ObtenerTodas OK, respuesta:', data);
        })
        .fail(function(xhr, status, error) {
            console.error('Error endpoint Categoria/ObtenerTodas:', status, error);
            console.log('Respuesta:', xhr.responseText);
        });
        
    // Verificar endpoint Impuestos/ObtenerTodos
    $.get('../Impuestos/ObtenerTodos')
        .done(function(data) {
            console.log('Endpoint Impuestos/ObtenerTodos OK, respuesta:', data);
        })
        .fail(function(xhr, status, error) {
            console.error('Error endpoint Impuestos/ObtenerTodos:', status, error);
            console.log('Respuesta:', xhr.responseText);
        });

    // Verificar endpoint UnidadMedida/ObtenerTodas
    $.get('../UnidadMedida/ObtenerTodas')
        .done(function(data) {
            console.log('Endpoint UnidadMedida/ObtenerTodas OK, respuesta:', data);
        })
        .fail(function(xhr, status, error) {
            console.error('Error endpoint UnidadMedida/ObtenerTodas:', status, error);
            console.log('Respuesta:', xhr.responseText);
        });
        
    // Verificar endpoint Almacen/ObtenerTodos
    $.get('../Almacen/ObtenerTodos')
        .done(function(data) {
            console.log('Endpoint Almacen/ObtenerTodos OK, respuesta:', data);
        })
        .fail(function(xhr, status, error) {
            console.error('Error endpoint Almacen/ObtenerTodos:', status, error);
            console.log('Respuesta:', xhr.responseText);
        });
}

// Al principio del archivo, después de las primeras declaraciones
var selectsInitialized = false;

// Replace the document ready at the end of the file with this single consolidated version
// Al finalizar el archivo, después de todas las definiciones de funciones
$(document).ready(function() {
    // Eliminar todos los offcanvas existentes para evitar duplicados
    $('#offcanvasCategoria, #offcanvasImpuesto').remove();
    
    // Diagnóstico de conexión y configuración de página
    diagnosticarConexion();
    
    // Configuración global
    const separadorDecimal = $('#Rendimiento').data('decimal') || ',';
    const locale = separadorDecimal === ',' ? 'es' : 'en';
    
    // Ocultar los botones + del HTML original
    $('.btn[data-bs-target="#offcanvasCategoria"], .btn[data-bs-target="#offcanvasMarca"]').hide();
    
    // Aplicar estilos personalizados para select2
    aplicarEstilosSelect2();
    
    // Inicializaciones
    inicializarSelect2();
    initSelects(); // Inicializa otros selectores que no son categoría ni marca
    
    // Cargar datos iniciales
    console.log('Inicializando formulario de item...');
    loadInitialData();
    
    // Inicialización de componentes
    initComponents();
    
    // Eventos
    setupEvents();
    
    // Asegurarse de que se inicialicen los eventos de impuestos
    setupImpuestosEvents();
    
    // Formatear campos numéricos
    formatearCamposNumericos();
    
    // Eliminar cualquier botón + que se haya añadido fuera del select2
    setTimeout(function() {
        // Buscamos botones + que no sean parte de la interfaz de select2
        $('.select2-container-wrapper .btn, .select2-container + .btn').not('.select2-selection__choice__remove').each(function() {
            // Verificar si realmente es un botón externo no deseado
            if ($(this).closest('.select2-container').length === 0 && $(this).find('.fa-plus, .fas.fa-plus-circle').length > 0) {
                $(this).remove();
                console.log('Botón + externo eliminado');
            }
        });
    }, 500);
    
    console.log('Inicialización del formulario completada.');
});

/**
 * Implementación completa para Select2 con botón para "Agregar Nuevo" en categoría y marca
 */
function initSelect2WithAddButton() {
    console.log("Inicializando select2 con botón 'Agregar nuevo'...");
    
    // 1. PRIMERO: Limpiar cualquier inicialización previa
    $('.select2-categoria, .select2-marca').off().select2('destroy');
    
    // 2. Crear contenedores personalizados para los select
    $('.select2-categoria, .select2-marca').each(function() {
        const $select = $(this);
        const id = $select.attr('id');
        const isCategoria = $select.hasClass('select2-categoria');
        const displayName = isCategoria ? 'Categoría' : 'Marca';
        
        // Crear un contenedor si no existe
        if (!$select.parent().hasClass('select2-container-wrapper')) {
            $select.wrap('<div class="select2-container-wrapper position-relative"></div>');
            
            // Agregar botón directamente junto al select
            $select.after(`
                <button type="button" class="btn btn-sm btn-primary position-absolute ${isCategoria ? 'btn-add-categoria' : 'btn-add-marca'}" 
                    style="right: 40px; top: 5px; z-index: 1000; padding: 2px 8px; font-size: 0.7rem;">
                    <i class="fas fa-plus"></i> Nueva
                </button>
            `);
        }
    });
    
    // 3. Inicializar select2 con configuraciones básicas
    $('.select2-categoria').select2({
        theme: 'bootstrap-5',
        width: '100%',
        placeholder: 'Seleccione una categoría',
        allowClear: true
    });
    
    $('.select2-marca').select2({
        theme: 'bootstrap-5', 
        width: '100%',
        placeholder: 'Seleccione una marca',
        allowClear: true
    });
    
    // 4. Agregar eventos para los botones
    $('.btn-add-categoria').off('click').on('click', function(e) {
        e.preventDefault();
        e.stopPropagation();
        crearCategoria();
    });
    
    $('.btn-add-marca').off('click').on('click', function(e) {
        e.preventDefault();
        e.stopPropagation();
        crearMarca();
    });
    
    // 5. Aplicar algo de CSS para mostrar correctamente los botones
    // Si el CSS no se aplica correctamente, podríamos inyectarlo directamente
    $("<style>")
        .prop("type", "text/css")
        .html(`
            .select2-container-wrapper {
                display: block;
                position: relative;
            }
            .select2-container--bootstrap-5 {
                display: block;
                width: 100% !important;
            }
            .btn-add-categoria, .btn-add-marca {
                position: absolute;
                right: 40px;
                top: 5px;
                z-index: 1000;
                padding: 2px 8px;
                font-size: 0.7rem;
                line-height: 1.2;
            }
        `)
        .appendTo("head");
    
    console.log("Select2 con botón 'Agregar nuevo' inicializado");
}

/**
 * Implementación completa de cargarCategorias
 */
function cargarCategorias(categoriaIdToSelect) {
    console.log('Cargando categorías...');
    
    // Obtener el elemento select
    const $select = $('.select2-categoria');
    
    // Limpiar y mostrar indicador de carga
    if ($select.hasClass('select2-hidden-accessible')) {
        $select.empty();
        $select.append(new Option('Cargando...', '')).trigger('change');
    } else {
        $select.empty();
        $select.append(new Option('Cargando...', ''));
    }
    
    // Hacer la solicitud AJAX
    $.ajax({
        url: '../Categoria/ObtenerTodas',
        type: 'GET',
        dataType: 'json',
        success: function(categorias) {
            console.log('Categorías cargadas exitosamente:', categorias);
            
            // Limpiar selector
            $select.empty();
            
            // Opción vacía para placeholder
            $select.append(new Option('', ''));
            
            // Agregar opción para "Agregar Nueva"
            const nuevaOption = new Option('Agregar Nueva Categoría', 'nueva');
            $select.append(nuevaOption);
            
            // Agregar categorías
            if (categorias && categorias.length > 0) {
                categorias.forEach(function(categoria) {
                    const option = new Option(categoria.nombre, categoria.id);
                    $(option).data('data', categoria);
                    $select.append(option);
                });
                
                // Seleccionar categoría específica si se indicó
                if (categoriaIdToSelect) {
                    $select.val(categoriaIdToSelect).trigger('change');
                }
            }
            
            // Actualizar select2
            $select.trigger('change');
        },
        error: function(xhr, status, error) {
            console.error('Error cargando categorías:', error);
            console.log('Estado:', status);
            console.log('Respuesta:', xhr.responseText);
            
            // Limpiar selector
            $select.empty();
            
            // Opción vacía para placeholder
            $select.append(new Option('', ''));
            
            // Opción para "Agregar Nueva"
            const nuevaOption = new Option('Agregar Nueva Categoría', 'nueva');
            $select.append(nuevaOption);
            
            // Datos estáticos como fallback
            cargarDatosEstaticos('categoria', $select);
            
            // Notificar al usuario
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No se pudieron cargar las categorías',
                toast: true,
                position: 'top-end',
                showConfirmButton: false,
                timer: 3000
            });
        }
    });
}

/**
 * Implementación completa de cargarMarcas
 */
function cargarMarcas(marcaIdToSelect) {
    console.log('Cargando marcas...');
    
    // Obtener el elemento select
    const $select = $('.select2-marca');
    
    // Limpiar y mostrar indicador de carga
    if ($select.hasClass('select2-hidden-accessible')) {
        $select.empty();
        $select.append(new Option('Cargando...', '')).trigger('change');
    } else {
        $select.empty();
        $select.append(new Option('Cargando...', ''));
    }
    
    // Hacer la solicitud AJAX
    $.ajax({
        url: '../Marca/ObtenerTodas',
        type: 'GET',
        dataType: 'json',
        success: function(marcas) {
            console.log('Marcas cargadas exitosamente:', marcas);
            
            // Limpiar selector
            $select.empty();
            
            // Opción vacía para placeholder
            $select.append(new Option('', ''));
            
            // Agregar opción para "Agregar Nueva"
            const nuevaOption = new Option('Agregar Nueva Marca', 'nueva');
            $select.append(nuevaOption);
            
            // Agregar marcas
            if (marcas && marcas.length > 0) {
                marcas.forEach(function(marca) {
                    const option = new Option(marca.nombre, marca.id);
                    $(option).data('data', marca);
                    $select.append(option);
                });
                
                // Seleccionar marca específica si se indicó
                if (marcaIdToSelect) {
                    $select.val(marcaIdToSelect).trigger('change');
                }
            }
            
            // Actualizar select2
            $select.trigger('change');
        },
        error: function(xhr, status, error) {
            console.error('Error cargando marcas:', error);
            console.log('Estado:', status);
            console.log('Respuesta:', xhr.responseText);
            
            // Limpiar selector
            $select.empty();
            
            // Opción vacía para placeholder
            $select.append(new Option('', ''));
            
            // Opción para "Agregar Nueva"
            const nuevaOption = new Option('Agregar Nueva Marca', 'nueva');
            $select.append(nuevaOption);
            
            // Datos estáticos como fallback
            cargarDatosEstaticos('marca', $select);
            
            // Notificar al usuario
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No se pudieron cargar las marcas',
                toast: true,
                position: 'top-end',
                showConfirmButton: false,
                timer: 3000
            });
        }
    });
}

/**
 * Diagnóstico de conexión mejorado
 */
function diagnosticarConexionSelect2() {
    console.log("Realizando diagnóstico de select2 y conexiones...");
    
    // Verificar jQuery y Select2
    console.log("Versión jQuery:", $.fn.jquery);
    console.log("Select2 disponible:", (typeof $.fn.select2 === 'function'));
    
    // Verificar selectores
    console.log("Elemento select2-categoria encontrado:", $('.select2-categoria').length);
    console.log("Elemento select2-marca encontrado:", $('.select2-marca').length);
    
    // Probar conexión a endpoints críticos
    $.ajax({
        url: '../api/status',
        type: 'GET',
        success: function() { console.log("Conexión API: OK"); },
        error: function(xhr) { console.error("Conexión API fallida:", xhr.status); }
    });
    
    $.ajax({
        url: '../Categoria/ObtenerTodas',
        type: 'GET',
        success: function(data) { console.log("Endpoint Categoría: OK,", data.length, "registros"); },
        error: function(xhr) { console.error("Endpoint Categoría fallido:", xhr.status); }
    });
    
    $.ajax({
        url: '../Marca/ObtenerTodas',
        type: 'GET',
        success: function(data) { console.log("Endpoint Marca: OK,", data.length, "registros"); },
        error: function(xhr) { console.error("Endpoint Marca fallido:", xhr.status); }
    });
}

/**
 * Inicializa todos los selectores Select2 del formulario
 */
function initSelects() {
    console.log('Inicializando select2 para los demás selectores...');
    
    // Ocultar los botones originales "+", no los eliminaremos para mantener compatibilidad
    $('#btnNuevaCategoria, #btnNuevaMarca').hide();
    
    // Configuración común para todos los select2
    const select2Config = {
        theme: 'bootstrap-5',
        width: '100%'
    };
    
    // Inicializar impuesto
    $('.select2-impuesto').select2({
        ...select2Config,
        templateResult: formatImpuesto,
        templateSelection: formatImpuestoSelection,
        escapeMarkup: function(markup) {
            return markup;
        }
    });
    
    // Inicializar unidades de medida
    $('.select2-uom').select2(select2Config);
    
    // Inicializar otros selectores, excluyendo los de categoría y marca
    $('.select2').not('.select2-categoria, .select2-marca').select2(select2Config);
    
    console.log('Select2 estándar inicializado correctamente');
}

/**
 * Carga los datos iniciales necesarios
 */
function loadInitialData() {
    console.log('Cargando datos iniciales...');
    
    // Cargar categorías
    cargarCategorias();
    
    // Cargar marcas
    cargarMarcas();
    
    // Cargar impuestos
    cargarImpuestos();
    
    // Verificar si estamos en modo edición
    const itemId = $('#Id').val();
    if (itemId && itemId > 0) {
        console.log('Modo edición - cargando datos existentes...');
        // Cargar datos de ítem existente
        cargarDatosItem(itemId);
    } else {
        console.log('Modo creación - preparando formulario...');
        // Cargar almacenes disponibles para nuevo ítem
        cargarAlmacenesDisponibles();
        // Agregar contenedor base
        agregarContenedorBase();
    }
}

/**
 * Inicializa los componentes UI del formulario
 */
function initComponents() {
    initTabs();
    initImageUpload();
    initCodigoBarras();
}

/**
 * Configura todos los eventos para los elementos interactivos
 */
function setupEvents() {
    setupCategoriaEvents();
    setupMarcaEvents();
    setupContenedoresEvents();
    setupProveedoresEvents();
    setupAlmacenesEvents();
    setupTarasEvents();
    setupProductoVentaEvents();
    setupFormSubmit();
}

/**
 * Configura eventos para proveedores
 */
function setupProveedoresEvents() {
    console.log('Configurando eventos para proveedores...');
    
    // Botón para agregar proveedor
    $('#btnAgregarProveedor').off('click').on('click', function() {
        agregarProveedor();
    });
    
    // Cambio en selector de proveedor
    $(document).off('change', '.select2-proveedor').on('change', '.select2-proveedor', function() {
        const $item = $(this).closest('.proveedor-item');
        const proveedorId = $(this).val();
        
        if (proveedorId) {
            cargarDatosProveedor(proveedorId, $item);
        }
    });
    
    // Cambios en precio y factor para calcular precio unitario
    $(document).off('input', '.precio-compra, .factor-conversion').on('input', '.precio-compra, .factor-conversion', function() {
        const $item = $(this).closest('.proveedor-item');
        calcularPrecioUnitario($item);
    });
    
    // Botón para eliminar proveedor
    $(document).off('click', '.btn-eliminar-proveedor').on('click', '.btn-eliminar-proveedor', function() {
        const $item = $(this).closest('.proveedor-item');
        
        // Pedir confirmación
        Swal.fire({
            title: '¿Está seguro?',
            text: "Esto eliminará el proveedor para este item",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                // Eliminar proveedor
                $item.remove();
                
                // Renumerar proveedores
                renumerarProveedores();
            }
        });
    });
}

/**
 * Configura eventos para taras
 */
function setupTarasEvents() {
    console.log('Configurando eventos para taras...');
    
    // Cambios en valor de tara
    $(document).off('input', '[name^="Taras"][name$=".ValorTara"]').on('input', '[name^="Taras"][name$=".ValorTara"]', function() {
        // Si se requiere alguna acción al cambiar el valor de tara
    });
}

// Funciones para inicialización

/**
 * Agrega una fila de tara a la tabla
 */
function agregarFilaTara(index, data) {
    // Obtener template
    const template = $('#taraTemplate').html();
    const taraIndex = typeof index !== 'undefined' ? index : $('#tablaTaras tbody tr').length;

    // Reemplazar valores
    let taraHtml = template
        .replace(/INDEX/g, taraIndex)
        .replace(/CONTENEDOR_ID/g, data.itemContenedorId)
        .replace(/NOMBRE_CONTENEDOR/g, data.nombreContenedor);

    const $fila = $(taraHtml);

    // Si hay datos, rellenar campos
    if (data) {
        $fila.find('input[name^="Taras[' + taraIndex + '].ValorTara"]').val(data.valorTara || 0);
        $fila.find('input[name^="Taras[' + taraIndex + '].Observacion"]').val(data.observacion || '');
    }

    // Agregar a la tabla
    $('#tablaTaras tbody').append($fila);
}

// Funciones de formato para categoría
function formatCategoria(categoria) {
    if (!categoria.id) {
        return categoria.text;
    }
    
    // Opción especial para crear nueva categoría
    if (categoria.id === 'nueva') {
        return $('<div class="select2-nueva-opcion"><i class="fas fa-plus-circle text-primary me-1"></i> Agregar Nueva Categoría</div>');
    }
    
    // Formato normal para categorías existentes
    return $('<div>' + categoria.text + '</div>');
}

function formatCategoriaSelection(categoria) {
    if (!categoria.id || categoria.id === 'nueva') {
        return categoria.text;
    }
    
    // Crear contenedor con botón de edición
    var $contenedor = $(
        '<div class="d-flex justify-content-between align-items-center w-100">' +
            '<span>' + categoria.text + '</span>' +
            '<button type="button" class="btn-editar-categoria btn btn-sm btn-link p-0 ms-2" data-id="' + categoria.id + '">' +
                '<i class="fas fa-pencil-alt"></i>' +
            '</button>' +
        '</div>'
    );
    
    // Agregar evento al botón de edición
    $contenedor.find('.btn-editar-categoria').on('mousedown', function(e) {
        e.preventDefault();
        e.stopPropagation();
        var categoriaId = $(this).data('id');
        setTimeout(function() {
            editarCategoria(categoriaId);
        }, 100);
    });
    
    return $contenedor;
}

// Funciones de formato para impuesto
function formatImpuesto(impuesto) {
    if (!impuesto.id) {
        return impuesto.text;
    }
    // Si es la opción para crear nuevo impuesto
    if (impuesto.id === 'new') {
        return $('<div><i class="fas fa-plus-circle text-primary me-1"></i> ' + impuesto.text + '</div>');
    }
    return $('<div>' + impuesto.text + '</div>');
}

function formatImpuestoSelection(impuesto) {
    if (!impuesto.id) {
        return impuesto.text;
    }
    
    // Si es la opción para crear nuevo impuesto
    if (impuesto.id === 'new') {
        return impuesto.text;
    }
    
    try {
        // Contenedor para el nombre y botón de edición
        var $container = $('<div class="d-flex justify-content-between align-items-center w-100"></div>');
        var $nombre = $('<div>' + impuesto.text + '</div>');
        var $btnEditar = $('<button type="button" class="btn btn-sm btn-outline-primary btn-editar-impuesto ms-2" data-id="' + impuesto.id + '"><i class="fas fa-pencil-alt"></i></button>');
        
        $container.append($nombre);
        $container.append($btnEditar);
        
        return $container;
    } catch (e) {
        console.error('Error en formatImpuestoSelection:', e);
        return impuesto.text;
    }
}

/**
 * Inicializa todos los selectores Select2 del formulario
 */
function initSelects() {
    console.log('Inicializando select2 para categorías y marcas...');
    
    // Ocultar los botones originales "+", no los eliminaremos para mantener compatibilidad
    $('#btnNuevaCategoria, #btnNuevaMarca').hide();
    
    // Configuración común para todos los select2
    const select2Config = {
        theme: 'bootstrap-5',
        width: '100%'
    };
    
    // Inicializar categoría con formato personalizado y botón "+" interno
    $('.select2-categoria').select2({
        ...select2Config,
        templateResult: formatCategoria,
        templateSelection: formatCategoriaSelection,
        escapeMarkup: function(markup) {
            return markup;
        }
    }).on('select2:open', function() {
        console.log('Select2 dropdown abierto, intentando agregar botón de creación...');
        // Esperar a que el dropdown esté completamente renderizado (aumenta el timeout)
        setTimeout(function() {
            // Usar un selector más específico y verificar si ya existe
            var $dropdown = $('.select2-container--open .select2-results');
            if ($dropdown.find('.add-new-categoria').length === 0) {
                $dropdown.append('<div class="add-new-categoria p-2 border-top mt-1"><button type="button" class="btn btn-sm btn-primary w-100"><i class="fas fa-plus me-1"></i> Nueva Categoría</button></div>');
                console.log('Botón de creación agregado al dropdown:', $dropdown.find('.add-new-categoria').length);
            }
        }, 50); // Aumentar el tiempo de espera para asegurar que el dropdown esté completamente renderizado
    });
    
    // Inicializar marca con formato personalizado y botón "+" interno
    $('.select2-marca').select2({
        ...select2Config,
        templateResult: formatMarca,
        templateSelection: formatMarcaSelection,
        escapeMarkup: function(markup) {
            return markup;
        }
    }).on('select2:open', function() {
        console.log('Select2 dropdown abierto, intentando agregar botón de creación...');
        setTimeout(function() {
            var $dropdown = $('.select2-container--open .select2-results');
            if ($dropdown.find('.add-new-marca').length === 0) {
                $dropdown.append('<div class="add-new-marca p-2 border-top mt-1"><button type="button" class="btn btn-sm btn-primary w-100"><i class="fas fa-plus me-1"></i> Nueva Marca</button></div>');
                console.log('Botón de creación agregado al dropdown:', $dropdown.find('.add-new-marca').length);
            }
        }, 50);
    });
    
    // Inicializar impuesto
    $('.select2-impuesto').select2({
        ...select2Config,
        templateResult: formatImpuesto,
        templateSelection: formatImpuestoSelection,
        escapeMarkup: function(markup) {
            return markup;
        }
    });
    
    // Inicializar unidades de medida
    $('.select2-uom').select2(select2Config);
    
    // Inicializar otros selectores
    $('.select2').select2(select2Config);
    
    // Simplifica los event handlers y usa delegación:
    $(document).off('click', '.add-new-categoria button, .add-new-marca button').on('click', '.add-new-categoria button, .add-new-marca button', function(e) {
        e.preventDefault();
        e.stopPropagation();
        
        // Determinar qué tipo de elemento se está creando
        var isCategoria = $(this).closest('.add-new-categoria').length > 0;
        
        // Cerrar todos los dropdowns abiertos de Select2
        $('.select2-hidden-accessible').select2('close');
        
        // Crear el elemento correspondiente
        if (isCategoria) {
            crearCategoria();
        } else {
            crearMarca();
        }
    });
    
    // Añadir estilos CSS para el botón personalizado
    $("<style>")
        .prop("type", "text/css")
        .html(`
            .add-new-categoria, .add-new-marca {
                border-top: 1px solid #e4e4e4 !important;
                padding: 8px !important;
                background-color: #f8f9fa;
                transition: background-color 0.2s;
            }
            .add-new-categoria:hover, .add-new-marca:hover {
                background-color: #f0f0f0;
            }
            .select2-container--open .select2-results {
                padding-bottom: 0 !important;
            }
        `)
        .appendTo("head");
    
    console.log('Select2 inicializado correctamente');
}

/**
 * Inicializa las pestañas y su navegación
 */
function initTabs() {
    // (Implementación pendiente o personalizada según necesidades)
}

// Manejar cambio de pestañas
$('.nav-tabs-underline .nav-link').on('click', function(e) {
    e.preventDefault();
    // Remover la clase active de todas las pestañas y contenidos
    $('.nav-tabs-underline .nav-link').removeClass('active');
    $('.tab-pane').removeClass('show active');

    $(this).addClass('active');
    const targetId = $(this).attr('href');
    $(targetId).addClass('show active');
});

// Validar campos requeridos al cambiar de pestaña
$('.nav-tabs-underline .nav-link').on('click', function() {
    const currentTab = $('.tab-pane.active');
    let hasErrors = false;
    // Verificar campos requeridos en la pestaña actual
    currentTab.find('[required]').each(function() {
        if ($(this).val() === '') {
            $(this).addClass('is-invalid');
            hasErrors = true;
        } else {
            $(this).removeClass('is-invalid');
        }
    });

    // Si hay errores, mostrar alerta
    if (hasErrors) {
        Swal.fire({
            icon: 'warning',
            title: 'Campos incompletos',
            text: 'Por favor complete los campos requeridos antes de continuar',
            confirmButtonText: 'Entendido'
        })
        
    }
});

/**
 * Maneja la carga y previsualización de imágenes
 */
function initImageUpload() {
    // (Implementación pendiente o personalizada según necesidades)
}

const imageInput = $('#ItemImage');
const imagePreview = $('#imagePreview');
const imageDefault = $('#imageDefault');
const btnResetImage = $('#btnResetImage');

// Cuando se selecciona una imagen
imageInput.on('change', function() {
    const file = this.files[0];
    if (file) {
        // Validar tipo de archivo
        const validTypes = ['image/jpeg', 'image/png', 'image/gif'];
        if (!validTypes.includes(file.type)) {
            Swal.fire({
                icon: 'error',
                title: 'Formato no válido',
                text: 'Por favor seleccione un archivo JPG, PNG o GIF',
                confirmButtonText: 'Aceptar'
            })
        
            imageInput.val('');
            return;
        }
        // Validar tamaño (máximo 800KB)
        if (file.size > 800 * 1024) {
            Swal.fire({
                icon: 'error',
                title: 'Archivo demasiado grande',
                text: 'El tamaño máximo permitido es 800KB',
                confirmButtonText: 'Aceptar'
            })
        
            imageInput.val('');
            return;
        }
        // Mostrar previsualización
        const reader = new FileReader();
        reader.onload = function(e) {
            $('#preview').attr('src', e.target.result);
            imagePreview.show();
            imageDefault.hide();
            btnResetImage.show();
        };
        reader.readAsDataURL(file);
    }
});

// Resetear imagen
btnResetImage.on('click', function() {
    imageInput.val('');
    imagePreview.hide();
    imageDefault.show();
    btnResetImage.hide();
    // Si estamos en modo edición y tenemos una imagen guardada
    if ($('#ImagenUrl').length > 0 && $('#ImagenUrl').val()) {
        $('#ImagenUrl').val(''); // Borrar la referencia a la imagen existente
    }
});

/**
 * Inicializa la funcionalidad de códigos de barras
 */
function initCodigoBarras() {
    // (Implementación pendiente o personalizada según necesidades)
}

const btnGenerarCodigo = $('#generarCodigoBarras');
const btnImprimirCodigo = $('#imprimirCodigoBarras');
const codigoInput = $('#CodigoBarras');
const previewContainer = $('#codigoBarrasPreview');

// Generar código de barras automáticamente
btnGenerarCodigo.on('click', function() {
    $.get('../Item/GenerarCodigoBarras', function(data) {
        if (data.codigo) {
            codigoInput.val(data.codigo);
            generarVistaPrevia(data.codigo);
            btnImprimirCodigo.prop('disabled', false);
        }
    })
        
});

// Imprimir código de barras
btnImprimirCodigo.on('click', function() {
    const codigo = codigoInput.val();
    const nombre = $('#Nombre').val() || 'Item';
    if (codigo) {
        $.post('../Item/ExportarCodigoBarras', { 
            codigo: codigo, 
            nombre: nombre 
        }, function(data) {
            if (data.success) {
                window.open(data.fileUrl, '_blank');
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: data.message || 'No se pudo generar el código de barras',
                    confirmButtonText: 'Aceptar'
                })
        
            }
        })
        
    }
});

// Cuando cambia el valor del input manualmente
codigoInput.on('input', function() {
    const codigo = $(this).val();
    if (codigo && codigo.trim() !== '') {
        generarVistaPrevia(codigo);
        btnImprimirCodigo.prop('disabled', false);
    } else {
        previewContainer.hide();
        btnImprimirCodigo.prop('disabled', true);
    }
});

// Generar vista previa del código de barras
function generarVistaPrevia(codigo) {
    previewContainer.show();
    JsBarcode("#barcode", codigo, {
        format: "CODE128",
        displayValue: true,
        fontSize: 14,
        height: 50,
        margin: 10
    })
        
}

// Inicializar vista previa si ya hay un código
if (codigoInput.val()) {
    generarVistaPrevia(codigoInput.val());
}

// Funciones para cargar datos existentes

/**
 * Carga contenedores existentes (modo edición)
 */
function cargarContenedoresExistentes() {
    // Vaciar tabla
    $('#tablaContenedores tbody').empty();

    // Si no hay datos, agregar base
    if (!window.contenedoresDatos || window.contenedoresDatos.length === 0) {
        agregarContenedorBase();
        return;
    }

    // Ordenar por orden
    window.contenedoresDatos.sort((a, b) => a.orden - b.orden);

    // Iterar y agregar cada contenedor
    window.contenedoresDatos.forEach((contenedor, index) => {
        // Obtener template
        const template = $('#contenedorTemplate').html();
        // Configurar según si es base o no
        const isBase = index === 0;
        const contenedorHtml = template
            .replace(/INDEX/g, index)
            .replace(/DISABLED-FIRST-ROW/g, isBase ? 'disabled' : '')
            .replace(/READONLY-EXCEPT-FIRST-ROW/g, isBase ? '' : 'readonly');
        const $fila = $(contenedorHtml);
        // Establecer valores
        $fila.find('.no-contenedor').text(index + 1);
        $fila.find('.es-principal').val(contenedor.esPrincipal.toString());
        $fila.find('.contenedor-superior-id').val(contenedor.contenedorSuperiorId || '');
        $fila.find('.unidad-medida-id').val(contenedor.unidadMedidaId || '');
        $fila.find('.etiqueta-contenedor').val(contenedor.etiqueta || '');
        $fila.find('.cantidad-contenedor').val(contenedor.factor || 1);
        $fila.find('.costo-contenedor').val(contenedor.costo || 0);
        // Agregar a tabla
        $('#tablaContenedores tbody').append($fila);
        // Inicializar select2
        $fila.find('.select2-contenedor').select2({
            theme: 'bootstrap-5',
            width: '100%',
            placeholder: 'Seleccione un contenedor',
            allowClear: true
        })
        
        // Cargar unidades de medida
        console.log('Cargando unidades de medida para contenedor existente...');
        $.get('../UnidadMedida/ObtenerTodas')
            .done(function(data) {
                console.log('Unidades de medida cargadas exitosamente:', data);
                // Poblar selector de contenedores con unidades
                if (data && data.length > 0) {
                    const $select = $fila.find('.select2-contenedor');
                    
                    // Agregar opciones
                    data.forEach(function(unidad) {
                        const option = new Option(unidad.nombre + ' (' + unidad.abreviatura + ')', unidad.nombre);
                        $select.append(option);
                    })
                
                    
                    // Si hay unidad de inventario seleccionada, usar esa
                    const unidadInventarioId = $('#UnidadMedidaInventarioId').val();
                    const unidadInventarioTexto = $('#UnidadMedidaInventarioId option:selected').text();
                    if (unidadInventarioId) {
                        // Extraer solo el nombre de la unidad (sin abreviatura)
                        let nombreUnidad = unidadInventarioTexto;
                        if (nombreUnidad.includes('(')) {
                            nombreUnidad = nombreUnidad.substring(0, nombreUnidad.indexOf('(')).trim();
                        }
                        
                        // Buscar y seleccionar
                        const option = $select.find('option').filter(function() {
                            return $(this).text().startsWith(nombreUnidad);
                        })
                
                        
                        if (option.length) {
                            $select.val(option.val()).trigger('change');
                            $fila.find('.unidad-medida-id').val(unidadInventarioId);
                        }
                    }
                }
            })
            .fail(function(xhr, status, error) {
                console.error('Error cargando unidades de medida:', error);
                console.log('URL:', '../UnidadMedida/ObtenerTodas');
                console.log('Estado:', status);
                console.log('Respuesta:', xhr.responseText);
                console.log('Status code:', xhr.status);
                // Cargar datos estáticos como fallback
                const $select = $fila.find('.select2-contenedor');
                const opciones = [
                    { nombre: 'Unidad (U)', valor: 'Unidad' },
                    { nombre: 'Caja (CJ)', valor: 'Caja' },
                    { nombre: 'Paquete (PQ)', valor: 'Paquete' }
                ];
                opciones.forEach(function(opcion) {
                    const option = new Option(opcion.nombre, opcion.valor);
                    $select.append(option);
                });
                $select.trigger('change');
            });
    })
        
    // Actualizar contenedor de venta
    actualizarSelectorContenedorVenta();
}

/**
 * Agrega el contenedor base (primer nivel)
 */
function agregarContenedorBase() {
    // Obtener el template
    const template = $('#contenedorTemplate').html();
    const $tbody = $('#tablaContenedores tbody');

    // Limpiar tabla
    $tbody.empty();

    // Crear el primer contenedor (base)
    const contenedorHtml = template
        .replace(/INDEX/g, '0')
        .replace(/DISABLED-FIRST-ROW/g, 'disabled')
        .replace(/READONLY-EXCEPT-FIRST-ROW/g, '');
    
    const $fila = $(contenedorHtml);

    // Marcar como principal
    $fila.find('.es-principal').val('true');

    // Fija el número de fila
    $fila.find('.no-contenedor').text('1');

    // Inicializar select2
    $fila.find('.select2-contenedor').select2({
        theme: 'bootstrap-5',
        width: '100%',
        placeholder: 'Seleccione un contenedor',
        allowClear: true
    })
        

    // Cargar unidades de medida
    console.log('Cargando unidades de medida para contenedor base...');
    $.get('../UnidadMedida/ObtenerTodas')
        .done(function(data) {
            console.log('Unidades de medida cargadas exitosamente:', data);
            // Poblar selector de contenedores con unidades
            if (data && data.length > 0) {
                const $select = $fila.find('.select2-contenedor');
                
                // Agregar opciones
                data.forEach(function(unidad) {
                    const option = new Option(unidad.nombre + ' (' + unidad.abreviatura + ')', unidad.nombre);
                    $select.append(option);
                })
            
                
                // Si hay unidad de inventario seleccionada, usar esa
                const unidadInventarioId = $('#UnidadMedidaInventarioId').val();
                const unidadInventarioTexto = $('#UnidadMedidaInventarioId option:selected').text();
                if (unidadInventarioId) {
                    // Extraer solo el nombre de la unidad (sin abreviatura)
                    let nombreUnidad = unidadInventarioTexto;
                    if (nombreUnidad.includes('(')) {
                        nombreUnidad = nombreUnidad.substring(0, nombreUnidad.indexOf('(')).trim();
                    }
                    
                    // Buscar y seleccionar
                    const option = $select.find('option').filter(function() {
                        return $(this).text().startsWith(nombreUnidad);
                    })
            
                    
                    if (option.length) {
                        $select.val(option.val()).trigger('change');
                        $fila.find('.unidad-medida-id').val(unidadInventarioId);
                    }
                }
            }
        })
        .fail(function(xhr, status, error) {
            console.error('Error cargando unidades de medida:', error);
            console.log('URL:', '../UnidadMedida/ObtenerTodas');
            console.log('Estado:', status);
            console.log('Respuesta:', xhr.responseText);
            console.log('Status code:', xhr.status);
            // Cargar datos estáticos como fallback
            const $select = $fila.find('.select2-contenedor');
            const opciones = [
                { nombre: 'Unidad (U)', valor: 'Unidad' },
                { nombre: 'Caja (CJ)', valor: 'Caja' },
                { nombre: 'Paquete (PQ)', valor: 'Paquete' }
            ];
            opciones.forEach(function(opcion) {
                const option = new Option(opcion.nombre, opcion.valor);
                $select.append(option);
            });
            $select.trigger('change');
        });
}

function agregarNuevoContenedor() {
    // Obtener el template
    const template = $('#contenedorTemplate').html();
    const $tbody = $('#tablaContenedores tbody');
    const index = $tbody.find('.fila-contenedor').length;

    // Crear nueva fila
    const contenedorHtml = template
        .replace(/INDEX/g, index)
        .replace(/DISABLED-FIRST-ROW/g, '')
        .replace(/READONLY-EXCEPT-FIRST-ROW/g, 'readonly');

    const $fila = $(contenedorHtml);

    // Establecer número de fila
    $fila.find('.no-contenedor').text(index + 1);

    // Establecer contenedor superior
    const $filaAnterior = $tbody.find('.fila-contenedor').last();
    if ($filaAnterior.length) {
        const nombreContenedorSuperior = $filaAnterior.find('.select2-contenedor').val();
        const idContenedorSuperior = index - 1;
        $fila.find('.contenedor-superior-id').val(idContenedorSuperior);
        // Hint para el usuario
        $fila.find('.hint-etiqueta').text('Cuántas unidades en 1 ' + nombreContenedorSuperior);
    }

    // Inicializar select2
    $fila.find('.select2-contenedor').select2({
        theme: 'bootstrap-5',
        width: '100%',
        placeholder: 'Seleccione un contenedor',
        allowClear: true
    })
        

    // Cargar opciones de contenedores (igual que en selector anterior)
    const $select = $fila.find('.select2-contenedor');
    $('.select2-contenedor').first().find('option').each(function() {
        if ($(this).val()) {
            const option = new Option($(this).text(), $(this).val());
            $select.append(option);
        }
    })
        

    // Agregar la fila a la tabla
    $tbody.append($fila);
}

function actualizarEtiquetaContenedor($fila) {
    $fila = $fila || $(this).closest('.fila-contenedor');
    const nombre = $fila.find('.select2-contenedor').val();
    const index = $fila.index();
    const $etiqueta = $fila.find('.etiqueta-contenedor');

    // Si es la primera fila (base)
    if (index === 0) {
        $etiqueta.val(nombre);
        return;
    }

    // Para otras filas, componer etiqueta con relación al contenedor superior
    const $filaSuperior = $fila.prev('.fila-contenedor');
    const nombreSuperior = $filaSuperior.find('.select2-contenedor').val();

    if (nombre && nombreSuperior) {
        $etiqueta.val(nombre + ' / ' + nombreSuperior);
        $fila.find('.hint-etiqueta').text('Cuántas unidades en 1 ' + nombreSuperior);
    }
}

function renumerarFilasContenedores() {
    $('#tablaContenedores tbody .fila-contenedor').each(function(index) {
        // Actualizar número visible
        $(this).find('.no-contenedor').text(index + 1);
        // Actualizar índices en los nombres de campos
        $(this).find('select, input').each(function() {
            const name = $(this).attr('name');
            if (name) {
                const newName = name.replace(/\[\d+\]/, '[' + index + ']');
                $(this).attr('name', newName);
            }
        })
        
        // Actualizar contenedor superior
        if (index > 0) {
            const idContenedorSuperior = index - 1;
            $(this).find('.contenedor-superior-id').val(idContenedorSuperior);
        }
    })
        
}

function calcularCostoContenedor($fila) {
    const index = $fila.index();
    // Si es la primera fila, el costo lo define el usuario
    if (index === 0) {
        return;
    }
    // Para las demás filas, calcular basado en la fila superior
    const $filaSuperior = $fila.prev('.fila-contenedor');
    const costoSuperior = parseFloat($filaSuperior.find('.costo-contenedor').val()) || 0;
    const cantidad = parseFloat($fila.find('.cantidad-contenedor').val()) || 1;
    // El costo es: costo superior / cantidad
    let costo = 0;
    if (costoSuperior > 0 && cantidad > 0) {
        costo = costoSuperior / cantidad;
    }
    // Formatear a 4 decimales
    costo = parseFloat(costo.toFixed(4));
    // Actualizar el campo
    $fila.find('.costo-contenedor').val(costo);
}

function recalcularConversiones() {
    // Recalcular todos los costos en cascada
    const $filas = $('#tablaContenedores tbody .fila-contenedor');
    // Saltamos la primera fila porque su costo es directo
    for (let i = 1; i < $filas.length; i++) {
        calcularCostoContenedor($($filas[i]));
    }
    // Actualizar selección de contenedor para compra/venta
    actualizarSelectorContenedorVenta();
}

/**
 * Carga proveedores existentes (modo edición)
 */
function cargarProveedoresExistentes() {
    // Si no hay datos, mostrar mensaje
    if (!window.proveedoresDatos || window.proveedoresDatos.length === 0) {
        // Agregar primer proveedor vacío
        agregarProveedor();
        return;
    }

    // Eliminar el contenedor de proveedores actual y recrearlo
    $('#proveedoresContainer').empty();

    // Iterar y agregar cada proveedor
    window.proveedoresDatos.forEach((proveedor, index) => {
        agregarProveedor(index, proveedor);
    })
        
}

function agregarProveedor(index, data) {
    // Obtener template
    const template = $('#proveedorTemplate').html();
    const proveedorIndex = typeof index !== 'undefined' ? index : $('.proveedor-item').length;
    // Reemplazar índice
    const proveedorHtml = template.replace(/INDEX/g, proveedorIndex);
    const $item = $(proveedorHtml);
    // Si hay datos, rellenar campos
    if (data) {
        $item.find('input[name^="Proveedores[' + proveedorIndex + '].ProveedorId"]').val(data.proveedorId);
        $item.find('input[name^="Proveedores[' + proveedorIndex + '].NombreCompra"]').val(data.nombreCompra);
        $item.find('input[name^="Proveedores[' + proveedorIndex + '].CodigoProveedor"]').val(data.codigoProveedor);
        $item.find('input[name^="Proveedores[' + proveedorIndex + '].PrecioCompra"]').val(data.precioCompra);
        $item.find('input[name^="Proveedores[' + proveedorIndex + '].FactorConversion"]').val(data.factorConversion);
        $item.find('input[name^="Proveedores[' + proveedorIndex + '].EsPrincipal"]').prop('checked', data.esPrincipal);
    }
    // Agregar al contenedor
    $('#proveedoresContainer').append($item);
    // Inicializar select2 para proveedor
    $item.find('.select2-proveedor').select2({
        theme: 'bootstrap-5',
        width: '100%',
        placeholder: 'Seleccione un proveedor',
        allowClear: true
    })
        
    // Cargar lista de proveedores
    $.get('../Proveedor/ObtenerTodos', function(proveedores) {
        if (proveedores && proveedores.length > 0) {
            const $select = $item.find('.select2-proveedor');
            // Agregar opciones
            proveedores.forEach(function(proveedor) {
                const option = new Option(proveedor.nombre, proveedor.id);
                $select.append(option);
            })
        
            // Si hay datos, seleccionar proveedor
            if (data && data.proveedorId) {
                $select.val(data.proveedorId).trigger('change');
            }
        }
    })
        
    // Inicializar select2 para unidad de medida/contenedor
    $item.find('.select2-contenedor-compra').select2({
        theme: 'bootstrap-5',
        width: '100%',
        placeholder: 'Seleccione una unidad',
        allowClear: true
    })
        
    // Cargar contenedores como unidades de compra
    const $selectUnidad = $item.find('.select2-contenedor-compra');
    $('#tablaContenedores tbody .fila-contenedor').each(function() {
        const contenedorId = $(this).index();
        const contenedorNombre = $(this).find('.select2-contenedor').val();
        const contenedorTexto = $(this).find('.select2-contenedor option:selected').text();
        if (contenedorNombre) {
            const option = new Option(contenedorTexto, contenedorId);
            $selectUnidad.append(option);
        }
    })
        
    // Si hay datos, seleccionar unidad de medida
    if (data && data.unidadMedidaCompraId) {
        $selectUnidad.val(data.unidadMedidaCompraId).trigger('change');
        // Mostrar el texto de la unidad
        const unidadTexto = $selectUnidad.find('option:selected').text();
        if (unidadTexto) {
            let textoDisplay = unidadTexto;
            if (textoDisplay.includes('(')) {
                textoDisplay = textoDisplay.substring(0, textoDisplay.indexOf('(')).trim();
            }
            $item.find('.unidad-medida-texto').text(textoDisplay);
        }
    }
    // Calcular precio unitario
    calcularPrecioUnitario($item);
}

function calcularPrecioUnitario($item) {
    const precioCompra = parseFloat($item.find('.precio-compra').val()) || 0;
    const factorConversion = parseFloat($item.find('.factor-conversion').val()) || 1;
    let precioUnitario = 0;
    if (precioCompra > 0 && factorConversion > 0) {
        precioUnitario = precioCompra / factorConversion;
    }
    // Formatear a 4 decimales
    precioUnitario = parseFloat(precioUnitario.toFixed(4));
    // Actualizar campo
    $item.find('.precio-unitario').val(precioUnitario);
}

function renumerarProveedores() {
    $('.proveedor-item').each(function(index) {
        // Actualizar índices en los nombres de campos
        $(this).find('select, input').each(function() {
            const name = $(this).attr('name');
            if (name) {
                const newName = name.replace(/\[\d+\]/, '[' + index + ']');
                $(this).attr('name', newName);
            }
        })
        
    })
        
}

function cargarDatosProveedor(proveedorId, $item) {
    if (!proveedorId) return;
    // Obtener datos del proveedor mediante AJAX
    $.get('../Proveedor/Obtener', { id: proveedorId }, function(data) {
        if (data) {
            // No sobrescribir el nombre de compra si ya tiene un valor personalizado
            const nombreCompraActual = $item.find('.nombre-compra').val();
            if (!nombreCompraActual || nombreCompraActual === '') {
                $item.find('.nombre-compra').val(data.nombre);
            }
            // Puedes agregar aquí otros campos a rellenar si es necesario
            // Ejemplo:
            // $item.find('.telefono-proveedor').val(data.telefono);
            // $item.find('.direccion-proveedor').val(data.direccion);
        }
    }).fail(function(xhr, status, error) {
        console.error('Error cargando proveedor:', error);
        console.log('Estado:', status);
        console.log('Respuesta:', xhr.responseText);
        cargarDatosEstaticos('proveedor', $item.find('.select2-proveedor'));
    });
}

/**
 * Carga almacenes disponibles y crea fields
 */
function cargarAlmacenesDisponibles() {
    // Obtener almacenes mediante AJAX
    console.log('Cargando almacenes disponibles...');
    $.get('../Almacen/ObtenerTodos')
        .done(function(almacenes) {
            console.log('Almacenes cargados exitosamente:', almacenes);
            if (almacenes && almacenes.length > 0) {
                // Limpiar contenedor
                $('#almacenesContainer').empty();
                
                // Crear un item para cada almacén
                almacenes.forEach(function(almacen, index) {
                    crearItemAlmacen(index, {
                        almacenId: almacen.id,
                        almacenNombre: almacen.nombre,
                        stock: 0,
                        nivelMinimo: 0
                    })
            
                })
            
            } else {
                // Mensaje si no hay almacenes
                $('#almacenesContainer').html(
                    '<div class="alert alert-info">' +
                    '<i class="fas fa-info-circle me-2"></i> ' +
                    'No hay almacenes disponibles. Por favor, cree almacenes primero.' +
                    '</div>'
                );
            }
        })
        .fail(function(xhr, status, error) {
            console.error('Error cargando almacenes:', error);
            console.log('URL:', '../Almacen/ObtenerTodos');
            console.log('Estado:', status);
            console.log('Respuesta:', xhr.responseText);
            console.log('Status code:', xhr.status);
            
            // Datos estáticos para almacenes
            const almacenesEstaticos = [
                { id: 1, nombre: 'Almacén Principal' }
            ];
            
            // Limpiar contenedor
            $('#almacenesContainer').empty();
            
            // Crear un item para cada almacén estático
            almacenesEstaticos.forEach(function(almacen, index) {
                crearItemAlmacen(index, {
                    almacenId: almacen.id,
                    almacenNombre: almacen.nombre,
                    stock: 0,
                    nivelMinimo: 0
                });
            });
        });
}

/**
 * Carga almacenes existentes (modo edición)
 */
function cargarAlmacenesExistentes() {
    // Si no hay datos, cargar almacenes disponibles
    if (!window.almacenesDatos || window.almacenesDatos.length === 0) {
        cargarAlmacenesDisponibles();
        return;
    }

    // Limpiar contenedor
    $('#almacenesContainer').empty();

    // Iterar y agregar cada almacén
    window.almacenesDatos.forEach((almacen, index) => {
        crearItemAlmacen(index, almacen);
    })
        

    // Actualizar stock total
    actualizarStockTotal();
}

/**
 * Carga taras existentes (modo edición)
 */
function cargarTarasExistentes() {
    // Si no hay contenedores, mostrar mensaje
    const hayContenedores = $('#tablaContenedores tbody .fila-contenedor').length > 0;
    if (!hayContenedores) {
        $('#tablaTaras #sinContenedores').show();
        return;
    }

    // Si no hay datos, generar filas vacías
    if (!window.tarasDatos || window.tarasDatos.length === 0) {
        actualizarTablaTaras();
        return;
    }

    // Limpiar tabla
    $('#tablaTaras tbody').empty();

    // Iterar y agregar cada tara
    window.tarasDatos.forEach((tara, index) => {
        // Buscar el contenedor correspondiente
        const contenedor = window.contenedoresDatos.find(c => c.id === tara.itemContenedorId);
        if (contenedor) {
            agregarFilaTara(index, {
                itemContenedorId: tara.itemContenedorId,
                nombreContenedor: contenedor.nombre,
                valorTara: tara.valorTara,
                observacion: tara.observacion
            })
        
        }
    })
        
}

/**
 * Carga datos del producto de venta (modo edición)
 */
function cargarProductoVentaExistente() {
    // Código para cargar producto de venta
}

// Funciones para eventos

/**
 * Configura eventos para categoría (select2, creación, herencia)
 */
function setupCategoriaEvents() {
    console.log('Configurando eventos para categoría...');
    
    // Crear el offcanvas si no existe
    if ($('#offcanvasCategoria').length === 0) {
        $('body').append(`
            <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasCategoria" aria-labelledby="offcanvasCategoriaLabel" style="width: 600px;">
                <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                    <h5 class="offcanvas-title" id="offcanvasCategoriaLabel">Categoría</h5>
                    <button type="button" class="btn-close text-reset" data-bs-dismiss="offcanvas" aria-label="Close" style="filter: invert(1);"></button>
                </div>
                <div class="offcanvas-body">
                    <!-- El formulario se cargará aquí dinámicamente -->
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                </div>
            </div>
        `);
    }
    
    // Botón para nueva categoría
    $('#btnNuevaCategoria').off('click').on('click', function() {
        console.log('Botón nueva categoría clickeado');
        crearCategoria();
    });
    
    // Eventos para el select2
    $('.select2-categoria').off('change').on('change', function() {
        const categoriaId = $(this).val();
        console.log('Categoría seleccionada:', categoriaId);
        
        if (categoriaId === 'crear_nueva') {
            console.log('Opción "Crear nueva categoría" seleccionada, abriendo offcanvas...');
            // Resetear select para evitar que quede seleccionada la opción "Crear nueva"
            $(this).val('').trigger('change');
            crearCategoria();
            return;
        }
        
        // Si se seleccionó una categoría válida, cargar sus datos
        if (categoriaId && categoriaId !== '') {
            cargarDatosCategoria(categoriaId);
        }
    });
}

/**
 * Crear nueva categoría mediante offcanvas
 */
function crearCategoria() {
    console.log('Iniciando creación de nueva categoría...');
    
    // Asegurarse de que existe el elemento offcanvas
    if ($('#offcanvasCategoria').length === 0) {
        console.log('Creando el elemento offcanvas para categoría...');
        const offcanvasHtml = `
            <div class="offcanvas offcanvas-end" style="width: 600px;" tabindex="-1" id="offcanvasCategoria" aria-labelledby="offcanvasCategoriaLabel">
                <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                    <h5 class="offcanvas-title" id="offcanvasCategoriaLabel">Gestionar Categoría</h5>
                    <button type="button" class="btn-close btn-close-white text-reset" data-bs-dismiss="offcanvas" aria-label="Close"></button>
                </div>
                <div class="offcanvas-body">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                </div>
            </div>
        `;
        $('body').append(offcanvasHtml);
    }
    
    // Mostrar offcanvas antes de cargar el form para mejor UX
    var offcanvasEl = document.getElementById('offcanvasCategoria');
    var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
    offcanvas.show();
    
    // Cargar formulario en el offcanvas
    console.log('Cargando formulario partial en el offcanvas...');
    $.get('../Categoria/CreatePartial', function(data) {
        console.log('Formulario parcial cargado correctamente');
        // Insertar el formulario en el offcanvas
        $('#offcanvasCategoria .offcanvas-body').html(data);
        
        // Inicializar formulario y validación
        $('#formCategoriaCreate').off('submit').on('submit', function(e) {
            e.preventDefault();
            console.log('Formulario de categoría enviado');
            
            // Enviar formulario por AJAX
            $.ajax({
                url: $(this).attr('action'),
                type: 'POST',
                data: $(this).serialize(),
                success: function(response) {
                    console.log('Respuesta recibida:', response);
                    if (response.success) {
                        // Cerrar offcanvas
                        var offcanvasEl = document.getElementById('offcanvasCategoria');
                        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                        offcanvas.hide();
                        
                        // Mostrar mensaje de éxito
                        Swal.fire({
                            icon: 'success',
                            title: 'Categoría creada',
                            text: 'La categoría ha sido creada con éxito',
                            timer: 2000,
                            showConfirmButton: false
                        });
                        
                        // Recargar categorías y seleccionar la nueva
                        cargarCategorias(response.categoriaId);
                    } else {
                        // Mostrar errores
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message || 'Ha ocurrido un error al crear la categoría'
                        });
                    }
                },
                error: function(xhr, status, error) {
                    console.error('Error al enviar formulario:', error, xhr.responseText);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Ha ocurrido un error al crear la categoría'
                    });
                }
            });
        });
        
        // Inicializar select2 dentro del offcanvas
        setTimeout(function() {
            $('#offcanvasCategoria .form-select').each(function() {
                if (!$(this).hasClass('select2-hidden-accessible')) {
                    $(this).select2({
                        theme: 'bootstrap-5',
                        width: '100%',
                        dropdownParent: $('#offcanvasCategoria')
                    });
                }
            });
        }, 300);
        
    }).fail(function(xhr, status, error) {
        console.error('Error cargando formulario de categoría:', error);
        console.log('Estado:', status);
        console.log('Respuesta:', xhr.responseText);
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudo cargar el formulario para crear categoría'
        });
    });
}

/**
 * Configura eventos para contenedores (agregar, eliminar, calcular)
 */
function setupContenedoresEvents() {
    console.log('Configurando eventos para contenedores...');
    
    // Botón para agregar contenedor
    $('#btnAgregarContenedor').off('click').on('click', function() {
        agregarNuevoContenedor();
    });
    
    // Cambio en selector de contenedor
    $(document).off('change', '.select2-contenedor').on('change', '.select2-contenedor', function() {
        const $fila = $(this).closest('.fila-contenedor');
        const nombre = $(this).val();
        const unidadMedidaId = $('#UnidadMedidaInventarioId').val();
        
        if (nombre) {
            // Guardar unidad de medida
            $fila.find('.unidad-medida-id').val(unidadMedidaId);
            
            // Actualizar etiqueta
            actualizarEtiquetaContenedor($fila);
        }
    });
    
    // Cambio en cantidad
    $(document).off('input', '.cantidad-contenedor').on('input', '.cantidad-contenedor', function() {
        recalcularConversiones();
    });
    
    // Cambio en costo base
    $(document).off('input', '.costo-contenedor:first').on('input', '.costo-contenedor:first', function() {
        recalcularConversiones();
    });
    
    // Botón para eliminar contenedor
    $(document).off('click', '.btn-eliminar-contenedor').on('click', '.btn-eliminar-contenedor', function() {
        const $fila = $(this).closest('.fila-contenedor');
        
        // Pedir confirmación
        Swal.fire({
            title: '¿Está seguro?',
            text: "Esto eliminará el contenedor y sus conversiones dependientes",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                // Eliminar fila
                $fila.remove();
                
                // Renumerar filas
                renumerarFilasContenedores();
                
                // Recalcular conversiones
                recalcularConversiones();
                
                // Actualizar selector de contenedor de venta
                actualizarSelectorContenedorVenta();
                
                // Actualizar taras
                actualizarTablaTaras();
            }
        });
    });
}

/**
 * Configura eventos para almacenes
 */
function setupAlmacenesEvents() {
    console.log('Configurando eventos para almacenes...');
    
    // Botón para distribuir stock
    $('#btnDistribuirStock').off('click').on('click', function() {
        distribuirStock();
    });
    
    // Cambios en stock para actualizar total
    $(document).off('input', '.stock-almacen').on('input', '.stock-almacen', function() {
        actualizarStockTotal();
    });
}

/**
 * Configura eventos para impuestos
 */
function setupImpuestosEvents() {
    console.log('Configurando eventos para impuestos...');
    
    // Crear el offcanvas si no existe
    if ($('#offcanvasImpuesto').length === 0) {
        $('body').append(`
            <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasImpuesto" aria-labelledby="offcanvasImpuestoLabel" style="width: 600px;">
                <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                    <h5 class="offcanvas-title" id="offcanvasImpuestoLabel">Impuesto</h5>
                    <button type="button" class="btn-close text-reset" data-bs-dismiss="offcanvas" aria-label="Close" style="filter: invert(1);"></button>
                </div>
                <div class="offcanvas-body">
                    <!-- El formulario se cargará aquí dinámicamente -->
                </div>
            </div>
        `);
    }
    
    // Botón para nuevo impuesto
    $('#btnNuevoImpuesto').off('click').on('click', function() {
        console.log('Botón nuevo impuesto clickeado');
        crearImpuesto();
    });
    
    // Eventos para los botones clonados dentro de select2
    $(document).off('click', '.select2-btn-container .btn').on('click', '.select2-btn-container .btn', function(e) {
        e.preventDefault();
        e.stopPropagation();
        
        // Determinar acción según el botón
        if ($(this).closest('.select2-container').prev('.select2-impuesto').length) {
            console.log('Botón nuevo impuesto clickeado desde select2');
            crearImpuesto();
        }
    });
    
    // Prevenir múltiples handlers para botón editar
    $(document).off('click', '.btn-editar-impuesto').on('click', '.btn-editar-impuesto', function(e) {
        e.preventDefault();
        e.stopPropagation();
        const impuestoId = $(this).data('id');
        editarImpuesto(impuestoId);
    });
}

/**
 * Configura eventos para producto de venta
 */
function setupProductoVentaEvents() {
    // Código para manejar producto de venta
}

/**
 * Configura validación adicional antes de enviar el formulario
 */
function setupFormSubmit() {
    // Código para validar y preparar el formulario
}

// Funciones para cálculos y utilitarias

/**
 * Calcula costo de un contenedor basado en su posición jerárquica
 */
function calcularCostoContenedor($fila) {
    // Código para calcular costos automáticamente
}

/**
 * Calcula conversión de unidades en cascada
 */
function recalcularConversiones() {
    // Código para recalcular toda la cascada de conversiones
}

/**
 * Actualiza stock total sumando todos los almacenes
 */
function actualizarStockTotal() {
    let total = 0;
    $('.stock-almacen').each(function() {
        total += parseFloat($(this).val()) || 0;
    })
        

    // Formatear a 2 decimales
    total = parseFloat(total.toFixed(2));

    // Actualizar campo
    $('#stockActualTotal').val(total);
}


/**
 * Distribuye el stock total entre almacenes
 */
function distribuirStock() {
    const stockTotal = parseFloat($('#stockActualTotal').val()) || 0;
    const almacenes = $('.almacen-item').length;

    if (stockTotal <= 0 || almacenes <= 0) {
        Swal.fire({
            icon: 'warning',
            title: 'Distribución no válida',
            text: 'El stock total debe ser mayor que cero y debe haber al menos un almacén',
            confirmButtonText: 'Entendido'
        })
        
        return;
    }

    // Dividir equitativamente
    const stockPorAlmacen = stockTotal / almacenes;

    // Formatear a 2 decimales
    const stockFormateado = parseFloat(stockPorAlmacen.toFixed(2));

    // Asignar a cada almacén
    $('.stock-almacen').val(stockFormateado);

    // Corregir diferencia por redondeo en el último almacén
    const stockCalculado = stockFormateado * (almacenes - 1);
    const ultimoStock = stockTotal - stockCalculado;
    $('.stock-almacen').last().val(parseFloat(ultimoStock.toFixed(2)));

    // Actualizar stock total
    actualizarStockTotal();

    // Mostrar mensaje de éxito
    Swal.fire({
        icon: 'success',
        title: 'Stock distribuido',
        text: 'El stock ha sido distribuido equitativamente entre los almacenes',
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000
    })
        
}


/**
 * Formatea campos numéricos según configuración local
 */
function formatearCamposNumericos() {
    // Configurar todos los campos numéricos para usar el separador decimal correcto
    $('input[type="number"], input[data-decimal]').each(function() {
        const separadorDecimal = $(this).data('decimal') || ',';
        
        // Formatear valor existente si es necesario
        if ($(this).val()) {
            let valor = parseFloat($(this).val().toString().replace(',', '.'));
            if (!isNaN(valor)) {
                if (separadorDecimal === ',') {
                    $(this).val(valor.toString().replace('.', ','));
                } else {
                    $(this).val(valor.toString());
                }
            }
        }
        
        // Manejar entrada con el separador correcto
        $(this).on('input', function() {
            let valor = $(this).val().toString();
            
            // Reemplazar punto por coma o viceversa según configuración
            if (separadorDecimal === ',' && valor.includes('.')) {
                $(this).val(valor.replace('.', ','));
            } else if (separadorDecimal === '.' && valor.includes(',')) {
                $(this).val(valor.replace(',', '.'));
            }
        })
        
    })
        
}


/**
 * Actualiza el selector de contenedor en Producto de Venta
 */
function actualizarSelectorContenedorVenta() {
    // Código para actualizar selector de contenedor en venta
}

/**
 * Calcula margen de utilidad para Producto de Venta
 */
function calcularMargenUtilidad() {
    // Código para calcular y mostrar margen
}

// Funciones para interacción con el backend

/**
 * Carga datos de una categoría desde el servidor
 */
function cargarDatosCategoria(categoriaId) {
    // Código para obtener y aplicar datos de una categoría
}

/**
 * Editar categoría existente mediante offcanvas
 */
function editarCategoria(categoriaId) {
    console.log('Iniciando edición de categoría ID:', categoriaId);
    
    // Asegurarse de que existe el elemento offcanvas
    if ($('#offcanvasCategoria').length === 0) {
        // Crear el offcanvas si no existe (similar a crearCategoria)
        const offcanvasHtml = `
            <div class="offcanvas offcanvas-end" style="width: 600px;" tabindex="-1" id="offcanvasCategoria" 
                 aria-labelledby="offcanvasCategoriaLabel">
                <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                    <h5 class="offcanvas-title" id="offcanvasCategoriaLabel">Editar Categoría</h5>
                    <button type="button" class="btn-close btn-close-white text-reset" 
                        data-bs-dismiss="offcanvas" aria-label="Close"></button>
                </div>
                <div class="offcanvas-body">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                </div>
            </div>
        `;
        $('body').append(offcanvasHtml);
    } else {
        // Actualizar título si el offcanvas ya existe
        $('#offcanvasCategoriaLabel').text('Editar Categoría');
    }
    
    // IMPORTANTE: Mostrar offcanvas antes de cargar el form
    var offcanvasEl = document.getElementById('offcanvasCategoria');
    var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
    offcanvas.show();
    
    // Cargar formulario en el offcanvas
    $.get('../Categoria/EditPartial/' + categoriaId, function(data) {
        // Insertar el formulario en el offcanvas
        $('#offcanvasCategoria .offcanvas-body').html(data);
        
        // Inicializar formulario y validación
        $('#offcanvasCategoria form').off('submit').on('submit', function(e) {
            e.preventDefault();
            
            // Enviar formulario por AJAX
            $.ajax({
                url: $(this).attr('action'),
                type: 'POST',
                data: $(this).serialize(),
                success: function(response) {
                    if (response.success) {
                        // Cerrar offcanvas
                        var offcanvasEl = document.getElementById('offcanvasCategoria');
                        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                        offcanvas.hide();
                        
                        // Mostrar mensaje de éxito
                        Swal.fire({
                            icon: 'success',
                            title: 'Categoría actualizada',
                            text: 'La categoría ha sido actualizada con éxito',
                            timer: 2000,
                            showConfirmButton: false
                        });
                        
                        // Recargar select2 para obtener los datos actualizados
                        $('.select2-categoria').empty().trigger('change');
                    } else {
                        // Mostrar errores
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message || 'Ha ocurrido un error al actualizar la categoría'
                        });
                    }
                },
                error: function(xhr, status, error) {
                    console.error('Error al enviar formulario:', error, xhr.responseText);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Ha ocurrido un error al actualizar la categoría'
                    });
                }
            });
        });
        
        // Inicializar select2 dentro del offcanvas
        setTimeout(function() {
            $('#offcanvasCategoria .form-select').each(function() {
                if (!$(this).hasClass('select2-hidden-accessible')) {
                    $(this).select2({
                        theme: 'bootstrap-5',
                        width: '100%',
                        dropdownParent: $('#offcanvasCategoria')
                    });
                }
            });
        }, 300);
        
    }).fail(function(xhr, status, error) {
        console.error('Error cargando formulario de edición de categoría:', error);
        console.log('Estado:', status);
        console.log('Respuesta:', xhr.responseText);
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudo cargar el formulario para editar la categoría'
        });
    });
}

/**
 * Crear nuevo impuesto mediante offcanvas
 */
function crearImpuesto() {
    console.log('Iniciando creación de nuevo impuesto...');
    
    // Asegurarse de que existe el elemento offcanvas
    if ($('#offcanvasImpuesto').length === 0) {
        console.log('Creando el elemento offcanvas para impuesto...');
        const offcanvasHtml = `
            <div class="offcanvas offcanvas-end" style="width: 600px;" tabindex="-1" id="offcanvasImpuesto" aria-labelledby="offcanvasImpuestoLabel">
                <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                    <h5 class="offcanvas-title" id="offcanvasImpuestoLabel">Gestionar Impuesto</h5>
                    <button type="button" class="btn-close btn-close-white text-reset" data-bs-dismiss="offcanvas" aria-label="Close"></button>
                </div>
                <div class="offcanvas-body">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                </div>
            </div>
        `;
        $('body').append(offcanvasHtml);
    }
    
    // Cargar formulario en el offcanvas
    console.log('Cargando formulario partial en el offcanvas...');
    $.get('../Impuestos/CreatePartial', function(data) {
        console.log('Formulario parcial de impuestos cargado correctamente');
        // Insertar el formulario en el offcanvas
        $('#offcanvasImpuesto .offcanvas-body').html(data);
        
        // Inicializar formulario y validación
        $('#offcanvasImpuesto form').off('submit').on('submit', function(e) {
            e.preventDefault();
            console.log('Formulario de impuesto enviado');
            
            // Enviar formulario por AJAX
            $.ajax({
                url: $(this).attr('action'),
                type: 'POST',
                data: $(this).serialize(),
                success: function(response) {
                    console.log('Respuesta recibida:', response);
                    if (response.success) {
                        // Cerrar offcanvas
                        var offcanvasEl = document.getElementById('offcanvasImpuesto');
                        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                        offcanvas.hide();
                        
                        // Mostrar mensaje de éxito
                        Swal.fire({
                            icon: 'success',
                            title: 'Impuesto creado',
                            text: 'El impuesto ha sido creado con éxito',
                            timer: 2000,
                            showConfirmButton: false
                        });
                        
                        // Recargar impuestos
                        cargarImpuestos();
                    } else {
                        // Mostrar errores
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message || 'Ha ocurrido un error al crear el impuesto'
                        });
                    }
                },
                error: function(xhr, status, error) {
                    console.error('Error al enviar formulario:', error, xhr.responseText);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Ha ocurrido un error al crear el impuesto'
                    });
                }
            });
        });
        
        // Mostrar offcanvas
        console.log('Mostrando offcanvas impuesto...');
        var offcanvasEl = document.getElementById('offcanvasImpuesto');
        var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
        offcanvas.show();
        
        // Inicializar select2 dentro del offcanvas
        setTimeout(function() {
            $('#offcanvasImpuesto .form-select').each(function() {
                if (!$(this).hasClass('select2-hidden-accessible')) {
                    $(this).select2({
                        theme: 'bootstrap-5',
                        width: '100%',
                        dropdownParent: $('#offcanvasImpuesto')
                    });
                }
            });
        }, 300);
        
    }).fail(function(xhr, status, error) {
        console.error('Error cargando formulario de impuesto:', error);
        console.log('Estado:', status);
        console.log('Respuesta:', xhr.responseText);
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudo cargar el formulario para crear impuesto'
        });
    });
}

/**
 * Editar impuesto existente mediante offcanvas
 */
function editarImpuesto(impuestoId) {
    console.log('Abriendo offcanvas para editar impuesto ID:', impuestoId);
    
    // Asegurarse de que existe el elemento offcanvas
    if ($('#offcanvasImpuesto').length === 0) {
        console.log('Creando el elemento offcanvas para impuesto...');
        const offcanvasHtml = `
            <div class="offcanvas offcanvas-end" style="width: 600px;" tabindex="-1" id="offcanvasImpuesto" aria-labelledby="offcanvasImpuestoLabel">
                <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                    <h5 class="offcanvas-title" id="offcanvasImpuestoLabel">Editar Impuesto</h5>
                    <button type="button" class="btn-close btn-close-white text-reset" data-bs-dismiss="offcanvas" aria-label="Close"></button>
                </div>
                <div class="offcanvas-body">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                </div>
            </div>
        `;
        $('body').append(offcanvasHtml);
    } else {
        // Actualizar título si el offcanvas ya existe
        $('#offcanvasImpuestoLabel').text('Editar Impuesto');
    }
    
    // Cargar formulario en el offcanvas
    $.get('../Impuestos/EditPartial/' + impuestoId, function(data) {
        // Insertar el formulario en el offcanvas
        $('#offcanvasImpuesto .offcanvas-body').html(data);
        
        // Inicializar formulario y validación
        $('#offcanvasImpuesto form').off('submit').on('submit', function(e) {
            e.preventDefault();
            
            // Enviar formulario por AJAX
            $.ajax({
                url: $(this).attr('action'),
                type: 'POST',
                data: $(this).serialize(),
                success: function(response) {
                    if (response.success) {
                        // Cerrar offcanvas
                        var offcanvasEl = document.getElementById('offcanvasImpuesto');
                        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                        offcanvas.hide();
                        
                        // Mostrar mensaje de éxito
                        Swal.fire({
                            icon: 'success',
                            title: 'Impuesto actualizado',
                            text: 'El impuesto ha sido actualizado con éxito',
                            timer: 2000,
                            showConfirmButton: false
                        });
                        
                        // Recargar impuestos
                        cargarImpuestos();
                    } else {
                        // Mostrar errores
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message || 'Ha ocurrido un error al actualizar el impuesto'
                        });
                    }
                },
                error: function(xhr, status, error) {
                    console.error('Error al enviar formulario:', error, xhr.responseText);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Ha ocurrido un error al actualizar el impuesto'
                    });
                }
            });
        });
        
        // Mostrar offcanvas
        var offcanvasEl = document.getElementById('offcanvasImpuesto');
        var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
        offcanvas.show();
        
        // Inicializar select2 dentro del offcanvas
        setTimeout(function() {
            $('#offcanvasImpuesto .form-select').each(function() {
                if (!$(this).hasClass('select2-hidden-accessible')) {
                    $(this).select2({
                        theme: 'bootstrap-5',
                        width: '100%',
                        dropdownParent: $('#offcanvasImpuesto')
                    });
                }
            });
        }, 300);
        
    }).fail(function(xhr, status, error) {
        console.error('Error cargando formulario de edición de impuesto:', error);
        console.log('Estado:', status);
        console.log('Respuesta:', xhr.responseText);
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudo cargar el formulario para editar el impuesto'
        });
    });
}

/**
 * Carga impuestos desde el servidor
 */
function cargarImpuestos() {
    // Limpiar y mostrar indicador de carga
    $('.select2-impuesto').empty().append(new Option('Cargando...', ''));
    
    console.log('Cargando impuestos...');
    
    // Cargar impuestos mediante AJAX
    $.get('../Impuestos/ObtenerTodos')
        .done(function(impuestos) {
            console.log('Impuestos cargados exitosamente:', impuestos);
            
            // Limpiar selector
            $('.select2-impuesto').empty();
            $('#ImpuestoId').empty();
            
            // Si hay datos, agregar opciones
            if (impuestos && impuestos.length > 0) {
                // Agregar opción vacía primero
                $('.select2-impuesto').append(new Option('Seleccione un impuesto', ''));
                $('#ImpuestoId').append(new Option('Seleccione un impuesto', ''));
                
                // Agregar cada impuesto al selector
                impuestos.forEach(function(impuesto) {
                    // Texto a mostrar (nombre + porcentaje)
                    const textoImpuesto = impuesto.porcentaje 
                        ? `${impuesto.nombre} (${impuesto.porcentaje}%)` 
                        : impuesto.nombre;
                    
                    // Agregar a cada select de impuestos que exista
                    $('.select2-impuesto').append(new Option(textoImpuesto, impuesto.id));
                    $('#ImpuestoId').append(new Option(textoImpuesto, impuesto.id));
                });
                
                // No añadir opción para crear nuevo impuesto
            } else {
                // Si no hay impuestos, mostrar mensaje
                $('.select2-impuesto').append(new Option('No hay impuestos disponibles', ''));
                $('#ImpuestoId').append(new Option('No hay impuestos disponibles', ''));
            }
            
            // Actualizar select2
            $('.select2-impuesto').trigger('change');
        })
        .fail(function(xhr, status, error) {
            console.error('Error cargando impuestos:', error);
            console.log('URL:', '../Impuestos/ObtenerTodos');
            console.log('Estado:', status);
            console.log('Respuesta:', xhr.responseText);
            console.log('Status code:', xhr.status);
            
            // Cargar datos estáticos como fallback
            cargarDatosEstaticos('impuesto', $('.select2-impuesto'));
        });
}

// Añadir detección de eventos para select2
$(document).on('select2:open', function(e) {
    console.log('Select2 abierto:', e.target, $(e.target).data());
});

$(document).on('select2:select', function(e) {
    console.log('Selección en select2:', e.target, e.params.data);
});

// Manejar clic en botón de editar categoría
$(document).on('click', '.btn-editar-categoria', function(e) {
    e.preventDefault();
    e.stopPropagation();
    const categoriaId = $(this).data('id');
    editarCategoria(categoriaId);
});

// Manejar clic en botón de editar impuesto
$(document).on('click', '.btn-editar-impuesto', function(e) {
    e.preventDefault();
    e.stopPropagation();
    const impuestoId = $(this).data('id');
    editarImpuesto(impuestoId);
});

/**
 * Crea un elemento para cada almacén en el contenedor
 */
function crearItemAlmacen(index, data) {
    // Crear un div para contener los datos del almacén
    var $item = $('<div class="card mb-3 almacen-item"></div>');
    var $cardBody = $('<div class="card-body"></div>');
    
    // Campos ocultos para almacenar los datos
    var hiddenFields = `
        <input type="hidden" name="Almacenes[${index}].AlmacenId" value="${data.almacenId}" />
        <input type="hidden" name="Almacenes[${index}].AlmacenNombre" value="${data.almacenNombre}" />
    `;
    
    // Cabecera con el nombre del almacén
    var header = `
        <div class="d-flex justify-content-between align-items-center mb-3">
            <h5 class="mb-0">${data.almacenNombre}</h5>
        </div>
    `;
    
    // Campos para stock y nivel mínimo
    var fields = `
        <div class="row">
            <div class="col-md-6 mb-3">
                <label class="form-label">Stock Actual</label>
                <input type="number" class="form-control stock-almacen" name="Almacenes[${index}].Stock" value="${data.stock || 0}" step="0.01" min="0" />
            </div>
            <div class="col-md-6 mb-3">
                <label class="form-label">Nivel Mínimo</label>
                <input type="number" class="form-control" name="Almacenes[${index}].NivelMinimo" value="${data.nivelMinimo || 0}" step="0.01" min="0" />
            </div>
        </div>
    `;
    
    // Combinar todo y agregar al contenedor
    $cardBody.append(hiddenFields + header + fields);
    $item.append($cardBody);
    $('#almacenesContainer').append($item);
    
    // Crear evento para actualizar el stock total al cambiar
    $item.find('.stock-almacen').on('input', function() {
        actualizarStockTotal();
    });
}

/**
 * Actualiza tabla de taras según los contenedores existentes
 */
function actualizarTablaTaras() {
    // Limpiar tabla
    $('#tablaTaras tbody').empty();
    
    // Verificar si hay contenedores
    const hayContenedores = $('#tablaContenedores tbody .fila-contenedor').length > 0;
    if (!hayContenedores) {
        $('#tablaTaras #sinContenedores').show();
        return;
    }
    
    $('#tablaTaras #sinContenedores').hide();
    
    // Crear una fila para cada contenedor
    $('#tablaContenedores tbody .fila-contenedor').each(function(index) {
        const contenedorId = index;
        const contenedorNombre = $(this).find('.select2-contenedor').val();
        
        if (contenedorNombre) {
            agregarFilaTara(index, {
                itemContenedorId: contenedorId,
                nombreContenedor: contenedorNombre,
                valorTara: 0,
                observacion: ''
            });
        }
    });
}

/**
 * Carga los datos de un item existente para edición
 * @param {number} itemId - ID del item a cargar
 */
function cargarDatosItem(itemId) {
    console.log('Cargando datos del item con ID:', itemId);
    
    // Cargar datos del item mediante AJAX
    $.get('../Item/ObtenerDatos/' + itemId)
        .done(function(data) {
            console.log('Datos del item cargados:', data);
            
            if (!data || !data.item) {
                console.error('No se recibieron datos válidos del item');
                return;
            }
            
            // Cargar datos básicos
            $('#Codigo').val(data.item.codigo);
            $('#Nombre').val(data.item.nombre);
            $('#Descripcion').val(data.item.descripcion);
            $('#MarcaId').val(data.item.marcaId).trigger('change');
            $('#CategoriaId').val(data.item.categoriaId).trigger('change');
            $('#UnidadMedidaId').val(data.item.unidadMedidaId).trigger('change');
            $('#ImpuestoId').val(data.item.impuestoId).trigger('change');
            
            // Cargar datos adicionales
            if (data.item.precio) {
                $('#Precio').val(data.item.precio);
            }
            
            if (data.item.costo) {
                $('#Costo').val(data.item.costo);
            }
            
            if (data.item.codigoBarras) {
                $('#CodigoBarras').val(data.item.codigoBarras);
            }
            
            // Cargar almacenes si existen
            if (data.almacenes && data.almacenes.length > 0) {
                cargarAlmacenesExistentes(data.almacenes);
            } else {
                cargarAlmacenesDisponibles();
            }
            
            // Cargar contenedores si existen
            if (data.contenedores && data.contenedores.length > 0) {
                cargarContenedoresExistentes(data.contenedores);
            } else {
                agregarContenedorBase();
            }
            
            // Cargar proveedores si existen
            if (data.proveedores && data.proveedores.length > 0) {
                cargarProveedoresExistentes(data.proveedores);
            }
            
            // Cargar taras si existen
            if (data.taras && data.taras.length > 0) {
                cargarTarasExistentes(data.taras);
            }
            
            // Actualizar interfaces después de cargar datos
            formatearCamposNumericos();
        })
        .fail(function(xhr, status, error) {
            console.error('Error cargando datos del item:', error);
            console.log('Respuesta:', xhr.responseText);
            
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No se pudieron cargar los datos del item'
            });
            
            // Como fallback, preparar el formulario para creación
            cargarAlmacenesDisponibles();
            agregarContenedorBase();
        });
}

// Funciones de formato para marca
function formatMarca(marca) {
    if (!marca.id) {
        return marca.text;
    }
    
    // Opción especial para crear nueva marca
    if (marca.id === 'nueva') {
        return $('<div class="select2-nueva-opcion"><i class="fas fa-plus-circle text-primary me-1"></i> Agregar Nueva Marca</div>');
    }
    
    // Formato normal para marcas existentes
    return $('<div>' + marca.text + '</div>');
}

function formatMarcaSelection(marca) {
    if (!marca.id || marca.id === 'nueva') {
        return marca.text;
    }
    
    // Crear contenedor con botón de edición
    var $contenedor = $(
        '<div class="d-flex justify-content-between align-items-center w-100">' +
            '<span>' + marca.text + '</span>' +
            '<button type="button" class="btn-editar-marca btn btn-sm btn-link p-0 ms-2" data-id="' + marca.id + '">' +
                '<i class="fas fa-pencil-alt"></i>' +
            '</button>' +
        '</div>'
    );
    
    // Agregar evento al botón de edición
    $contenedor.find('.btn-editar-marca').on('mousedown', function(e) {
        e.preventDefault();
        e.stopPropagation();
        var marcaId = $(this).data('id');
        setTimeout(function() {
            editarMarca(marcaId);
        }, 100);
    });
    
    return $contenedor;
}

/**
 * Carga marcas desde el servidor
 * @param {number} marcaIdToSelect - ID opcional de la marca a seleccionar una vez cargadas
 */
function cargarMarcas(marcaIdToSelect) {
    // Limpiar y mostrar indicador de carga
    $('.select2-marca').empty().append(new Option('Cargando...', ''));
    
    console.log('Cargando marcas...');
    
    // Cargar marcas mediante AJAX
    $.get('../Marca/ObtenerTodas')
        .done(function(marcas) {
            console.log('Marcas cargadas exitosamente:', marcas);
            
            // Limpiar selector
            $('.select2-marca').empty();
            
            // Si hay datos, agregar opciones
            if (marcas && marcas.length > 0) {
                // Agregar opción vacía primero (opcional)
                $('.select2-marca').append(new Option('Seleccione una marca', ''));
                
                // Agregar cada marca al selector
                marcas.forEach(function(marca) {
                    console.log('Agregando marca:', marca);
                    const option = new Option(marca.nombre, marca.id);
                    // Almacenar datos completos para Select2
                    $(option).data('data', marca);
                    $('.select2-marca').append(option);
                });
                
                // Si hay un ID para seleccionar, hacerlo
                if (marcaIdToSelect) {
                    console.log('Seleccionando marca ID:', marcaIdToSelect);
                    $('.select2-marca').val(marcaIdToSelect);
                    $('.select2-marca').trigger('change');
                }
            } else {
                // Si no hay marcas, mostrar mensaje
                $('.select2-marca').append(new Option('No hay marcas disponibles', ''));
            }
            
            // Ya no añadimos la opción para crear nueva marca aquí,
            // ahora usamos el botón en el dropdown
            
            // Actualizar select2
            $('.select2-marca').trigger('change');
        })
        .fail(function(xhr, status, error) {
            console.error('Error cargando marcas:', error);
            console.log('URL:', '../Marca/ObtenerTodas');
            console.log('Estado:', status);
            console.log('Respuesta:', xhr.responseText);
            console.log('Status code:', xhr.status);
            
            // Cargar datos estáticos como fallback
            cargarDatosEstaticos('marca', $('.select2-marca'));
            
            // Actualizar select2
            $('.select2-marca').trigger('change');
        });
}

/**
 * Crear nueva marca mediante offcanvas
 */
function crearMarca() {
    console.log('Iniciando creación de nueva marca...');
    
    // Asegurarse de que existe el elemento offcanvas
    if ($('#offcanvasMarca').length === 0) {
        console.log('Creando el elemento offcanvas para marca...');
        const offcanvasHtml = `
            <div class="offcanvas offcanvas-end" style="width: 600px;" tabindex="-1" id="offcanvasMarca" aria-labelledby="offcanvasMarcaLabel">
                <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                    <h5 class="offcanvas-title" id="offcanvasMarcaLabel">Gestionar Marca</h5>
                    <button type="button" class="btn-close btn-close-white text-reset" data-bs-dismiss="offcanvas" aria-label="Close"></button>
                </div>
                <div class="offcanvas-body">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                </div>
            </div>
        `;
        $('body').append(offcanvasHtml);
    }
    
    // Mostrar offcanvas antes de cargar el form para mejor UX
    var offcanvasEl = document.getElementById('offcanvasMarca');
    var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
    offcanvas.show();
    
    // Cargar formulario en el offcanvas
    console.log('Cargando formulario partial en el offcanvas...');
    $.get('../Marca/CreatePartial', function(data) {
        console.log('Formulario parcial cargado correctamente');
        // Insertar el formulario en el offcanvas
        $('#offcanvasMarca .offcanvas-body').html(data);
        
        // Inicializar formulario y validación
        $('#formMarcaCreate').off('submit').on('submit', function(e) {
            e.preventDefault();
            console.log('Formulario de marca enviado');
            
            // Enviar formulario por AJAX
            $.ajax({
                url: $(this).attr('action'),
                type: 'POST',
                data: $(this).serialize(),
                success: function(response) {
                    console.log('Respuesta recibida:', response);
                    if (response.success) {
                        // Cerrar offcanvas
                        var offcanvasEl = document.getElementById('offcanvasMarca');
                        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                        offcanvas.hide();
                        
                        // Mostrar mensaje de éxito
                        Swal.fire({
                            icon: 'success',
                            title: 'Marca creada',
                            text: 'La marca ha sido creada con éxito',
                            timer: 2000,
                            showConfirmButton: false
                        });
                        
                        // Recargar marcas y seleccionar la nueva
                        cargarMarcas(response.marcaId);
                    } else {
                        // Mostrar errores
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message || 'Ha ocurrido un error al crear la marca'
                        });
                    }
                },
                error: function(xhr, status, error) {
                    console.error('Error al enviar formulario:', error, xhr.responseText);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Ha ocurrido un error al crear la marca'
                    });
                }
            });
        });
        
        // Inicializar select2 dentro del offcanvas
        setTimeout(function() {
            $('#offcanvasMarca .form-select').each(function() {
                if (!$(this).hasClass('select2-hidden-accessible')) {
                    $(this).select2({
                        theme: 'bootstrap-5',
                        width: '100%',
                        dropdownParent: $('#offcanvasMarca')
                    });
                }
            });
        }, 300);
        
    }).fail(function(xhr, status, error) {
        console.error('Error cargando formulario de marca:', error);
        console.log('Estado:', status);
        console.log('Respuesta:', xhr.responseText);
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudo cargar el formulario para crear marca'
        });
    });
}

/**
 * Editar marca existente mediante offcanvas
 */
function editarMarca(marcaId) {
    console.log('Abriendo offcanvas para editar marca ID:', marcaId);
    
    // Asegurarse de que existe el elemento offcanvas
    if ($('#offcanvasMarca').length === 0) {
        console.log('Creando el elemento offcanvas para marca...');
        const offcanvasHtml = `
            <div class="offcanvas offcanvas-end" style="width: 600px;" tabindex="-1" id="offcanvasMarca" aria-labelledby="offcanvasMarcaLabel">
                <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                    <h5 class="offcanvas-title" id="offcanvasMarcaLabel">Editar Marca</h5>
                    <button type="button" class="btn-close btn-close-white text-reset" data-bs-dismiss="offcanvas" aria-label="Close"></button>
                </div>
                <div class="offcanvas-body">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                </div>
            </div>
        `;
        $('body').append(offcanvasHtml);
    } else {
        // Actualizar título si el offcanvas ya existe
        $('#offcanvasMarcaLabel').text('Editar Marca');
    }
    
    // Cargar formulario en el offcanvas
    $.get('../Marca/EditPartial/' + marcaId, function(data) {
        // Insertar el formulario en el offcanvas
        $('#offcanvasMarca .offcanvas-body').html(data);
        
        // Inicializar formulario y validación
        $('#offcanvasMarca form').off('submit').on('submit', function(e) {
            e.preventDefault();
            
            // Enviar formulario por AJAX
            $.ajax({
                url: $(this).attr('action'),
                type: 'POST',
                data: $(this).serialize(),
                success: function(response) {
                    if (response.success) {
                        // Cerrar offcanvas
                        var offcanvasEl = document.getElementById('offcanvasMarca');
                        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                        offcanvas.hide();
                        
                        // Mostrar mensaje de éxito
                        Swal.fire({
                            icon: 'success',
                            title: 'Marca actualizada',
                            text: 'La marca ha sido actualizada con éxito',
                            timer: 2000,
                            showConfirmButton: false
                        });
                        
                        // Recargar marcas
                        cargarMarcas();
                    } else {
                        // Mostrar errores
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message || 'Ha ocurrido un error al actualizar la marca'
                        });
                    }
                },
                error: function(xhr, status, error) {
                    console.error('Error al enviar formulario:', error, xhr.responseText);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Ha ocurrido un error al actualizar la marca'
                    });
                }
            });
        });
        
        // Mostrar offcanvas
        var offcanvasEl = document.getElementById('offcanvasMarca');
        var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
        offcanvas.show();
        
        // Inicializar select2 dentro del offcanvas
        setTimeout(function() {
            $('#offcanvasMarca .form-select').each(function() {
                if (!$(this).hasClass('select2-hidden-accessible')) {
                    $(this).select2({
                        theme: 'bootstrap-5',
                        width: '100%',
                        dropdownParent: $('#offcanvasMarca')
                    });
                }
            });
        }, 300);
        
    }).fail(function(xhr, status, error) {
        console.error('Error cargando formulario de edición de marca:', error);
        console.log('Estado:', status);
        console.log('Respuesta:', xhr.responseText);
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudo cargar el formulario para editar la marca'
        });
    });
}

/**
 * Configura eventos para marca (select2, creación, herencia)
 */
function setupMarcaEvents() {
    console.log('Configurando eventos para marca...');
    
    // Crear el offcanvas si no existe
    if ($('#offcanvasMarca').length === 0) {
        $('body').append(`
            <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasMarca" aria-labelledby="offcanvasMarcaLabel" style="width: 600px;">
                <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                    <h5 class="offcanvas-title" id="offcanvasMarcaLabel">Marca</h5>
                    <button type="button" class="btn-close text-reset" data-bs-dismiss="offcanvas" aria-label="Close" style="filter: invert(1);"></button>
                </div>
                <div class="offcanvas-body">
                    <!-- El formulario se cargará aquí dinámicamente -->
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                </div>
            </div>
        `);
    }
    
    // Botón para nueva marca
    $('#btnNuevaMarca').off('click').on('click', function() {
        console.log('Botón nueva marca clickeado');
        crearMarca();
    });
    
    // Eventos para el select2
    $('.select2-marca').off('change').on('change', function() {
        const marcaId = $(this).val();
        console.log('Marca seleccionada:', marcaId);
        
        if (marcaId === 'crear_nueva') {
            console.log('Opción "Crear nueva marca" seleccionada, abriendo offcanvas...');
            // Resetear select para evitar que quede seleccionada la opción "Crear nueva"
            $(this).val('').trigger('change');
            crearMarca();
            return;
        }
    });
}

/**
 * Configura eventos para categoría (select2, creación, herencia)
 */
function setupCategoriaEvents() {
    console.log('Configurando eventos para categoría...');
    
    // Crear el offcanvas si no existe
    if ($('#offcanvasCategoria').length === 0) {
        $('body').append(`
            <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasCategoria" aria-labelledby="offcanvasCategoriaLabel" style="width: 600px;">
                <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                    <h5 class="offcanvas-title" id="offcanvasCategoriaLabel">Categoría</h5>
                    <button type="button" class="btn-close text-reset" data-bs-dismiss="offcanvas" aria-label="Close" style="filter: invert(1);"></button>
                </div>
                <div class="offcanvas-body">
                    <!-- El formulario se cargará aquí dinámicamente -->
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                </div>
            </div>
        `);
    }
    
    // Botón para nueva categoría
    $('#btnNuevaCategoria').off('click').on('click', function() {
        console.log('Botón nueva categoría clickeado');
        crearCategoria();
    });
    
    // Eventos para el select2
    $('.select2-categoria').off('change').on('change', function() {
        const categoriaId = $(this).val();
        console.log('Categoría seleccionada:', categoriaId);
        
        if (categoriaId === 'crear_nueva') {
            console.log('Opción "Crear nueva categoría" seleccionada, abriendo offcanvas...');
            // Resetear select para evitar que quede seleccionada la opción "Crear nueva"
            $(this).val('').trigger('change');
            crearCategoria();
            return;
        }
        
        // Si se seleccionó una categoría válida, cargar sus datos
        if (categoriaId && categoriaId !== '') {
            cargarDatosCategoria(categoriaId);
        }
    });
}

// Eventos básicos de UI que no van en los setup functions
$(document).off('click', '.btn-nuevo-contenedor').on('click', '.btn-nuevo-contenedor', function() {
    $('#modalNuevoContenedor').modal('show');
});

// Guardar nuevo contenedor
$('#guardarNuevoContenedor').on('click', function() {
    const nombre = $('#nuevoContenedorName').val().trim();
    const unidadId = $('#nuevoContenedorUnidadId').val();
    
    if (!nombre) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'El nombre del contenedor es requerido'
        });
        
        return;
    }
    
    if (!unidadId) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'La unidad de medida es requerida'
        });
        
        return;
    }
    
    // Crear contenedor en todos los selects
    const unidadTexto = $('#nuevoContenedorUnidadId option:selected').text();
    $('.select2-contenedor').each(function() {
        const newOption = new Option(nombre + ' (' + unidadTexto + ')', nombre);
        $(this).append(newOption);
    });
    
    // Seleccionar en el select activo
    const $activeSelect = $('.btn-nuevo-contenedor:focus').closest('.fila-contenedor').find('.select2-contenedor');
    $activeSelect.val(nombre).trigger('change');
    
    // Guardar unidad de medida
    $activeSelect.closest('.fila-contenedor').find('.unidad-medida-id').val(unidadId);
    
    // Cerrar modal y limpiar
    $('#modalNuevoContenedor').modal('hide');
    $('#nuevoContenedorName').val('');
    $('#nuevoContenedorUnidadId').val('').trigger('change');
});

/**
 * Función para cargar datos estáticos como fallback en caso de error AJAX
 */
function cargarDatosEstaticos(tipo, $select) {
    const datos = {
        'categoria': [{ id: 1, nombre: 'Categoría predeterminada' }],
        'marca': [{ id: 1, nombre: 'Marca genérica' }],
        'impuesto': [{ id: 1, nombre: 'Impuesto estándar' }],
        'unidadMedida': [{ id: 1, nombre: 'Unidad', abreviatura: 'U' }],
        'proveedor': [{ id: 1, nombre: 'Proveedor genérico' }],
        'almacen': [{ id: 1, nombre: 'Almacén principal' }]
    };
    
    if (datos[tipo]) {
        const options = datos[tipo].map(item => new Option(item.nombre, item.id));
        $select.append(options);
        $select.trigger('change');
    }
}

/**
 * Carga categorías desde el servidor
 * @param {number} categoriaIdToSelect - ID opcional de la categoría a seleccionar una vez cargadas
 */
function cargarCategorias(categoriaIdToSelect) {
    // Limpiar y mostrar indicador de carga
    $('.select2-categoria').empty().append(new Option('Cargando...', ''));
    
    console.log('Cargando categorías...');
    
    // Cargar categorías mediante AJAX
    $.get('../Categoria/ObtenerTodas')
        .done(function(categorias) {
            console.log('Categorías cargadas exitosamente:', categorias);
            
            // Limpiar selector
            $('.select2-categoria').empty();
            
            // Si hay datos, agregar opciones
            if (categorias && categorias.length > 0) {
                // Agregar opción vacía primero (opcional)
                $('.select2-categoria').append(new Option('Seleccione una categoría', ''));
                
                // Agregar cada categoría al selector
                categorias.forEach(function(categoria) {
                    console.log('Agregando categoría:', categoria);
                    const option = new Option(categoria.nombre, categoria.id);
                    // Almacenar los datos completos para Select2
                    $(option).data('data', categoria);
                    $('.select2-categoria').append(option);
                });
                
                // Si hay un ID para seleccionar, hacerlo
                if (categoriaIdToSelect) {
                    console.log('Seleccionando categoría ID:', categoriaIdToSelect);
                    $('.select2-categoria').val(categoriaIdToSelect);
                    $('.select2-categoria').trigger('change');
                }
            } else {
                // Si no hay categorías, mostrar mensaje
                $('.select2-categoria').append(new Option('No hay categorías disponibles', ''));
            }
            
            // Ya no añadimos la opción para crear nueva categoría aquí,
            // ahora usamos el botón en el dropdown
            
            // Actualizar select2
            $('.select2-categoria').trigger('change');
        })
        .fail(function(xhr, status, error) {
            console.error('Error cargando categorías:', error);
            console.log('URL:', '../Categoria/ObtenerTodas');
            console.log('Estado:', status);
            console.log('Respuesta:', xhr.responseText);
            
            // Cargar datos estáticos como fallback
            cargarDatosEstaticos('categoria', $('.select2-categoria'));
            
            // Actualizar select2
            $('.select2-categoria').trigger('change');
        });
}

// 1. Definir funciones de formato para categorías
function formatCategoria(categoria) {
    if (!categoria.id) {
        return categoria.text;
    }
    
    // Opción especial para crear nueva categoría
    if (categoria.id === 'nueva') {
        return $('<div class="select2-nueva-opcion"><i class="fas fa-plus-circle text-primary me-1"></i> Agregar Nueva Categoría</div>');
    }
    
    // Formato normal para categorías existentes
    return $('<div>' + categoria.text + '</div>');
}

function formatCategoriaSelection(categoria) {
    if (!categoria.id || categoria.id === 'nueva') {
        return categoria.text;
    }
    
    // Crear contenedor con botón de edición
    var $contenedor = $(
        '<div class="d-flex justify-content-between align-items-center w-100">' +
            '<span>' + categoria.text + '</span>' +
            '<button type="button" class="btn-editar-categoria btn btn-sm btn-link p-0 ms-2" data-id="' + categoria.id + '">' +
                '<i class="fas fa-pencil-alt"></i>' +
            '</button>' +
        '</div>'
    );
    
    // Agregar evento al botón de edición
    $contenedor.find('.btn-editar-categoria').on('mousedown', function(e) {
        e.preventDefault();
        e.stopPropagation();
        var categoriaId = $(this).data('id');
        setTimeout(function() {
            editarCategoria(categoriaId);
        }, 100);
    });
    
    return $contenedor;
}

// 2. Definir funciones de formato para marcas
function formatMarca(marca) {
    if (!marca.id) {
        return marca.text;
    }
    
    // Opción especial para crear nueva marca
    if (marca.id === 'nueva') {
        return $('<div class="select2-nueva-opcion"><i class="fas fa-plus-circle text-primary me-1"></i> Agregar Nueva Marca</div>');
    }
    
    // Formato normal para marcas existentes
    return $('<div>' + marca.text + '</div>');
}

function formatMarcaSelection(marca) {
    if (!marca.id || marca.id === 'nueva') {
        return marca.text;
    }
    
    // Crear contenedor con botón de edición
    var $contenedor = $(
        '<div class="d-flex justify-content-between align-items-center w-100">' +
            '<span>' + marca.text + '</span>' +
            '<button type="button" class="btn-editar-marca btn btn-sm btn-link p-0 ms-2" data-id="' + marca.id + '">' +
                '<i class="fas fa-pencil-alt"></i>' +
            '</button>' +
        '</div>'
    );
    
    // Agregar evento al botón de edición
    $contenedor.find('.btn-editar-marca').on('mousedown', function(e) {
        e.preventDefault();
        e.stopPropagation();
        var marcaId = $(this).data('id');
        setTimeout(function() {
            editarMarca(marcaId);
        }, 100);
    });
    
    return $contenedor;
}

// 3. Inicialización de Select2 para categorías y marcas
function inicializarSelect2() {
    try {
        // Verificar si select2 está disponible
        if (typeof $.fn.select2 === 'undefined') {
            console.error('Error: Select2 no está disponible');
            return;
        }

        console.log('Inicializando select2 para categorías y marcas...');

        // Desactivar cualquier inicialización previa de forma segura
        if ($('.select2-categoria').hasClass('select2-hidden-accessible')) {
            try { $('.select2-categoria').select2('destroy'); } catch (e) { console.warn('Error al destruir select2-categoria:', e); }
        }

        if ($('.select2-marca').hasClass('select2-hidden-accessible')) {
            try { $('.select2-marca').select2('destroy'); } catch (e) { console.warn('Error al destruir select2-marca:', e); }
        }

        // Inicializar select2 para categorías
        $('.select2-categoria').select2({
            theme: 'bootstrap-5',
            width: '100%',
            placeholder: 'Seleccione una categoría',
            allowClear: true,
            templateResult: formatCategoria,
            templateSelection: formatCategoriaSelection,
            escapeMarkup: function(m) { return m; }
        }).on('select2:select', function(e) {
            if (e.params.data.id === 'nueva') {
                // Reestablecer la selección
                $(this).val(null).trigger('change');
                // Abrir offcanvas para crear nueva categoría
                crearCategoria();
            }
        });

        // Inicializar select2 para marcas
        $('.select2-marca').select2({
            theme: 'bootstrap-5',
            width: '100%',
            placeholder: 'Seleccione una marca',
            allowClear: true,
            templateResult: formatMarca,
            templateSelection: formatMarcaSelection,
            escapeMarkup: function(m) { return m; }
        }).on('select2:select', function(e) {
            if (e.params.data.id === 'nueva') {
                // Reestablecer la selección
                $(this).val(null).trigger('change');
                // Abrir offcanvas para crear nueva marca
                crearMarca();
            }
        });

        console.log('Select2 inicializado correctamente');
    } catch (error) {
        console.error('Error al inicializar select2:', error);
    }
}

// Estilos adicionales para mejorar la apariencia
function aplicarEstilosSelect2() {
    $("<style>")
        .prop("type", "text/css")
        .html(`
            .select2-nueva-opcion {
                color: #0d6efd;
                border-top: 1px solid #dee2e6;
                padding-top: 8px;
                margin-top: 4px;
                font-weight: 500;
            }
            .select2-results__option {
                padding: 6px 12px;
            }
            .select2-container--bootstrap-5 .select2-selection--single {
                height: calc(1.5em + 0.75rem + 2px);
                padding: 0.375rem 2.25rem 0.375rem 0.75rem;
            }
            .btn-editar-categoria, .btn-editar-marca {
                color: #0d6efd;
                margin-left: 0.5rem;
                padding: 0;
                min-width: 20px;
            }
            .btn-editar-categoria:hover, .btn-editar-marca:hover {
                color: #0a58ca;
            }
        `)
        .appendTo("head");
}

/**
 * Implementación completa de Select2 para categorías y marcas 
 * con apertura de offcanvas para creación/edición
 */

// 1. Configuración inicial para elementos Select2
$(document).ready(function() {
    console.log('Inicializando formulario de ítems...');
    
    // Ocultar botones originales
    $('#btnNuevaCategoria, #btnNuevaMarca').hide();
    
    // Inicializar Select2 para categorías
    initCategoriaSelect();
    
    // Inicializar Select2 para marcas
    initMarcaSelect();
    
    // Inicializar otros componentes...
    // ... existing code ...
});

/**
 * Inicializa el select2 para categorías con capacidad de búsqueda, 
 * creación y edición, abriendo el offcanvas correspondiente
 */
function initCategoriaSelect() {
    $('.select2-categoria').select2({
        theme: 'bootstrap-5',
        placeholder: 'Seleccione una categoría',
        allowClear: true,
        width: '100%',
        language: {
            noResults: function() {
                return "No se encontraron resultados";
            }
        },
        escapeMarkup: function(markup) {
            return markup;
        },
        ajax: {
            url: '../Categoria/ObtenerTodas',
            dataType: 'json',
            delay: 250,
            data: function(params) {
                return {
                    term: params.term || ''
                };
            },
            processResults: function(data) {
                var results = data;
                
                // Añadir opción para crear nueva categoría cuando no hay resultados
                if (this.term && this.term.trim() !== '') {
                    results.push({
                        id: 'nueva',
                        text: 'Crear categoría: "' + this.term + '"',
                        term: this.term
                    });
                } else {
                    // O si no hay término de búsqueda, agregar opción al final
                    results.push({
                        id: 'nueva',
                        text: 'Agregar Nueva Categoría',
                        term: ''
                    });
                }
                
                return {
                    results: results.map(function(item) {
                        return {
                            id: item.id || item.id,
                            text: item.nombre || item.text,
                            term: item.term || ''
                        };
                    })
                };
            },
            cache: false // Desactivar caché para siempre mostrar la opción de creación
        },
        templateResult: formatCategoriaResult,
        templateSelection: formatCategoriaSelection
    }).on('select2:select', function(e) {
        var data = e.params.data;
        
        // Si se seleccionó la opción para crear nueva categoría
        if (data.id === 'nueva') {
            console.log('Seleccionada opción "Crear nueva categoría", abriendo offcanvas...');
            
            // Eliminar la selección actual
            $(this).val(null).trigger('change');
            
            // IMPORTANTE: Abrir el offcanvas para crear categoría
            crearCategoria(data.term);
        }
    });
}

/**
 * Inicializa el select2 para marcas con capacidad de búsqueda, 
 * creación y edición, abriendo el offcanvas correspondiente
 */
function initMarcaSelect() {
    $('.select2-marca').select2({
        theme: 'bootstrap-5',
        placeholder: 'Seleccione una marca',
        allowClear: true,
        width: '100%',
        language: {
            noResults: function() {
                return "No se encontraron resultados";
            }
        },
        escapeMarkup: function(markup) {
            return markup;
        },
        ajax: {
            url: '../Marca/ObtenerTodas',
            dataType: 'json',
            delay: 250,
            data: function(params) {
                return {
                    term: params.term || ''
                };
            },
            processResults: function(data) {
                var results = data;
                
                // Añadir opción para crear nueva marca cuando no hay resultados
                if (this.term && this.term.trim() !== '') {
                    results.push({
                        id: 'nueva',
                        text: 'Crear marca: "' + this.term + '"',
                        term: this.term
                    });
                } else {
                    // O si no hay término de búsqueda, agregar opción al final
                    results.push({
                        id: 'nueva',
                        text: 'Agregar Nueva Marca',
                        term: ''
                    });
                }
                
                return {
                    results: results.map(function(item) {
                        return {
                            id: item.id || item.id,
                            text: item.nombre || item.text,
                            term: item.term || ''
                        };
                    })
                };
            },
            cache: false // Desactivar caché para siempre mostrar la opción de creación
        },
        templateResult: formatMarcaResult,
        templateSelection: formatMarcaSelection
    }).on('select2:select', function(e) {
        var data = e.params.data;
        
        // Si se seleccionó la opción para crear nueva marca
        if (data.id === 'nueva') {
            console.log('Seleccionada opción "Crear nueva marca", abriendo offcanvas...');
            
            // Eliminar la selección actual
            $(this).val(null).trigger('change');
            
            // IMPORTANTE: Abrir el offcanvas para crear marca
            crearMarca(data.term);
        }
    });
}

/**
 * Formatea la visualización de resultados de categorías en el dropdown
 */
function formatCategoriaResult(categoria) {
    if (categoria.loading) {
        return categoria.text;
    }
    
    if (categoria.id === 'nueva') {
        return '<div class="select2-result-categoria">' +
               '<div class="select2-result-categoria__action">' +
               '<i class="fas fa-plus-circle text-primary me-1"></i> ' + 
               categoria.text + '</div></div>';
    }
    
    return '<div class="select2-result-categoria">' +
           '<div class="select2-result-categoria__name">' + categoria.text + '</div>' +
           '</div>';
}

/**
 * Formatea la visualización de la categoría seleccionada con botón de edición
 */
function formatCategoriaSelection(categoria) {
    if (!categoria.id || categoria.id === 'nueva') {
        return categoria.text;
    }
    
    // Crear contenedor con botón de edición
    var $container = $('<div class="d-flex align-items-center justify-content-between w-100"></div>');
    var $name = $('<div>' + categoria.text + '</div>');
    var $actions = $('<div class="categoria-actions ms-2"></div>');
    
    var $editBtn = $('<button type="button" class="btn btn-link btn-sm p-0 me-2 edit-categoria" ' +
                    'data-id="' + categoria.id + '" data-name="' + categoria.text + '">' +
                    '<i class="fas fa-pencil-alt text-primary"></i></button>');
    
    $actions.append($editBtn);
    $container.append($name);
    $container.append($actions);
    
    // Manejar evento de edición - IMPORTANTE: Abre el offcanvas de edición
    setTimeout(function() {
        $('.edit-categoria').off('click').on('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            console.log('Botón editar categoría clickeado, abriendo offcanvas...');
            editarCategoria($(this).data('id'));
        });
    }, 100);
    
    return $container;
}

/**
 * Formatea la visualización de resultados de marcas en el dropdown
 */
function formatMarcaResult(marca) {
    if (marca.loading) {
        return marca.text;
    }
    
    if (marca.id === 'nueva') {
        return '<div class="select2-result-marca">' +
               '<div class="select2-result-marca__action">' +
               '<i class="fas fa-plus-circle text-primary me-1"></i> ' + 
               marca.text + '</div></div>';
    }
    
    return '<div class="select2-result-marca">' +
           '<div class="select2-result-marca__name">' + marca.text + '</div>' +
           '</div>';
}

/**
 * Formatea la visualización de la marca seleccionada con botón de edición
 */
function formatMarcaSelection(marca) {
    if (!marca.id || marca.id === 'nueva') {
        return marca.text;
    }
    
    // Crear contenedor con botón de edición
    var $container = $('<div class="d-flex align-items-center justify-content-between w-100"></div>');
    var $name = $('<div>' + marca.text + '</div>');
    var $actions = $('<div class="marca-actions ms-2"></div>');
    
    var $editBtn = $('<button type="button" class="btn btn-link btn-sm p-0 me-2 edit-marca" ' +
                    'data-id="' + marca.id + '" data-name="' + marca.text + '">' +
                    '<i class="fas fa-pencil-alt text-primary"></i></button>');
    
    $actions.append($editBtn);
    $container.append($name);
    $container.append($actions);
    
    // Manejar evento de edición - IMPORTANTE: Abre el offcanvas de edición
    setTimeout(function() {
        $('.edit-marca').off('click').on('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            console.log('Botón editar marca clickeado, abriendo offcanvas...');
            editarMarca($(this).data('id'));
        });
    }, 100);
    
    return $container;
}

/**
 * Crea una nueva categoría - IMPORTANTE: Abre el offcanvas
 */
function crearCategoria(nombre) {
    console.log('Iniciando creación de nueva categoría...');
    
    // Asegurarse de que existe el elemento offcanvas
    if ($('#offcanvasCategoria').length === 0) {
        console.log('Creando el elemento offcanvas para categoría...');
        const offcanvasHtml = `
            <div class="offcanvas offcanvas-end" style="width: 600px;" tabindex="-1" id="offcanvasCategoria" 
                 aria-labelledby="offcanvasCategoriaLabel">
                <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                    <h5 class="offcanvas-title" id="offcanvasCategoriaLabel">Crear Categoría</h5>
                    <button type="button" class="btn-close btn-close-white text-reset" 
                        data-bs-dismiss="offcanvas" aria-label="Close"></button>
                </div>
                <div class="offcanvas-body">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                </div>
            </div>
        `;
        $('body').append(offcanvasHtml);
    }
    
    // IMPORTANTE: Mostrar offcanvas antes de cargar el form
    var offcanvasEl = document.getElementById('offcanvasCategoria');
    var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
    offcanvas.show();
    
    // Cargar formulario en el offcanvas
    console.log('Cargando formulario partial en el offcanvas...');
    $.get('../Categoria/CreatePartial', function(data) {
        console.log('Formulario parcial cargado correctamente');
        
        // Insertar el formulario en el offcanvas
        $('#offcanvasCategoria .offcanvas-body').html(data);
        
        // Si pasamos un nombre predeterminado, establecerlo en el campo
        if (nombre) {
            $('#offcanvasCategoria .offcanvas-body #Nombre').val(nombre);
        }
        
        // Inicializar formulario y validación
        $('#formCategoriaCreate').off('submit').on('submit', function(e) {
            e.preventDefault();
            console.log('Formulario de categoría enviado');
            
            // Enviar formulario por AJAX
            $.ajax({
                url: $(this).attr('action'),
                type: 'POST',
                data: $(this).serialize(),
                success: function(response) {
                    console.log('Respuesta recibida:', response);
                    if (response.success) {
                        // Cerrar offcanvas
                        var offcanvasEl = document.getElementById('offcanvasCategoria');
                        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                        offcanvas.hide();
                        
                        // Mostrar mensaje de éxito
                        Swal.fire({
                            icon: 'success',
                            title: 'Categoría creada',
                            text: 'La categoría ha sido creada con éxito',
                            timer: 2000,
                            showConfirmButton: false
                        });
                        
                        // Recargar Select2
                        $('.select2-categoria').empty();
                        
                        // Desencadenar proceso de recarga
                        $('.select2-categoria').trigger({
                            type: 'select2:select',
                            params: {
                                data: {
                                    id: response.categoriaId || response.id,
                                    text: response.nombre
                                }
                            }
                        });
                    } else {
                        // Mostrar errores
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message || 'Ha ocurrido un error al crear la categoría'
                        });
                    }
                },
                error: function(xhr, status, error) {
                    console.error('Error al enviar formulario:', error, xhr.responseText);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Ha ocurrido un error al crear la categoría'
                    });
                }
            });
        });
        
        // Inicializar select2 dentro del offcanvas
        setTimeout(function() {
            $('#offcanvasCategoria .form-select').each(function() {
                if (!$(this).hasClass('select2-hidden-accessible')) {
                    $(this).select2({
                        theme: 'bootstrap-5',
                        width: '100%',
                        dropdownParent: $('#offcanvasCategoria')
                    });
                }
            });
        }, 300);
        
    }).fail(function(xhr, status, error) {
        console.error('Error cargando formulario de categoría:', error);
        console.log('Estado:', status);
        console.log('Respuesta:', xhr.responseText);
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudo cargar el formulario para crear categoría'
        });
    });
}

/**
 * Crea una nueva marca - IMPORTANTE: Abre el offcanvas
 */
function crearMarca(nombre) {
    console.log('Iniciando creación de nueva marca...');
    
    // Asegurarse de que existe el elemento offcanvas
    if ($('#offcanvasMarca').length === 0) {
        console.log('Creando el elemento offcanvas para marca...');
        const offcanvasHtml = `
            <div class="offcanvas offcanvas-end" style="width: 600px;" tabindex="-1" id="offcanvasMarca" 
                 aria-labelledby="offcanvasMarcaLabel">
                <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                    <h5 class="offcanvas-title" id="offcanvasMarcaLabel">Crear Marca</h5>
                    <button type="button" class="btn-close btn-close-white text-reset" 
                        data-bs-dismiss="offcanvas" aria-label="Close"></button>
                </div>
                <div class="offcanvas-body">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                </div>
            </div>
        `;
        $('body').append(offcanvasHtml);
    }
    
    // IMPORTANTE: Mostrar offcanvas antes de cargar el form
    var offcanvasEl = document.getElementById('offcanvasMarca');
    var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
    offcanvas.show();
    
    // Cargar formulario en el offcanvas (similar a crearCategoria)
    $.get('../Marca/CreatePartial', function(data) {
        // Implementación similar a crearCategoria
        $('#offcanvasMarca .offcanvas-body').html(data);
        
        // Si pasamos un nombre predeterminado, establecerlo en el campo
        if (nombre) {
            $('#offcanvasMarca .offcanvas-body #Nombre').val(nombre);
        }
        
        // Inicializar formulario y manejo AJAX similar a crearCategoria
        $('#formMarcaCreate').off('submit').on('submit', function(e) {
            e.preventDefault();
            
            $.ajax({
                url: $(this).attr('action'),
                type: 'POST',
                data: $(this).serialize(),
                success: function(response) {
                    if (response.success) {
                        // Cerrar offcanvas
                        var offcanvasEl = document.getElementById('offcanvasMarca');
                        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                        offcanvas.hide();
                        
                        // Mostrar mensaje y recargar select2
                        Swal.fire({
                            icon: 'success',
                            title: 'Marca creada',
                            text: 'La marca ha sido creada con éxito',
                            timer: 2000,
                            showConfirmButton: false
                        });
                        
                        // Recargar Select2
                        $('.select2-marca').empty();
                        
                        // Desencadenar proceso de recarga
                        $('.select2-marca').trigger({
                            type: 'select2:select',
                            params: {
                                data: {
                                    id: response.marcaId || response.id,
                                    text: response.nombre
                                }
                            }
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message || 'Ha ocurrido un error al crear la marca'
                        });
                    }
                },
                error: function(xhr, status, error) {
                    console.error('Error al enviar formulario:', error);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Ha ocurrido un error al crear la marca'
                    });
                }
            });
        });
        
        // Inicializar select2 dentro del offcanvas
        setTimeout(function() {
            $('#offcanvasMarca .form-select').each(function() {
                if (!$(this).hasClass('select2-hidden-accessible')) {
                    $(this).select2({
                        theme: 'bootstrap-5',
                        width: '100%',
                        dropdownParent: $('#offcanvasMarca')
                    });
                }
            });
        }, 300);
    }).fail(function(xhr, status, error) {
        console.error('Error cargando formulario de marca:', error);
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudo cargar el formulario para crear marca'
        });
    });
}

/**
 * Edita una marca existente - IMPORTANTE: Abre el offcanvas
 */
function editarMarca(marcaId) {
    // Implementación similar a editarCategoria
    console.log('Iniciando edición de marca ID:', marcaId);
    
    // Asegurarse de que existe el elemento offcanvas
    if ($('#offcanvasMarca').length === 0) {
        const offcanvasHtml = `
            <div class="offcanvas offcanvas-end" style="width: 600px;" tabindex="-1" id="offcanvasMarca" 
                 aria-labelledby="offcanvasMarcaLabel">
                <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                    <h5 class="offcanvas-title" id="offcanvasMarcaLabel">Editar Marca</h5>
                    <button type="button" class="btn-close btn-close-white text-reset" 
                        data-bs-dismiss="offcanvas" aria-label="Close"></button>
                </div>
                <div class="offcanvas-body">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                </div>
            </div>
        `;
        $('body').append(offcanvasHtml);
    } else {
        $('#offcanvasMarcaLabel').text('Editar Marca');
    }
    
    // IMPORTANTE: Mostrar offcanvas
    var offcanvasEl = document.getElementById('offcanvasMarca');
    var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
    offcanvas.show();
    
    // Cargar formulario y resto de implementación similar a editarCategoria
    $.get('../Marca/EditPartial/' + marcaId, function(data) {
        $('#offcanvasMarca .offcanvas-body').html(data);
        
        $('#offcanvasMarca form').off('submit').on('submit', function(e) {
            e.preventDefault();
            
            $.ajax({
                url: $(this).attr('action'),
                type: 'POST',
                data: $(this).serialize(),
                success: function(response) {
                    if (response.success) {
                        var offcanvasEl = document.getElementById('offcanvasMarca');
                        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                        offcanvas.hide();
                        
                        Swal.fire({
                            icon: 'success',
                            title: 'Marca actualizada',
                            text: 'La marca ha sido actualizada con éxito',
                            timer: 2000,
                            showConfirmButton: false
                        });
                        
                        // Recargar select2
                        $('.select2-marca').empty().trigger('change');
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message || 'Ha ocurrido un error al actualizar la marca'
                        });
                    }
                },
                error: function(xhr, status, error) {
                    console.error('Error al enviar formulario:', error);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Ha ocurrido un error al actualizar la marca'
                    });
                }
            });
        });
        
        // Inicializar select2 dentro del offcanvas
        setTimeout(function() {
            $('#offcanvasMarca .form-select').each(function() {
                if (!$(this).hasClass('select2-hidden-accessible')) {
                    $(this).select2({
                        theme: 'bootstrap-5',
                        width: '100%',
                        dropdownParent: $('#offcanvasMarca')
                    });
                }
            });
        }, 300);
    }).fail(function(xhr, status, error) {
        console.error('Error cargando formulario de edición de marca:', error);
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudo cargar el formulario para editar la marca'
        });
    });
}

/**
 * SCRIPT PARA CORREGIR PROBLEMAS DE SELECT2 EN CATEGORÍA Y MARCA
 * Este script debe reemplazar toda la implementación anterior
 */

// 1. LIMPIEZA INICIAL: Al cargar la página, remover elementos duplicados
$(document).ready(function() {
    console.log('Iniciando corrección de problemas en Select2...');
    
    // Eliminar elementos Select2 duplicados
    $('.select2-container--bootstrap-5').not(':first-of-type').each(function() {
        console.log('Eliminando contenedor Select2 duplicado');
        $(this).remove();
    });
    
    // Ocultar botones originales
    $('#btnNuevaCategoria, #btnNuevaMarca').hide();
    
    // Reinicializar Select2 correctamente
    setTimeout(function() {
        initializeSelects();
    }, 100);
});

// 2. INICIALIZACIÓN CORREGIDA DE SELECT2
function initializeSelects() {
    // Destruir instancias previas si existen
    if ($('.select2-categoria').hasClass('select2-hidden-accessible')) {
        try {
            $('.select2-categoria').select2('destroy');
        } catch(e) {
            console.warn('Error al destruir select2-categoria:', e);
        }
    }
    
    if ($('.select2-marca').hasClass('select2-hidden-accessible')) {
        try {
            $('.select2-marca').select2('destroy');
        } catch(e) {
            console.warn('Error al destruir select2-marca:', e);
        }
    }
    
    // Inicializar categoría
    $('.select2-categoria').select2({
        theme: 'bootstrap-5',
        placeholder: 'Seleccione una categoría',
        allowClear: true,
        width: '100%',
        templateResult: formatCategoriaOption,
        templateSelection: formatCategoriaSelection,
        language: {
            noResults: function() {
                return "No se encontraron resultados";
            }
        },
        escapeMarkup: function(m) { return m; }
    });
    
    // Inicializar marca
    $('.select2-marca').select2({
        theme: 'bootstrap-5',
        placeholder: 'Seleccione una marca',
        allowClear: true,
        width: '100%',
        templateResult: formatMarcaOption,
        templateSelection: formatMarcaSelection,
        language: {
            noResults: function() {
                return "No se encontraron resultados";
            }
        },
        escapeMarkup: function(m) { return m; }
    });
    
    // Cargar datos
    loadCategorias();
    loadMarcas();
    
    // Configurar eventos
    setupEvents();
}

// 3. CARGAR DATOS DESDE LA BD
function loadCategorias() {
    $.ajax({
        url: '../Categoria/ObtenerTodas',
        type: 'GET',
        dataType: 'json',
        success: function(data) {
            console.log('Categorías cargadas:', data);
            
            // Vaciar select y agregar opción placeholder
            $('.select2-categoria').empty();
            $('.select2-categoria').append(new Option('', '', true, true));
            
            // Agregar categorías
            if (data && data.length > 0) {
                data.forEach(function(cat) {
                    // Asegurarse de que id y nombre sean valores correctos
                    const id = cat.id || cat.Id;
                    const nombre = cat.nombre || cat.Nombre || '(Sin nombre)';
                    
                    const option = new Option(nombre, id, false, false);
                    $('.select2-categoria').append(option);
                });
            }
            
            // Agregar opción "Agregar Nueva" (solo una vez)
            if ($('.select2-categoria option[value="nueva"]').length === 0) {
                const nuevaOption = new Option('+ Agregar Nueva Categoría', 'nueva', false, false);
                $('.select2-categoria').append(nuevaOption);
            }
            
            // Refrescar select2
            $('.select2-categoria').trigger('change');
        },
        error: function(xhr, status, error) {
            console.error('Error cargando categorías:', error);
            
            // Cargar datos estáticos como fallback
            const categorias = [
                { id: 1, nombre: 'Bebidas' },
                { id: 2, nombre: 'Alimentos' },
                { id: 3, nombre: 'Limpieza' }
            ];
            
            $('.select2-categoria').empty();
            $('.select2-categoria').append(new Option('', '', true, true));
            
            categorias.forEach(function(cat) {
                const option = new Option(cat.nombre, cat.id, false, false);
                $('.select2-categoria').append(option);
            });
            
            const nuevaOption = new Option('+ Agregar Nueva Categoría', 'nueva', false, false);
            $('.select2-categoria').append(nuevaOption);
            
            $('.select2-categoria').trigger('change');
        }
    });
}

function loadMarcas() {
    $.ajax({
        url: '../Marca/ObtenerTodas',
        type: 'GET',
        dataType: 'json',
        success: function(data) {
            console.log('Marcas cargadas:', data);
            
            // Vaciar select y agregar opción placeholder
            $('.select2-marca').empty();
            $('.select2-marca').append(new Option('', '', true, true));
            
            // Agregar marcas
            if (data && data.length > 0) {
                data.forEach(function(marca) {
                    // Asegurarse de que id y nombre sean valores correctos
                    const id = marca.id || marca.Id;
                    const nombre = marca.nombre || marca.Nombre || '(Sin nombre)';
                    
                    const option = new Option(nombre, id, false, false);
                    $('.select2-marca').append(option);
                });
            }
            
            // Agregar opción "Agregar Nueva" (solo una vez)
            if ($('.select2-marca option[value="nueva"]').length === 0) {
                const nuevaOption = new Option('+ Agregar Nueva Marca', 'nueva', false, false);
                $('.select2-marca').append(nuevaOption);
            }
            
            // Refrescar select2
            $('.select2-marca').trigger('change');
        },
        error: function(xhr, status, error) {
            console.error('Error cargando marcas:', error);
            
            // Cargar datos estáticos como fallback
            const marcas = [
                { id: 1, nombre: 'Genérica' },
                { id: 2, nombre: 'Nacional' },
                { id: 3, nombre: 'Importada' }
            ];
            
            $('.select2-marca').empty();
            $('.select2-marca').append(new Option('', '', true, true));
            
            marcas.forEach(function(marca) {
                const option = new Option(marca.nombre, marca.id, false, false);
                $('.select2-marca').append(option);
            });
            
            const nuevaOption = new Option('+ Agregar Nueva Marca', 'nueva', false, false);
            $('.select2-marca').append(nuevaOption);
            
            $('.select2-marca').trigger('change');
        }
    });
}

// 4. FORMATO PARA OPCIONES DEL DROPDOWN
function formatCategoriaOption(categoria) {
    if (!categoria.id) {
        return categoria.text;
    }
    
    if (categoria.id === 'nueva') {
        return '<div class="py-1 border-top mt-1">' +
               '<i class="fas fa-plus-circle text-primary me-1"></i> ' +
               categoria.text + '</div>';
    }
    
    return '<div>' + categoria.text + '</div>';
}

function formatMarcaOption(marca) {
    if (!marca.id) {
        return marca.text;
    }
    
    if (marca.id === 'nueva') {
        return '<div class="py-1 border-top mt-1">' +
               '<i class="fas fa-plus-circle text-primary me-1"></i> ' +
               marca.text + '</div>';
    }
    
    return '<div>' + marca.text + '</div>';
}

// 5. FORMATO PARA ELEMENTOS SELECCIONADOS (CON BOTÓN DE EDITAR)
function formatCategoriaSelection(categoria) {
    if (!categoria.id || categoria.id === 'nueva') {
        return categoria.text;
    }
    
    // Crear contenedor con el nombre y el botón de edición
    return '<div class="d-flex align-items-center justify-content-between w-100">' +
           '<span>' + categoria.text + '</span>' +
           '<button type="button" class="btn btn-sm btn-link p-0 edit-categoria" ' +
           'data-id="' + categoria.id + '">' +
           '<i class="fas fa-pencil-alt text-primary"></i></button>' +
           '</div>';
}

function formatMarcaSelection(marca) {
    if (!marca.id || marca.id === 'nueva') {
        return marca.text;
    }
    
    // Crear contenedor con el nombre y el botón de edición
    return '<div class="d-flex align-items-center justify-content-between w-100">' +
           '<span>' + marca.text + '</span>' +
           '<button type="button" class="btn btn-sm btn-link p-0 edit-marca" ' +
           'data-id="' + marca.id + '">' +
           '<i class="fas fa-pencil-alt text-primary"></i></button>' +
           '</div>';
}

// 6. CONFIGURAR EVENTOS
function setupEvents() {
    // Evento cuando se selecciona una categoría
    $('.select2-categoria').off('select2:select').on('select2:select', function(e) {
        var data = e.params.data;
        
        // Si se seleccionó "Agregar Nueva"
        if (data.id === 'nueva') {
            // Restablecer la selección
            $(this).val('').trigger('change');
            
            // Abrir offcanvas para crear nueva categoría
            crearCategoria();
        }
    });
    
    // Evento cuando se selecciona una marca
    $('.select2-marca').off('select2:select').on('select2:select', function(e) {
        var data = e.params.data;
        
        // Si se seleccionó "Agregar Nueva"
        if (data.id === 'nueva') {
            // Restablecer la selección
            $(this).val('').trigger('change');
            
            // Abrir offcanvas para crear nueva marca
            crearMarca();
        }
    });
    
    // Evento para botón de editar categoría (delegado al documento)
    $(document).off('click', '.edit-categoria').on('click', '.edit-categoria', function(e) {
        e.preventDefault();
        e.stopPropagation();
        
        const categoriaId = $(this).data('id');
        editarCategoria(categoriaId);
    });
    
    // Evento para botón de editar marca (delegado al documento)
    $(document).off('click', '.edit-marca').on('click', '.edit-marca', function(e) {
        e.preventDefault();
        e.stopPropagation();
        
        const marcaId = $(this).data('id');
        editarMarca(marcaId);
    });
}

// 7. FUNCIONES PARA CREAR/EDITAR CATEGORÍAS Y MARCAS
function crearCategoria() {
    // Verificar si ya existe el offcanvas y crearlo si no
    if ($('#offcanvasCategoria').length === 0) {
        $('body').append(`
            <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasCategoria" 
                 aria-labelledby="offcanvasCategoriaLabel" style="width: 600px;">
                <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                    <h5 class="offcanvas-title" id="offcanvasCategoriaLabel">Crear Categoría</h5>
                    <button type="button" class="btn-close btn-close-white text-reset" 
                            data-bs-dismiss="offcanvas" aria-label="Close"></button>
                </div>
                <div class="offcanvas-body">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                </div>
            </div>
        `);
    }
    
    // Mostrar offcanvas
    var offcanvasEl = document.getElementById('offcanvasCategoria');
    var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
    offcanvas.show();
    
    // Cargar formulario
    $.get('../Categoria/CreatePartial', function(data) {
        $('#offcanvasCategoria .offcanvas-body').html(data);
        
        // IMPORTANTE: Modificar el formulario para prevenir redirección
        $('#offcanvasCategoria form').off('submit').on('submit', function(e) {
            e.preventDefault();
            
            // Enviar por AJAX
            $.ajax({
                url: $(this).attr('action'),
                type: 'POST',
                data: $(this).serialize(),
                success: function(response) {
                    // Cerrar offcanvas sin importar resultado
                    var offcanvasEl = document.getElementById('offcanvasCategoria');
                    var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                    offcanvas.hide();
                    
                    if (response.success) {
                        // Mostrar mensaje de éxito
                        Swal.fire({
                            icon: 'success',
                            title: 'Categoría creada',
                            text: 'La categoría ha sido creada con éxito',
                            timer: 2000,
                            showConfirmButton: false
                        });
                        
                        // Recargar datos
                        loadCategorias();
                        
                        // Seleccionar la nueva categoría
                        setTimeout(function() {
                            var categoriaId = response.categoriaId || response.id;
                            if (categoriaId) {
                                $('.select2-categoria').val(categoriaId).trigger('change');
                            }
                        }, 500);
                    } else {
                        // Mostrar error
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message || 'Ha ocurrido un error al crear la categoría'
                        });
                    }
                },
                error: function() {
                    // Cerrar offcanvas
                    var offcanvasEl = document.getElementById('offcanvasCategoria');
                    var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                    offcanvas.hide();
                    
                    // Mostrar error
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Ha ocurrido un error al crear la categoría'
                    });
                }
            });
        });
    });
}

function editarCategoria(categoriaId) {
    // Verificar si ya existe el offcanvas y crearlo si no
    if ($('#offcanvasCategoria').length === 0) {
        $('body').append(`
            <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasCategoria" 
                 aria-labelledby="offcanvasCategoriaLabel" style="width: 600px;">
                <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                    <h5 class="offcanvas-title" id="offcanvasCategoriaLabel">Editar Categoría</h5>
                    <button type="button" class="btn-close btn-close-white text-reset" 
                            data-bs-dismiss="offcanvas" aria-label="Close"></button>
                </div>
                <div class="offcanvas-body">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                </div>
            </div>
        `);
    } else {
        $('#offcanvasCategoriaLabel').text('Editar Categoría');
    }
    
    // Mostrar offcanvas
    var offcanvasEl = document.getElementById('offcanvasCategoria');
    var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
    offcanvas.show();
    
    // Cargar formulario
    $.get('../Categoria/EditPartial/' + categoriaId, function(data) {
        $('#offcanvasCategoria .offcanvas-body').html(data);
        
        // IMPORTANTE: Modificar el formulario para prevenir redirección
        $('#offcanvasCategoria form').off('submit').on('submit', function(e) {
            e.preventDefault();
            
            // Enviar por AJAX
            $.ajax({
                url: $(this).attr('action'),
                type: 'POST',
                data: $(this).serialize(),
                success: function(response) {
                    // Cerrar offcanvas sin importar resultado
                    var offcanvasEl = document.getElementById('offcanvasCategoria');
                    var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                    offcanvas.hide();
                    
                    if (response.success) {
                        // Mostrar mensaje de éxito
                        Swal.fire({
                            icon: 'success',
                            title: 'Categoría actualizada',
                            text: 'La categoría ha sido actualizada con éxito',
                            timer: 2000,
                            showConfirmButton: false
                        });
                        
                        // Recargar datos
                        loadCategorias();
                    } else {
                        // Mostrar error
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message || 'Ha ocurrido un error al actualizar la categoría'
                        });
                    }
                },
                error: function() {
                    // Cerrar offcanvas
                    var offcanvasEl = document.getElementById('offcanvasCategoria');
                    var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                    offcanvas.hide();
                    
                    // Mostrar error
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Ha ocurrido un error al actualizar la categoría'
                    });
                }
            });
        });
    });
}

function crearMarca() {
    // Verificar si ya existe el offcanvas y crearlo si no
    if ($('#offcanvasMarca').length === 0) {
        $('body').append(`
            <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasMarca" 
                 aria-labelledby="offcanvasMarcaLabel" style="width: 600px;">
                <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                    <h5 class="offcanvas-title" id="offcanvasMarcaLabel">Crear Marca</h5>
                    <button type="button" class="btn-close btn-close-white text-reset" 
                            data-bs-dismiss="offcanvas" aria-label="Close"></button>
                </div>
                <div class="offcanvas-body">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                </div>
            </div>
        `);
    }
    
    // Mostrar offcanvas
    var offcanvasEl = document.getElementById('offcanvasMarca');
    var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
    offcanvas.show();
    
    // Cargar formulario
    $.get('../Marca/CreatePartial', function(data) {
        $('#offcanvasMarca .offcanvas-body').html(data);
        
        // IMPORTANTE: Modificar el formulario para prevenir redirección
        $('#offcanvasMarca form').off('submit').on('submit', function(e) {
            e.preventDefault();
            
            // Enviar por AJAX
            $.ajax({
                url: $(this).attr('action'),
                type: 'POST',
                data: $(this).serialize(),
                success: function(response) {
                    // Cerrar offcanvas sin importar resultado
                    var offcanvasEl = document.getElementById('offcanvasMarca');
                    var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                    offcanvas.hide();
                    
                    if (response.success) {
                        // Mostrar mensaje de éxito
                        Swal.fire({
                            icon: 'success',
                            title: 'Marca creada',
                            text: 'La marca ha sido creada con éxito',
                            timer: 2000,
                            showConfirmButton: false
                        });
                        
                        // Recargar datos
                        loadMarcas();
                        
                        // Seleccionar la nueva marca
                        setTimeout(function() {
                            var marcaId = response.marcaId || response.id;
                            if (marcaId) {
                                $('.select2-marca').val(marcaId).trigger('change');
                            }
                        }, 500);
                    } else {
                        // Mostrar error
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message || 'Ha ocurrido un error al crear la marca'
                        });
                    }
                },
                error: function() {
                    // Cerrar offcanvas
                    var offcanvasEl = document.getElementById('offcanvasMarca');
                    var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                    offcanvas.hide();
                    
                    // Mostrar error
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Ha ocurrido un error al crear la marca'
                    });
                }
            });
        });
    });
}

function editarMarca(marcaId) {
    // Verificar si ya existe el offcanvas y crearlo si no
    if ($('#offcanvasMarca').length === 0) {
        $('body').append(`
            <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasMarca" 
                 aria-labelledby="offcanvasMarcaLabel" style="width: 600px;">
                <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                    <h5 class="offcanvas-title" id="offcanvasMarcaLabel">Editar Marca</h5>
                    <button type="button" class="btn-close btn-close-white text-reset" 
                            data-bs-dismiss="offcanvas" aria-label="Close"></button>
                </div>
                <div class="offcanvas-body">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                </div>
            </div>
        `);
    } else {
        $('#offcanvasMarcaLabel').text('Editar Marca');
    }
    
    // Mostrar offcanvas
    var offcanvasEl = document.getElementById('offcanvasMarca');
    var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
    offcanvas.show();
    
    // Cargar formulario
    $.get('../Marca/EditPartial/' + marcaId, function(data) {
        $('#offcanvasMarca .offcanvas-body').html(data);
        
        // IMPORTANTE: Modificar el formulario para prevenir redirección
        $('#offcanvasMarca form').off('submit').on('submit', function(e) {
            e.preventDefault();
            
            // Enviar por AJAX
            $.ajax({
                url: $(this).attr('action'),
                type: 'POST',
                data: $(this).serialize(),
                success: function(response) {
                    // Cerrar offcanvas sin importar resultado
                    var offcanvasEl = document.getElementById('offcanvasMarca');
                    var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                    offcanvas.hide();
                    
                    if (response.success) {
                        // Mostrar mensaje de éxito
                        Swal.fire({
                            icon: 'success',
                            title: 'Marca actualizada',
                            text: 'La marca ha sido actualizada con éxito',
                            timer: 2000,
                            showConfirmButton: false
                        });
                        
                        // Recargar datos
                        loadMarcas();
                    } else {
                        // Mostrar error
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message || 'Ha ocurrido un error al actualizar la marca'
                        });
                    }
                },
                error: function() {
                    // Cerrar offcanvas
                    var offcanvasEl = document.getElementById('offcanvasMarca');
                    var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                    offcanvas.hide();
                    
                    // Mostrar error
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Ha ocurrido un error al actualizar la marca'
                    });
                }
            });
        });
    });
}

// 8. ESTILOS CSS PERSONALIZADOS
$("<style>")
    .prop("type", "text/css")
    .html(`
        /* Remover contenedores duplicados */
        .select2-container + .select2-container {
            display: none !important;
        }
        
        /* Estilo para el botón de editar */
        .edit-categoria, .edit-marca {
            color: #0d6efd;
            cursor: pointer;
            padding: 0 !important;
            margin-left: 8px !important;
            opacity: 0.8;
            transition: opacity 0.2s;
        }
        
        .edit-categoria:hover, .edit-marca:hover {
            opacity: 1;
        }
        
        /* Estilo para la opción "Agregar Nueva" */
        .select2-results__options .select2-results__option:last-child {
            border-top: 1px solid #eee;
            margin-top: 4px;
            padding-top: 6px;
        }
        
        /* Ocultar botones duplicados de agregar categoría/marca */
        .select2-dropdown .select2-results__option button + button {
            display: none !important;
        }
        
        /* Estilo para el botón dentro del dropdown */
        .select2-btn-container {
            display: none !important; /* Ocultar por completo */
        }
    `)
    .appendTo("head");

// 9. EJECUTAR INICIALIZACIÓN
// Este código se ejecutará cada vez que se cargue la página o cuando haya cambios en el DOM
// que requieran reinicializar los select2
function reinitializeSelects() {
    console.log('Reinicializando Selects...');
    
    // Limpiar cualquier estado previo
    $('.select2-categoria, .select2-marca').off();
    
    // Reinicializar todo
    initializeSelects();
}

// Exponer función global para poder llamarla desde fuera
window.reinitializeSelects = reinitializeSelects;

/**
 * CORRECCIONES FINALES PARA SELECT2 DE CATEGORÍA Y MARCA
 * 
 * 1. Ocultar el botón azul grande de "Nueva Categoría/Marca"
 * 2. Remover validación del campo descripción en formularios de marca
 */

// Aplicar estas correcciones inmediatamente al cargar la página
$(document).ready(function() {
    console.log('Aplicando correcciones finales a Select2...');
    
    // Ejecutar después de un breve retraso para asegurar que todo esté cargado
    setTimeout(ocultarBotonesAgregaNueva, 500);
    
    // También ejecutar cada vez que se abra un dropdown de Select2
    $(document).on('select2:open', function() {
        setTimeout(ocultarBotonesAgregaNueva, 100);
    });
});

/**
 * Oculta los botones grandes azules de "Nueva Categoría/Marca" 
 * pero mantiene las opciones con ícono "+" en el dropdown
 */
function ocultarBotonesAgregaNueva() {
    // 1. Ocultar botones azules grandes de "Nueva Categoría/Marca"
    $('.select2-results__options .btn-primary, .select2-dropdown .btn-primary').each(function() {
        var $btn = $(this);
        
        // Si es un botón grande azul con texto "Nueva Categoría" o "Nueva Marca"
        if ($btn.text().includes('Nueva Categoría') || $btn.text().includes('Nueva Marca')) {
            console.log('Ocultando botón grande:', $btn.text());
            $btn.parent().hide();
        }
    });
    
    // 2. Aplicar CSS directo para asegurar que los botones no aparezcan
    if (!$('#fixSelectStyles').length) {
        $('head').append(`
            <style id="fixSelectStyles">
                /* Ocultar botones azules grandes en dropdown */
                .select2-results__options + .btn-primary,
                .select2-results__options ~ div > .btn-primary,
                .select2-dropdown button.btn-primary, 
                .select2-dropdown .btn-primary {
                    display: none !important;
                }
                
                /* Asegurar que la opción con ícono "+" sea visible */
                .select2-results__option:has(i.fas.fa-plus-circle) {
                    display: block !important;
                }
            </style>
        `);
    }
}

/**
 * Sobrescribe el manejo del formulario de marca para quitar validación del campo descripción
 * Esta función reemplaza las funciones previas de crearMarca y editarMarca
 */
function modificarValidacionFormularioMarca() {
    // Crear nueva marca sin validación de descripción
    window.crearMarca = function() {
        console.log('Abriendo offcanvas para crear marca (sin validación de descripción)');
        
        // Verificar si ya existe el offcanvas y crearlo si no
        if ($('#offcanvasMarca').length === 0) {
            $('body').append(`
                <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasMarca" 
                     aria-labelledby="offcanvasMarcaLabel" style="width: 600px;">
                    <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                        <h5 class="offcanvas-title" id="offcanvasMarcaLabel">Crear Marca</h5>
                        <button type="button" class="btn-close btn-close-white text-reset" 
                                data-bs-dismiss="offcanvas" aria-label="Close"></button>
                    </div>
                    <div class="offcanvas-body">
                        <div class="spinner-border text-primary" role="status">
                            <span class="visually-hidden">Cargando...</span>
                        </div>
                    </div>
                </div>
            `);
        }
        
        // Mostrar offcanvas
        var offcanvasEl = document.getElementById('offcanvasMarca');
        var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
        offcanvas.show();
        
        // Cargar formulario
        $.get('../Marca/CreatePartial', function(data) {
            $('#offcanvasMarca .offcanvas-body').html(data);
            
            // IMPORTANTE: Prevenir validación predeterminada
            const $form = $('#offcanvasMarca form');
            
            // Eliminar required del campo descripción si existe
            $form.find('[name="Descripcion"]').removeAttr('required');
            $form.find('[name="Descripcion"]').removeAttr('data-val-required');
            
            // Modificar la validación de jquery validate si está presente
            if ($.validator && $form.validate) {
                try {
                    $form.validate().settings.rules.Descripcion = {};
                    $form.validate().settings.messages.Descripcion = {};
                } catch (e) {
                    console.warn('No se pudo modificar la validación:', e);
                }
            }
            
            // Manejar envío del formulario sin redirección
            $form.off('submit').on('submit', function(e) {
                e.preventDefault();
                
                // Enviar por AJAX
                $.ajax({
                    url: $(this).attr('action'),
                    type: 'POST',
                    data: $(this).serialize(),
                    success: function(response) {
                        // Cerrar offcanvas sin importar resultado
                        var offcanvasEl = document.getElementById('offcanvasMarca');
                        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                        offcanvas.hide();
                        
                        if (response.success) {
                            // Mostrar mensaje de éxito
                            Swal.fire({
                                icon: 'success',
                                title: 'Marca creada',
                                text: 'La marca ha sido creada con éxito',
                                timer: 2000,
                                showConfirmButton: false
                            });
                            
                            // Recargar marcas
                            if (typeof loadMarcas === 'function') {
                                loadMarcas();
                            }
                            
                            // Seleccionar la nueva marca
                            setTimeout(function() {
                                var marcaId = response.marcaId || response.id;
                                if (marcaId) {
                                    $('.select2-marca').val(marcaId).trigger('change');
                                }
                            }, 500);
                        } else {
                            // Mostrar error
                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: response.message || 'Ha ocurrido un error al crear la marca'
                            });
                        }
                    },
                    error: function() {
                        // Cerrar offcanvas
                        var offcanvasEl = document.getElementById('offcanvasMarca');
                        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                        offcanvas.hide();
                        
                        // Mostrar error
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: 'Ha ocurrido un error al crear la marca'
                        });
                    }
                });
            });
        });
    };
    
    // Editar marca sin validación de descripción
    window.editarMarca = function(marcaId) {
        console.log('Abriendo offcanvas para editar marca (sin validación de descripción)');
        
        // Verificar si ya existe el offcanvas y crearlo si no
        if ($('#offcanvasMarca').length === 0) {
            $('body').append(`
                <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasMarca" 
                     aria-labelledby="offcanvasMarcaLabel" style="width: 600px;">
                    <div class="offcanvas-header" style="background-color: #0A1172; color: white;">
                        <h5 class="offcanvas-title" id="offcanvasMarcaLabel">Editar Marca</h5>
                        <button type="button" class="btn-close btn-close-white text-reset" 
                                data-bs-dismiss="offcanvas" aria-label="Close"></button>
                    </div>
                    <div class="offcanvas-body">
                        <div class="spinner-border text-primary" role="status">
                            <span class="visually-hidden">Cargando...</span>
                        </div>
                    </div>
                </div>
            `);
        } else {
            $('#offcanvasMarcaLabel').text('Editar Marca');
        }
        
        // Mostrar offcanvas
        var offcanvasEl = document.getElementById('offcanvasMarca');
        var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
        offcanvas.show();
        
        // Cargar formulario
        $.get('../Marca/EditPartial/' + marcaId, function(data) {
            $('#offcanvasMarca .offcanvas-body').html(data);
            
            // IMPORTANTE: Prevenir validación predeterminada
            const $form = $('#offcanvasMarca form');
            
            // Eliminar required del campo descripción si existe
            $form.find('[name="Descripcion"]').removeAttr('required');
            $form.find('[name="Descripcion"]').removeAttr('data-val-required');
            
            // Modificar la validación de jquery validate si está presente
            if ($.validator && $form.validate) {
                try {
                    $form.validate().settings.rules.Descripcion = {};
                    $form.validate().settings.messages.Descripcion = {};
                } catch (e) {
                    console.warn('No se pudo modificar la validación:', e);
                }
            }
            
            // Manejar envío del formulario sin redirección
            $form.off('submit').on('submit', function(e) {
                e.preventDefault();
                
                // Enviar por AJAX
                $.ajax({
                    url: $(this).attr('action'),
                    type: 'POST',
                    data: $(this).serialize(),
                    success: function(response) {
                        // Cerrar offcanvas sin importar resultado
                        var offcanvasEl = document.getElementById('offcanvasMarca');
                        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                        offcanvas.hide();
                        
                        if (response.success) {
                            // Mostrar mensaje de éxito
                            Swal.fire({
                                icon: 'success',
                                title: 'Marca actualizada',
                                text: 'La marca ha sido actualizada con éxito',
                                timer: 2000,
                                showConfirmButton: false
                            });
                            
                            // Recargar marcas
                            if (typeof loadMarcas === 'function') {
                                loadMarcas();
                            }
                        } else {
                            // Mostrar error
                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: response.message || 'Ha ocurrido un error al actualizar la marca'
                            });
                        }
                    },
                    error: function() {
                        // Cerrar offcanvas
                        var offcanvasEl = document.getElementById('offcanvasMarca');
                        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                        offcanvas.hide();
                        
                        // Mostrar error
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: 'Ha ocurrido un error al actualizar la marca'
                        });
                    }
                });
            });
        });
    };
}

// Ejecutar la eliminación de validación
modificarValidacionFormularioMarca();

// Aplicar un MutationObserver para detectar y corregir cambios en el DOM
// Esto es útil porque Select2 puede recrear elementos dinámicamente
function setupMutationObserver() {
    // Crear un observador que detecte cambios en el DOM
    var observer = new MutationObserver(function(mutations) {
        mutations.forEach(function(mutation) {
            // Si se agregaron nodos al DOM
            if (mutation.addedNodes.length > 0) {
                // Aplicar nuestras correcciones
                ocultarBotonesAgregaNueva();
            }
        });
    });
    
    // Configurar el observador para monitorear toda la página
    observer.observe(document.body, { 
        childList: true, 
        subtree: true 
    });
    
    console.log('Observador de mutaciones configurado para correcciones automáticas');
}

// Iniciar el observador
setupMutationObserver();