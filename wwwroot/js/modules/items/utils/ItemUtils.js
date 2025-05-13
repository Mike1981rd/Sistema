/**
 * Items Utilities
 * Common utility functions for the items module
 */

// Format currency values
export function formatCurrency(value, currency = 'DOP') {
    if (value === null || value === undefined) {
        return '';
    }
    
    return new Intl.NumberFormat('es-DO', {
        style: 'currency',
        currency: currency
    }).format(value);
}

// Format date values
export function formatDate(date, format = 'short') {
    if (!date) {
        return '';
    }
    
    const dateObj = new Date(date);
    
    if (isNaN(dateObj.getTime())) {
        return '';
    }
    
    const options = {
        short: {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit'
        },
        long: {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        },
        time: {
            hour: '2-digit',
            minute: '2-digit'
        }
    };
    
    return new Intl.DateTimeFormat('es-DO', options[format] || options.short).format(dateObj);
}

// Format stock status
export function formatStockStatus(stock, min, max) {
    if (stock <= min) {
        return {
            text: 'Bajo',
            class: 'text-danger'
        };
    } else if (stock <= max) {
        return {
            text: 'Normal',
            class: 'text-warning'
        };
    } else {
        return {
            text: 'Alto',
            class: 'text-success'
        };
    }
}

// Generate barcode
export function generateBarcode(value, elementId) {
    if (!value || !elementId) {
        return;
    }
    
    try {
        JsBarcode(`#${elementId}`, value, {
            format: 'CODE128',
            width: 2,
            height: 100,
            displayValue: true,
            fontSize: 20,
            font: 'monospace',
            textAlign: 'center',
            textPosition: 'bottom',
            textMargin: 2,
            background: '#ffffff'
        });
    } catch (error) {
        console.error('Error generating barcode:', error);
    }
}

// Validate item data
export function validateItemData(data) {
    const errors = [];
    
    if (!data.codigo) {
        errors.push('El código es requerido');
    }
    
    if (!data.nombre) {
        errors.push('El nombre es requerido');
    }
    
    if (!data.categoriaId) {
        errors.push('La categoría es requerida');
    }
    
    if (data.precioVenta < 0) {
        errors.push('El precio de venta no puede ser negativo');
    }
    
    if (data.stock < 0) {
        errors.push('El stock no puede ser negativo');
    }
    
    return {
        isValid: errors.length === 0,
        errors: errors
    };
}

// Debounce function
export function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Throttle function
export function throttle(func, limit) {
    let inThrottle;
    return function executedFunction(...args) {
        if (!inThrottle) {
            func(...args);
            inThrottle = true;
            setTimeout(() => inThrottle = false, limit);
        }
    };
} 