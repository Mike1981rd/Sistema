// contenedores.js - Versión mejorada con funcionalidad completa para Select2
$(document).ready(function () {
    // Ocultar el campo Unidad de Medida (Inventario)
    $('[for="UnidadMedidaInventarioId"]').closest('.row, .form-group').hide();

    console.log("Inicializando módulo de conversiones de unidades...");

    // Variables para tracking de contenedores recién creados
    let ultimoContenedorCreado = null;

    // Botón para agregar un nuevo contenedor
    $(document).on('click', '#btnAgregarContenedor', function () {
        agregarFilaContenedor();
    });

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
            <h5 class="offcanvas-title" id="offcanvasContenedorLabel">Crear Contenedor</h5>
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
                    <select class="form-select contenedor-select" name="ItemContenedores[${filas}].ContenedorId" required>
                        <option value="">Seleccione un contenedor</option>
                    </select>
                    <input type="hidden" name="ItemContenedores[${filas}].Id" value="${datos ? datos.id || '' : ''}">
                </td>
                <td>
                    <input type="text" class="form-control etiqueta-input" name="ItemContenedores[${filas}].Etiqueta" readonly
                           value="${datos ? datos.etiqueta || '' : ''}">
                </td>
                <td>
                    <input type="number" class="form-control cantidad-input" name="ItemContenedores[${filas}].Cantidad" 
                           value="${esLaPrimera ? '1' : (datos ? datos.cantidad || '' : '')}" 
                           min="${esLaPrimera ? '1' : '0.01'}" step="0.01"
                           ${esLaPrimera ? 'readonly' : ''}>
                </td>
                <td>
                    <input type="number" class="form-control ${esLaPrimera ? 'costo-base' : 'costo-derivado'}" 
                           name="ItemContenedores[${filas}].Costo" 
                           value="${datos ? datos.costo || '0.0000' : '0.0000'}"
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
        const $select = $('.contenedor-select').last();

        $select.select2({
            theme: 'bootstrap-5',
            placeholder: 'Seleccione o cree un contenedor',
            allowClear: true,
            width: '100%',
            dropdownParent: $('body'),
            ajax: {
                url: window.location.origin + '/Contenedor/Buscar',
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    console.log("Enviando búsqueda contenedor:", params.term);
                    return {
                        term: params.term || ''
                    };
                },
                processResults: function (data, params) {
                    console.log("Resultados contenedores recibidos:", data);
                    var results = data.results || [];

                    if (results.length == 0 && params.term && params.term.trim() !== '') {
                        console.log("Agregando opción para crear nuevo contenedor:", params.term);
                        results.push({
                            id: 'new',
                            text: 'Crear contenedor: "' + params.term + '"',
                            term: params.term,
                            _isNew: true  // Marca para identificar elemento nuevo
                        });
                    }

                    return {
                        results: results
                    };
                },
                error: function (xhr, status, error) {
                    console.error("Error en la búsqueda de contenedores:", error);
                    console.error("Estado:", status);
                    console.error("Respuesta:", xhr.responseText);
                },
                cache: true
            },
            templateResult: formatContenedorResult,
            templateSelection: formatContenedorSelection
        }).on('select2:select', function (e) {
            var data = e.params.data;
            console.log("Contenedor seleccionado:", data);

            if (data.id === 'new') {
                $(this).val(null).trigger('change');
                console.log("Creando contenedor:", data.term);
                abrirOffcanvasContenedor(data.term);
            } else {
                // Actualizar etiqueta
                actualizarEtiqueta($(this).closest('tr'));

                // Recalcular costos
                recalcularCostos();
            }
        });

        // Si hay datos, seleccionar el contenedor
        if (datos && datos.contenedorId) {
            // Crear la opción y seleccionarla
            const option = new Option(datos.contenedorNombre, datos.contenedorId, true, true);
            $select.append(option).trigger('change');
        }

        // Actualizar botones de eliminar
        actualizarBotonesEliminar();

        // Habilitar el select de unidad de compra preferida si es la primera fila
        if (esLaPrimera) {
            setTimeout(function () {
                $('#UnidadCompraId').prop('disabled', false);
            }, 500);
        }
    }

    // Actualizar botones de eliminar (solo en la última fila)
    function actualizarBotonesEliminar() {
        // Quitar todos los botones de eliminar
        $('.btn-eliminar-contenedor').remove();

        // Agregar botón solo a la última fila que no sea la primera
        const $rows = $('#contenedores-body tr');
        if ($rows.length > 1) {
            const $lastRow = $rows.last();
            const $lastCell = $lastRow.find('td:last');

            $lastCell.html(botonEliminarMinimalista);
        }
    }

    // Eliminar fila de contenedor
    function eliminarFilaContenedor($button) {
        const $row = $button.closest('tr');
        const index = $row.data('index');

        // No permitir eliminar la primera fila (unidad base)
        if (index === 0) {
            alert('No se puede eliminar la unidad base');
            return;
        }

        // Confirmar eliminación
        if (confirm('¿Está seguro de eliminar esta conversión?')) {
            // Si la fila tiene un ID, agregarlo a la lista de eliminados
            const id = $row.find('input[name$=\".Id\"]').val();

            if (id) {
                // Crear campo oculto para enviar al servidor
                if ($('#contenedores-eliminados').length === 0) {
                    $('form').append('<div id=\"contenedores-eliminados\" style=\"display:none;\"></div>');
                }

                $('#contenedores-eliminados').append(`<input type=\"hidden\" name=\"ItemContenedoresEliminados\" value=\"${id}\">`);
            }

            // Eliminar la fila
            $row.remove();

            // Renumerar las filas
            renumerarFilas();

            // Actualizar botones de eliminar
            actualizarBotonesEliminar();

            // Recalcular costos
            recalcularCostos();
        }
    }

    // Renumerar filas después de eliminar
    function renumerarFilas() {
        $('#contenedores-body tr').each(function (idx) {
            // Actualizar atributo data-index
            $(this).attr('data-index', idx);

            // Actualizar número visible
            $(this).find('td:first').text(idx + 1);

            // Actualizar nombres de los campos
            $(this).find('select, input').each(function () {
                const name = $(this).attr('name');
                if (name) {
                    const newName = name.replace(/ItemContenedores\\[\\d+\\]/, `ItemContenedores[${idx}]`);
                    $(this).attr('name', newName);
                }
            });
        });
    }

    // Actualizar etiqueta al cambiar contenedor
    function actualizarEtiqueta($row) {
        const index = $row.data('index');
        const contenedorActualNombre = $row.find('.contenedor-select option:selected').text();

        if (index === 0) {
            // La base muestra solo su propio nombre
            $row.find('.etiqueta-input').val(contenedorActualNombre);
            return;
        }

        // Para las demás filas, usar el contenedor actual / contenedor ANTERIOR (no el base)
        const $filaAnterior = $row.prev('tr');
        const contenedorAnteriorNombre = $filaAnterior.find('.contenedor-select option:selected').text();

        if (contenedorActualNombre && contenedorAnteriorNombre) {
            const etiqueta = `${contenedorActualNombre}/${contenedorAnteriorNombre}`;
            $row.find('.etiqueta-input').val(etiqueta);
        }
    }

    // Recalcular todos los costos derivados - CORREGIDO
    function recalcularCostos() {
        // Para cada fila que no sea la base
        $('#contenedores-body tr').each(function(idx) {
            const $row = $(this);
            const index = $row.data('index');
            
            if (index === 0) return; // Saltar la fila base
            
            // Obtener la fila anterior (NO la base)
            const $filaAnterior = $row.prev('tr');
            
            // Obtener costo de la fila anterior (NO el costo base)
            const costoAnterior = parseFloat($filaAnterior.find('.costo-derivado, .costo-base').val()) || 0;
            
            // Cantidad de la fila actual
            const cantidad = parseFloat($row.find('.cantidad-input').val()) || 0;
            if (cantidad <= 0) return;
            
            // Cálculo: Costo anterior / Cantidad actual
            const costo = costoAnterior / cantidad;
            
            // Formatear con 4 decimales
            $row.find('.costo-derivado').val(costo.toFixed(4));
        });
    }

    // Cargar datos iniciales
    function cargarDatosIniciales() {
        const itemId = $('#Id').val();

        // Si es un nuevo item, agregar una fila vacía
        if (!itemId) {
            agregarFilaContenedor();
            return;
        }

        // Cargar conversiones existentes
        $.ajax({
            url: `/Item/ObtenerConversiones/${itemId}`,
            method: 'GET',
            success: function (data) {
                // Si hay datos, cargarlos
                if (data && data.length > 0) {
                    data.forEach(function (item) {
                        agregarFilaContenedor(item);
                    });
                } else {
                    // Si no hay datos, agregar una fila vacía
                    agregarFilaContenedor();
                }
            },
            error: function () {
                console.error('Error al cargar conversiones');
                agregarFilaContenedor();
            }
        });
    }

    // Iniciar carga de datos
    cargarDatosIniciales();
});

// Función para abrir offcanvas (crear)
function abrirOffcanvasContenedor(termino) {
    $('.contenedor-select').select2('close');
    $('#offcanvasContenedorLabel').text('Crear Contenedor');
    $('#nombreContenedor').val(termino || '');
    $('#idContenedor').val('0');

    // Limpiar eventos previos de ambos modos y de cualquier otro posible
    $(document).off('click.crearContenedor');
    $(document).off('click.editarContenedor');
    $(document).off('click', '#btnSaveContenedor');

    // Evento solo para crear (usa el namespace y el selector directo)
    $(document).on('click.crearContenedor', '#btnSaveContenedor', function (e) {
        e.preventDefault();
        var nombre = $('#nombreContenedor').val();
        if (!nombre) {
            Swal.fire('Error', 'El nombre es requerido', 'error');
            return;
        }
        $.ajax({
            url: '/Contenedor/Create',
            type: 'POST',
            data: { nombre: nombre },
            success: function (response) {
                if (response.success) {
                    var newOption = new Option(response.nombre, response.id, true, true);
                    $('.contenedor-select').append(newOption).val(response.id).trigger('change');
                    $('#offcanvasContenedor').offcanvas('hide');
                    Swal.fire('Éxito', 'Contenedor guardado correctamente', 'success');
                } else {
                    Swal.fire('Error', response.message || 'No se pudo guardar', 'error');
                }
            },
            error: function () {
                Swal.fire('Error', 'No se pudo guardar el contenedor', 'error');
            }
        });
    });

    $('#offcanvasContenedor').offcanvas('show');
}

// Función para editar 
function editContenedor(id, nombre) {
    $('.contenedor-select').select2('close');
    $('#offcanvasContenedorLabel').text('Editar Contenedor');
    $('#nombreContenedor').val(nombre || '');
    $('#idContenedor').val(id);

    // Limpiar eventos previos
    $(document).off('click.crearContenedor');
    $(document).off('click.editarContenedor');

    // Evento solo para editar
    $(document).on('click.editarContenedor', '#btnSaveContenedor', function () {
        var nuevoNombre = $('#nombreContenedor').val();
        if (!nuevoNombre) {
            Swal.fire('Error', 'El nombre es requerido', 'error');
            return;
        }
        $.ajax({
            url: '/Contenedor/Edit/' + id,
            type: 'POST',
            data: { id: id, nombre: nuevoNombre },
            success: function (response) {
                if (response.success) {
                    $('.contenedor-select option[value="' + id + '"]')
                        .text(nuevoNombre);
                    $('.contenedor-select').val(id).trigger('change');
                    $('#offcanvasContenedor').offcanvas('hide');
                    Swal.fire('Éxito', 'Contenedor actualizado correctamente', 'success');
                } else {
                    Swal.fire('Error', response.message || 'No se pudo actualizar', 'error');
                }
            },
            error: function () {
                Swal.fire('Error', 'No se pudo actualizar el contenedor', 'error');
            }
        });
    });

    $('#offcanvasContenedor').offcanvas('show');
}