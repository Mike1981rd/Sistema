/**
 * Clientes Index Page JS
 * Handles client list table functionality including checkboxes, filtering, and other interactions
 */

$(document).ready(function() {
    // Agregar un contenedor personalizado para mejorar el diseño y solucionar problemas de estilo
    $('#clientes-table').wrap('<div class="custom-datatable-container"></div>');
    
    // Handle "check all" checkbox
    $('#check-all').on('change', function() {
        const isChecked = $(this).prop('checked');
        $('.cliente-check').prop('checked', isChecked);
        updateBulkActions();
    });
    
    // Update "check all" status when individual checkboxes change
    $('.cliente-check').on('change', function() {
        const totalCheckboxes = $('.cliente-check').length;
        const checkedCheckboxes = $('.cliente-check:checked').length;
        
        $('#check-all').prop('checked', totalCheckboxes === checkedCheckboxes);
        updateBulkActions();
    });
    
    // Handle export buttons
    $('#export-excel').on('click', function() {
        exportClientes('excel');
    });
    
    $('#export-pdf').on('click', function() {
        exportClientes('pdf');
    });
    
    $('#export-csv').on('click', function() {
        exportClientes('csv');
    });
    
    // Initialize DataTable for better filtering, sorting, and pagination
    const table = $('#clientes-table').DataTable({
        language: {
            lengthMenu: 'Mostrar _MENU_ registros',
            zeroRecords: 'No se encontraron resultados',
            info: 'Mostrando _START_ a _END_ de _TOTAL_ registros',
            infoEmpty: 'Mostrando 0 a 0 de 0 registros',
            infoFiltered: '(filtrado de _MAX_ registros totales)',
            search: 'Buscar:',
            paginate: {
                first: 'Primero',
                last: 'Último',
                next: 'Siguiente',
                previous: 'Anterior'
            }
        },
        responsive: true,
        // Simplificamos la estructura DOM para evitar problemas de diseño
        dom: 
            "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        ordering: true,
        columnDefs: [
            { orderable: false, targets: [0, 6] }, // Disable sorting for checkbox and actions columns
            { responsivePriority: 1, targets: [1, 2] }, // Prioritize name and ID columns
            { responsivePriority: 2, targets: 6 } // Prioritize actions column
        ],
        pageLength: 10,
        lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "Todos"]],
        initComplete: function() {
            // Hide the default search box since we have our custom one
            $('.dataTables_filter').hide();
            
            // Conectar el buscador personalizado
            $('#searchInput').on('keyup', function() {
                table.search($(this).val()).draw();
            });
            
            // Aplicar estilos a los elementos de DataTables
            $('.dataTables_length select').addClass('form-select form-select-sm');
            $('.dataTables_info').addClass('text-muted small');
            
            // Fix para la longitud de menú
            fixDatatablesUI();
        },
        drawCallback: function() {
            // Aplicar arreglos visuales cada vez que se redibuja la tabla
            fixDatatablesUI();
        }
    });
    
    // Nueva función para corregir problemas de UI en DataTables
    function fixDatatablesUI() {
        // Corregir la etiqueta "Mostrar X registros"
        $('.dataTables_length label').each(function() {
            const html = $(this).html();
            // Solo reemplazar si no se ha modificado ya
            if (html.indexOf('_MENU_') >= 0 || html.indexOf('select') >= 0) {
                const select = $(this).find('select').detach();
                select.addClass('form-select form-select-sm mx-2');
                // Mover el select fuera del label para evitar problemas de z-index
                $(this).html('Mostrar ');
                $(this).after(select);
                select.after(' registros');
            }
        });
        
        // Asegurar que los controles de paginación tengan los estilos correctos
        $('.paginate_button').addClass('page-link');
        
        // Asegurar que la información tenga el estilo correcto
        $('.dataTables_info').addClass('text-muted small');
        
        // Corregir z-index para evitar problemas de superposición
        $('.dataTables_length select').css('z-index', '999');
    }
    
    // Función para actualizar las acciones en masa
    function updateBulkActions() {
        const checkedRows = $('.cliente-check:checked').length;
        
        if (checkedRows > 0) {
            if ($('#bulk-actions').length === 0) {
                const actionsHtml = `
                    <div id="bulk-actions" class="card mt-3">
                        <div class="card-body py-2">
                            <div class="d-flex align-items-center">
                                <span class="me-3"><strong>${checkedRows}</strong> cliente(s) seleccionado(s)</span>
                                <button id="btn-export-selected" class="btn btn-sm btn-outline-secondary me-2">
                                    <i class="fas fa-download me-1"></i> Exportar
                                </button>
                                <button id="btn-delete-selected" class="btn btn-sm btn-outline-danger">
                                    <i class="fas fa-trash-alt me-1"></i> Eliminar
                                </button>
                            </div>
                        </div>
                    </div>
                `;
                
                $(actionsHtml).insertAfter('.table-responsive');
                
                // Configurar eventos para las acciones en masa
                $('#btn-export-selected').on('click', function() {
                    exportSelectedClientes();
                });
                
                $('#btn-delete-selected').on('click', function() {
                    deleteSelectedClientes();
                });
            } else {
                $('#bulk-actions span strong').text(checkedRows);
            }
        } else {
            $('#bulk-actions').remove();
        }
    }
    
    // Función para exportar clientes (simulada)
    function exportClientes(format) {
        console.log(`Exportando todos los clientes en formato ${format}`);
        // Aquí iría la implementación real de exportación
        alert(`Se exportarán todos los clientes en formato ${format}`);
    }
    
    // Función para exportar clientes seleccionados (simulada)
    function exportSelectedClientes() {
        const selectedIds = getSelectedClientIds();
        console.log('Exportando clientes seleccionados:', selectedIds);
        // Aquí iría la implementación real de exportación
        alert(`Se exportarán los clientes seleccionados: ${selectedIds.join(', ')}`);
    }
    
    // Función para eliminar clientes seleccionados (simulada)
    function deleteSelectedClientes() {
        const selectedIds = getSelectedClientIds();
        if (confirm(`¿Está seguro de que desea eliminar ${selectedIds.length} cliente(s)? Esta acción no se puede deshacer.`)) {
            console.log('Eliminando clientes:', selectedIds);
            // Aquí iría la implementación real de eliminación
            alert(`Eliminar: ${selectedIds.join(', ')}`);
        }
    }
    
    // Función para obtener los IDs de los clientes seleccionados
    function getSelectedClientIds() {
        const ids = [];
        $('.cliente-check:checked').each(function() {
            ids.push($(this).val());
        });
        return ids;
    }
}); 