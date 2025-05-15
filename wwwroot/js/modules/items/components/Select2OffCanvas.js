/**
 * Select2OffCanvas - Componente para manejar select2 con integración de offcanvas
 * Gestiona la funcionalidad de select2 con opciones para crear/editar mediante offcanvas
 */
export class Select2OffCanvas {
    /**
     * Constructor
     * @param {Object} config - Configuración del componente
     * @param {string} config.selectId - ID del elemento select
     * @param {string} config.offCanvasId - ID del offcanvas (sin #)
     * @param {string} config.searchUrl - URL para búsqueda AJAX
     * @param {string} config.createUrl - URL para creación
     * @param {string} config.updateUrl - URL para actualización
     * @param {string} config.entityName - Nombre de la entidad (ej: "categoría")
     * @param {Function} config.onItemSelected - Callback cuando se selecciona un item
     */
    constructor(config) {
        this.config = {
            placeholder: `Seleccione una ${config.entityName}`,
            allowClear: true,
            ...config
        };
        
        this.select = document.getElementById(config.selectId);
        this.offCanvas = document.getElementById(config.offCanvasId);
        this.offCanvasBS = null;
        
        if (!this.select || !this.offCanvas) {
            console.error(`Select2OffCanvas: No se encontró el select #${config.selectId} o el offcanvas #${config.offCanvasId}`);
            return;
        }
        
        this.init();
    }
    
    /**
     * Inicializa el componente
     */
    init() {
        // Aplicar estilos al offcanvas
        this.offCanvas.classList.remove('offcanvas-sm', 'offcanvas-md', 'offcanvas-lg');
        this.offCanvas.style.width = '800px';  // Ancho fijo más amplio
        
        const header = this.offCanvas.querySelector('.offcanvas-header');
        if (header) {
            header.style.backgroundColor = '#3944BC';
            header.style.color = 'white';
            
            // Asegurar que el título sea blanco
            const title = header.querySelector('.offcanvas-title, h5');
            if (title) {
                title.classList.add('text-white');
            }
            
            // Asegurar que el botón de cerrar sea visible
            const closeBtn = header.querySelector('.btn-close');
            if (closeBtn) {
                closeBtn.classList.add('btn-close-white');
            }
        }
        
        // Inicializar offcanvas
        this.offCanvasBS = new bootstrap.Offcanvas(this.offCanvas);
        
        // Inicializar select2
        $(this.select).select2({
            theme: 'bootstrap-5',
            placeholder: this.config.placeholder,
            allowClear: this.config.allowClear,
            width: '100%',
            dropdownParent: $('body'),
            ajax: {
                url: this.config.searchUrl,
                dataType: 'json',
                delay: 250,
                data: (params) => {
                    return {
                        term: params.term || ''
                    };
                },
                processResults: (data) => {
                    // Añadir opción para crear nuevo item si la búsqueda no coincide
                    const results = data.results || [];
                    
                    if (results.length === 0 && this._term && this._term.trim() !== '') {
                        results.push({
                            id: 'new',
                            text: `Crear ${this.config.entityName}: "${this._term}"`,
                            term: this._term
                        });
                    }
                    
                    return {
                        results: results
                    };
                },
                cache: true
            },
            templateResult: this._formatOptionResult.bind(this),
            templateSelection: this._formatOptionSelection.bind(this)
        }).on('select2:selecting', (e) => {
            // Guardar el término de búsqueda
            this._term = $(this.select).data('select2')?.dropdown?.$search?.val() || '';
        }).on('select2:select', (e) => {
            const data = e.params.data;
            
            // Si se selecciona la opción "Crear nuevo"
            if (data.id === 'new') {
                // Eliminar la selección actual
                $(this.select).val(null).trigger('change');
                
                // Preparar y abrir el offcanvas para creación
                this._prepareOffCanvasForCreation(data.term);
            } else {
                // Llamar al callback si existe
                if (typeof this.config.onItemSelected === 'function') {
                    this.config.onItemSelected(data);
                }
            }
        });
        
        // Configurar los listeners para los botones de edición
        this._setupEditListeners();
        
        // Configurar el formulario dentro del offcanvas
        this._setupOffCanvasForm();
    }
    
    /**
     * Formatea la visualización de opciones en el dropdown
     * @private
     */
    _formatOptionResult(option) {
        if (option.loading) {
            return option.text;
        }
        
        if (option.id === 'new') {
            return $('<div class="select2-result-option">' +
                     '<div class="select2-result-option__action"><i class="fas fa-plus-circle text-success me-1"></i> ' + 
                     option.text + '</div>' +
                     '</div>');
        }
        
        return $('<div class="select2-result-option">' +
                 '<div class="select2-result-option__name">' + option.text + '</div>' +
                 '</div>');
    }
    
    /**
     * Formatea la visualización de la opción seleccionada
     * @private
     */
    _formatOptionSelection(option) {
        if (!option.id || option.id === 'new') {
            return option.text;
        }
        
        // Añadir icono de edición junto al nombre
        const $container = $('<div class="d-flex align-items-center justify-content-between w-100"></div>');
        const $optionName = $('<div>' + option.text + '</div>');
        const $actions = $('<div class="option-actions ms-2"></div>');
        
        const $editBtn = $('<button type="button" class="btn btn-link btn-sm p-0 edit-option" ' +
                          'data-id="' + option.id + '" data-name="' + option.text + '">' +
                          '<i class="fas fa-pencil-alt text-primary"></i></button>');
        
        $actions.append($editBtn);
        $container.append($optionName);
        $container.append($actions);
        
        // Manejar evento de botón de edición
        setTimeout(() => {
            $('.edit-option').off('click').on('click', (e) => {
                e.preventDefault();
                e.stopPropagation();
                this._prepareOffCanvasForEditing($(e.currentTarget).data('id'), $(e.currentTarget).data('name'));
            });
        }, 100);
        
        return $container;
    }
    
    /**
     * Configura listeners para los botones de edición
     * @private
     */
    _setupEditListeners() {
        // Este método es llamado después de cada cambio
        // para asegurar que los nuevos botones tengan listeners
    }
    
    /**
     * Prepara el offcanvas para la creación de un nuevo item
     * @param {string} term - Término de búsqueda actual para prellenar
     * @private
     */
    _prepareOffCanvasForCreation(term) {
        // Aplicar estilos al offcanvas
        this.offCanvas.classList.remove('offcanvas-sm', 'offcanvas-md', 'offcanvas-lg');
        this.offCanvas.style.width = '800px';  // Ancho fijo más amplio
        
        const header = this.offCanvas.querySelector('.offcanvas-header');
        if (header) {
            header.style.backgroundColor = '#3944BC';
            header.style.color = 'white';
            
            // Asegurar que el título sea blanco
            const title = header.querySelector('.offcanvas-title, h5');
            if (title) {
                title.classList.add('text-white');
            }
            
            // Asegurar que el botón de cerrar sea visible
            const closeBtn = header.querySelector('.btn-close');
            if (closeBtn) {
                closeBtn.classList.add('btn-close-white');
            }
        }

        // Limpiar el formulario
        const form = this.offCanvas.querySelector('form');
        if (form) {
            form.reset();
            
            // Establecer el título del offcanvas
            const title = this.offCanvas.querySelector('.offcanvas-title');
            if (title) {
                title.textContent = `Nueva ${this.config.entityName}`;
            }
            
            // Prellenar el campo de nombre con el término buscado
            const nameInput = form.querySelector('#Nombre') || form.querySelector('[name="Nombre"]');
            if (nameInput && term) {
                nameInput.value = term;
            }
            
            // Establecer la acción como creación
            form.setAttribute('data-mode', 'create');
            
            // Restablecer la acción del formulario si es necesario
            if (this.config.createUrl) {
                form.action = this.config.createUrl;
            }
            
            // Mostrar el offcanvas
            this.offCanvasBS.show();
        }
    }
    
    /**
     * Prepara el offcanvas para la edición de un item
     * @param {string|number} id - ID del item a editar
     * @param {string} name - Nombre actual del item
     * @private
     */
    _prepareOffCanvasForEditing(id, name) {
        // Aplicar estilos al offcanvas
        this.offCanvas.classList.remove('offcanvas-sm', 'offcanvas-md', 'offcanvas-lg');
        this.offCanvas.style.width = '800px';  // Ancho fijo más amplio
        
        const header = this.offCanvas.querySelector('.offcanvas-header');
        if (header) {
            header.style.backgroundColor = '#3944BC';
            header.style.color = 'white';
            
            // Asegurar que el título sea blanco
            const title = header.querySelector('.offcanvas-title, h5');
            if (title) {
                title.classList.add('text-white');
            }
            
            // Asegurar que el botón de cerrar sea visible
            const closeBtn = header.querySelector('.btn-close');
            if (closeBtn) {
                closeBtn.classList.add('btn-close-white');
            }
        }

        // Obtener datos completos del item vía AJAX
        $.ajax({
            url: `${this.config.detailUrl}?id=${id}`,
            method: 'GET',
            success: (data) => {
                // Limpiar el formulario
                const form = this.offCanvas.querySelector('form');
                if (form) {
                    form.reset();
                    
                    // Establecer el título del offcanvas
                    const title = this.offCanvas.querySelector('.offcanvas-title');
                    if (title) {
                        title.textContent = `Editar ${this.config.entityName}`;
                    }
                    
                    // Llenar el formulario con los datos recibidos
                    this._fillFormWithData(form, data);
                    
                    // Establecer la acción como edición
                    form.setAttribute('data-mode', 'edit');
                    
                    // Restablecer la acción del formulario si es necesario
                    if (this.config.updateUrl) {
                        form.action = this.config.updateUrl;
                    }
                    
                    // Mostrar el offcanvas
                    this.offCanvasBS.show();
                }
            },
            error: () => {
                alert(`No se pudo cargar la información de la ${this.config.entityName}`);
            }
        });
    }
    
    /**
     * Llena el formulario con los datos del item
     * @param {HTMLFormElement} form - Formulario a llenar
     * @param {Object} data - Datos del item
     * @private
     */
    _fillFormWithData(form, data) {
        // Iterar sobre todas las propiedades del objeto data
        for (const [key, value] of Object.entries(data)) {
            const input = form.querySelector(`#${key}`) || form.querySelector(`[name="${key}"]`);
            if (input) {
                if (input.type === 'checkbox') {
                    input.checked = !!value;
                } else if (input.tagName === 'SELECT') {
                    // Para select normales
                    input.value = value;
                    
                    // Para select2, actualizar también con trigger
                    if ($(input).hasClass('select2-hidden-accessible')) {
                        $(input).val(value).trigger('change');
                    }
                } else {
                    input.value = value;
                }
            }
        }
    }
    
    /**
     * Configura el formulario dentro del offcanvas
     * @private
     */
    _setupOffCanvasForm() {
        const form = this.offCanvas.querySelector('form');
        if (!form) return;
        
        // Reemplazar el submit normal por AJAX
        form.addEventListener('submit', (e) => {
            e.preventDefault();
            
            // Serializar datos del formulario
            const formData = new FormData(form);
            
            // Determinar URL según modo (crear/editar)
            const isCreate = form.getAttribute('data-mode') === 'create';
            const url = isCreate ? this.config.createUrl : this.config.updateUrl;
            
            // Enviar datos vía AJAX
            $.ajax({
                url: url,
                method: isCreate ? 'POST' : 'PUT',
                data: formData,
                processData: false,
                contentType: false,
                success: (response) => {
                    if (response.success) {
                        // Cerrar el offcanvas
                        this.offCanvasBS.hide();
                        
                        // Añadir o actualizar la opción en el select
                        if (isCreate) {
                            // Crear nueva opción
                            const newOption = new Option(response.item.text, response.item.id, true, true);
                            $(this.select).append(newOption).trigger('change');
                        } else {
                            // Actualizar opción existente
                            const option = $(this.select).find(`option[value="${response.item.id}"]`);
                            option.text(response.item.text);
                            
                            // Si está seleccionada, actualizar visualización
                            if ($(this.select).val() == response.item.id) {
                                $(this.select).trigger('change');
                            }
                        }
                        
                        // Llamar al callback si existe
                        if (typeof this.config.onItemSaved === 'function') {
                            this.config.onItemSaved(response.item, isCreate);
                        }
                        
                        // Mostrar notificación
                        alert(response.message || `${this.config.entityName} ${isCreate ? 'creada' : 'actualizada'} correctamente`);
                    } else {
                        alert(`Error: ${response.message || 'Ocurrió un error'}`);
                    }
                },
                error: () => {
                    alert(`Error al ${isCreate ? 'crear' : 'actualizar'} la ${this.config.entityName}`);
                }
            });
        });
    }
} 