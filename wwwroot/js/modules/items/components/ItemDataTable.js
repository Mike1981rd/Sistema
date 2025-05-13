/**
 * Items DataTable Component
 * Handles the initialization and management of the items data table
 */

export function initializeDataTable() {
    console.log('Initializing Items DataTable...');
    
    // Check if DataTable element exists
    const tableElement = document.getElementById('itemsTable');
    if (!tableElement) {
        console.warn('Items table element not found');
        return;
    }
    
    // Initialize DataTable with basic configuration
    const dataTable = $('#itemsTable').DataTable({
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json'
        },
        dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>' +
             '<"row"<"col-sm-12"tr>>' +
             '<"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
        responsive: true,
        pageLength: 10,
        ordering: true
    });
    
    // Store DataTable instance globally
    window.itemsDataTable = dataTable;
    
    console.log('Items DataTable initialized successfully');
}

// Helper functions for column rendering
function formatPrice(data, type, row) {
    if (type === 'display') {
        return new Intl.NumberFormat('es-DO', {
            style: 'currency',
            currency: 'DOP'
        }).format(data);
    }
    return data;
}

function formatStock(data, type, row) {
    if (type === 'display') {
        const stockClass = data <= row.stockMinimo ? 'text-danger' : 
                          data <= row.stockMaximo ? 'text-warning' : 'text-success';
        return `<span class="${stockClass}">${data}</span>`;
    }
    return data;
}

function formatStatus(data, type, row) {
    if (type === 'display') {
        const statusClass = data ? 'badge bg-success' : 'badge bg-danger';
        const statusText = data ? 'Activo' : 'Inactivo';
        return `<span class="${statusClass}">${statusText}</span>`;
    }
    return data;
}

function renderActions(data, type, row) {
    if (type === 'display') {
        return `
            <div class="btn-group btn-group-sm">
                <button type="button" class="btn btn-primary btn-edit" data-id="${row.id}">
                    <i class="fas fa-edit"></i>
                </button>
                <button type="button" class="btn btn-danger btn-delete" data-id="${row.id}">
                    <i class="fas fa-trash"></i>
                </button>
            </div>
        `;
    }
    return null;
} 