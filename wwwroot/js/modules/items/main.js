/**
 * Items Module - Main Entry Point
 * Sistema Contable Aurora
 */

import { initializeDataTable } from './components/ItemDataTable.js';
import { initializeFilters } from './components/ItemFilters.js';
import { initializeActions } from './components/ItemActions.js';
import { initializeExport } from './components/ItemExport.js';
import { formatCurrency, formatDate } from './utils/ItemUtils.js';

// Initialize module when DOM is ready
document.addEventListener('DOMContentLoaded', () => {
    console.log('Initializing Items Module...');
    
    // Initialize components
    initializeDataTable();
    initializeFilters();
    initializeActions();
    initializeExport();
    
    // Add global utilities to window object
    window.itemUtils = {
        formatCurrency,
        formatDate
    };
    
    console.log('Items Module initialized successfully');
}); 