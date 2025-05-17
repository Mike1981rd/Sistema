// Funcionalidad mejorada para manejo de proveedores (versión simple)
$(document).ready(function() {
    let proveedores = [];
    let proveedorCount = 0;
    
    const btnAgregarProveedor = document.getElementById('btnAgregarProveedor');
    const selectProveedor = document.getElementById('ProveedorId');
    const tablaProveedores = document.getElementById('proveedores-body');
    const sinProveedores = document.getElementById('sinProveedores');
    
    // Esperar a que Select2 se inicialice
    setTimeout(() => {
        // Habilitar botón agregar cuando se seleccione un proveedor
        $('#ProveedorId').on('select2:select', function(e) {
            btnAgregarProveedor.disabled = false;
        });
        
        $('#ProveedorId').on('select2:clear', function(e) {
            btnAgregarProveedor.disabled = true;
        });
        
        // Agregar proveedor a la tabla
        btnAgregarProveedor.addEventListener('click', function() {
            console.log('Botón agregar clickeado');
            
            // Obtener el valor del select2
            const proveedorId = $('#ProveedorId').val();
            const proveedorData = $('#ProveedorId').select2('data');
            const proveedorText = proveedorData && proveedorData[0] ? proveedorData[0].text : '';
            
            console.log('Proveedor ID:', proveedorId, 'Texto:', proveedorText);
            
            if (!proveedorId) return;
            
            // Verificar si el proveedor ya está agregado
            if (proveedores.find(p => p.id === proveedorId)) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Proveedor duplicado',
                    text: 'Este proveedor ya ha sido agregado',
                    toast: true,
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 3000
                });
                return;
            }
            
            // Ocultar mensaje de sin proveedores
            sinProveedores.style.display = 'none';
            
            // Crear nueva fila
            const newRow = document.createElement('tr');
            const isPrincipal = proveedores.length === 0;
            const rowId = 'proveedor-' + proveedorCount++;
            
            newRow.id = rowId;
            newRow.innerHTML = `
                <td class="text-center">
                    <input type="radio" name="proveedorPrincipal" value="${proveedorId}" 
                           ${isPrincipal ? 'checked' : ''} class="form-check-input" />
                </td>
                <td>
                    ${proveedorText}
                    <input type="hidden" name="Proveedores[${proveedores.length}].ProveedorId" value="${proveedorId}" />
                    <input type="hidden" name="Proveedores[${proveedores.length}].EsPrincipal" value="${isPrincipal}" class="es-principal" />
                    <input type="hidden" name="Proveedores[${proveedores.length}].NombreCompra" value="${proveedorText}" />
                    <input type="hidden" name="Proveedores[${proveedores.length}].PrecioCompra" value="0.01" />
                    <input type="hidden" name="Proveedores[${proveedores.length}].FactorConversion" value="1" />
                    <!-- NO incluir UnidadMedidaCompraId si no se está usando -->
                </td>
                <td>
                    <span class="badge bg-${isPrincipal ? 'success' : 'secondary'}">
                        ${isPrincipal ? 'Principal' : 'Secundario'}
                    </span>
                </td>
                <td class="text-center">
                    <button type="button" class="btn btn-sm btn-danger" onclick="eliminarProveedor('${rowId}', '${proveedorId}')">
                        <i class="fas fa-trash"></i>
                    </button>
                </td>
            `;
            
            tablaProveedores.appendChild(newRow);
            
            // Agregar proveedor al array
            proveedores.push({
                id: proveedorId,
                nombre: proveedorText,
                isPrincipal: isPrincipal
            });
            
            // Limpiar select
            $('#ProveedorId').val(null).trigger('change');
            btnAgregarProveedor.disabled = true;
            
            console.log('Proveedor agregado exitosamente');
            
            // Actualizar estados cuando se cambie el radio button
            updateRadioListeners();
        });
    }, 500);
    
    // Función para actualizar los listeners de radio buttons
    function updateRadioListeners() {
        const radios = document.querySelectorAll('input[name="proveedorPrincipal"]');
        radios.forEach(radio => {
            radio.addEventListener('change', function() {
                if (this.checked) {
                    // Actualizar todos los badges y valores hidden
                    document.querySelectorAll('#proveedores-body tr').forEach(row => {
                        const isPrincipalInput = row.querySelector('.es-principal');
                        const badge = row.querySelector('.badge');
                        const isChecked = row.querySelector('input[type="radio"]').checked;
                        
                        if (isPrincipalInput) {
                            isPrincipalInput.value = isChecked;
                        }
                        
                        if (badge) {
                            badge.className = 'badge bg-' + (isChecked ? 'success' : 'secondary');
                            badge.textContent = isChecked ? 'Principal' : 'Secundario';
                        }
                    });
                    
                    // Actualizar array de proveedores
                    proveedores.forEach(p => {
                        p.isPrincipal = p.id === this.value;
                    });
                }
            });
        });
    }
    
    // Función para reindexar los campos hidden después de eliminar
    function reindexarProveedores() {
        const rows = document.querySelectorAll('#proveedores-body tr');
        rows.forEach((row, index) => {
            const hiddenInputs = row.querySelectorAll('input[type="hidden"]');
            hiddenInputs.forEach(input => {
                // Actualizar el índice en el nombre del campo
                input.name = input.name.replace(/\[\d+\]/, `[${index}]`);
            });
        });
    }
    
    // Función global para eliminar proveedor
    window.eliminarProveedor = function(rowId, proveedorId) {
        Swal.fire({
            title: '¿Eliminar proveedor?',
            text: "¿Está seguro de que desea eliminar este proveedor?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                // Eliminar de la tabla
                const row = document.getElementById(rowId);
                const wasPrincipal = row.querySelector('input[type="radio"]').checked;
                row.remove();
                
                // Eliminar del array
                proveedores = proveedores.filter(p => p.id !== proveedorId);
                
                // Si se eliminó el principal y quedan otros, hacer al primero principal
                if (wasPrincipal && proveedores.length > 0) {
                    const firstRadio = document.querySelector('input[name="proveedorPrincipal"]');
                    if (firstRadio) {
                        firstRadio.checked = true;
                        firstRadio.dispatchEvent(new Event('change'));
                    }
                }
                
                // Si no quedan proveedores, mostrar mensaje
                if (proveedores.length === 0) {
                    sinProveedores.style.display = '';
                }
                
                // Re-indexar los inputs
                reindexarProveedores();
                
                Swal.fire({
                    icon: 'success',
                    title: 'Eliminado',
                    text: 'El proveedor ha sido eliminado',
                    toast: true,
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 3000
                });
            }
        });
    };
});