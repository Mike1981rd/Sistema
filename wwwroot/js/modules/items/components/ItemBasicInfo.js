/**
 * Componente para manejar la información básica del item
 */
export class ItemBasicInfo {
    constructor(container, itemState) {
        this.container = container;
        this.itemState = itemState;
        
        // Referencias a elementos DOM
        this.elements = {
            codigoInput: null,
            codigoBarrasInput: null,
            nombreInput: null,
            categoriaSelect: null,
            marcaSelect: null,
            impuestoSelect: null,
            descripcionInput: null,
            estadoSelect: null
        };
    }
    
    /**
     * Inicializa el componente
     */
    init() {
        this._initializeDOM();
        this._setupFormFields();
        this._setupSelects();
        this._setupEventListeners();
    }
    
    /**
     * Inicializa referencias a elementos del DOM
     * @private
     */
    _initializeDOM() {
        this.elements.codigoInput = this.container.querySelector('#Codigo');
        this.elements.codigoBarrasInput = this.container.querySelector('#CodigoBarras');
        this.elements.nombreInput = this.container.querySelector('#Nombre');
        this.elements.categoriaSelect = this.container.querySelector('#CategoriaId');
        this.elements.marcaSelect = this.container.querySelector('#MarcaId');
        this.elements.impuestoSelect = this.container.querySelector('#ImpuestoId');
        this.elements.descripcionInput = this.container.querySelector('#Descripcion');
        this.elements.estadoSelect = this.container.querySelector('#Estado');
        
        // Verificar elementos críticos
        if (!this.elements.nombreInput || !this.elements.categoriaSelect) {
            console.error('ItemBasicInfo: No se encontraron elementos críticos en el DOM');
        }
    }
    
    /**
     * Configura los campos del formulario
     * @private
     */
    _setupFormFields() {
        // Configurar el campo de código como readonly
        if (this.elements.codigoInput) {
            this.elements.codigoInput.readOnly = true;
            this.elements.codigoInput.classList.add('bg-light');
        }
        
        // Configurar botones de código de barras
        this._setupBarcodeButtons();
    }
    
    /**
     * Configura los selects con datos estáticos temporales
     * @private
     */
    _setupSelects() {
        // Datos estáticos temporales mientras arreglamos los endpoints
        const categorias = [
            {id: 1, text: "Electrónicos"},
            {id: 2, text: "Comestibles"},
            {id: 3, text: "Ropa"}
        ];
        
        const marcas = [
            {id: 1, text: "Genérica"},
            {id: 2, text: "Marca A"},
            {id: 3, text: "Marca B"}
        ];
        
        const impuestos = [
            {id: 1, text: "ITBIS 18%"},
            {id: 2, text: "Exento"}
        ];
        
        // Inicializar select2 para categorías
        if (this.elements.categoriaSelect) {
            $(this.elements.categoriaSelect).select2({
                theme: 'bootstrap-5',
                placeholder: 'Seleccione una categoría',
                allowClear: true,
                width: '100%',
                data: categorias
            });
        }
        
        // Inicializar select2 para marcas
        if (this.elements.marcaSelect) {
            $(this.elements.marcaSelect).select2({
                theme: 'bootstrap-5',
                placeholder: 'Seleccione una marca',
                allowClear: true,
                width: '100%',
                data: marcas
            });
        }
        
        // Inicializar select2 para impuestos
        if (this.elements.impuestoSelect) {
            $(this.elements.impuestoSelect).select2({
                theme: 'bootstrap-5',
                placeholder: 'Seleccione un impuesto',
                allowClear: true,
                width: '100%',
                data: impuestos
            });
        }
        
        // Configurar botones de agregar
        const addCategoriaBtn = this.container.querySelector('.add-categoria');
        const addMarcaBtn = this.container.querySelector('.add-marca');
        
        if (addCategoriaBtn) {
            addCategoriaBtn.addEventListener('click', () => {
                alert('Función para agregar categoría (pendiente implementar)');
            });
        }
        
        if (addMarcaBtn) {
            addMarcaBtn.addEventListener('click', () => {
                alert('Función para agregar marca (pendiente implementar)');
            });
        }
    }
    
    /**
     * Configura los botones de código de barras
     * @private
     */
    _setupBarcodeButtons() {
        const barcodeContainer = this.container.querySelector('.barcode-actions');
        const generateBtn = barcodeContainer?.querySelector('.generate-barcode');
        const printBtn = barcodeContainer?.querySelector('.print-barcode');
        
        if (generateBtn) {
            generateBtn.addEventListener('click', this._handleGenerateBarcode.bind(this));
        }
        
        if (printBtn) {
            printBtn.addEventListener('click', this._handlePrintBarcode.bind(this));
        }
    }
    
    /**
     * Configura los listeners de eventos
     * @private
     */
    _setupEventListeners() {
        // Manejar cambios en el campo de nombre
        if (this.elements.nombreInput) {
            this.elements.nombreInput.addEventListener('change', () => {
                if (this.itemState) {
                    this.itemState.updateItem({
                        nombre: this.elements.nombreInput.value
                    });
                }
            });
        }
        
        // Manejar cambios en el campo de descripción
        if (this.elements.descripcionInput) {
            this.elements.descripcionInput.addEventListener('change', () => {
                if (this.itemState) {
                    this.itemState.updateItem({
                        descripcion: this.elements.descripcionInput.value
                    });
                }
            });
        }
        
        // Manejar cambios en el campo de estado
        if (this.elements.estadoSelect) {
            this.elements.estadoSelect.addEventListener('change', () => {
                if (this.itemState) {
                    this.itemState.updateItem({
                        estado: this.elements.estadoSelect.value === 'true'
                    });
                }
            });
        }
    }
    
    /**
     * Maneja la generación de código de barras
     * @private
     */
    _handleGenerateBarcode() {
        // Verificar si ya existe un código de barras
        if (this.elements.codigoBarrasInput && this.elements.codigoBarrasInput.value.trim() !== '') {
            if (!confirm('Ya existe un código de barras. ¿Desea generar uno nuevo?')) {
                return;
            }
        }
        
        // Lógica para generar código de barras
        $.ajax({
            url: '/Items/GenerarCodigoBarras',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                itemId: this.itemState?.getCurrentState()?.id
            }),
            success: (response) => {
                if (response.success) {
                    // Actualizar campo de código de barras
                    if (this.elements.codigoBarrasInput) {
                        this.elements.codigoBarrasInput.value = response.codigoBarras;
                    }
                    
                    // Actualizar estado
                    if (this.itemState) {
                        this.itemState.updateItem({
                            codigoBarras: response.codigoBarras
                        });
                    }
                    
                    // Mostrar mensaje de éxito
                    alert('Código de barras generado correctamente');
                } else {
                    alert(`Error: ${response.message || 'No se pudo generar el código de barras'}`);
                }
            },
            error: () => {
                alert('Error al generar el código de barras');
            }
        });
    }
    
    /**
     * Maneja la impresión de código de barras
     * @private
     */
    _handlePrintBarcode() {
        // Verificar que exista un código de barras
        if (!this.elements.codigoBarrasInput || this.elements.codigoBarrasInput.value.trim() === '') {
            alert('Primero debe generar un código de barras');
            return;
        }
        
        // Mostrar modal de opciones de impresión
        this._showPrintOptions();
    }
    
    /**
     * Muestra las opciones de impresión de código de barras
     * @private
     */
    _showPrintOptions() {
        // Opciones posibles
        const options = [
            { value: '2x2', text: '2x2 - 4 etiquetas por página' },
            { value: '2x3', text: '2x3 - 6 etiquetas por página' },
            { value: '3x2', text: '3x2 - 6 etiquetas por página' }
        ];
        
        // Crear modal con opciones
        const modalHtml = `
        <div class="modal fade" id="modalPrintOptions" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Opciones de impresión</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="mb-3">
                            <label for="printFormat" class="form-label">Formato de impresión</label>
                            <select id="printFormat" class="form-select">
                                ${options.map(opt => `<option value="${opt.value}">${opt.text}</option>`).join('')}
                            </select>
                        </div>
                        <div class="mb-3">
                            <label for="printQuantity" class="form-label">Cantidad de etiquetas</label>
                            <input type="number" id="printQuantity" class="form-control" min="1" value="1">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <button type="button" class="btn btn-primary" id="btnPrint">Imprimir</button>
                    </div>
                </div>
            </div>
        </div>
        `;
        
        // Añadir modal al DOM
        const modalElement = document.createElement('div');
        modalElement.innerHTML = modalHtml;
        document.body.appendChild(modalElement);
        
        // Mostrar modal
        const modal = new bootstrap.Modal(document.getElementById('modalPrintOptions'));
        modal.show();
        
        // Configurar botón de impresión
        document.getElementById('btnPrint').addEventListener('click', () => {
            const format = document.getElementById('printFormat').value;
            const quantity = document.getElementById('printQuantity').value;
            
            // Cerrar modal
            modal.hide();
            
            // Ejecutar impresión
            this._printBarcode(format, quantity);
            
            // Eliminar modal del DOM después de cerrarlo
            document.getElementById('modalPrintOptions').addEventListener('hidden.bs.modal', function () {
                this.remove();
            });
        });
    }
    
    /**
     * Ejecuta la impresión de código de barras
     * @param {string} format - Formato de impresión (2x2, 2x3, 3x2)
     * @param {number} quantity - Cantidad de etiquetas
     * @private
     */
    _printBarcode(format, quantity) {
        const codigoBarras = this.elements.codigoBarrasInput.value;
        const itemNombre = this.elements.nombreInput.value;
        
        // Crear y enviar formulario para imprimir
        const form = document.createElement('form');
        form.method = 'POST';
        form.action = '/Items/ImprimirCodigoBarras';
        form.target = '_blank'; // Abrir en nueva pestaña
        
        // Añadir campos ocultos
        const addHiddenField = (name, value) => {
            const input = document.createElement('input');
            input.type = 'hidden';
            input.name = name;
            input.value = value;
            form.appendChild(input);
        };
        
        addHiddenField('codigoBarras', codigoBarras);
        addHiddenField('nombre', itemNombre);
        addHiddenField('formato', format);
        addHiddenField('cantidad', quantity);
        
        // Añadir ID del item si existe
        if (this.itemState?.getCurrentState()?.id) {
            addHiddenField('id', this.itemState.getCurrentState().id);
        }
        
        // Añadir formulario al DOM, enviarlo y eliminarlo
        document.body.appendChild(form);
        form.submit();
        document.body.removeChild(form);
    }
    
    /**
     * Actualiza el componente con datos del estado global
     */
    updateFromState() {
        const state = this.itemState?.getCurrentState();
        if (!state) return;
        
        // Actualizar campos de texto
        if (this.elements.codigoInput && state.codigo) {
            this.elements.codigoInput.value = state.codigo;
        }
        
        if (this.elements.codigoBarrasInput && state.codigoBarras) {
            this.elements.codigoBarrasInput.value = state.codigoBarras;
        }
        
        if (this.elements.nombreInput && state.nombre) {
            this.elements.nombreInput.value = state.nombre;
        }
        
        if (this.elements.descripcionInput && state.descripcion) {
            this.elements.descripcionInput.value = state.descripcion;
        }
        
        // Actualizar selects
        if (this.elements.categoriaSelect && state.categoriaId) {
            // Buscar si ya existe la opción
            const option = document.querySelector(`#${this.elements.categoriaSelect.id} option[value="${state.categoriaId}"]`);
            
            if (!option && state.categoriaNombre) {
                // Si no existe la opción, crearla
                const newOption = new Option(state.categoriaNombre, state.categoriaId, true, true);
                $(this.elements.categoriaSelect).append(newOption);
            }
            
            // Actualizar select2
            $(this.elements.categoriaSelect).val(state.categoriaId).trigger('change');
        }
        
        if (this.elements.marcaSelect && state.marcaId) {
            // Similar a categoría
            const option = document.querySelector(`#${this.elements.marcaSelect.id} option[value="${state.marcaId}"]`);
            
            if (!option && state.marcaNombre) {
                const newOption = new Option(state.marcaNombre, state.marcaId, true, true);
                $(this.elements.marcaSelect).append(newOption);
            }
            
            $(this.elements.marcaSelect).val(state.marcaId).trigger('change');
        }
        
        if (this.elements.impuestoSelect && state.impuestoId) {
            $(this.elements.impuestoSelect).val(state.impuestoId).trigger('change');
        }
        
        if (this.elements.estadoSelect && state.estado !== undefined) {
            this.elements.estadoSelect.value = state.estado.toString();
        }
    }
    
    /**
     * Muestra un error de validación para un campo específico
     * @param {string} field - Nombre del campo
     * @param {string} errorMessage - Mensaje de error
     */
    showFieldError(field, errorMessage) {
        // Obtener el elemento correspondiente al campo
        let element = null;
        
        switch (field) {
            case 'Nombre':
                element = this.elements.nombreInput;
                break;
            case 'CategoriaId':
                element = this.elements.categoriaSelect;
                break;
            case 'MarcaId':
                element = this.elements.marcaSelect;
                break;
            case 'ImpuestoId':
                element = this.elements.impuestoSelect;
                break;
            case 'Descripcion':
                element = this.elements.descripcionInput;
                break;
            default:
                // Buscar dinámicamente si no es uno de los conocidos
                element = this.container.querySelector(`#${field}`);
        }
        
        if (element) {
            // Añadir clase de error
            element.classList.add('is-invalid');
            
            // Verificar si ya existe un mensaje de error
            let errorElement = element.nextElementSibling;
            if (!errorElement || !errorElement.classList.contains('invalid-feedback')) {
                // Crear elemento para el mensaje
                errorElement = document.createElement('div');
                errorElement.classList.add('invalid-feedback');
                element.parentNode.insertBefore(errorElement, element.nextSibling);
            }
            
            // Actualizar mensaje
            errorElement.textContent = errorMessage;
        }
    }
    
    /**
     * Limpia los errores de validación de todos los campos
     */
    clearFieldErrors() {
        // Limpiar todos los campos con errores
        const invalidFields = this.container.querySelectorAll('.is-invalid');
        invalidFields.forEach(field => {
            field.classList.remove('is-invalid');
            
            // Eliminar mensaje de error
            const errorElement = field.nextElementSibling;
            if (errorElement && errorElement.classList.contains('invalid-feedback')) {
                errorElement.textContent = '';
            }
        });
    }
} 