// proveedor_select.js - Módulo para la funcionalidad del select2 de proveedores
import { Select2OffCanvas } from '/js/modules/items/components/Select2OffCanvas.js';

document.addEventListener('DOMContentLoaded', function() {
    new Select2OffCanvas({
        selectId: 'ProveedorId',
        offCanvasId: 'offcanvasProveedor',
        searchUrl: '/Clientes/Buscar?tipoCliente=proveedor',
        createUrl: '/Clientes/CreatePartial',
        updateUrl: '/Clientes/EditPartial',
        detailUrl: '/Clientes/GetDetails',
        entityName: 'proveedor'
        // Puedes agregar onItemSelected, onItemSaved, etc. si lo necesitas
    });
}); 