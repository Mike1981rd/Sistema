$(document).ready(function() {
    console.log("Inicializando contenedores para edición...");
    
    // Variable global para contenedores
    window.contenedoresData = [];
    
    // Función global para agregar fila de contenedor
    window.agregarFilaContenedor = function(datos = null) {
        const index = $('#tablaContenedores tbody tr').length;
        const esLaPrimera = index === 0;
        
        const html = `
            <tr data-index="${index}">
                <td>${index + 1}</td>
                <td>
                    <select class="form-select contenedor-select" name="ItemContenedores[${index}].UnidadMedidaId" required>
                        <option value="">Seleccione un contenedor</option>
                    </select>
                    <input type="hidden" name="ItemContenedores[${index}].Id" value="${datos ? datos.id || '' : ''}">
                </td>
                <td>
                    <input type="text" class="form-control etiqueta-input" name="ItemContenedores[${index}].Etiqueta" 
                           value="${datos ? datos.etiqueta || '' : ''}" readonly>
                </td>
                <td>
                    <input type="number" class="form-control cantidad-input" name="ItemContenedores[${index}].Factor" 
                           value="${esLaPrimera ? '1' : (datos ? datos.cantidad || '1' : '1')}" 
                           min="0.01" step="0.01" ${esLaPrimera ? 'readonly' : ''} required>
                </td>
                <td>
                    <input type="number" class="form-control costo-input" name="ItemContenedores[${index}].Costo" 
                           value="${datos ? datos.costo || '' : ''}" 
                           min="0" step="0.01" required>
                </td>
                <td>
                    <!-- Delete button will be added dynamically -->
                </td>
            </tr>
        `;
        
        $('#tablaContenedores tbody').append(html);
        
        // Inicializar select2 para el contenedor
        const $select = $('#tablaContenedores tbody tr:last .contenedor-select');
        initContenedorSelect($select);
        
        // Si hay datos, seleccionar el contenedor
        if (datos && datos.unidadMedidaId) {
            // Crear opción y seleccionarla
            const option = new Option(datos.unidadMedidaNombre || 'Contenedor', datos.unidadMedidaId, true, true);
            $select.append(option).trigger('change');
        }
        
        // Guardar datos
        if (datos) {
            window.contenedoresData.push(datos);
        }
        
        // Actualizar botones y notificar cambios
        actualizarBotonesEliminar();
        notificarCambioContenedores();
    };
    
    // Inicializar select de contenedor
    function initContenedorSelect($select) {
        $select.select2({
            theme: 'bootstrap-5',
            placeholder: 'Seleccione un contenedor',
            allowClear: true,
            width: '100%',
            ajax: {
                url: '/UnidadMedida/Buscar',
                dataType: 'json',
                delay: 250,
                data: function(params) {
                    return {
                        term: params.term || ''
                    };
                },
                processResults: function(data) {
                    return {
                        results: data.results || []
                    };
                },
                cache: true
            }
        });
    }
    
    // Actualizar botones de eliminar
    window.actualizarBotonesEliminar = function() {
        // Eliminar todos los botones primero
        $('#tablaContenedores tbody td:last-child').empty();
        
        // Agregar botón solo en la última fila si hay más de una fila
        const filas = $('#tablaContenedores tbody tr');
        if (filas.length > 1) {
            const ultimaFila = filas.last();
            ultimaFila.find('td:last').html('<button type="button" class="btn btn-link p-0 text-danger btn-eliminar"><i class="fas fa-times-circle"></i></button>');
        }
    };
    
    // Botón agregar fila
    $('#btnAgregarFila').click(function() {
        agregarFilaContenedor();
    });
    
    // Eliminar contenedor
    $(document).on('click', '.btn-eliminar', function() {
        const filas = $('#tablaContenedores tbody tr');
        if (filas.length > 1) {
            $(this).closest('tr').remove();
            // Renumerar filas
            $('#tablaContenedores tbody tr').each(function(index) {
                $(this).find('td:first').text(index + 1);
                $(this).attr('data-index', index);
                // Actualizar nombres de campos
                $(this).find('input, select').each(function() {
                    const name = $(this).attr('name');
                    if (name) {
                        const newName = name.replace(/ItemContenedores\[\d+\]/, `ItemContenedores[${index}]`);
                        $(this).attr('name', newName);
                    }
                });
            });
            actualizarBotonesEliminar();
            notificarCambioContenedores();
        }
    });
    
    // Cambio en selector de contenedor
    $(document).on('change', '.contenedor-select', function() {
        const $row = $(this).closest('tr');
        const $etiqueta = $row.find('.etiqueta-input');
        const selectedText = $(this).find('option:selected').text();
        
        if ($row.data('index') === 0) {
            $etiqueta.val(selectedText);
        } else {
            const baseText = $('#tablaContenedores tbody tr:first .contenedor-select option:selected').text();
            $etiqueta.val(selectedText + '/' + baseText);
        }
        
        // Notificar cambio
        notificarCambioContenedores();
    });
    
    // Notificar cambio de contenedores a proveedores
    function notificarCambioContenedores() {
        // Actualizar lista global
        window.contenedoresData = [];
        $('#tablaContenedores tbody tr').each(function() {
            const $row = $(this);
            const value = $row.find('.contenedor-select').val();
            const text = $row.find('.contenedor-select option:selected').text();
            
            if (value) {
                window.contenedoresData.push({
                    value: value,
                    text: text
                });
            }
        });
        
        // Actualizar contenedores en proveedores
        if (window.actualizarContenedoresProveedores) {
            window.actualizarContenedoresProveedores();
        }
    }
    
    // Inicializar select2 para unidad de inventario con búsqueda AJAX
    $('.select2-um').select2({
        theme: 'bootstrap-5',
        width: '100%',
        placeholder: 'Seleccione una unidad',
        allowClear: true,
        ajax: {
            url: '/UnidadMedida/Buscar',
            dataType: 'json',
            delay: 250,
            data: function(params) {
                return {
                    term: params.term || ''
                };
            },
            processResults: function(data) {
                return {
                    results: data.results || []
                };
            },
            cache: true
        }
    });
    
    // Si hay un valor preseleccionado, cargarlo
    const unidadInventarioId = $('#UnidadMedidaInventarioId').val();
    const unidadInventarioText = $('#UnidadMedidaInventarioId option:selected').text();
    if (unidadInventarioId && unidadInventarioText) {
        const option = new Option(unidadInventarioText, unidadInventarioId, true, true);
        $('#UnidadMedidaInventarioId').append(option).trigger('change');
    }
    
    // Cargar contenedores existentes al iniciar
    const itemId = $('#Id').val();
    if (itemId && !window.contenedoresLoaded) {
        window.contenedoresLoaded = true;
        console.log('Cargando contenedores para item:', itemId);
        
        $.ajax({
            url: `/Item/ObtenerContenedores/${itemId}`,
            type: 'GET',
            success: function(data) {
                console.log('Contenedores cargados:', data);
                if (data && data.length > 0) {
                    data.forEach(function(contenedor) {
                        agregarFilaContenedor(contenedor);
                    });
                } else {
                    agregarFilaContenedor();
                }
                actualizarBotonesEliminar();
                notificarCambioContenedores();
            },
            error: function(err) {
                console.error('Error al cargar contenedores:', err);
                agregarFilaContenedor();
                actualizarBotonesEliminar();
            }
        });
    }
});