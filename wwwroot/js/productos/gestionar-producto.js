// gestionar-producto.js - Version limpia
// Clonado exactamente del módulo Items

// Variable para controlar si la pestaña de recetas ha sido abierta
let recetaTabOpened = false;
let recetaDataLoaded = false;

// Verificar que jQuery esté disponible
if (typeof jQuery === 'undefined') {
    console.error('jQuery no está disponible!');
}

$(document).ready(function() {
    console.log('Document ready ejecutado en gestionar-producto.js');
    // Solo el código del Select2 de categorías clonado de Items
    initCategoriasSelect2();
    
    // Inicializar precios dinámicos
    initPricingSystem();
    
    // Inicializar Select2 para rutas de impresión
    initRutasImpresionSelect2();
    
    // Inicializar componente de carga de imágenes
    initImageUploader();
    
    // Inicializar selector de color Pickr
    initColorPicker();
    
    // Inicializar Select2 para cuentas contables
    initCuentasContablesSelect2();
    
    // Evento para el botón de edición en la selección (después de seleccionar)
    $(document).on('click', '.edit-categoria', function(e) {
        e.preventDefault();
        e.stopPropagation();
        var id = $(this).data('id');
        var nombre = $(this).data('name');
        editCategoria(id, nombre);
    });
    
    // Agregar manejo de herencia de categorías
    initCategoriaInheritance();
    
    // Detectar cuando se abre la pestaña de recetas
    $('#inventario-tab').on('click', function() {
        recetaTabOpened = true;
        const productoId = $('#productoId').val();
        if (productoId && productoId !== '0' && !recetaDataLoaded) {
            cargarRecetaExistente(productoId);
            recetaDataLoaded = true;
        }
    });
    
    // El campo de costo ahora es readonly y se actualiza desde la receta
    // por lo que removemos el listener de input
    
    // Si estamos editando un producto, cargar su datos (pero no la receta automáticamente)
    const productoId = $('#productoId').val();
    console.log('ProductoId encontrado en el DOM:', productoId);
    console.log('Tipo de productoId:', typeof productoId);
    
    if (productoId && productoId !== '0' && productoId !== '') {
        console.log('Iniciando carga de producto existente con ID:', productoId);
        // Deshabilitar la herencia mientras se cargan los datos
        allowInheritancePopup = false;
        // Esperar un poco para asegurar que TinyMCE esté inicializado
        setTimeout(() => {
            console.log('Ejecutando cargarDatosProductoExistente después del timeout');
            cargarDatosProductoExistente(productoId);
        }, 1000); // Aumentar el timeout a 1 segundo
    } else {
        console.log('No se encontró productoId válido, modo creación');
    }
});

// Variable global para controlar cuándo mostrar el popup de herencia
let allowInheritancePopup = false;

// Función para manejar la herencia de datos desde la categoría
function initCategoriaInheritance() {
    let ultimaCategoriaCreada = null;
    
    // Activar el popup después de un pequeño delay para evitar que aparezca en la carga inicial
    setTimeout(() => {
        allowInheritancePopup = true;
    }, 2000);
    
    $('#categoriaId').off('change.inheritance').on('change.inheritance', function() {
        const categoriaId = $(this).val();
        
        if (!categoriaId) return;
        
        // No procesar si no está permitido mostrar el popup (carga inicial)
        if (!allowInheritancePopup) {
            console.log('Omitiendo herencia durante carga inicial');
            return;
        }
        
        // Verificar si es una categoría recién creada
        const dataItem = $('#categoriaId').select2('data')[0];
        
        if (ultimaCategoriaCreada && ultimaCategoriaCreada.id == categoriaId) {
            console.log('Omitiendo procesamiento para categoría recién creada:', categoriaId);
            return;
        }
        
        if (dataItem && (dataItem._isNew || dataItem.id === 'new')) {
            console.log('Omitiendo procesamiento para categoría nueva:', dataItem);
            return;
        }
        
        // Mostrar indicador de carga
        Swal.fire({
            title: 'Procesando...',
            text: 'Obteniendo datos de la categoría',
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });
        
        // Limpiar valores anteriores
        const selectsToReset = [
            '#impuestoId',
            '#cuentaVentasId',
            '#cuentaComprasInventariosId',
            '#cuentaCostoVentasGastosId',
            '#cuentaDescuentosId',
            '#cuentaDevolucionesId',
            '#cuentaAjustesId',
            '#cuentaCostoMateriaPrimaId'
        ];
        
        selectsToReset.forEach(selector => {
            const $select = $(selector);
            if ($select.length > 0) {
                $select.val(null).trigger('change');
            }
        });
        
        // Obtener datos de la categoría
        $.ajax({
            url: `/Productos/ObtenerDatosCategoria/${categoriaId}`,
            type: 'GET',
            success: function(response) {
                console.log('Respuesta categoría:', response);
                
                if (response && response.success) {
                    let camposActualizados = [];
                    let promesasPendientes = [];
                    
                    // Función para precargar una opción en un select2
                    function precargarYSeleccionarOpcion(selector, valorId, nombreCampo, urlBusqueda) {
                        return new Promise((resolve) => {
                            if (!valorId) {
                                resolve(false);
                                return;
                            }
                            
                            const $select = $(selector);
                            if ($select.length === 0) {
                                console.log(`Selector ${selector} no encontrado`);
                                resolve(false);
                                return;
                            }
                            
                            console.log(`Precargando ${nombreCampo} con ID ${valorId}`);
                            
                            // Limpiar opción previa
                            $select.find(`option[value="${valorId}"]`).remove();
                            
                            // Crear opción temporal
                            const nuevaOpcion = new Option(`${nombreCampo} (ID: ${valorId})`, valorId, true, true);
                            $select.append(nuevaOpcion);
                            $select.val(valorId).trigger('change');
                            camposActualizados.push(nombreCampo);
                            
                            // Configurar la llamada AJAX según el tipo de campo
                            let ajaxUrl = urlBusqueda;
                            let ajaxData = {};
                            
                            // Para cuentas contables
                            if (selector.includes('cuenta') || selector.includes('Cuenta')) {
                                ajaxData = { term: valorId, exactId: true };
                            }
                            // Para impuestos  
                            else if (selector.includes('impuesto')) {
                                ajaxData = { term: valorId, exactId: true };
                            }
                            // Para rutas de impresión
                            else if (selector.includes('rutaImpresora')) {
                                ajaxData = { term: valorId };
                            }
                            
                            // Buscar detalles completos
                            $.ajax({
                                url: ajaxUrl,
                                type: 'GET',
                                data: ajaxData,
                                success: function(data) {
                                    console.log(`Respuesta para ${nombreCampo}:`, data);
                                    let nombreDescriptivo = null;
                                    
                                    // Manejar respuesta de cuentas contables (array directo)
                                    if (Array.isArray(data) && data.length > 0) {
                                        const item = data[0];
                                        if (item.codigo && item.nombre) {
                                            nombreDescriptivo = `${item.codigo} - ${item.nombre}`;
                                        }
                                    }
                                    // Manejar respuesta con results
                                    else if (data.results && Array.isArray(data.results) && data.results.length > 0) {
                                        const item = data.results[0];
                                        if (item.text) {
                                            nombreDescriptivo = item.text;
                                        } else if (item.nombre) {
                                            nombreDescriptivo = item.nombre;
                                        }
                                    }
                                    // Manejar objeto simple
                                    else if (data && typeof data === 'object' && !Array.isArray(data)) {
                                        if (data.text) {
                                            nombreDescriptivo = data.text;
                                        } else if (data.nombre) {
                                            nombreDescriptivo = data.nombre;
                                        }
                                    }
                                    
                                    if (nombreDescriptivo) {
                                        console.log(`Actualizando texto a: ${nombreDescriptivo}`);
                                        const $option = $select.find(`option[value="${valorId}"]`);
                                        if ($option.length) {
                                            $option.text(nombreDescriptivo);
                                            $select.trigger('change');
                                            
                                            // Actualizar el texto mostrado en el select2
                                            setTimeout(() => {
                                                const $rendered = $select.next('.select2-container').find('.select2-selection__rendered');
                                                if ($rendered.length) {
                                                    $rendered.text(nombreDescriptivo);
                                                    $rendered.attr('title', nombreDescriptivo);
                                                }
                                            }, 50);
                                        }
                                    }
                                    resolve(true);
                                },
                                error: function(xhr, status, error) {
                                    console.error(`Error al buscar detalles para ${nombreCampo}:`, error);
                                    resolve(false);
                                }
                            });
                        });
                    }
                    
                    // Actualizar impuestos en la pestaña de precios
                    const $firstImpuestoSelect = $('.select2-impuestos').first();
                    if ($firstImpuestoSelect.length > 0) {
                        // Array para guardar los IDs de impuestos a seleccionar
                        let impuestosParaSeleccionar = [];
                        
                        // Agregar impuesto regular si existe
                        if (response.impuestoId) {
                            console.log(`Heredando impuesto ID: ${response.impuestoId} a la pestaña de precios`);
                            impuestosParaSeleccionar.push({
                                id: response.impuestoId,
                                tipo: 'Regular'
                            });
                        } else {
                            console.log('No hay impuesto regular para heredar');
                        }
                        
                        // Agregar impuesto de propina si existe
                        if (response.propinaImpuestoId) {
                            console.log(`Heredando impuesto de propina ID: ${response.propinaImpuestoId} a la pestaña de precios`);
                            console.log(`Tipo de propinaImpuestoId: ${typeof response.propinaImpuestoId}`);
                            console.log(`Valor de propinaImpuestoId: ${response.propinaImpuestoId}`);
                            impuestosParaSeleccionar.push({
                                id: response.propinaImpuestoId,
                                tipo: 'Propina'
                            });
                        } else {
                            console.log('No hay impuesto de propina para heredar');
                            console.log('Valor de response.propinaImpuestoId:', response.propinaImpuestoId);
                        }
                        
                        console.log('Lista final de impuestos para seleccionar:', impuestosParaSeleccionar);
                        
                        // Si hay impuestos para seleccionar, procesar
                        if (impuestosParaSeleccionar.length > 0) {
                            promesasPendientes.push(
                                new Promise((resolve) => {
                                    // Limpiar selección actual
                                    $firstImpuestoSelect.val(null).trigger('change');
                                    
                                    // Cargar todos los impuestos disponibles y luego seleccionar los requeridos
                                    $.ajax({
                                        url: '/api/impuestos/Buscar',
                                        type: 'GET',
                                        data: { term: '' },
                                        success: function(response) {
                                            const impuestosDisponibles = response.results || response;
                                            console.log('Impuestos disponibles:', impuestosDisponibles);
                                            
                                            // Procesar cada impuesto para seleccionar
                                            impuestosParaSeleccionar.forEach(impuestoObj => {
                                                const impuestoEncontrado = impuestosDisponibles.find(imp => imp.id == impuestoObj.id);
                                                
                                                if (impuestoEncontrado) {
                                                    console.log(`✅ Impuesto ${impuestoObj.tipo} ${impuestoObj.id} encontrado:`, impuestoEncontrado);
                                                    
                                                    const textoImpuesto = `${impuestoEncontrado.nombre} (${impuestoEncontrado.porcentaje}%) - ${impuestoObj.tipo}`;
                                                    
                                                    // Crear la opción si no existe
                                                    if (!$firstImpuestoSelect.find(`option[value="${impuestoEncontrado.id}"]`).length) {
                                                        const newOption = new Option(textoImpuesto, impuestoEncontrado.id, false, false);
                                                        $firstImpuestoSelect.append(newOption);
                                                    }
                                                    
                                                    // Guardar el porcentaje del impuesto
                                                    window.impuestosDataGlobal = window.impuestosDataGlobal || {};
                                                    window.impuestosDataGlobal[impuestoEncontrado.id] = parseFloat(impuestoEncontrado.porcentaje);
                                                    console.log(`Guardando porcentaje del impuesto ${impuestoObj.tipo} ${impuestoEncontrado.id}: ${impuestoEncontrado.porcentaje}%`);
                                                } else {
                                                    console.warn(`❌ Impuesto ${impuestoObj.tipo} ${impuestoObj.id} NO ENCONTRADO en impuestos disponibles.`);
                                                    
                                                    // Si no se encuentra, crear una opción temporal para que no falle
                                                    const textoTemporal = `Impuesto ${impuestoObj.id} (${impuestoObj.tipo} - No disponible)`;
                                                    if (!$firstImpuestoSelect.find(`option[value="${impuestoObj.id}"]`).length) {
                                                        const newOption = new Option(textoTemporal, impuestoObj.id, false, false);
                                                        $firstImpuestoSelect.append(newOption);
                                                    }
                                                }
                                            });
                                            
                                            // Seleccionar todos los impuestos
                                            const selectedValues = impuestosParaSeleccionar.map(obj => obj.id.toString());
                                            $firstImpuestoSelect.val(selectedValues).trigger('change');
                                            
                                            if (response.impuestoId) camposActualizados.push('Impuesto');
                                            if (response.propinaImpuestoId) camposActualizados.push('Impuesto de Propina');
                                            
                                            // Disparar el cálculo del precio total
                                            setTimeout(() => {
                                                $firstImpuestoSelect.trigger('select2:select');
                                            }, 100);
                                            
                                            resolve(true);
                                        },
                                        error: function(xhr, status, error) {
                                            console.error('Error al cargar impuestos disponibles:', error);
                                            resolve(false);
                                        }
                                    });
                                })
                            );
                        }
                    }
                    
                    // Actualizar ruta de impresión
                    if (response.rutaImpresoraId) {
                        console.log(`Heredando ruta de impresión ID: ${response.rutaImpresoraId}`);
                        // Para rutas de impresión, simplemente seleccionar el valor ya que las opciones están precargadas
                        const $rutaSelect = $('#rutaImpresoraId');
                        if ($rutaSelect.length > 0) {
                            $rutaSelect.val(response.rutaImpresoraId).trigger('change');
                            camposActualizados.push('Ruta de Impresión');
                        }
                    }
                    
                    // Actualizar cuentas contables
                    if (response.cuentaVentasId) {
                        promesasPendientes.push(
                            precargarYSeleccionarOpcion('#cuentaVentasId', response.cuentaVentasId, 'Cuenta de Ventas',
                                `/Productos/BuscarCuentasContables?term=${response.cuentaVentasId}&exactId=true`)
                        );
                    }
                    
                    if (response.cuentaComprasInventariosId) {
                        promesasPendientes.push(
                            precargarYSeleccionarOpcion('#cuentaComprasInventariosId', response.cuentaComprasInventariosId, 'Cuenta de Compras/Inventarios',
                                `/Productos/BuscarCuentasContables?term=${response.cuentaComprasInventariosId}&exactId=true`)
                        );
                    }
                    
                    if (response.cuentaCostoVentasGastosId) {
                        promesasPendientes.push(
                            precargarYSeleccionarOpcion('#cuentaCostoVentasGastosId', response.cuentaCostoVentasGastosId, 'Cuenta de Costo de Ventas',
                                `/Productos/BuscarCuentasContables?term=${response.cuentaCostoVentasGastosId}&exactId=true`)
                        );
                    }
                    
                    if (response.cuentaDescuentosId) {
                        promesasPendientes.push(
                            precargarYSeleccionarOpcion('#cuentaDescuentosId', response.cuentaDescuentosId, 'Cuenta de Descuentos',
                                `/Productos/BuscarCuentasContables?term=${response.cuentaDescuentosId}&exactId=true`)
                        );
                    }
                    
                    if (response.cuentaDevolucionesId) {
                        promesasPendientes.push(
                            precargarYSeleccionarOpcion('#cuentaDevolucionesId', response.cuentaDevolucionesId, 'Cuenta de Devoluciones',
                                `/Productos/BuscarCuentasContables?term=${response.cuentaDevolucionesId}&exactId=true`)
                        );
                    }
                    
                    if (response.cuentaAjustesId) {
                        promesasPendientes.push(
                            precargarYSeleccionarOpcion('#cuentaAjustesId', response.cuentaAjustesId, 'Cuenta de Ajustes',
                                `/Productos/BuscarCuentasContables?term=${response.cuentaAjustesId}&exactId=true`)
                        );
                    }
                    
                    if (response.cuentaCostoMateriaPrimaId) {
                        promesasPendientes.push(
                            precargarYSeleccionarOpcion('#cuentaCostoMateriaPrimaId', response.cuentaCostoMateriaPrimaId, 'Cuenta de Costo de Materia Prima',
                                `/Productos/BuscarCuentasContables?term=${response.cuentaCostoMateriaPrimaId}&exactId=true`)
                        );
                    }
                    
                    // Esperar a que todas las promesas terminen
                    Promise.all(promesasPendientes).then(() => {
                        Swal.close();
                        
                        if (camposActualizados.length > 0) {
                            Swal.fire({
                                icon: 'success',
                                title: 'Datos heredados',
                                html: `<strong>Campos actualizados:</strong><br>${camposActualizados.join('<br>')}`,
                                timer: 3000,
                                timerProgressBar: true
                            });
                        }
                    });
                } else {
                    Swal.close();
                    Swal.fire({
                        icon: 'warning',
                        title: 'Advertencia',
                        text: 'No se pudieron obtener los datos de la categoría'
                    });
                }
            },
            error: function() {
                Swal.close();
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al obtener datos de la categoría'
                });
            }
        });
    });
}

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

// Función para inicializar el componente de carga de imágenes
function initImageUploader() {
    const imageUploaderCard = document.getElementById('imageUploaderCard');
    const imagenArchivoInput = document.getElementById('imagenArchivo');
    const imagenPreview = document.getElementById('imagenPreview');
    const uploadPlaceholder = document.getElementById('uploadPlaceholder');
    const removeImageBtn = document.getElementById('removeImageBtn');
    const imagenUrlInput = document.getElementById('imagenUrl');
    
    // Si hay una URL de imagen existente (modo edición), mostrarla
    const productoId = document.getElementById('productoId');
    if (productoId && productoId.value && imagenUrlInput.value) {
        mostrarImagenExistente(imagenUrlInput.value);
    }
    
    // Click en la card para abrir selector de archivos
    if (imageUploaderCard) {
        imageUploaderCard.addEventListener('click', function(e) {
            // No activar si se hace click en el botón de quitar
            if (e.target !== removeImageBtn && !removeImageBtn.contains(e.target)) {
                imagenArchivoInput.click();
            }
        });
        
        // Drag and drop
        imageUploaderCard.addEventListener('dragover', function(e) {
            e.preventDefault();
            e.stopPropagation();
            imageUploaderCard.classList.add('dragover');
        });
        
        imageUploaderCard.addEventListener('dragleave', function(e) {
            e.preventDefault();
            e.stopPropagation();
            imageUploaderCard.classList.remove('dragover');
        });
        
        imageUploaderCard.addEventListener('drop', function(e) {
            e.preventDefault();
            e.stopPropagation();
            imageUploaderCard.classList.remove('dragover');
            
            const files = e.dataTransfer.files;
            if (files.length > 0) {
                const file = files[0];
                if (file.type.startsWith('image/')) {
                    // Simular que el archivo fue seleccionado en el input
                    const dataTransfer = new DataTransfer();
                    dataTransfer.items.add(file);
                    imagenArchivoInput.files = dataTransfer.files;
                    handleFileSelect(file);
                } else {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Archivo no válido',
                        text: 'Por favor, selecciona un archivo de imagen válido.'
                    });
                }
            }
        });
    }
    
    // Selección de archivo
    if (imagenArchivoInput) {
        imagenArchivoInput.addEventListener('change', function(e) {
            if (e.target.files && e.target.files[0]) {
                handleFileSelect(e.target.files[0]);
            }
        });
    }
    
    // Botón quitar imagen
    if (removeImageBtn) {
        removeImageBtn.addEventListener('click', function(e) {
            e.stopPropagation();
            imagenArchivoInput.value = '';
            imagenPreview.src = '#';
            imagenPreview.style.display = 'none';
            uploadPlaceholder.style.display = 'flex';
            removeImageBtn.style.display = 'none';
            imagenUrlInput.value = '';
        });
    }
    
    // Función para manejar la selección de archivo
    function handleFileSelect(file) {
        // Validar tamaño (2MB máximo)
        const maxSize = 2 * 1024 * 1024; // 2MB
        if (file.size > maxSize) {
            Swal.fire({
                icon: 'warning',
                title: 'Archivo muy grande',
                text: 'El archivo debe ser menor a 2MB.'
            });
            imagenArchivoInput.value = '';
            return;
        }
        
        if (file.type.startsWith('image/')) {
            const reader = new FileReader();
            reader.onload = function(e) {
                imagenPreview.src = e.target.result;
                imagenPreview.style.display = 'block';
                uploadPlaceholder.style.display = 'none';
                removeImageBtn.style.display = 'inline-block';
                // Limpiar URL existente ya que ahora hay un archivo nuevo
                imagenUrlInput.value = '';
            }
            reader.readAsDataURL(file);
        } else {
            Swal.fire({
                icon: 'warning',
                title: 'Archivo no válido',
                text: 'Por favor, selecciona un archivo de imagen válido (JPG, PNG, GIF).'
            });
            imagenArchivoInput.value = '';
        }
    }
    
    // Función para mostrar imagen existente (modo edición)
    function mostrarImagenExistente(url) {
        if (url) {
            imagenPreview.src = url;
            imagenPreview.style.display = 'block';
            uploadPlaceholder.style.display = 'none';
            removeImageBtn.style.display = 'inline-block';
        }
    }
}

// Función para inicializar Pickr color picker
function initColorPicker() {
    const colorValueInput = document.getElementById('colorBotonTPV_value');
    const colorPickerTrigger = document.getElementById('colorPickerTrigger');

    if (colorPickerTrigger && colorValueInput) {
        const pickrInstance = Pickr.create({
            el: colorPickerTrigger, // El div que actuará como botón
            theme: 'classic',
            useAsButton: true, // MUY IMPORTANTE: hace que 'el' actúe como el botón que muestra el picker
            default: colorValueInput.value || '#d62828', // Color inicial

            swatches: [
                '#F44336', '#E91E63', '#9C27B0', '#673AB7', '#3F51B5', 
                '#2196F3', '#03A9F4', '#00BCD4', '#009688', '#4CAF50',
                '#8BC34A', '#CDDC39', '#FFEB3B', '#FFC107', '#FF9800',
                '#FF5722', '#795548', '#9E9E9E', '#607D8B', '#000000'
            ],

            components: {
                preview: true,
                opacity: true, // La imagen de referencia muestra un slider de opacidad
                hue: true,
                interaction: {
                    hex: true,  // Para mostrar el input HEX dentro del picker
                    rgba: false, // No mostrar input RGBA
                    hsla: false, // No mostrar input HSLA
                    hsva: false, // No mostrar input HSVA
                    cmyk: false, // No mostrar input CMYK
                    input: true, // Habilitar el campo de entrada de texto para HEX/RGBA dentro del picker
                    clear: false,
                    save: true  // Botón para confirmar selección (o "Aplicar")
                }
            },
            strings: { save: 'Aplicar', hex: 'HEX' } // O 'Guardar'
        });

        // Establecer el color de fondo inicial del botón trigger
        const initialColorObj = pickrInstance.getColor();
        if (initialColorObj) {
            colorPickerTrigger.style.backgroundColor = initialColorObj.toRGBA().toString(0); // Usar RGBA para opacidad
        }

        pickrInstance.on('save', (color, instance) => {
            if (color) {
                // Si opacity es true, toRGBA().toString(0) da "rgba(r,g,b,a)"
                // Si quieres solo HEX #RRGGBB, usa color.toHEXA().toString(0)
                // La imagen parece mostrar solo HEX en el input de arriba, así que usaremos HEX.
                colorValueInput.value = color.toHEXA().toString(0); // #RRGGBB
                colorPickerTrigger.style.backgroundColor = color.toRGBA().toString(); // El botón sí puede tener opacidad
            }
            pickrInstance.hide(); // Opcional: ocultar después de guardar si no lo hace por defecto
        });

        // Sincronizar si el usuario edita el input de texto directamente
        colorValueInput.addEventListener('change', function(event) {
            const success = pickrInstance.setColor(event.target.value, true); // true para modo silencioso
            if (success) {
                colorPickerTrigger.style.backgroundColor = pickrInstance.getColor().toRGBA().toString();
            } else {
                // Opcional: manejar color inválido, ej: revertir al color anterior de pickr
                // colorValueInput.value = pickrInstance.getColor().toHEXA().toString(0);
            }
        });
        
        // Manejar cambios en tiempo real mientras se ajusta el color
        pickrInstance.on('change', (color, instance) => {
            if (color) {
                colorValueInput.value = color.toHEXA().toString(0); // Actualizar input en tiempo real
                colorPickerTrigger.style.backgroundColor = color.toRGBA().toString();
            }
        });
        
        // También escuchar el evento input para actualizaciones más inmediatas en el input
        colorValueInput.addEventListener('input', function(event) {
            const newColor = event.target.value;
            if (/^#[0-9A-F]{6}$/i.test(newColor)) {
                const success = pickrInstance.setColor(newColor, true);
                if (success) {
                    colorPickerTrigger.style.backgroundColor = pickrInstance.getColor().toRGBA().toString();
                }
            }
        });
    }
}

// Sistema de precios dinámicos
// Variables globales para el sistema de precios
let priceRowsContainer;
let btnAddPriceRow;
let priceRowIndex = 0;
let impuestosDataGlobal = {};

function initPricingSystem() {
    priceRowsContainer = document.getElementById('priceRowsContainer');
    btnAddPriceRow = document.getElementById('btnAddPriceRow');
    priceRowIndex = 0;
    
    // Hacer el store de impuestos realmente global
    if (!window.impuestosDataGlobal) {
        window.impuestosDataGlobal = {};
    }
    impuestosDataGlobal = window.impuestosDataGlobal;

    console.log('Iniciando sistema de precios...');
    
    // Asegurar que la primera fila tenga los datos correctos
    function initializeFirstRow() {
        const firstRow = priceRowsContainer.querySelector('.price-row');
        if (firstRow) {
            console.log('[DEBUG] Inicializando primera fila de precios');
            initializePriceRow(firstRow, 0);
        }
    }

    function initializePriceRow(rowElement, index) {
        const nombreNivelInput = rowElement.querySelector('.nombre-nivel');
        const precioBaseInput = rowElement.querySelector('.precio-base');
        const impuestoSelect = rowElement.querySelector('.select2-impuestos');
        const precioTotalInput = rowElement.querySelector('.precio-total');

        console.log(`Inicializando fila ${index}...`);

        // Sincronizar el nombre del nivel con el atributo data
        if (nombreNivelInput) {
            nombreNivelInput.addEventListener('input', function() {
                rowElement.setAttribute('data-nombre-nivel', this.value);
            });
        }

        // Destruir instancia previa si existe (importante para elementos clonados)
        if ($(impuestoSelect).data('select2')) {
            $(impuestoSelect).select2('destroy');
        }

        console.log(`[DEBUG] Inicializando Select2 para fila ${index}, elemento:`, impuestoSelect);

        // Inicializar Select2 para impuestos
        $(impuestoSelect).select2({
            theme: "bootstrap-5",
            dropdownParent: document.body,
            placeholder: 'Seleccione impuestos',
            allowClear: true,
            width: '100%',
            ajax: {
                url: '/api/impuestos/Buscar',
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    console.log(`[DEBUG] Select2 fila ${index} - buscando impuestos:`, params.term);
                    return {
                        term: params.term || ''
                    };
                },
                processResults: function (response) {
                    const data = response.results || response;
                    console.log(`[DEBUG] Select2 fila ${index} - resultados recibidos:`, data?.length || 0);
                    if (data && data.length) {
                        // Almacenar los porcentajes en el store global
                        data.forEach(item => {
                            impuestosDataGlobal[item.id] = parseFloat(item.porcentaje);
                            window.impuestosDataGlobal[item.id] = parseFloat(item.porcentaje);
                        });
                        return {
                            results: data.map(item => ({
                                id: item.id,
                                text: item.text, // Ya viene formateado desde el servidor
                                percentage: parseFloat(item.porcentaje)
                            }))
                        };
                    }
                    return { results: [] };
                },
                cache: true
            }
        });

        // Verificar que Select2 se inicializó correctamente
        setTimeout(() => {
            const isInitialized = $(impuestoSelect).data('select2') !== undefined;
            console.log(`[DEBUG] Select2 fila ${index} inicializado:`, isInitialized);
            if (!isInitialized) {
                console.error(`[ERROR] Select2 fila ${index} NO se inicializó correctamente`);
            }
        }, 100);

        // Actualizar cálculos cuando cambian los impuestos seleccionados
        $(impuestoSelect).on('select2:select select2:unselect', function(e) {
            calcularPrecioTotal();
        });

        // Funciones de cálculo
        function calcularPrecioTotal() {
            const base = parseFloat(precioBaseInput.value) || 0;
            let totalImpuestosPorcentaje = 0;
            
            // Obtener los impuestos seleccionados
            const selectedValues = $(impuestoSelect).val() || [];
            console.log(`[DEBUG] Calculando precio total para fila ${index}, impuestos seleccionados:`, selectedValues);
            
            selectedValues.forEach(value => {
                const percentage = (window.impuestosDataGlobal && window.impuestosDataGlobal[value]) || 0;
                console.log(`[DEBUG] Impuesto ${value}: ${percentage}%`);
                totalImpuestosPorcentaje += percentage / 100;
            });
            
            console.log(`[DEBUG] Porcentaje total de impuestos: ${totalImpuestosPorcentaje * 100}%`);
            const total = base * (1 + totalImpuestosPorcentaje);
            console.log(`[DEBUG] Precio calculado: ${base} * (1 + ${totalImpuestosPorcentaje}) = ${total}`);
            precioTotalInput.value = total.toFixed(2);
        }

        function calcularPrecioBase() {
            const total = parseFloat(precioTotalInput.value) || 0;
            let totalImpuestosPorcentaje = 0;
            
            // Obtener los impuestos seleccionados
            const selectedValues = $(impuestoSelect).val() || [];
            console.log(`[DEBUG] Calculando precio base para fila ${index}, impuestos seleccionados:`, selectedValues);
            
            selectedValues.forEach(value => {
                const percentage = (window.impuestosDataGlobal && window.impuestosDataGlobal[value]) || 0;
                console.log(`[DEBUG] Impuesto ${value}: ${percentage}%`);
                totalImpuestosPorcentaje += percentage / 100;
            });
            
            if ((1 + totalImpuestosPorcentaje) === 0) {
                precioBaseInput.value = total.toFixed(2);
                return;
            }
            
            console.log(`[DEBUG] Porcentaje total de impuestos: ${totalImpuestosPorcentaje * 100}%`);
            const base = total / (1 + totalImpuestosPorcentaje);
            console.log(`[DEBUG] Precio base calculado: ${total} / (1 + ${totalImpuestosPorcentaje}) = ${base}`);
            precioBaseInput.value = base.toFixed(2);
        }

        // Event Listeners con prevención de bucles
        let isCalculatingBase = false;
        let isCalculatingTotal = false;

        precioBaseInput.addEventListener('input', function() {
            if (isCalculatingBase) return;
            isCalculatingTotal = true;
            calcularPrecioTotal();
            isCalculatingTotal = false;
            // Actualizar precio en pestaña de recetas si es la primera fila
            if (index === 0) {
                actualizarPrecioEnRecetas();
            }
        });

        precioTotalInput.addEventListener('input', function() {
            if (isCalculatingTotal) return;
            isCalculatingBase = true;
            calcularPrecioBase();
            isCalculatingBase = false;
            // Actualizar precio en pestaña de recetas si es la primera fila
            if (index === 0) {
                actualizarPrecioEnRecetas();
            }
        });

        // Actualizar cuando cambien los impuestos seleccionados
        $(impuestoSelect).on('change', function() {
            if (isCalculatingBase) return;
            isCalculatingTotal = true;
            calcularPrecioTotal();
            isCalculatingTotal = false;
            // Actualizar precio en pestaña de recetas si es la primera fila
            if (index === 0) {
                actualizarPrecioEnRecetas();
            }
        });

        // Botón eliminar
        const btnEliminar = rowElement.querySelector('.btn-eliminar-precio');
        if (btnEliminar) {
            btnEliminar.addEventListener('click', function() {
                rowElement.remove();
                updateDeleteButtons();
                updateInputNames();
            });
        }

        // Actualizar nombres de inputs para el binding
        updateInputNames();
    }

    function updateInputNames() {
        priceRowsContainer.querySelectorAll('.price-row').forEach((row, index) => {
            row.querySelectorAll('[data-original-name]').forEach(input => {
                const originalName = input.getAttribute('data-original-name');
                const newName = `Precios[${index}].${originalName}`;
                input.name = newName;
                
                // Para Select2, también actualizar el atributo name
                if (input.classList.contains('select2-impuestos')) {
                    console.log(`[DEBUG] Actualizando name del select de impuestos fila ${index}: ${newName}`);
                }
            });
        });
    }

    function addPriceRow() {
        // Crear nueva fila a partir de un template limpio
        const template = document.createElement('div');
        const nombreNivel = `Precio Nivel ${priceRowIndex + 1}`;
        console.log(`[DEBUG] Creando nueva fila con nombre: ${nombreNivel}, índice: ${priceRowIndex + 1}`);
        template.innerHTML = `
            <div class="row price-row gx-3 mb-3 align-items-center" data-index="${priceRowIndex + 1}" data-nombre-nivel="${nombreNivel}">
                <div class="col-md-2">
                    <label class="form-label">Nombre del nivel *</label>
                    <input type="text" class="form-control nombre-nivel" placeholder="Ej: Precio VIP" required 
                           name="Precios[${priceRowIndex + 1}].NombreNivel" data-original-name="NombreNivel" value="${nombreNivel}">
                    <span class="text-danger validation-message"></span>
                </div>
                <div class="col-md-2">
                    <label class="form-label">Precio base *</label>
                    <div class="input-group">
                        <span class="input-group-text">$</span>
                        <input type="number" class="form-control precio-base" step="0.01" placeholder="0.00" required 
                               name="Precios[${priceRowIndex + 1}].PrecioBase" data-original-name="PrecioBase" value="0.00">
                    </div>
                    <span class="text-danger validation-message"></span>
                </div>
                <div class="col-auto d-flex align-items-end pb-1">
                    <span class="h4 mx-1">+</span>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Impuestos</label>
                    <select class="form-select select2-impuestos" multiple="multiple" style="width: 100%;" 
                            name="Precios[${priceRowIndex + 1}].ImpuestoIds" data-original-name="ImpuestoIds">
                        <!-- Las opciones se cargarán por AJAX -->
                    </select>
                    <span class="text-danger validation-message"></span>
                </div>
                <div class="col-auto d-flex align-items-end pb-1">
                    <span class="h4 mx-1">=</span>
                </div>
                <div class="col-md-2">
                    <label class="form-label">Precio Total *</label>
                    <div class="input-group">
                        <span class="input-group-text">$</span>
                        <input type="number" class="form-control precio-total" step="0.01" placeholder="0.00" required
                               name="Precios[${priceRowIndex + 1}].PrecioTotal" data-original-name="PrecioTotal" value="0.00">
                    </div>
                    <span class="text-danger validation-message"></span>
                </div>
                <div class="col-auto d-flex align-items-center">
                    <button type="button" class="btn btn-sm btn-outline-danger btn-eliminar-precio" title="Eliminar este nivel de precio">
                        <i class="fas fa-trash-alt"></i>
                    </button>
                </div>
            </div>
        `;

        const newRow = template.firstElementChild;
        priceRowsContainer.appendChild(newRow);
        priceRowIndex++;
        initializePriceRow(newRow, priceRowIndex);
        updateDeleteButtons();
    }

    function updateDeleteButtons() {
        const rows = priceRowsContainer.querySelectorAll('.price-row');
        rows.forEach((row, index) => {
            const btnEliminar = row.querySelector('.btn-eliminar-precio');
            if (btnEliminar) {
                if (index === 0) { // Primera fila
                    btnEliminar.style.display = 'none';
                } else { // Filas subsecuentes
                    btnEliminar.style.display = rows.length > 1 ? 'inline-flex' : 'none'; // Mostrar si hay más de una fila
                }
            }
        });
    }

    // Inicializar TODAS las filas existentes (incluida la primera del HTML)
    const existingRows = priceRowsContainer.querySelectorAll('.price-row');
    existingRows.forEach((row, index) => {
        console.log(`[DEBUG] Inicializando fila existente ${index}`);
        initializePriceRow(row, index);
    });
    
    // Actualizar precio en recetas con el valor inicial
    if (existingRows.length > 0) {
        setTimeout(actualizarPrecioEnRecetas, 100);
    }

    // Botón añadir fila
    if (btnAddPriceRow) {
        btnAddPriceRow.addEventListener('click', addPriceRow);
    }

    // Inicializar el botón de eliminar de la primera fila
    updateDeleteButtons();

    // Función para recopilar datos al enviar el formulario
    window.collectPricingData = function() {
        const preciosData = [];
        priceRowsContainer.querySelectorAll('.price-row').forEach((row, index) => {
            const precioBase = row.querySelector('.precio-base').value;
            const precioTotal = row.querySelector('.precio-total').value;
            const impuestoIds = [];
            
            row.querySelectorAll('.impuesto-checkbox:checked').forEach(checkbox => {
                impuestoIds.push(parseInt(checkbox.value));
            });

            preciosData.push({
                PrecioBase: parseFloat(precioBase) || 0,
                PrecioTotal: parseFloat(precioTotal) || 0,
                ImpuestoIds: impuestoIds
            });
        });
        return preciosData;
    };

    // Hacer funciones accesibles globalmente
    window.updateDeleteButtons = updateDeleteButtons;
    window.addPriceRow = addPriceRow;
    window.initializePriceRow = initializePriceRow;
    
    // Inicializar la primera fila inmediatamente
    initializeFirstRow();
}

// Función para inicializar selectores de cuentas contables
function initCuentasContablesSelect2() {
    // Inicializar cada selector de cuentas
    $('.select2-cuenta-contable').each(function() {
        const $this = $(this);
        const cuentaInicial = $this.val();
        const cuentaTextoInicial = $this.find('option:selected').text();
        
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
                url: '/Productos/BuscarCuentasContables',
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
            templateResult: formatCuentaResult,
            templateSelection: formatCuentaSelection
        });
        
        // Si hay un valor inicial, cargarlo
        if (cuentaInicial && cuentaTextoInicial) {
            const newOption = new Option(cuentaTextoInicial, cuentaInicial, true, true);
            $this.append(newOption).trigger('change');
        }
    });
}

// Función para formatear resultados de búsqueda de cuentas
function formatCuentaResult(cuenta) {
    if (!cuenta.id) {
        return cuenta.text;
    }
    
    const $resultado = $(
        '<div class="select2-result">' +
            '<span class="badge bg-secondary me-2">' + (cuenta.codigo || '') + '</span>' +
            '<span>' + (cuenta.nombre || cuenta.text) + '</span>' +
        '</div>'
    );
    return $resultado;
}

// Función para formatear la selección de cuenta
function formatCuentaSelection(cuenta) {
    if (!cuenta.id) {
        return cuenta.text;
    }
    return cuenta.text;
}

// Función para actualizar el precio del producto en la pestaña de recetas
function actualizarPrecioEnRecetas() {
    // Obtener el precio total de la primera fila de precios
    const primeraFila = document.querySelector('#priceRowsContainer .price-row');
    if (!primeraFila) return;
    
    const precioTotalInput = primeraFila.querySelector('.precio-total');
    if (!precioTotalInput) return;
    
    const precioTotal = parseFloat(precioTotalInput.value) || 0;
    
    // Actualizar el elemento en la pestaña de recetas
    const precioVentaProductoElement = document.getElementById('precioVentaProducto');
    if (precioVentaProductoElement) {
        precioVentaProductoElement.textContent = '$' + precioTotal.toFixed(2);
    }
    
    // Actualizar el costo unitario desde el campo de costo
    const costoInput = document.getElementById('costo');
    if (costoInput) {
        const costoUnitario = parseFloat(costoInput.value) || 0;
        const costoUnitarioElement = document.getElementById('costoUnitarioProducto');
        if (costoUnitarioElement) {
            costoUnitarioElement.textContent = '$' + costoUnitario.toFixed(2);
        }
        
        // Calcular y actualizar el costo total (costo unitario * cantidad base)
        const cantidadBase = 1; // Por defecto es 1, pero podríamos tener un campo para esto
        const costoTotal = costoUnitario * cantidadBase;
        const costoTotalElement = document.getElementById('costoTotalProducto');
        if (costoTotalElement) {
            costoTotalElement.textContent = '$' + costoTotal.toFixed(2);
        }
    }
    
    // También actualizar los cálculos de costos
    if (typeof calcularCostos === 'function') {
        calcularCostos();
    }
}

// IMPORTANTE: Exponer las funciones globalmente para que funcionen los eventos onclick
window.editCategoria = editCategoria;

// Inicializar el manejo del formulario de producto
$(document).ready(function() {
    // Manejar el submit del formulario
    $('#formProducto').on('submit', function(e) {
        e.preventDefault();
        guardarProducto();
    });
});

// Función para guardar el producto completo
function guardarProducto() {
    // Validar el formulario
    if (!$('#formProducto')[0].checkValidity()) {
        $('#formProducto')[0].reportValidity();
        return;
    }
    
    // Obtener el ID del producto (si existe)
    const productoId = $('#productoId').val();
    const esNuevo = !productoId || productoId === '0';
    
    // Verificar si hay una imagen nueva para subir
    const imagenArchivo = $('#imagenArchivo')[0].files[0];
    
    if (imagenArchivo) {
        // Si hay imagen nueva, primero subirla
        subirImagenYGuardarProducto(imagenArchivo, productoId, esNuevo);
    } else {
        // Si no hay imagen nueva, guardar producto directamente
        guardarProductoSinImagen(productoId, esNuevo);
    }
}

// Función para subir imagen y luego guardar producto
function subirImagenYGuardarProducto(archivo, productoId, esNuevo) {
    const formDataImagen = new FormData();
    formDataImagen.append('imagen', archivo);
    formDataImagen.append('tipo', 'producto');
    
    // Mostrar loading
    Swal.fire({
        title: 'Subiendo imagen...',
        text: 'Por favor espere',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
    
    $.ajax({
        url: '/api/upload/imagen', // Necesitaremos crear este endpoint
        type: 'POST',
        data: formDataImagen,
        processData: false,
        contentType: false,
        success: function(response) {
            if (response.success && response.url) {
                // Imagen subida exitosamente, guardar producto con la URL
                $('#imagenUrl').val(response.url);
                guardarProductoSinImagen(productoId, esNuevo);
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al subir la imagen: ' + (response.mensaje || 'Error desconocido')
                });
            }
        },
        error: function(xhr) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Error al subir la imagen'
            });
        }
    });
}

// Función para guardar producto sin manejar imagen nueva
function guardarProductoSinImagen(productoId, esNuevo) {
    // Recolectar datos del formulario
    const formData = new FormData($('#formProducto')[0]);
    
    // Convertir FormData a objeto
    const producto = {};
    
    // Primero, obtener los valores de precio de la primera fila
    const primeraFilaPrecio = document.querySelector('#priceRowsContainer .price-row');
    if (primeraFilaPrecio) {
        const precioBase = primeraFilaPrecio.querySelector('.precio-base')?.value || '0';
        const precioTotal = primeraFilaPrecio.querySelector('.precio-total')?.value || '0';
        producto.PrecioVenta = parseFloat(precioBase) || 0;
    }
    
    formData.forEach((value, key) => {
        // Ignorar campos de array de precios
        if (key.startsWith('Precios[')) {
            return;
        }
        
        // Convertir strings vacíos a null para campos numéricos opcionales
        if (value === '' && (key === 'TiempoPreparacion' || key === 'ImpuestoId' || 
            key === 'RutaImpresoraId' || key === 'ItemId' || key === 'ItemContenedorId' ||
            key === 'MarcaId' || key === 'UnidadMedidaInventarioId' || key === 'NivelMinimo' ||
            key === 'StockActual' || key === 'Rendimiento' || key === 'CuentaVentasId' ||
            key === 'CuentaComprasInventariosId' || key === 'CuentaCostoVentasGastosId' ||
            key === 'CuentaDescuentosId' || key === 'CuentaDevolucionesId' || key === 'CuentaAjustesId' ||
            key === 'CuentaCostoMateriaPrimaId')) {
            producto[key] = null;
        } else if (key === 'TiempoPreparacion' && value !== '') {
            // Convertir a número si tiene valor
            producto[key] = parseInt(value) || null;
        } else if ((key === 'NivelMinimo' || key === 'StockActual' || key === 'Rendimiento' || 
                   key === 'Costo' || key === 'OrdenClasificacion' || key === 'PrecioVenta') && value !== '') {
            // Convertir a número para campos numéricos
            producto[key] = parseFloat(value) || 0;
        } else if ((key === 'MarcaId' || key === 'ImpuestoId' || key === 'RutaImpresoraId' ||
                   key === 'ItemId' || key === 'ItemContenedorId' || key === 'UnidadMedidaInventarioId' ||
                   key === 'CuentaVentasId' || key === 'CuentaComprasInventariosId' ||
                   key === 'CuentaCostoVentasGastosId' || key === 'CuentaDescuentosId' ||
                   key === 'CuentaDevolucionesId' || key === 'CuentaAjustesId' ||
                   key === 'CuentaCostoMateriaPrimaId') && value !== '') {
            // Convertir a número para IDs
            producto[key] = parseInt(value) || null;
        } else {
            producto[key] = value;
        }
    });
    
    // Convertir valores booleanos
    producto.EsActivo = $('#esActivo').is(':checked');
    producto.PermiteModificadores = $('#permiteModificadores').is(':checked');
    producto.RequierePuntoCoccion = $('#requierePuntoCoccion').is(':checked');
    producto.DisponibleParaVenta = $('#disponibleParaVenta').is(':checked');
    producto.RequierePreparacion = $('#requierePreparacion').is(':checked');
    
    // Asegurar que se envíe EmpresaId (por ahora hardcoded a 4)
    producto.EmpresaId = 4;
    
    // Asegurar valores por defecto
    producto.Costo = parseFloat($('#costo').val()) || 0;
    producto.OrdenClasificacion = parseInt($('#ordenClasificacion').val()) || 0;
    
    // Asegurar que se incluya la URL de la imagen
    const imagenUrl = $('#imagenUrl').val();
    if (imagenUrl) {
        producto.ImagenUrl = imagenUrl;
    }
    
    // Extraer impuestos de la primera fila solo para compatibilidad
    const primeraFila = document.querySelector('#priceRowsContainer .price-row:first-child');
    if (primeraFila) {
        const $selectImpuestos = $(primeraFila).find('.select2-impuestos');
        const impuestosSeleccionados = $selectImpuestos.val();
        
        if (impuestosSeleccionados && impuestosSeleccionados.length > 0) {
            // Solo para compatibilidad con sistema viejo
            producto.ImpuestoIds = impuestosSeleccionados.map(id => parseInt(id));
            producto.ImpuestoId = parseInt(impuestosSeleccionados[0]) || null;
        } else {
            producto.ImpuestoIds = [];
            producto.ImpuestoId = null;
        }
    } else {
        producto.ImpuestoIds = [];
        producto.ImpuestoId = null;
    }
    
    // Extraer múltiples precios (nuevo sistema)
    const preciosRecopilados = recopilarDatosPrecios();
    console.log('Precios recopilados:', preciosRecopilados);
    
    // Log detallado de cada precio
    preciosRecopilados.forEach((precio, index) => {
        console.log(`[DEBUG] Precio ${index + 1}:`, {
            NombreNivel: precio.NombreNivel,
            PrecioBase: precio.PrecioBase,
            PrecioTotal: precio.PrecioTotal,
            ImpuestoIds: precio.ImpuestoIds,
            EsPrincipal: precio.EsPrincipal
        });
    });
    
    producto.Precios = preciosRecopilados;
    
    // Mostrar loading
    Swal.fire({
        title: 'Guardando...',
        text: 'Por favor espere',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
    
    // Determinar URL y método
    const url = esNuevo ? '/api/productos' : `/api/productos/${productoId}`;
    const method = esNuevo ? 'POST' : 'PUT';
    
    // Debug: Ver qué se está enviando
    console.log('Datos a enviar:', producto);
    console.log('URL:', url);
    console.log('Método:', method);
    
    // Log específico del JSON que se envía
    const jsonToSend = JSON.stringify(producto);
    console.log('JSON a enviar:', jsonToSend);
    console.log('Tamaño del JSON:', jsonToSend.length, 'caracteres');
    
    // Guardar producto principal
    $.ajax({
        url: url,
        type: method,
        contentType: 'application/json',
        data: jsonToSend,
        success: function(response) {
            console.log('Respuesta del servidor:', response);
            
            // Para POST (crear) la respuesta contiene el objeto completo
            // Para PUT (actualizar) la respuesta puede ser vacía o contener el objeto
            let nuevoProductoId;
            
            if (esNuevo) {
                // Al crear, la respuesta debe contener el objeto con id
                nuevoProductoId = response?.id || response?.value?.id;
            } else {
                // Al actualizar, usar el ID existente
                nuevoProductoId = productoId;
            }
            
            console.log('ID del producto:', nuevoProductoId);
            
            // Solo guardar receta si la pestaña fue abierta (y hay cambios en la receta)
            if (recetaTabOpened && $('#recetasTableBody tr').length > 0 && !$('#recetasTableBody').find('td[colspan="7"]').length) {
                guardarReceta(nuevoProductoId);
            } else {
                // No hay cambios en la receta o la pestaña no fue abierta, mostrar éxito
                Swal.fire({
                    icon: 'success',
                    title: 'Guardado exitoso',
                    text: esNuevo ? 'El producto ha sido creado correctamente' : 'El producto ha sido actualizado correctamente',
                    showConfirmButton: true,
                    confirmButtonText: 'Aceptar'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = '/Productos';
                    }
                });
            }
        },
        error: function(xhr) {
            console.error('Error completo:', xhr);
            console.error('Respuesta del servidor:', xhr.responseJSON);
            console.error('Texto de respuesta:', xhr.responseText);
            
            let mensajeError = 'Error al guardar el producto';
            
            if (xhr.responseJSON) {
                if (xhr.responseJSON.mensaje) {
                    mensajeError = xhr.responseJSON.mensaje;
                } else if (xhr.responseJSON.errors) {
                    // Manejar errores de validación de ASP.NET
                    const errores = [];
                    for (let campo in xhr.responseJSON.errors) {
                        errores.push(`${campo}: ${xhr.responseJSON.errors[campo].join(', ')}`);
                    }
                    mensajeError = errores.join('\n');
                }
            }
            
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: mensajeError
            });
        }
    });
}

// Función para guardar la receta
function guardarReceta(productoId) {
    // Recolectar ingredientes de la tabla
    const ingredientes = [];
    
    $('#recetasTableBody tr').each(function() {
        const $row = $(this);
        
        // Saltar filas vacías o de mensaje
        if ($row.find('td[colspan="7"]').length > 0) {
            return;
        }
        
        const itemId = $row.attr('data-item-id');
        const itemContenedorId = $row.find('.contenedor-select').val();
        const cantidad = parseFloat($row.find('.cantidad-input').val()) || 0;
        const costoUnitario = parseFloat($row.find('.costo-input').val()) || 0;
        
        if (itemId && itemContenedorId && cantidad > 0) {
            ingredientes.push({
                itemId: parseInt(itemId),
                itemContenedorId: parseInt(itemContenedorId),
                cantidad: cantidad,
                costoUnitario: costoUnitario
            });
        }
    });
    
    // Obtener notas y margen
    const notasReceta = $('#notasReceta').val();
    const margenGanancia = parseFloat($('#margenSlider').val()) || 0;
    
    // Crear objeto de receta
    const receta = {
        productoId: productoId,
        notasReceta: notasReceta,
        margenGanancia: margenGanancia,
        ingredientes: ingredientes
    };
    
    // Guardar receta
    $.ajax({
        url: `/api/productos/${productoId}/receta`,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(receta),
        success: function(response) {
            Swal.fire({
                icon: 'success',
                title: 'Guardado exitoso',
                text: 'El producto y su receta han sido guardados correctamente',
                showConfirmButton: true,
                confirmButtonText: 'Aceptar'
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = '/Productos';
                }
            });
        },
        error: function(xhr) {
            // Si falla la receta pero el producto se guardó, mostrar advertencia
            Swal.fire({
                icon: 'warning',
                title: 'Producto guardado con advertencias',
                text: 'El producto se guardó pero hubo un error al guardar la receta: ' + (xhr.responseJSON?.mensaje || 'Error desconocido'),
                showConfirmButton: true,
                confirmButtonText: 'Aceptar'
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = '/Productos';
                }
            });
        }
    });
}
window.abrirOffcanvasCategoria = abrirOffcanvasCategoria;
window.actualizarPrecioEnRecetas = actualizarPrecioEnRecetas;

// Función para cargar receta existente
function cargarRecetaExistente(productoId) {
    console.log('Cargando receta para producto:', productoId);
    $.ajax({
        url: `/api/productos/${productoId}/receta`,
        type: 'GET',
        success: function(response) {
            console.log('Respuesta del servidor:', response);
            console.log('¿Tiene ingredientes?', response.Ingredientes);
            console.log('Longitud de ingredientes:', response.Ingredientes ? response.Ingredientes.length : 'undefined');
            if (response && response.Ingredientes && response.Ingredientes.length > 0) {
                // Limpiar tabla
                $('#recetasTableBody').empty();
                
                // Cargar notas y margen
                if (response.NotasReceta) {
                    $('#notasReceta').val(response.NotasReceta);
                }
                
                if (response.MargenGanancia) {
                    $('#margenSlider').val(response.MargenGanancia).trigger('input');
                }
                
                // Cargar cada ingrediente
                response.Ingredientes.forEach(function(ingrediente) {
                    const newRow = document.createElement('tr');
                    newRow.setAttribute('data-item-id', ingrediente.ItemId);
                    
                    newRow.innerHTML = `
                        <td data-id="${ingrediente.ItemId}">${ingrediente.NombreItem || ''}</td>
                        <td>${ingrediente.MarcaNombre || '-'}</td>
                        <td>
                            <input type="number" class="form-control form-control-sm cantidad-input" 
                                   value="${ingrediente.Cantidad}" min="0" step="0.01">
                        </td>
                        <td>
                            <select class="form-select form-select-sm contenedor-select unidad-select" 
                                    onchange="actualizarCostoContenedor(this)" data-selected="${ingrediente.ItemContenedorId}">
                                <option value="">Cargando...</option>
                            </select>
                        </td>
                        <td>
                            <input type="number" class="form-control form-control-sm costo-input" 
                                   value="${ingrediente.CostoUnitario.toFixed(2)}" min="0" step="0.01" readonly>
                        </td>
                        <td class="subtotal-cell">$${(ingrediente.Cantidad * ingrediente.CostoUnitario).toFixed(2)}</td>
                        <td>
                            <button type="button" class="btn btn-remove" title="Eliminar">
                                <i class="fas fa-trash"></i>
                            </button>
                        </td>
                    `;
                    
                    $('#recetasTableBody').append(newRow);
                    
                    // Cargar contenedores para este item
                    cargarContenedoresParaItem(ingrediente.ItemId, newRow, ingrediente.ItemContenedorId);
                });
                
                // Actualizar totales
                calcularTotales();
            } else {
                console.log('No hay ingredientes en la receta o respuesta vacía');
            }
        },
        error: function(xhr) {
            console.error('Error al cargar receta:', xhr);
        }
    });
}

// Función para cargar contenedores de un item específico
function cargarContenedoresParaItem(itemId, row, selectedContenedorId) {
    $.ajax({
        url: `/api/item/${itemId}/Contenedores`,
        type: 'GET',
        success: function(contenedores) {
            if (contenedores && contenedores.length > 0) {
                const $select = $(row).find('.contenedor-select');
                $select.empty();
                $select.append('<option value="">Seleccione unidad...</option>');
                
                contenedores.forEach(function(contenedor) {
                    const option = new Option(
                        contenedor.unidadMedidaNombre || contenedor.nombre,
                        contenedor.id,
                        false,
                        contenedor.id == selectedContenedorId
                    );
                    $(option).attr('data-costo', contenedor.costo || 0);
                    $select.append(option);
                });
                
                // Si hay un valor seleccionado, actualizar el costo
                if (selectedContenedorId) {
                    $select.val(selectedContenedorId);
                }
            }
        },
        error: function(xhr) {
            console.error('Error al cargar contenedores:', xhr);
            const $select = $(row).find('.contenedor-select');
            $select.html('<option value="">Error al cargar</option>');
        }
    });
}

// Función para cargar datos del producto existente (modo edición)
function cargarDatosProductoExistente(productoId) {
    console.log('=== INICIO cargarDatosProductoExistente ===');
    console.log('ProductoId recibido:', productoId);
    console.log('URL que se llamará:', `/api/productos/${productoId}`);
    
    // Verificar que los campos existan en el DOM
    console.log('Verificando campos del formulario:');
    console.log('- Campo nombre existe:', $('#nombre').length > 0);
    console.log('- Campo nombreCortoTPV existe:', $('#nombreCortoTPV').length > 0);
    console.log('- Campo plu existe:', $('#plu').length > 0);
    console.log('- Campo categoriaId existe:', $('#categoriaId').length > 0);
    
    $.ajax({
        url: `/api/productos/${productoId}`,
        type: 'GET',
        dataType: 'json',
        xhrFields: {
            withCredentials: true
        },
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        success: function(response, textStatus, xhr) {
            console.log('=== DEPURACIÓN CARGA DE PRODUCTO ===');
            console.log('Estado HTTP:', xhr.status);
            console.log('Texto de estado:', textStatus);
            console.log('Respuesta completa del API:', response);
            console.log('Tipo de respuesta:', typeof response);
            console.log('Propiedades de la respuesta:', Object.keys(response || {}));
            
            // Log adicional para verificar estructura
            if (response) {
                console.log('¿Tiene response.value?', response.hasOwnProperty('value'));
                console.log('¿Tiene response.Value?', response.hasOwnProperty('Value'));
                console.log('¿Tiene response.id?', response.hasOwnProperty('id'));
                console.log('¿Tiene response.Id?', response.hasOwnProperty('Id'));
                console.log('JSON.stringify de response:', JSON.stringify(response, null, 2));
            }
            
            // Verificar si la respuesta tiene una estructura válida
            let productData = response;
            
            // Intentar diferentes estructuras de respuesta
            if (response && response.value) {
                // Si viene envuelto en ActionResult
                productData = response.value;
                console.log('Usando response.value:', productData);
            } else if (response && response.Value) {
                // Si viene con Value en mayúscula
                productData = response.Value;
                console.log('Usando response.Value:', productData);
            } else if (response && response.result) {
                // Si viene con result
                productData = response.result;
                console.log('Usando response.result:', productData);
            }
            
            // Validación más flexible - verificar si tiene propiedades de producto
            const tieneIdValido = productData && (productData.Id || productData.id);
            const tieneNombreValido = productData && (productData.Nombre || productData.nombre);
            
            console.log('¿Tiene ID válido?', tieneIdValido);
            console.log('¿Tiene Nombre válido?', tieneNombreValido);
            
            if (tieneIdValido || tieneNombreValido) {
                console.log('Cargando datos del producto:', productData);
                console.log('ID del producto:', productData.Id || productData.id);
                console.log('Nombre del producto:', productData.Nombre || productData.nombre);
                
                // Llenar campos básicos (usando PascalCase del API)
                $('#nombre').val(productData.Nombre || '');
                $('#nombreCortoTPV').val(productData.NombreCortoTPV || '');
                $('#descripcionEditor').val(productData.Descripcion || '');
                $('#plu').val(productData.PLU || '');
                $('#colorBotonTPV_value').val(productData.ColorBotonTPV || '#d62828');
                $('#costo').val(productData.Costo || 0);
                $('#ordenClasificacion').val(productData.OrdenClasificacion || 0);
                
                // Actualizar el color picker si existe
                if (window.pickrInstance) {
                    window.pickrInstance.setColor(productData.ColorBotonTPV || '#d62828');
                }
                
                // Llenar checkboxes (usando PascalCase del API)
                $('#esActivo').prop('checked', productData.EsActivo);
                $('#permiteModificadores').prop('checked', productData.PermiteModificadores);
                $('#requierePuntoCoccion').prop('checked', productData.RequierePuntoCoccion);
                $('#disponibleParaVenta').prop('checked', productData.DisponibleParaVenta);
                $('#requierePreparacion').prop('checked', productData.RequierePreparacion);
                
                // Tiempo de preparación
                if (productData.TiempoPreparacion) {
                    $('#tiempoPreparacion').val(productData.TiempoPreparacion);
                    $('#divTiempoPreparacion').show();
                }
                
                // Cargar categoría (sin activar herencia)
                if (productData.Categoria) {
                    console.log('Cargando categoría:', productData.Categoria);
                    const categoriaOption = new Option(productData.Categoria.Nombre, productData.Categoria.Id, true, true);
                    $('#categoriaId').append(categoriaOption);
                    // No usar trigger('change') para evitar activar la herencia durante la carga inicial
                    $('#categoriaId').val(productData.Categoria.Id);
                }
                
                // Cargar ruta de impresora
                if (productData.RutaImpresoraId) {
                    $('#rutaImpresoraId').val(productData.RutaImpresoraId).trigger('change');
                }
                
                // Cargar múltiples precios (nuevo sistema)
                setTimeout(function() {
                    cargarPreciosProducto(productoId);
                }, 600);
                
                // Cargar impuestos en el select2 de la primera fila
                const $firstImpuestoSelect = $('.select2-impuestos').first();
                if ($firstImpuestoSelect.length > 0) {
                    let impuestosParaCargar = [];
                    
                    // Primero, verificar si hay múltiples impuestos (nueva estructura)
                    if (productData.Impuestos && productData.Impuestos.length > 0) {
                        console.log('Cargando múltiples impuestos:', productData.Impuestos);
                        impuestosParaCargar = productData.Impuestos.map(imp => ({
                            id: imp.Id || imp.id,
                            nombre: imp.Nombre || imp.nombre,
                            porcentaje: imp.Porcentaje || imp.porcentaje
                        }));
                    }
                    // Si no hay múltiples impuestos, verificar el campo único (compatibilidad)
                    else if (productData.ImpuestoId) {
                        console.log('Cargando impuesto único (compatibilidad):', productData.ImpuestoId);
                        if (productData.Impuesto) {
                            impuestosParaCargar = [{
                                id: productData.ImpuestoId,
                                nombre: productData.Impuesto.Nombre || productData.Impuesto.nombre,
                                porcentaje: productData.Impuesto.Porcentaje || productData.Impuesto.porcentaje
                            }];
                        } else {
                            // Si no viene el objeto Impuesto, solo tenemos el ID
                            impuestosParaCargar = [{ id: productData.ImpuestoId }];
                        }
                    }
                    
                    if (impuestosParaCargar.length > 0) {
                        // Array para guardar las promesas de carga
                        let promesasImpuestos = impuestosParaCargar.map(impuesto => {
                            return new Promise((resolve) => {
                                // Si ya tenemos el nombre y porcentaje, no necesitamos buscar
                                if (impuesto.nombre && impuesto.porcentaje !== undefined) {
                                    const text = `${impuesto.nombre} (${impuesto.porcentaje}%)`;
                                    if (!$firstImpuestoSelect.find(`option[value="${impuesto.id}"]`).length) {
                                        const newOption = new Option(text, impuesto.id, false, false);
                                        $firstImpuestoSelect.append(newOption);
                                    }
                                    // Guardar el porcentaje en el store global
                                    window.impuestosDataGlobal = window.impuestosDataGlobal || {};
                                    window.impuestosDataGlobal[impuesto.id] = impuesto.porcentaje;
                                    resolve();
                                } else {
                                    // Si solo tenemos el ID, buscar los detalles
                                    $.ajax({
                                        url: `/api/impuestos/Buscar?term=${impuesto.id}`,
                                        type: 'GET',
                                        success: function(data) {
                                            console.log(`Respuesta de búsqueda de impuesto ${impuesto.id}:`, data);
                                            if (data.results && data.results.length > 0) {
                                                const impuestoData = data.results[0];
                                                // Crear la opción si no existe
                                                if (!$firstImpuestoSelect.find(`option[value="${impuestoData.id}"]`).length) {
                                                    const newOption = new Option(impuestoData.text, impuestoData.id, false, false);
                                                    $firstImpuestoSelect.append(newOption);
                                                }
                                                // Guardar el porcentaje en el store global
                                                if (impuestoData.porcentaje !== undefined) {
                                                    window.impuestosDataGlobal = window.impuestosDataGlobal || {};
                                                    window.impuestosDataGlobal[impuestoData.id] = parseFloat(impuestoData.porcentaje);
                                                }
                                            }
                                            resolve();
                                        },
                                        error: function(xhr) {
                                            console.error(`Error al cargar impuesto ${impuesto.id}:`, xhr);
                                            resolve();
                                        }
                                    });
                                }
                            });
                        });
                        
                        // Esperar a que todos los impuestos se carguen y luego seleccionarlos
                        Promise.all(promesasImpuestos).then(() => {
                            const idsParaSeleccionar = impuestosParaCargar.map(imp => imp.id.toString());
                            $firstImpuestoSelect.val(idsParaSeleccionar).trigger('change');
                            console.log('Impuestos cargados y seleccionados:', idsParaSeleccionar);
                            
                            // Disparar el cálculo del precio total
                            setTimeout(() => {
                                $firstImpuestoSelect.trigger('select2:select');
                            }, 100);
                        });
                    }
                }
                
                // DEBUG: Ver qué propiedades tiene productData
                console.log('ProductData recibido:', productData);
                console.log('CuentaVentasId:', productData.CuentaVentasId);
                
                // Deshabilitar herencia temporalmente
                $('#categoriaId').off('change.inheritance');
                
                // Asegurar que Select2 esté inicializado antes de cargar cuentas
                setTimeout(function() {
                    console.log('Iniciando carga de cuentas contables del producto');
                    console.log('CuentaVentasId:', productData.CuentaVentasId);
                    console.log('CuentaComprasInventariosId:', productData.CuentaComprasInventariosId);
                    
                    // Verificar que los Select2 estén inicializados
                    $('.select2-cuenta-contable').each(function() {
                        const isInitialized = $(this).hasClass('select2-hidden-accessible');
                        console.log(`Select ${this.id} inicializado:`, isInitialized);
                    });
                    
                    // Cargar cuentas contables específicas del producto
                    cargarCuentaContable('#cuentaVentasId', productData.CuentaVentasId);
                    cargarCuentaContable('#cuentaComprasInventariosId', productData.CuentaComprasInventariosId);
                    cargarCuentaContable('#cuentaCostoVentasGastosId', productData.CuentaCostoVentasGastosId);
                    cargarCuentaContable('#cuentaDescuentosId', productData.CuentaDescuentosId);
                    cargarCuentaContable('#cuentaDevolucionesId', productData.CuentaDevolucionesId);
                    cargarCuentaContable('#cuentaAjustesId', productData.CuentaAjustesId);
                    cargarCuentaContable('#cuentaCostoMateriaPrimaId', productData.CuentaCostoMateriaPrimaId);
                    
                    // Reactivar herencia después de 1 segundo
                    setTimeout(function() {
                        initCategoriaInheritance();
                    }, 1000);
                }, 1000); // Aumentar el tiempo de espera a 1 segundo
                
                // Cargar imagen si existe
                if (productData.ImagenUrl) {
                    $('#imagenUrl').val(productData.ImagenUrl);
                    $('#imagenPreview').attr('src', productData.ImagenUrl).show();
                    $('#uploadPlaceholder').hide();
                    $('#removeImageBtn').show();
                }
                
                // Actualizar TinyMCE si existe
                if (typeof tinymce !== 'undefined' && tinymce.get('descripcionEditor')) {
                    try {
                        tinymce.get('descripcionEditor').setContent(productData.Descripcion || '');
                    } catch (e) {
                        console.log('TinyMCE no está listo aún, estableciendo valor directo en textarea');
                        $('#descripcionEditor').val(productData.Descripcion || '');
                    }
                } else {
                    // Si TinyMCE no está disponible, establecer el valor directamente
                    $('#descripcionEditor').val(productData.Descripcion || '');
                }
                
                // Después de cargar todos los datos, permitir la herencia nuevamente
                setTimeout(() => {
                    allowInheritancePopup = true;
                    console.log('Herencia de categoría activada después de cargar datos del producto');
                }, 1000);
            } else {
                console.error('Datos del producto no válidos:');
                console.error('Response completo:', response);
                console.error('ProductData:', productData);
                console.error('Estructura de productData:', JSON.stringify(productData, null, 2));
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'No se pudieron cargar los datos del producto. Respuesta inválida del servidor.'
                });
            }
        },
        error: function(xhr, textStatus, errorThrown) {
            console.error('Error al cargar datos del producto:');
            console.error('Estado HTTP:', xhr.status);
            console.error('Texto de estado:', textStatus);
            console.error('Error:', errorThrown);
            console.error('Respuesta del servidor:', xhr.responseText);
            
            let mensaje = 'No se pudieron cargar los datos del producto';
            
            if (xhr.status === 404) {
                mensaje = 'Producto no encontrado';
            } else if (xhr.status === 400) {
                try {
                    const errorResponse = JSON.parse(xhr.responseText);
                    mensaje = errorResponse.mensaje || mensaje;
                } catch (e) {
                    // No se pudo parsear la respuesta
                }
            }
            
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: mensaje
            });
        }
    });
}

// Función auxiliar para cargar una cuenta contable específica
function cargarCuentaContable(selector, cuentaId) {
    console.log(`Intentando cargar cuenta ${selector} con ID:`, cuentaId);
    if (!cuentaId) {
        console.log(`No hay ID para ${selector}, saltando`);
        return;
    }
    
    $.ajax({
        url: `/Productos/BuscarCuentasContables?term=${cuentaId}&exactId=true`,
        type: 'GET',
        success: function(data) {
            console.log(`Respuesta para ${selector}:`, data);
            if (data && data.length > 0) {
                const cuenta = data[0];
                const $select = $(selector);
                
                console.log(`Select ${selector} encontrado:`, $select.length > 0);
                console.log(`Tiene Select2:`, $select.hasClass('select2-hidden-accessible'));
                
                // Limpiar opciones existentes excepto placeholder
                $select.find('option[value!=""]').remove();
                
                // Construir el texto a mostrar
                const textoOpcion = cuenta.text || (cuenta.codigo && cuenta.nombre ? `${cuenta.codigo} - ${cuenta.nombre}` : 'Cuenta ' + cuenta.id);
                
                // Agregar nueva opción
                const option = new Option(textoOpcion, cuenta.id, true, true);
                $select.append(option);
                
                // Forzar actualización de Select2
                if ($select.hasClass('select2-hidden-accessible')) {
                    $select.val(cuenta.id).trigger('change');
                } else {
                    console.log(`Select ${selector} NO tiene Select2 inicializado`);
                    $select.val(cuenta.id);
                }
                
                console.log(`Cuenta ${selector} cargada exitosamente, valor: ${$select.val()}`);
            } else {
                console.log(`No se encontraron resultados para ${selector}`);
            }
        },
        error: function(xhr) {
            console.error(`Error al cargar cuenta contable ${selector}:`, xhr);
        }
    });
}

// ========== FUNCIONES PARA MÚLTIPLES PRECIOS ==========

/**
 * Carga los precios existentes de un producto en edición
 */
function cargarPreciosProducto(productoId) {
    console.log('Cargando precios del producto:', productoId);
    
    if (!productoId || productoId === '0') {
        console.log('No hay producto ID válido, saltando carga de precios');
        return;
    }
    
    $.ajax({
        url: `/api/productos/${productoId}/precios`,
        type: 'GET',
        success: function(precios) {
            console.log('[DEBUG] Precios cargados desde API:', precios);
            console.log('[DEBUG] Tipo de precios:', typeof precios);
            console.log('[DEBUG] Es array?:', Array.isArray(precios));
            if (precios && precios.length > 0) {
                console.log('[DEBUG] Primer precio estructura:', JSON.stringify(precios[0], null, 2));
            }
            
            if (precios && precios.length > 0) {
                console.log(`[DEBUG] Se encontraron ${precios.length} precios para cargar`);
                
                // Limpiar filas existentes excepto la primera
                const $container = $('#priceRowsContainer');
                const $firstRow = $container.find('.price-row').first();
                $container.find('.price-row:not(:first)').remove();
                
                // Cargar cada precio
                precios.forEach((precio, index) => {
                    console.log(`[DEBUG] Procesando precio ${index}:`, precio);
                    console.log(`[DEBUG] Precio ${index} - NombreNivel: ${precio.NombreNivel || precio.nombreNivel}`);
                    console.log(`[DEBUG] Precio ${index} - ImpuestoIds: [${(precio.ImpuestoIds || precio.impuestoIds || []).join(',')}]`);
                    console.log(`[DEBUG] Precio ${index} - PrecioBase: ${precio.PrecioBase || precio.precioBase}`);
                    console.log(`[DEBUG] Precio ${index} - PrecioTotal: ${precio.PrecioTotal || precio.precioTotal}`);
                    
                    if (index === 0) {
                        // Cargar en la primera fila existente
                        console.log('[DEBUG] Cargando en primera fila existente');
                        cargarPrecioEnFila($firstRow, precio);
                    } else {
                        // Agregar nueva fila para precios adicionales
                        console.log('[DEBUG] Creando nueva fila para precio adicional');
                        const $newRow = agregarFilaPrecio();
                        if ($newRow && $newRow.length) {
                            cargarPrecioEnFila($newRow, precio);
                        } else {
                            console.error('[ERROR] No se pudo crear nueva fila para precio');
                        }
                    }
                });
                
                // Actualizar botones de eliminar
                if (typeof window.updateDeleteButtons === 'function') {
                    window.updateDeleteButtons();
                } else {
                    console.warn('window.updateDeleteButtons no está disponible');
                }
            } else {
                console.log('No se encontraron precios para el producto');
            }
        },
        error: function(xhr, status, error) {
            console.error('Error al cargar precios del producto:', error);
        }
    });
}

/**
 * Carga un precio específico en una fila de precio
 */
function cargarPrecioEnFila($row, precio) {
    // Datos básicos del precio - soportar tanto PascalCase (API) como camelCase (JavaScript)
    const nombreNivel = precio.NombreNivel || precio.nombreNivel || 'Precio Base';
    const precioBase = precio.PrecioBase || precio.precioBase || 0;
    const precioTotal = precio.PrecioTotal || precio.precioTotal || 0;
    const id = precio.Id || precio.id || '';
    const esPrincipal = precio.EsPrincipal || precio.esPrincipal || false;
    const orden = precio.Orden || precio.orden || 0;
    const impuestoIds = precio.ImpuestoIds || precio.impuestoIds || [];
    
    console.log(`[DEBUG] Cargando precio en fila: ${nombreNivel}, Base: ${precioBase}, Total: ${precioTotal}`);
    
    $row.find('.nombre-nivel').val(nombreNivel);
    $row.find('.precio-base').val(precioBase);
    $row.find('.precio-total').val(precioTotal);
    
    // Guardar ID del precio para edición
    $row.attr('data-precio-id', id);
    $row.attr('data-nombre-nivel', nombreNivel);
    $row.attr('data-es-principal', esPrincipal);
    $row.attr('data-orden', orden);
    
    // Asegurar que el Select2 esté inicializado antes de cargar impuestos
    const $impuestoSelect = $row.find('.select2-impuestos');
    if ($impuestoSelect.length && !$impuestoSelect.data('select2')) {
        console.log('[DEBUG] Inicializando Select2 antes de cargar impuestos');
        // Obtener índice de la fila
        const index = $row.closest('.price-row').index();
        if (typeof window.initializePriceRow === 'function') {
            window.initializePriceRow($row[0], index);
        }
    }
    
    // Cargar impuestos específicos de este precio
    if (impuestoIds && impuestoIds.length > 0) {
        console.log('[DEBUG] Cargando impuestos específicos:', impuestoIds);
        cargarImpuestosEnFila($row, impuestoIds);
    } else {
        console.log('[DEBUG] No hay impuestos específicos para este precio');
        // Si no tiene impuestos específicos, heredar del producto
        cargarImpuestosDelProductoEnFila($row);
    }
}

/**
 * Carga impuestos específicos en una fila de precio
 */
function cargarImpuestosEnFila($row, impuestoIds) {
    const $impuestoSelect = $row.find('.select2-impuestos');
    const rowIndex = $row.closest('.price-row').index();
    
    console.log(`[DEBUG] cargarImpuestosEnFila - Fila ${rowIndex}, ImpuestoIds recibidos:`, impuestoIds);
    console.log(`[DEBUG] cargarImpuestosEnFila - Tipo de impuestoIds:`, typeof impuestoIds, 'Es array:', Array.isArray(impuestoIds));
    
    if (!$impuestoSelect.length || !impuestoIds || !impuestoIds.length) {
        console.log(`[DEBUG] cargarImpuestosEnFila - Fila ${rowIndex}: No hay impuestos para cargar o select no encontrado`);
        return;
    }
    
    // Usar el mecanismo existente de carga de impuestos
    console.log(`[DEBUG] cargarImpuestosEnFila - Fila ${rowIndex}: Iniciando carga de ${impuestoIds.length} impuestos`);
    const promesasImpuestos = impuestoIds.map((impuestoId, index) => {
        console.log(`[DEBUG] cargarImpuestosEnFila - Fila ${rowIndex}: Cargando impuesto ${index + 1}/${impuestoIds.length} - ID: ${impuestoId}`);
        return $.ajax({
            url: '/api/impuestos/Buscar',
            data: { term: impuestoId.toString() },
            type: 'GET'
        }).then(function(response) {
            const data = response.results || response;
            console.log(`[DEBUG] Respuesta de /api/impuestos/Buscar para ID ${impuestoId}:`, data);
            if (data && data.length > 0) {
                const impuesto = data[0];
                console.log(`[DEBUG] Cargando impuesto ${impuesto.id}: ${impuesto.text} (${impuesto.porcentaje}%)`);
                
                // Guardar el porcentaje en el store global - CRÍTICO
                if (!window.impuestosDataGlobal) {
                    window.impuestosDataGlobal = {};
                }
                window.impuestosDataGlobal[impuesto.id] = parseFloat(impuesto.porcentaje);
                console.log(`[DEBUG] Guardado en window.impuestosDataGlobal[${impuesto.id}] = ${impuesto.porcentaje}`);
                
                // Agregar opción si no existe
                if ($impuestoSelect.find(`option[value="${impuesto.id}"]`).length === 0) {
                    const option = new Option(impuesto.text, impuesto.id, false, false);
                    $impuestoSelect.append(option);
                }
            } else {
                console.warn(`[DEBUG] No se encontró impuesto con ID ${impuestoId}`);
            }
        });
    });
    
    // Esperar a que todos los impuestos se carguen y luego seleccionarlos
    Promise.all(promesasImpuestos).then(() => {
        const idsParaSeleccionar = impuestoIds.map(id => id.toString());
        console.log(`[DEBUG] cargarImpuestosEnFila - Fila ${rowIndex}: Seleccionando impuestos:`, idsParaSeleccionar);
        console.log(`[DEBUG] cargarImpuestosEnFila - Fila ${rowIndex}: Opciones disponibles en select:`, $impuestoSelect.find('option').map((i, opt) => $(opt).val()).get());
        
        $impuestoSelect.val(idsParaSeleccionar).trigger('change');
        console.log(`[DEBUG] cargarImpuestosEnFila - Fila ${rowIndex}: Impuestos seleccionados finalmente:`, $impuestoSelect.val());
        
        // Forzar recálculo después de un pequeño delay para asegurar que los datos estén disponibles
        setTimeout(() => {
            console.log('[DEBUG] Forzando recálculo después de cargar impuestos');
            console.log('[DEBUG] window.impuestosDataGlobal disponible:', window.impuestosDataGlobal);
            
            // Obtener el índice de la fila para identificar el nivel
            const index = $row.closest('.price-row').index();
            
            // Disparar recálculo basado en el precio base (más confiable)
            const $precioBase = $row.find('.precio-base');
            if ($precioBase.length && $precioBase.val()) {
                console.log(`[DEBUG] Disparando recálculo en fila ${index} basado en precio base: ${$precioBase.val()}`);
                $precioBase.trigger('input');
            }
        }, 500); // Dar tiempo para que los porcentajes se carguen en window.impuestosDataGlobal
    });
}

/**
 * Carga impuestos del producto (herencia) en una fila
 */
function cargarImpuestosDelProductoEnFila($row) {
    const productoId = $('#productoId').val();
    if (!productoId) return;
    
    // Obtener impuestos del producto desde el endpoint existente
    $.ajax({
        url: `/api/productos/${productoId}`,
        type: 'GET',
        success: function(response) {
            const productData = response.value || response.Value || response.result || response;
            if (productData && productData.Impuestos) {
                const impuestoIds = productData.Impuestos.map(imp => imp.Id || imp.id);
                cargarImpuestosEnFila($row, impuestoIds);
            }
        },
        error: function(error) {
            console.error('Error al cargar impuestos del producto:', error);
        }
    });
}

/**
 * Agrega una nueva fila de precio
 */
function agregarFilaPrecio() {
    console.log('[DEBUG] agregarFilaPrecio llamada');
    
    // SIEMPRE usar la función global addPriceRow (forzar)
    if (typeof window.addPriceRow === 'function') {
        console.log('[DEBUG] Usando window.addPriceRow');
        window.addPriceRow();
        
        // Retornar la fila recién creada
        const $container = $('#priceRowsContainer');
        const $newRow = $container.find('.price-row').last();
        return $newRow;
    }
    
    console.log('[DEBUG] window.addPriceRow no disponible, usando fallback');
    
    // Fallback: crear fila manualmente
    const $container = $('#priceRowsContainer');
    const filaIndex = $container.find('.price-row').length;
    const nombreNivel = `Precio Nivel ${filaIndex + 1}`;
    
    const template = `
        <div class="row price-row gx-3 mb-3 align-items-center" data-index="${filaIndex}" data-nombre-nivel="${nombreNivel}">
            <div class="col-md-2">
                <label class="form-label">Nombre del nivel *</label>
                <input type="text" class="form-control nombre-nivel" placeholder="Ej: Precio VIP" required 
                       name="Precios[${filaIndex}].NombreNivel" data-original-name="NombreNivel" value="${nombreNivel}">
                <span class="text-danger validation-message"></span>
            </div>
            <div class="col-md-2">
                <label class="form-label">Precio base *</label>
                <div class="input-group">
                    <span class="input-group-text">$</span>
                    <input type="number" class="form-control precio-base" step="0.01" placeholder="0.00" required 
                           name="Precios[${filaIndex}].PrecioBase" data-original-name="PrecioBase" value="0.00">
                </div>
                <span class="text-danger validation-message"></span>
            </div>
            <div class="col-auto d-flex align-items-end pb-1">
                <span class="h4 mx-1">+</span>
            </div>
            <div class="col-md-3">
                <label class="form-label">Impuestos</label>
                <select class="form-select select2-impuestos" multiple="multiple" style="width: 100%;" 
                        name="Precios[${filaIndex}].ImpuestoIds" data-original-name="ImpuestoIds">
                    <!-- Las opciones se cargarán por AJAX -->
                </select>
                <span class="text-danger validation-message"></span>
            </div>
            <div class="col-auto d-flex align-items-end pb-1">
                <span class="h4 mx-1">=</span>
            </div>
            <div class="col-md-2">
                <label class="form-label">Precio Total *</label>
                <div class="input-group">
                    <span class="input-group-text">$</span>
                    <input type="number" class="form-control precio-total" step="0.01" placeholder="0.00" required
                           name="Precios[${filaIndex}].PrecioTotal" data-original-name="PrecioTotal" value="0.00">
                </div>
                <span class="text-danger validation-message"></span>
            </div>
            <div class="col-auto d-flex align-items-center">
                <button type="button" class="btn btn-sm btn-outline-danger btn-eliminar-precio" title="Eliminar este nivel de precio">
                    <i class="fas fa-trash-alt"></i>
                </button>
            </div>
        </div>
    `;
    
    const $newRow = $(template);
    $container.append($newRow);
    
    // Inicializar la fila si la función está disponible
    if (typeof window.initializePriceRow === 'function') {
        console.log('[DEBUG] Inicializando fila con window.initializePriceRow');
        window.initializePriceRow($newRow[0], filaIndex);
    }
    
    // Actualizar botones de eliminar
    if (typeof window.updateDeleteButtons === 'function') {
        console.log('[DEBUG] Actualizando botones con window.updateDeleteButtons');
        window.updateDeleteButtons();
    }
    
    return $newRow;
}

/**
 * Recopila los datos de precios para enviar al servidor
 */
function recopilarDatosPrecios() {
    const precios = [];
    
    console.log('[DEBUG] Iniciando recopilarDatosPrecios...');
    
    $('#priceRowsContainer .price-row').each(function(index) {
        const $row = $(this);
        const nombreNivel = $row.find('.nombre-nivel').val() || $row.attr('data-nombre-nivel') || (index === 0 ? 'Precio Base' : `Precio ${index + 1}`);
        const precioBase = parseFloat($row.find('.precio-base').val()) || 0;
        const precioTotal = parseFloat($row.find('.precio-total').val()) || 0;
        const impuestoIds = $row.find('.select2-impuestos').val() || [];
        
        console.log(`[DEBUG] Fila ${index}: nombre=${nombreNivel}, base=${precioBase}, total=${precioTotal}, impuestos=${JSON.stringify(impuestoIds)}`);
        console.log(`[DEBUG] Tipo de impuestoIds:`, typeof impuestoIds, 'Es array:', Array.isArray(impuestoIds));
        
        // Incluir el precio aunque sea 0, pero validar que tenga al menos nombre
        if (nombreNivel.trim() !== '') {
            const impuestosProcessed = impuestoIds.map(id => parseInt(id));
            const precio = {
                Id: $row.attr('data-precio-id') ? parseInt($row.attr('data-precio-id')) : null,
                NombreNivel: nombreNivel.trim(),
                PrecioBase: precioBase,
                PrecioTotal: precioTotal,
                ImpuestoIds: impuestosProcessed,
                Orden: index,
                EsPrincipal: index === 0, // El primer precio siempre es principal
                Activo: true
            };
            
            console.log(`[DEBUG] Precio agregado con ${impuestosProcessed.length} impuestos:`, precio);
            precios.push(precio);
        }
    });
    
    console.log(`[DEBUG] Total precios recopilados: ${precios.length}`);
    return precios;
}