// Botón para agregar proveedor
$('#btnAgregarProveedor').on('click', function() {
    agregarProveedor();
});

// Botón para eliminar proveedor
$(document).on('click', '.btn-eliminar-proveedor', function() {
    // Si solo hay un proveedor, no permitir eliminarlo
    if ($('.proveedor-item').length <= 1) {
        Swal.fire({
            icon: 'warning',
            title: 'Operación no permitida',
            text: 'Debe mantener al menos un proveedor',
            confirmButtonText: 'Entendido'
        });
        return;
    }
    
    const $item = $(this).closest('.proveedor-item');
    
    // Confirmar eliminación
    Swal.fire({
        title: '¿Está seguro?',
        text: "Se eliminará este proveedor",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Eliminar item
            $item.remove();
            
            // Renumerar proveedores
            renumerarProveedores();
        }
    });
});

// Selección de proveedor
$(document).on('change', '.select2-proveedor', function() {
    const $item = $(this).closest('.proveedor-item');
    const proveedorId = $(this).val();
    
    if (proveedorId) {
        // Cargar datos del proveedor
        const proveedorTexto = $(this).find('option:selected').text();
        $item.find('.nombre-compra').val(proveedorTexto);
        
        // Cargar más datos si es necesario
        cargarDatosProveedor(proveedorId, $item);
    }
});

// Cambios en precio o factor para calcular precio unitario
$(document).on('input', '.precio-compra, .factor-conversion', function() {
    const $item = $(this).closest('.proveedor-item');
    calcularPrecioUnitario($item);
});

// Botón para crear nuevo proveedor
$(document).on('click', '.btn-nuevo-proveedor', function() {
    const $item = $(this).closest('.proveedor-item');
    crearProveedor($item);
});

// Checkbox de proveedor principal
$(document).on('change', '.proveedor-principal', function() {
    if ($(this).is(':checked')) {
        // Desmarcar otros
        $('.proveedor-principal').not(this).prop('checked', false);
    }
});

// Change en unidad de medida para actualizar texto
$(document).on('change', '.select2-contenedor-compra', function() {
    const $item = $(this).closest('.proveedor-item');
    const unidadTexto = $(this).find('option:selected').text();
    
    if (unidadTexto) {
        // Extraer solo el nombre (sin abreviatura)
        let textoDisplay = unidadTexto;
        if (textoDisplay.includes('(')) {
            textoDisplay = textoDisplay.substring(0, textoDisplay.indexOf('(')).trim();
        }
        
        $item.find('.unidad-medida-texto').text(textoDisplay);
        
        // Recalcular precio unitario
        calcularPrecioUnitario($item);
    }
});
