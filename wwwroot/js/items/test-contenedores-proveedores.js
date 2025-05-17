// Script de prueba para contenedores en proveedores
$(document).ready(function() {
    console.log('=== TEST DE CONTENEDORES EN PROVEEDORES ===');
    
    // Esperar 2 segundos para que todo se cargue
    setTimeout(() => {
        // 1. Verificar que existe la tabla de contenedores
        const contenedoresTable = document.getElementById('contenedores-body');
        console.log('Tabla de contenedores encontrada:', !!contenedoresTable);
        
        if (contenedoresTable) {
            // 2. Contar filas de contenedores
            const contenedorRows = contenedoresTable.querySelectorAll('tr');
            console.log('Número de filas de contenedores:', contenedorRows.length);
            
            // 3. Listar contenedores disponibles
            contenedorRows.forEach((row, index) => {
                const contenedorSelect = row.querySelector('.contenedor-select');
                if (contenedorSelect) {
                    const selectedOption = contenedorSelect.selectedOptions[0];
                    if (selectedOption && selectedOption.value) {
                        console.log(`Contenedor ${index}: ${selectedOption.text} (ID: ${selectedOption.value})`);
                    }
                }
            });
        }
        
        // 4. Verificar que existe la función actualizarContenedoresProveedores
        console.log('Función actualizarContenedoresProveedores existe:', typeof window.actualizarContenedoresProveedores === 'function');
        
        // 5. Verificar que existen selectores de contenedor en proveedores
        const contenedorProveedorSelects = document.querySelectorAll('.select2-contenedor-proveedor');
        console.log('Número de selectores de contenedor en proveedores:', contenedorProveedorSelects.length);
        
        // 6. Verificar opciones en cada selector
        contenedorProveedorSelects.forEach((select, index) => {
            console.log(`Selector ${index}: ${select.options.length} opciones`);
            Array.from(select.options).forEach(option => {
                console.log(`  - ${option.text} (value: ${option.value})`);
            });
        });
        
        // 7. Probar la actualización de contenedores
        if (typeof window.actualizarContenedoresProveedores === 'function') {
            console.log('Ejecutando actualizarContenedoresProveedores()...');
            window.actualizarContenedoresProveedores();
            
            // Esperar un poco y verificar de nuevo
            setTimeout(() => {
                console.log('=== VERIFICACIÓN DESPUÉS DE ACTUALIZAR ===');
                const updatedSelects = document.querySelectorAll('.select2-contenedor-proveedor');
                updatedSelects.forEach((select, index) => {
                    console.log(`Selector ${index} actualizado: ${select.options.length} opciones`);
                    Array.from(select.options).forEach(option => {
                        console.log(`  - ${option.text} (value: ${option.value})`);
                    });
                });
            }, 1000);
        }
    }, 2000);
});

// Función de diagnóstico manual
window.diagnosticarContenedoresProveedores = function() {
    console.log('=== DIAGNÓSTICO MANUAL ===');
    
    // Verificar tabla de contenedores
    const contenedoresTable = document.getElementById('contenedores-body');
    if (!contenedoresTable) {
        console.error('No se encontró la tabla de contenedores (contenedores-body)');
        return;
    }
    
    // Verificar filas y selectores
    const contenedorRows = contenedoresTable.querySelectorAll('tr');
    console.log(`Filas de contenedores: ${contenedorRows.length}`);
    
    contenedorRows.forEach((row, index) => {
        const select = row.querySelector('.contenedor-select');
        if (select) {
            const value = select.value;
            const text = select.selectedOptions[0]?.text;
            console.log(`Contenedor ${index}: ${text} (ID: ${value})`);
        } else {
            console.warn(`Fila ${index}: No se encontró select con clase .contenedor-select`);
        }
    });
    
    // Verificar proveedores
    const proveedorSelects = document.querySelectorAll('.select2-contenedor-proveedor');
    console.log(`Selectores de contenedor en proveedores: ${proveedorSelects.length}`);
    
    // Probar población manual
    if (proveedorSelects.length > 0) {
        console.log('Probando poblar el primer selector de proveedor...');
        const firstSelect = proveedorSelects[0];
        
        // Limpiar y poblar
        $(firstSelect).empty();
        $(firstSelect).append('<option value="">Seleccione contenedor</option>');
        
        contenedorRows.forEach((row, index) => {
            const contenedorSelect = row.querySelector('.contenedor-select');
            if (contenedorSelect && contenedorSelect.value) {
                const option = new Option(
                    contenedorSelect.selectedOptions[0].text,
                    contenedorSelect.value,
                    false,
                    false
                );
                $(firstSelect).append(option);
                console.log(`Agregada opción: ${option.text} (${option.value})`);
            }
        });
        
        // Refrescar Select2
        $(firstSelect).trigger('change');
    }
};

console.log('Script de prueba cargado. Usa diagnosticarContenedoresProveedores() para diagnóstico manual.');