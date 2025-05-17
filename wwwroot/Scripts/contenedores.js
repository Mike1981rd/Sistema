// contenedores.js - Versión mejorada con funcionalidad completa para Select2
console.log('contenedores.js: Script cargado y ejecutándose (IIFE externa)');
(function() {
    console.log('contenedores.js: Dentro de IIFE');
    // Esperar hasta que jQuery esté disponible completamente
    function checkjQuery() {
        console.log('contenedores.js: Entrando en checkjQuery');
        console.log('Verificando disponibilidad de jQuery...');
        if (typeof jQuery === 'undefined') {
            console.log('jQuery no está disponible todavía, esperando...');
            setTimeout(checkjQuery, 100);
            return;
        }
        
        console.log('jQuery encontrado, inicializando contenedores.js');
        initContenedores(jQuery);
    }
    
    // Función principal que inicializa todo el módulo
    function initContenedores($) {
        console.log('contenedores.js: Entrando en initContenedores (PRIMERA LÍNEA DENTRO)');
        console.log('contenedores.js: Entrando en initContenedores');

        // Inicializar Select2 para Proveedor
        inicializarSelectProveedor();
        console.log('contenedores.js: Después de inicializarSelectProveedor');

        // Ocultar el campo Unidad de Medida (Inventario)
        $('[for="UnidadMedidaInventarioId"]').closest('.row, .form-group').hide();
        console.log('contenedores.js: Después de ocultar UnidadMedidaInventarioId');

        console.log("Inicializando módulo de conversiones de unidades...");

        // Variables para tracking de contenedores recién creados
        let ultimoContenedorCreado = null;

        console.log('contenedores.js: Antes de enlazar evento para #btnAgregarContenedor');
        // Botón para agregar un nuevo contenedor
        $(document).on('click', '#btnAgregarContenedor', function () {
            console.log('#btnAgregarContenedor clickeado! Intentando agregar fila...');
            agregarFilaContenedor();
            
            // Actualizar contenedores en proveedores si la función existe
            if (typeof window.actualizarContenedoresProveedores === 'function') {
                window.actualizarContenedoresProveedores();
            }
        });
        console.log('contenedores.js: Después de enlazar evento para #btnAgregarContenedor');

        // Eliminar un contenedor
        $(document).on('click', '.btn-eliminar-contenedor', function () {
            eliminarFilaContenedor($(this));
        });

        // Cambio en selector de contenedor
        $(document).on('change', '.contenedor-select', function () {
            actualizarEtiqueta($(this).closest('tr'));

            // Si el contenedor acaba de ser creado, evitar procesamiento automático
            const contenedorId = $(this).val();
            if (ultimoContenedorCreado && ultimoContenedorCreado.id == contenedorId) {
                console.log('Omitiendo procesamiento para contenedor recién creado:', contenedorId);
                return;
            }
        });

        // Cambio en precio base
        $(document).on('input', '.costo-base', function () {
            recalcularCostos();
        });

        // Cambio en cantidad
        $(document).on('input', '.cantidad-input', function () {
            const $row = $(this).closest('tr');
            const index = $row.data('index');

            // La primera fila siempre tiene cantidad 1
            if (index === 0) {
                $(this).val(1);
            }

            recalcularCostos();
        });

        // Botón de editar contenedor
        $(document).on('click', '.edit-contenedor', function (e) {
            e.preventDefault();
            e.stopPropagation();
            const id = $(this).data('id');
            const nombre = $(this).data('name');
            editContenedor(id, nombre);
        });

        // Formateo para los resultados en el dropdown de contenedores
        function formatContenedorResult(contenedor) {
            if (contenedor.loading) {
                return contenedor.text;
            }

            if (contenedor.id === 'new') {
                return $('<div class="select2-result-contenedor">' +
                    '<div class="select2-result-contenedor__action"><i class="fas fa-plus-circle text-success me-1"></i> ' +
                    contenedor.text + '</div>' +
                    '</div>');
            }

            return $('<div class="select2-result-contenedor">' +
                '<div class="select2-result-contenedor__name">' + contenedor.text + '</div>' +
                '</div>');
        }

        // Formateo para la selección de contenedores con botones de edición
        function formatContenedorSelection(contenedor) {
            if (!contenedor.id || contenedor.id === 'new') {
                return contenedor.text;
            }

            // Añadir iconos de edición junto al nombre
            var $container = $('<div class="d-flex align-items-center justify-content-between w-100"></div>');
            var $contenedorNombre = $('<div>' + contenedor.text + '</div>');
            var $actions = $('<div class="contenedor-actions ms-2"></div>');

            var $editBtn = $('<button type="button" class="btn btn-link btn-sm p-0 me-2 edit-contenedor" ' +
                'data-id="' + contenedor.id + '" data-name="' + contenedor.text + '">' +
                '<i class="fas fa-pencil-alt text-primary"></i></button>');

            $actions.append($editBtn);
            $container.append($contenedorNombre);
            $container.append($actions);

            // Ocultar el botón "x" del Select2 (Clear)
            setTimeout(function () {
                $('.select2-selection__clear').hide();
            }, 100);

            // Asignar manejadores de eventos inmediatamente
            $editBtn.on('click', function (e) {
                e.preventDefault();
                e.stopPropagation();
                editContenedor(contenedor.id, contenedor.text);
                return false;
            });

            return $container;
        }

        // 1. Eliminar cualquier offcanvas existente para evitar conflictos
        $('#offcanvasContenedor').remove();

        // 2. Crear y añadir un HTML muy simple directamente en la página
        const offcanvasHTML = `
        <div class="offcanvas offcanvas-end" id="offcanvasContenedor" style="width: 400px;">
            <div class="offcanvas-header bg-primary text-white">
                <h5 class="offcanvas-title" id="offcanvasContenedorLabel">Crear Unidad de Medida</h5>
                <button type="button" class="btn-close text-reset" data-bs-dismiss="offcanvas" aria-label="Close"></button>
            </div>
            <div class="offcanvas-body">
                <div class="mb-3">
                    <label for="nombreContenedor" class="form-label">Nombre</label>
                    <input type="text" class="form-control" id="nombreContenedor">
                    <input type="hidden" id="idContenedor" value="0">
                </div>
                <div class="text-end">
                    <button type="button" class="btn btn-secondary me-2" data-bs-dismiss="offcanvas">Cancelar</button>
                    <button type="button" class="btn btn-primary" id="btnSaveContenedor">Guardar</button>
                </div>
            </div>
        </div>`;

        $('body').append(offcanvasHTML);

        // Cambiar el botón eliminar por uno minimalista
        const botonEliminarMinimalista = '<button type="button" class="btn btn-sm btn-link text-danger btn-eliminar-contenedor"><i class="fas fa-times-circle"></i></button>';

        // Función para agregar una nueva fila de contenedor
        function agregarFilaContenedor(datos = null) {
            const filas = $('#contenedores-body tr').length;
            const esLaPrimera = filas === 0;

            // Crear fila HTML
            const html = `
                <tr data-index="${filas}">
                    <td>${filas + 1}</td>
                    <td>
                        <select class="form-select contenedor-select" name="Contenedores[${filas}].UnidadMedidaId" required>
                            <option value="">Seleccione un contenedor</option>
                        </select>
                        <input type="hidden" name="Contenedores[${filas}].Id" value="${datos ? datos.id || '' : ''}">
                        <input type="hidden" name="Contenedores[${filas}].Nombre" class="contenedor-nombre" value="${datos ? datos.nombre || '' : ''}">
                    </td>
                    <td>
                        <input type="text" class="form-control etiqueta-input" name="Contenedores[${filas}].Etiqueta" readonly
                            value="${datos ? datos.etiqueta || '' : ''}">
                    </td>
                    <td>
                        <input type="number" class="form-control cantidad-input" name="Contenedores[${filas}].Factor" 
                            value="${esLaPrimera ? '1' : (datos ? datos.factor || '' : '')}" 
                            min="${esLaPrimera ? '1' : '0.01'}" step="0.01"
                            ${esLaPrimera ? 'readonly' : ''}>
                    </td>
                    <td>
                        <input type="number" class="form-control ${esLaPrimera ? 'costo-base' : 'costo-derivado'}" 
                            name="Contenedores[${filas}].Costo" 
                            value="${datos ? datos.costo || '' : ''}"
                            min="0" step="0.0001"
                            ${!esLaPrimera ? 'readonly' : ''}>
                    </td>
                    <td>
                        ${!esLaPrimera ?
                    botonEliminarMinimalista
                    : ''}
                    </td>
                </tr>
            `;

            // Añadir a la tabla
            $('#contenedores-body').append(html);

            // Inicializar select2 para el contenedor
            const $select = $(`#contenedores-body tr[data-index="${filas}"] .contenedor-select`);
            
            // Si el contenedor ya está inicializado como select2, destruirlo primero
            if ($select.hasClass('select2-hidden-accessible')) {
                $select.select2('destroy');
            }
            
            $select.select2({
                theme: 'bootstrap-5',
                placeholder: 'Seleccione un contenedor',
                width: '100%',
                ajax: {
                    url: '/Contenedor/Buscar',
                    dataType: 'json',
                    delay: 250,
                    data: function (params) {
                        return {
                            term: params.term || ''
                        };
                    },
                    processResults: function (data, params) {
                        var results = data.results || [];
                        
                        // Añadir la opción de crear si es necesario
                        if (results.length === 0 && params.term && params.term.trim() !== '') {
                            results.push({
                                id: 'new',
                                text: 'Crear contenedor: "' + params.term + '"',
                                term: params.term
                            });
                        }
                        
                        return {
                            results: results
                        };
                    },
                    cache: true
                },
                language: {
                    noResults: function () {
                        return "No hay resultados";
                    }
                },
                escapeMarkup: function (markup) {
                    return markup;
                },
                templateResult: formatContenedorResult,
                templateSelection: formatContenedorSelection
            }).on('select2:select', function (e) {
                if (e.params.data.id === 'new') {
                    // Crear nuevo contenedor
                    const term = e.params.data.term;
                    abrirOffcanvasContenedor(term);
                    
                    // Restablecer el select
                    $(this).val(null).trigger('change');
                }
            });
            
            // Si hay datos preexistentes, establecer el valor
            if (datos && datos.unidadMedidaId) {
                const option = new Option(datos.nombre, datos.unidadMedidaId, true, true);
                $select.append(option).trigger('change');
            }
            
            // Actualizar la UI después de agregar la fila
            actualizarBotonesEliminar();
            renumerarFilas();
        }

        // Actualizar visibilidad de botones eliminar
        function actualizarBotonesEliminar() {
            const $filas = $('#contenedores-body tr');
            
            // Siempre debe haber al menos una fila
            if ($filas.length <= 1) {
                $('.btn-eliminar-contenedor').hide();
            } else {
                $('.btn-eliminar-contenedor').show();
            }
        }

        // Eliminar fila de contenedor
        function eliminarFilaContenedor($button) {
            const $fila = $button.closest('tr');
            $fila.remove();
            
            // Actualizar la UI
            renumerarFilas();
            actualizarBotonesEliminar();
            recalcularCostos();
            
            // Actualizar contenedores en proveedores si la función existe
            if (typeof window.actualizarContenedoresProveedores === 'function') {
                window.actualizarContenedoresProveedores();
            }
        }

        // Renumerar las filas
        function renumerarFilas() {
            $('#contenedores-body tr').each(function (index) {
                // Actualizar el número visible
                $(this).find('td:first').text(index + 1);
                
                // Actualizar atributos de datos
                $(this).attr('data-index', index);
                
                // Actualizar nombres de campos
                $(this).find('[name^="Contenedores["]').each(function() {
                    const nameParts = $(this).attr('name').split('.');
                    $(this).attr('name', `Contenedores[${index}].${nameParts[1]}`);
                });
            });
        }

        // Actualizar etiqueta basada en el contenedor seleccionado
        function actualizarEtiqueta($row) {
            const contenedorId = $row.find('.contenedor-select').val();
            if (!contenedorId) return;

            const contenedorNombreActual = $row.find('.contenedor-select option:selected').text();
            $row.find('.contenedor-nombre').val(contenedorNombreActual);

            let etiquetaFinal = contenedorNombreActual;
            const rowIndex = $row.index(); // es 0-indexed

            if (rowIndex > 0) {
                const $prevRow = $row.prev('tr');
                if ($prevRow.length > 0) {
                    // Usar el nombre original del contenedor de la fila anterior, no su etiqueta compuesta
                    const nombreContenedorAnterior = $prevRow.find('.contenedor-nombre').val(); 
                    if (nombreContenedorAnterior) {
                        etiquetaFinal = `${contenedorNombreActual}/${nombreContenedorAnterior}`;
                    }
                }
            }
            $row.find('.etiqueta-input').val(etiquetaFinal);
        }

        // Recalcular costos de contenedores
        function recalcularCostos() {
            let costoAnterior = parseFloat($('#contenedores-body tr:first .costo-base').val()) || 0;

            $('#contenedores-body tr').each(function (index) {
                const $currentRow = $(this);
                if (index === 0) {
                    // Asegurarse de que el costo base es el de la primera fila para el inicio del cálculo
                    costoAnterior = parseFloat($currentRow.find('.costo-base').val()) || 0;
                    // No hay nada que calcular para la primera fila, su costo es la base.
                    return;
                }

                const factorActual = parseFloat($currentRow.find('.cantidad-input').val()) || 0;
                let costoCalculado = 0;

                if (factorActual > 0 && costoAnterior > 0) {
                    costoCalculado = costoAnterior / factorActual;
                } else {
                    console.warn(`No se puede calcular el costo para la fila ${index + 1} debido a factor o costo anterior inválido. Factor: ${factorActual}, Costo Anterior: ${costoAnterior}`);
                }
                
                $currentRow.find('.costo-derivado').val(costoCalculado.toFixed(4));
                
                // El costo calculado de esta fila es el "costoAnterior" para la siguiente iteración
                // Sin embargo, es más robusto siempre leer el costo del input de la fila anterior en la siguiente iteración,
                // por si el usuario lo modifica manualmente (aunque sean readonly)
                // La variable costoAnterior se actualizará al inicio de la próxima iteración relevante.
            });

            // Actualizar el costo anterior para la siguiente fila fuera del if(index===0)
            // Esto se hace obteniendo el costo de la fila actual después de ser calculado.
            // No, es mejor leer el costo de la fila PREVIA en cada iteración.

            // Nueva lógica para recalcular costos iterativamente:
            $('#contenedores-body tr').each(function(i) {
                const $filaActual = $(this);
                if (i === 0) {
                    // La primera fila tiene el costo base, no se calcula.
                    return; // continue
                }

                const $filaAnterior = $filaActual.prev('tr');
                if ($filaAnterior.length === 0) {
                    console.error("Error: Fila anterior no encontrada para fila", i);
                    return; // continue
                }

                const costoFilaAnterior = parseFloat($filaAnterior.find('input[name$=".Costo"]').val()) || 0;
                const factorFilaActual = parseFloat($filaActual.find('.cantidad-input').val()) || 0;
                let costoCalculadoActual = 0;

                if (factorFilaActual > 0 && costoFilaAnterior > 0) {
                    costoCalculadoActual = costoFilaAnterior / factorFilaActual;
                } else {
                    console.warn(`Advertencia al calcular costo para fila ${i+1}: Factor (${factorFilaActual}) o Costo Anterior (${costoFilaAnterior}) no son válidos.`);
                }
                $filaActual.find('.costo-derivado').val(costoCalculadoActual.toFixed(4));
            });
        }

        // Cargar datos iniciales si existen
        function cargarDatosIniciales() {
            // Si no hay contenedores, agregar uno vacío
            if ($('#contenedores-body tr').length === 0) {
                agregarFilaContenedor();
            }
        }

        // Exponer la función para cargar contenedores existentes globalmente
        console.log('contenedores.js: Antes de definir window.cargarContenedoresExistentes');
        window.cargarContenedoresExistentes = function(contenedores) {
            console.log('contenedores.js: window.cargarContenedoresExistentes fue llamada con:', contenedores);
            
            // Limpiar contenedores existentes
            $('#contenedores-body').empty();
            
            // Si no hay contenedores, agregar uno vacío
            if (!contenedores || contenedores.length === 0) {
                agregarFilaContenedor();
                return;
            }
            
            // Agregar cada contenedor
            contenedores.forEach(function(contenedor) {
                agregarFilaContenedor({
                    id: contenedor.id,
                    unidadMedidaId: contenedor.unidadMedidaId,
                    nombre: contenedor.nombre,
                    etiqueta: contenedor.etiqueta,
                    factor: contenedor.factor,
                    costo: contenedor.costo
                });
            });
            
            // Actualizar la UI
            actualizarBotonesEliminar();
            renumerarFilas();
        };
        console.log('contenedores.js: Después de definir window.cargarContenedoresExistentes');

        // Inicialización a nivel de documento
        $(document).ready(function() {
            console.log('contenedores.js: $(document).ready dentro de initContenedores');
            cargarDatosIniciales();
            
            // Inicializar el guardado de contenedor
            $(document).on('click', '#btnSaveContenedor', function() {
                const nombre = $('#nombreContenedor').val();
                const id = $('#idContenedor').val();
                
                if (!nombre) {
                    alert('El nombre es obligatorio');
                    return;
                }
                
                $.ajax({
                    url: '/Contenedor/Create',
                    type: 'POST',
                    data: {
                        Id: id,
                        Nombre: nombre
                    },
                    success: function(response) {
                        if (response.success) {
                            // Guardar referencia para evitar doble procesamiento
                            ultimoContenedorCreado = {
                                id: response.data.id,
                                nombre: response.data.nombre
                            };
                            
                            // Cerrar offcanvas
                            bootstrap.Offcanvas.getInstance($('#offcanvasContenedor')).hide();
                            
                            // Agregar opción al select activo
                            const option = new Option(response.data.nombre, response.data.id, true, true);
                            $('.contenedor-select:focus').append(option).trigger('change');
                            
                            // Actualizar etiqueta
                            actualizarEtiqueta($('.contenedor-select:focus').closest('tr'));
                        } else {
                            alert(response.message || 'Error al guardar el contenedor');
                        }
                    },
                    error: function() {
                        alert('Error al guardar el contenedor');
                    }
                });
            });
        });

        // Exponer funciones globalmente para ser utilizadas desde fuera
        window.abrirOffcanvasContenedor = abrirOffcanvasContenedor;
        window.editContenedor = editContenedor;
        window.inicializarSelectProveedor = inicializarSelectProveedor;
    }

    // Función para abrir el offcanvas de contenedor
    function abrirOffcanvasContenedor(termino) {
        // Resetear formulario
        $('#nombreContenedor').val(termino || '');
        $('#idContenedor').val('0');
        
        // Cambiar título
        $('#offcanvasContenedorLabel').text('Crear Unidad de Medida');
        
        // Abrir offcanvas
        const offcanvasEl = document.getElementById('offcanvasContenedor');
        if (offcanvasEl) {
            const offcanvas = new bootstrap.Offcanvas(offcanvasEl);
            offcanvas.show();
            
            // Focus en el campo de nombre
            setTimeout(function() {
                $('#nombreContenedor').focus();
            }, 500);
        } else {
            console.error('Offcanvas element not found');
        }
    }

    // Función para editar un contenedor existente
    function editContenedor(id, nombre) {
        // Establecer valores en el formulario
        $('#nombreContenedor').val(nombre);
        $('#idContenedor').val(id);
        
        // Cambiar título
        $('#offcanvasContenedorLabel').text('Editar Unidad de Medida');
        
        // Abrir offcanvas
        const offcanvasEl = document.getElementById('offcanvasContenedor');
        if (offcanvasEl) {
            const offcanvas = new bootstrap.Offcanvas(offcanvasEl);
            offcanvas.show();
            
            // Focus en el campo de nombre
            setTimeout(function() {
                $('#nombreContenedor').focus();
            }, 500);
        } else {
            console.error('Offcanvas element not found');
        }
    }

    // Inicializar Select2 para Proveedor
    function inicializarSelectProveedor() {
        console.log('Intentando inicializar proveedores...');
        
        if (typeof jQuery !== 'undefined') {
            console.log('jQuery encontrado');
            const $ = jQuery;
            
            if (typeof $.fn.select2 !== 'undefined') {
                console.log('Select2 encontrado');
                
                // Si estamos añadiendo este select desde AJAX, podría ya estar inicializado
                $('.proveedor-select').each(function() {
                    const $select = $(this);
                    
                    // Si ya está inicializado, ignorar
                    if ($select.hasClass('select2-hidden-accessible')) {
                        return;
                    }
                    
                    $select.select2({
                        theme: 'bootstrap-5',
                        placeholder: 'Seleccione un proveedor',
                        width: '100%',
                        ajax: {
                            url: '/Proveedor/Buscar',
                            dataType: 'json',
                            delay: 250,
                            data: function (params) {
                                return {
                                    term: params.term || ''
                                };
                            },
                            processResults: function (data, params) {
                                var results = data.results || [];
                                
                                // Añadir la opción de crear si es necesario
                                if (results.length === 0 && params.term && params.term.trim() !== '') {
                                    results.push({
                                        id: 'new',
                                        text: 'Crear proveedor: "' + params.term + '"',
                                        term: params.term
                                    });
                                }
                                
                                return {
                                    results: results
                                };
                            },
                            cache: true
                        },
                        language: {
                            noResults: function () {
                                return "No hay resultados";
                            }
                        },
                        escapeMarkup: function (markup) {
                            return markup;
                        },
                        templateResult: formatProveedorResult,
                        templateSelection: formatProveedorSelection
                    }).on('select2:select', function (e) {
                        if (e.params.data.id === 'new') {
                            // Crear nuevo proveedor
                            const term = e.params.data.term;
                            abrirOffcanvasProveedor(term);
                            
                            // Restablecer el select
                            $(this).val(null).trigger('change');
                        }
                    });
                });
            } else {
                console.warn('Select2 no está disponible');
            }
        } else {
            console.warn('jQuery no está disponible');
        }
    }

    // Formateo para los resultados en el dropdown de proveedores
    function formatProveedorResult(proveedor) {
        if (proveedor.loading) {
            return proveedor.text;
        }

        if (proveedor.id === 'new') {
            return '<div class="select2-result-proveedor"><div class="select2-result-proveedor__action"><i class="fas fa-plus-circle text-success me-1"></i> ' + proveedor.text + '</div></div>';
        }

        return '<div class="select2-result-proveedor"><div class="select2-result-proveedor__name">' + proveedor.text + '</div></div>';
    }

    // Formateo para la selección de proveedores con botones de edición
    function formatProveedorSelection(proveedor) {
        if (!proveedor.id || proveedor.id === 'new') {
            return proveedor.text;
        }

        // Añadir iconos de edición junto al nombre
        var container = '<div class="d-flex align-items-center justify-content-between w-100">';
        var proveedorNombre = '<div>' + proveedor.text + '</div>';
        var actions = '<div class="proveedor-actions ms-2">';
        
        var editBtn = '<button type="button" class="btn btn-link btn-sm p-0 me-2 edit-proveedor" ' +
            'data-id="' + proveedor.id + '">' +
            '<i class="fas fa-pencil-alt text-primary"></i></button>';
        
        actions += editBtn + '</div>';
        container += proveedorNombre + actions + '</div>';

        return container;
    }

    // Función para abrir el offcanvas de proveedor
    function abrirOffcanvasProveedor(termino) {
        // Verificar primero si existe el offcanvas
        if ($('#offcanvasProveedor').length === 0) {
            console.error('Offcanvas de proveedor no encontrado');
            return;
        }

        // Resetear formulario si existe
        if ($('#formProveedor').length > 0) {
            $('#formProveedor')[0].reset();
            $('#formProveedor #ProveedorId').val('0');
        }
        
        // Cambiar título
        $('#offcanvasProveedorLabel').text('Crear Proveedor');
        
        // Establecer nombre sugerido si se proporciona
        if (termino && $('#ProveedorNombre').length > 0) {
            $('#ProveedorNombre').val(termino);
        }
        
        // Abrir offcanvas
        const offcanvasEl = document.getElementById('offcanvasProveedor');
        if (offcanvasEl) {
            const offcanvas = new bootstrap.Offcanvas(offcanvasEl);
            offcanvas.show();
            
            // Focus en el campo de nombre
            setTimeout(function() {
                $('#ProveedorNombre').focus();
            }, 500);
        } else {
            console.error('Offcanvas element not found');
        }
    }

    // Función para editar un proveedor existente
    function editProveedor(id) {
        // Realizar una solicitud AJAX para obtener los datos del proveedor
        $.ajax({
            url: '/Proveedor/GetById',
            type: 'GET',
            data: { id: id },
            success: function(data) {
                if (data.success) {
                    const proveedor = data.data;
                    
                    // Establecer valores en el formulario
                    $('#ProveedorId').val(proveedor.id);
                    $('#ProveedorNombre').val(proveedor.nombre);
                    $('#ProveedorRuc').val(proveedor.ruc);
                    $('#ProveedorDireccion').val(proveedor.direccion);
                    $('#ProveedorTelefono').val(proveedor.telefono);
                    $('#ProveedorEmail').val(proveedor.email);
                    
                    // Cambiar título
                    $('#offcanvasProveedorLabel').text('Editar Proveedor');
                    
                    // Abrir offcanvas
                    const offcanvasEl = document.getElementById('offcanvasProveedor');
                    if (offcanvasEl) {
                        const offcanvas = new bootstrap.Offcanvas(offcanvasEl);
                        offcanvas.show();
                    }
                } else {
                    alert('Error al cargar los datos del proveedor: ' + data.message);
                }
            },
            error: function() {
                alert('Error al cargar los datos del proveedor');
            }
        });
    }
    
    // Iniciar la comprobación de jQuery
    checkjQuery();
})();