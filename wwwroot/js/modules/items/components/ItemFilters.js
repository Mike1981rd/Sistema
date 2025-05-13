/**
 * Items Filters Component
 * Handles the initialization and management of item filters
 */

export function initializeFilters() {
    console.log('Initializing Items Filters...');
    
    // Initialize filter elements
    const filterElements = {
        search: document.getElementById('filterSearch'),
        categoria: document.getElementById('filterCategoria'),
        estado: document.getElementById('filterEstado'),
        stock: document.getElementById('filterStock')
    };
    
    // Store filter elements globally
    window.itemFilters = {
        elements: filterElements,
        
        // Get current filter values
        getFilters() {
            return {
                search: filterElements.search?.value || '',
                categoria: filterElements.categoria?.value || '',
                estado: filterElements.estado?.value || '',
                stock: filterElements.stock?.value || ''
            };
        },
        
        // Apply filters to DataTable
        applyFilters() {
            if (window.itemsDataTable) {
                window.itemsDataTable.ajax.reload();
            }
        },
        
        // Reset all filters
        resetFilters() {
            Object.values(filterElements).forEach(element => {
                if (element) {
                    element.value = '';
                }
            });
            this.applyFilters();
        }
    };
    
    // Add event listeners
    Object.values(filterElements).forEach(element => {
        if (element) {
            element.addEventListener('change', () => window.itemFilters.applyFilters());
        }
    });
    
    // Initialize search with debounce
    if (filterElements.search) {
        let searchTimeout;
        filterElements.search.addEventListener('input', (e) => {
            clearTimeout(searchTimeout);
            searchTimeout = setTimeout(() => {
                window.itemFilters.applyFilters();
            }, 500);
        });
    }
    
    // Initialize reset button
    const resetButton = document.getElementById('resetFilters');
    if (resetButton) {
        resetButton.addEventListener('click', () => window.itemFilters.resetFilters());
    }
    
    console.log('Items Filters initialized successfully');
} 