/**
 * Items Export Component
 * Handles the initialization and management of item data export
 */

export function initializeExport() {
    console.log('Initializing Items Export...');
    
    // Initialize export buttons
    const exportButtons = {
        excel: document.getElementById('btnExportExcel'),
        pdf: document.getElementById('btnExportPdf'),
        csv: document.getElementById('btnExportCsv')
    };
    
    // Store export handlers globally
    window.itemExport = {
        // Export to Excel
        exportToExcel() {
            const filters = window.itemFilters?.getFilters() || {};
            const queryString = new URLSearchParams(filters).toString();
            window.location.href = `/Item/ExportExcel?${queryString}`;
        },
        
        // Export to PDF
        exportToPdf() {
            const filters = window.itemFilters?.getFilters() || {};
            const queryString = new URLSearchParams(filters).toString();
            window.open(`/Item/ExportPdf?${queryString}`, '_blank');
        },
        
        // Export to CSV
        exportToCsv() {
            const filters = window.itemFilters?.getFilters() || {};
            const queryString = new URLSearchParams(filters).toString();
            window.location.href = `/Item/ExportCsv?${queryString}`;
        }
    };
    
    // Add event listeners
    if (exportButtons.excel) {
        exportButtons.excel.addEventListener('click', () => window.itemExport.exportToExcel());
    }
    
    if (exportButtons.pdf) {
        exportButtons.pdf.addEventListener('click', () => window.itemExport.exportToPdf());
    }
    
    if (exportButtons.csv) {
        exportButtons.csv.addEventListener('click', () => window.itemExport.exportToCsv());
    }
    
    console.log('Items Export initialized successfully');
} 