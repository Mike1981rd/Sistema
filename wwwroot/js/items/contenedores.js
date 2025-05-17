// contenedores.js - Gestión de conversiones de unidades para items
$(document).ready(function() {
    console.log("Inicializando módulo de conversiones de unidades...");
    
    // Botón para agregar un nuevo contenedor
    $(document).on('click', '#btnAgregarContenedor', function() {
        agregarFilaContenedor();
    });
    
    // Eliminar un contenedor
    $(document).on('click', '.btn-eliminar-contenedor', function() {
        eliminarFilaContenedor($(this));
    });
    
    // Cambio en selector de contenedor
    $(document).on('change', '.contenedor-select', function() {
        actualizarEtiqueta($(this));
    });
    
    // Cambio en precio base
    $(document).on('input', '.costo-base', function() {
        recalcularCostos();
    });
    
    // Cambio en cantidad
    $(document).on('input', '.cantidad-input', function() {
        const $row = $(this).closest('tr');
        const index = $row.data('index');
        
        // La primera fila siempre tiene cantidad 1
        if (index === 0) {
            $(this).val(1);
        }
        
        recalcularCostos();
    });
    
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
                           value="${datos ? datos.costo || '' : ''}"
                           min="0" step="0.01"
                           ${!esLaPrimera ? 'readonly' : ''}>
                </td>
                <td>
                    ${!esLaPrimera ? 
                      '<button type="button" class="btn btn-sm btn-danger btn-eliminar-contenedor"><i class="fas fa-trash"></i></button>' 
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
            placeholder: 'Seleccione un contenedor',
            allowClear: true,
            width: '100%',
            ajax: {
                url: '/Contenedor/Buscar',
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
            setTimeout(function() {
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
            
            $lastCell.html('<button type="button" class="btn btn-sm btn-danger btn-eliminar-contenedor"><i class="fas fa-trash"></i></button>');
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
            const id = $row.find('input[name$=".Id"]').val();
            
            if (id) {
                // Crear campo oculto para enviar al servidor
                if ($('#contenedores-eliminados').length === 0) {
                    $('form').append('<div id="contenedores-eliminados" style="display:none;"></div>');
                }
                
                $('#contenedores-eliminados').append(`<input type="hidden" name="ItemContenedoresEliminados" value="${id}">`);
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
        $('#contenedores-body tr').each(function(idx) {
            // Actualizar atributo data-index
            $(this).attr('data-index', idx);
            
            // Actualizar número visible
            $(this).find('td:first').text(idx + 1);
            
            // Actualizar nombres de los campos
            $(this).find('select, input').each(function() {
                const name = $(this).attr('name');
                if (name) {
                    const newName = name.replace(/ItemContenedores\[\d+\]/, `ItemContenedores[${idx}]`);
                    $(this).attr('name', newName);
                }
            });
        });
    }
    
    // Actualizar etiqueta al cambiar contenedor
    function actualizarEtiqueta($select) {
        const $row = $select.closest('tr');
        const index = $row.data('index');
        
        // Obtener nombre del contenedor seleccionado
        const contenedorNombre = $select.find('option:selected').text();
        
        if (index === 0) {
            // Es la unidad base
            $row.find('.etiqueta-input').val(contenedorNombre);
            
            // Actualizar todas las etiquetas
            actualizarTodasLasEtiquetas();
        } else {
            // Es una unidad derivada
            const contenedorBaseNombre = $('#contenedores-body tr[data-index="0"] .contenedor-select option:selected').text();
            
            if (contenedorBaseNombre && contenedorNombre) {
                const etiqueta = `${contenedorNombre}/${contenedorBaseNombre}`;
                $row.find('.etiqueta-input').val(etiqueta);
            }
        }
    }
    
    // Actualizar todas las etiquetas derivadas
    function actualizarTodasLasEtiquetas() {
        $('#contenedores-body tr').each(function() {
            const index = $(this).data('index');
            if (index > 0) {
                const $select = $(this).find('.contenedor-select');
                actualizarEtiqueta($select);
            }
        });
    }
    
    // Recalcular todos los costos derivados
    function recalcularCostos() {
        // Obtener costo y cantidad base
        const $firstRow = $('#contenedores-body tr[data-index="0"]');
        const costoBase = parseFloat($firstRow.find('.costo-base').val()) || 0;
        
        // Actualizar costos de todas las filas derivadas
        $('#contenedores-body tr').each(function() {
            const index = $(this).data('index');
            
            if (index > 0) {
                const cantidad = parseFloat($(this).find('.cantidad-input').val()) || 0;
                const costo = costoBase * cantidad;
                
                $(this).find('.costo-derivado').val(costo.toFixed(2));
            }
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
            success: function(data) {
                // Si hay datos, cargarlos
                if (data && data.length > 0) {
                    data.forEach(function(item) {
                        agregarFilaContenedor(item);
                    });
                } else {
                    // Si no hay datos, agregar una fila vacía
                    agregarFilaContenedor();
                }
            },
            error: function() {
                console.error('Error al cargar conversiones');
                agregarFilaContenedor();
            }
        });
    }
    
    // Iniciar carga de datos
    cargarDatosIniciales();
}); 