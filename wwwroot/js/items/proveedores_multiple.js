document.addEventListener('DOMContentLoaded', function() {
    let proveedores = [];
    let proveedorCount = 0;
    
    const btnAgregarProveedor = document.getElementById('btnAgregarProveedor');
    const selectProveedor = document.getElementById('ProveedorId');
    const tablaProveedores = document.getElementById('proveedores-body');
    const sinProveedores = document.getElementById('sinProveedores');
    
    // Esperar un momento para que select2 se inicialice
    setTimeout(() => {
        // Habilitar botón agregar cuando se seleccione un proveedor
        $('#ProveedorId').on('select2:select', function(e) {
            btnAgregarProveedor.disabled = false;
        });
        
        $('#ProveedorId').on('select2:clear', function(e) {
            btnAgregarProveedor.disabled = true;
        });
        
        $('#ProveedorId').on('select2:unselect', function(e) {
            btnAgregarProveedor.disabled = true;
        });
        
        // También escuchar el evento change normal
        $('#ProveedorId').on('change', function() {
            console.log('Change event triggered', this.value);
            btnAgregarProveedor.disabled = !this.value;
        });
        
        // Verificar estado inicial
        if (selectProveedor.value) {
            btnAgregarProveedor.disabled = false;
        }
        
        // Agregar proveedor a la tabla - MOVIDO DENTRO DEL TIMEOUT
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
    }, 500); // Cerrar setTimeout
    
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
                updateInputIndexes();
                
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
    
    // Función para re-indexar los inputs después de eliminar
    function updateInputIndexes() {
        const rows = document.querySelectorAll('#proveedores-body tr:not(#sinProveedores)');
        rows.forEach((row, index) => {
            const inputs = row.querySelectorAll('input[type="hidden"]');
            inputs.forEach(input => {
                const name = input.name;
                if (name.includes('Proveedores[')) {
                    input.name = name.replace(/Proveedores\[\d+\]/, `Proveedores[${index}]`);
                }
            });
        });
    }
    
    // Función para inicializar con datos existentes (para el modo edición)
    window.initProveedoresWithData = function(proveedoresExistentes) {
        console.log('Inicializando proveedores con datos existentes:', proveedoresExistentes);
        
        if (!proveedoresExistentes || proveedoresExistentes.length === 0) return;
        
        // Ocultar mensaje de sin proveedores
        if (sinProveedores) {
            sinProveedores.style.display = 'none';
        }
        
        // Limpiar tabla existente (excepto el mensaje de sin proveedores)
        const filas = tablaProveedores.querySelectorAll('tr:not(#sinProveedores)');
        filas.forEach(fila => fila.remove());
        
        // Resetear arrays
        proveedores = [];
        proveedorCount = 0;
        
        // Agregar cada proveedor existente
        proveedoresExistentes.forEach((proveedor, index) => {
            const rowId = 'proveedor-' + proveedorCount++;
            const newRow = document.createElement('tr');
            
            newRow.id = rowId;
            newRow.innerHTML = `
                <td class="text-center">
                    <input type="radio" name="proveedorPrincipal" value="${proveedor.proveedorId}" 
                           ${proveedor.esPrincipal ? 'checked' : ''} class="form-check-input" />
                </td>
                <td>
                    ${proveedor.proveedorNombre || 'Proveedor ' + proveedor.proveedorId}
                    <input type="hidden" name="Proveedores[${index}].ProveedorId" value="${proveedor.proveedorId}" />
                    <input type="hidden" name="Proveedores[${index}].EsPrincipal" value="${proveedor.esPrincipal}" class="es-principal" />
                    <input type="hidden" name="Proveedores[${index}].NombreCompra" value="${proveedor.nombreCompra || ''}" />
                    <input type="hidden" name="Proveedores[${index}].CodigoProveedor" value="${proveedor.codigoProveedor || ''}" />
                    <input type="hidden" name="Proveedores[${index}].PrecioCompra" value="${proveedor.precioCompra || 0}" />
                    <input type="hidden" name="Proveedores[${index}].UnidadMedidaCompraId" value="${proveedor.unidadMedidaCompraId || ''}" />
                    <input type="hidden" name="Proveedores[${index}].FactorConversion" value="${proveedor.factorConversion || 1}" />
                </td>
                <td>
                    <span class="badge bg-${proveedor.esPrincipal ? 'success' : 'secondary'}">
                        ${proveedor.esPrincipal ? 'Principal' : 'Secundario'}
                    </span>
                </td>
                <td class="text-center">
                    <button type="button" class="btn btn-sm btn-danger" onclick="eliminarProveedor('${rowId}', '${proveedor.proveedorId}')">
                        <i class="fas fa-trash"></i>
                    </button>
                </td>
            `;
            
            tablaProveedores.appendChild(newRow);
            
            // Agregar al array interno
            proveedores.push({
                id: proveedor.proveedorId,
                nombre: proveedor.proveedorNombre,
                isPrincipal: proveedor.esPrincipal
            });
        });
        
        // Actualizar listeners de radio buttons
        updateRadioListeners();
    };
    
    // Hacer la función disponible globalmente también
    window.initializeProveedoresModule = function() {
        console.log('initializeProveedoresModule llamado');
    };
});