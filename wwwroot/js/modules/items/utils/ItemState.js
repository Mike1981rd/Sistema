/**
 * Gestión de estado del ítem
 * Centraliza el estado del ítem y notifica a los componentes sobre cambios
 */

/**
 * Clase para gestionar el estado global del item
 */
export class ItemState {
    constructor(initialState = {}) {
        // Estado actual del item
        this.state = this._getInitialState();
        
        // Sobrescribir con estado inicial proporcionado
        this.updateItem(initialState);
        
        // Callbacks para notificar cambios
        this.callbacks = [];
    }
    
    /**
     * Devuelve la estructura inicial del estado
     * @return {Object} Estado inicial
     * @private
     */
    _getInitialState() {
        return {
            // Identificación
            id: null,
            codigo: '',
            codigoBarras: '',
            
            // Información básica
            nombre: '',
            descripcion: '',
            estado: true,
            
            // Categorización
            categoriaId: null,
            categoriaNombre: '',
            marcaId: null,
            marcaNombre: '',
            impuestoId: null,
            impuestoNombre: '',
            
            // Inventario
            stockActual: 0,
            stockMinimo: 0,
            stockMaximo: 0,
            unidadMedidaId: null,
            unidadMedidaNombre: '',
            ubicacionId: null,
            ubicacionNombre: '',
            almacenId: null,
            almacenNombre: '',
            
            // Precios
            precioCosto: 0,
            precioVenta: 0,
            precioMayorista: 0,
            utilidadPorcentaje: 0,
            
            // Contabilidad
            cuentaVentaId: null,
            cuentaVentaNombre: '',
            cuentaCompraId: null,
            cuentaCompraNombre: '',
            cuentaInventarioId: null,
            cuentaInventarioNombre: ''
        };
    }
    
    /**
     * Registra un callback para ser notificado de cambios en el estado
     * @param {Function} callback - Función a ejecutar cuando cambie el estado
     * @return {number} Identificador del callback para poder eliminarlo
     */
    subscribe(callback) {
        this.callbacks.push(callback);
        return this.callbacks.length - 1;
    }
    
    /**
     * Elimina un callback de notificación
     * @param {number} id - Identificador del callback a eliminar
     */
    unsubscribe(id) {
        this.callbacks[id] = null;
    }
    
    /**
     * Notifica a todos los callbacks sobre cambios en el estado
     * @param {Object} changes - Cambios realizados en esta actualización
     * @private
     */
    _notifyChanges(changes) {
        this.callbacks.forEach(callback => {
            if (callback) callback(this.state, changes);
        });
    }
    
    /**
     * Actualiza el estado del item con nuevos valores
     * @param {Object} newValues - Nuevos valores para actualizar el estado
     */
    updateItem(newValues) {
        if (!newValues) return;
        
        // Hacer copia para notificar cambios
        const changes = {};
        
        // Actualizar solo los campos que vienen en newValues
        for (const [key, value] of Object.entries(newValues)) {
            if (this.state.hasOwnProperty(key) && this.state[key] !== value) {
                changes[key] = value;
                this.state[key] = value;
            }
        }
        
        // Si hubo cambios, notificar
        if (Object.keys(changes).length > 0) {
            this._notifyChanges(changes);
        }
    }
    
    /**
     * Obtiene el estado actual completo
     * @return {Object} Estado actual
     */
    getCurrentState() {
        return {...this.state};
    }
    
    /**
     * Restablece el estado a sus valores iniciales
     */
    resetState() {
        const oldState = this.state;
        this.state = this._getInitialState();
        this._notifyChanges(this.state);
    }
}

/**
 * Inicializa el estado del item
 * @param {Object} initialState - Estado inicial opcional
 * @return {ItemState} Instancia del gestor de estado
 */
export function initItemState(initialState = {}) {
    // Intentar obtener datos del servidor si estamos en modo edición
    const urlParams = new URLSearchParams(window.location.search);
    const itemId = urlParams.get('id');
    
    // Si tenemos un ID pero no hay datos iniciales, cargar del servidor
    if (itemId && !initialState.id) {
        const stateManager = new ItemState({id: itemId});
        
        // Cargar datos del servidor
        fetch(`/Items/ObtenerDatos?id=${itemId}`)
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    stateManager.updateItem(data.item);
                }
            })
            .catch(error => {
                console.error('Error al cargar datos del item:', error);
            });
            
        return stateManager;
    }
    
    // Si ya tenemos datos o es nuevo item, usar valores proporcionados
    return new ItemState(initialState);
} 