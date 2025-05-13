/**
 * Items Actions Component
 * Handles the initialization and management of item actions
 */

export function initializeActions() {
    console.log('Initializing Items Actions...');
    
    // Initialize action buttons
    const actionButtons = {
        create: document.getElementById('btnCreateItem'),
        edit: document.querySelectorAll('.btn-edit'),
        delete: document.querySelectorAll('.btn-delete'),
        print: document.getElementById('btnPrintItems')
    };
    
    // Store action handlers globally
    window.itemActions = {
        // Create new item
        createItem() {
            window.location.href = '/Item/Create';
        },
        
        // Edit existing item
        editItem(id) {
            window.location.href = `/Item/Edit/${id}`;
        },
        
        // Delete item with confirmation
        deleteItem(id) {
            if (confirm('¿Está seguro de que desea eliminar este item?')) {
                fetch(`/Item/Delete/${id}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                    }
                })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        // Reload DataTable
                        if (window.itemsDataTable) {
                            window.itemsDataTable.ajax.reload();
                        }
                        // Show success message
                        showNotification('Item eliminado exitosamente', 'success');
                    } else {
                        showNotification(data.message || 'Error al eliminar el item', 'error');
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    showNotification('Error al eliminar el item', 'error');
                });
            }
        },
        
        // Print items
        printItems() {
            const filters = window.itemFilters?.getFilters() || {};
            const queryString = new URLSearchParams(filters).toString();
            window.open(`/Item/Print?${queryString}`, '_blank');
        }
    };
    
    // Add event listeners
    if (actionButtons.create) {
        actionButtons.create.addEventListener('click', () => window.itemActions.createItem());
    }
    
    actionButtons.edit.forEach(button => {
        button.addEventListener('click', (e) => {
            const id = e.currentTarget.dataset.id;
            window.itemActions.editItem(id);
        });
    });
    
    actionButtons.delete.forEach(button => {
        button.addEventListener('click', (e) => {
            const id = e.currentTarget.dataset.id;
            window.itemActions.deleteItem(id);
        });
    });
    
    if (actionButtons.print) {
        actionButtons.print.addEventListener('click', () => window.itemActions.printItems());
    }
    
    // Helper function to show notifications
    function showNotification(message, type = 'info') {
        // Check if toast container exists
        let toastContainer = document.querySelector('.toast-container');
        if (!toastContainer) {
            toastContainer = document.createElement('div');
            toastContainer.className = 'toast-container position-fixed bottom-0 end-0 p-3';
            document.body.appendChild(toastContainer);
        }
        
        // Create toast element
        const toast = document.createElement('div');
        toast.className = `toast align-items-center text-white bg-${type} border-0`;
        toast.setAttribute('role', 'alert');
        toast.setAttribute('aria-live', 'assertive');
        toast.setAttribute('aria-atomic', 'true');
        
        toast.innerHTML = `
            <div class="d-flex">
                <div class="toast-body">
                    ${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
        `;
        
        // Add toast to container
        toastContainer.appendChild(toast);
        
        // Initialize and show toast
        const bsToast = new bootstrap.Toast(toast);
        bsToast.show();
        
        // Remove toast after it's hidden
        toast.addEventListener('hidden.bs.toast', () => {
            toast.remove();
        });
    }
    
    console.log('Items Actions initialized successfully');
} 