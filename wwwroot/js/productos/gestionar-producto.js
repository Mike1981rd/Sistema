// gestionar-producto.js - Version limpia
// Clonado exactamente del módulo Items

$(document).ready(function() {
    // Solo el código del Select2 de categorías clonado de Items
    initCategoriasSelect2();
    
    // Inicializar precios dinámicos
    initPricingSystem();
    
    // Inicializar Select2 para rutas de impresión
    initRutasImpresionSelect2();
    
    // Inicializar componente de carga de imágenes
    initImageUploader();
    
    // Inicializar selector de color Pickr
    initColorPicker();
    
    // Inicializar Select2 para cuentas contables
    initCuentasContablesSelect2();
    
    // Evento para el botón de edición en la selección (después de seleccionar)
    $(document).on('click', '.edit-categoria', function(e) {
        e.preventDefault();
        e.stopPropagation();
        var id = $(this).data('id');
        var nombre = $(this).data('name');
        editCategoria(id, nombre);
    });
});

function initCategoriasSelect2() {
    // Código clonado exactamente de form.js del módulo Items
    var $categoriaSelect = $('#categoriaId');
    var categoriaInicial = $categoriaSelect.val();
    var categoriaTextoInicial = $categoriaSelect.find('option:selected').text();
    
    $categoriaSelect.select2({
        theme: 'bootstrap-5',
        placeholder: 'Seleccione o cree una categoría',
        allowClear: true,
        width: '100%',
        dropdownParent: $('body'),
        ajax: {
            url: window.location.origin + '/Categoria/Buscar',
            dataType: 'json',
            delay: 250,
            data: function(params) {
                console.log("Enviando búsqueda categoría:", params.term);
                return {
                    term: params.term || ''
                };
            },
            processResults: function(data, params) {
                console.log("Resultados categorías recibidos:", data);
                var results = data.results || [];
                
                if (results.length == 0 && params.term && params.term.trim() !== '') {
                    console.log("Agregando opción para crear nueva categoría:", params.term);
                    results.push({
                        id: 'new',
                        text: 'Crear categoría: "' + params.term + '"',
                        term: params.term,
                        _isNew: true  // Marca para identificar elemento nuevo
                    });
                }
                
                return {
                    results: results
                };
            },
            error: function(xhr, status, error) {
                console.error("Error en la búsqueda de categorías:", error);
                console.error("Estado:", status);
                console.error("Respuesta:", xhr.responseText);
            },
            cache: true
        },
        templateResult: formatCategoriaResult,
        templateSelection: formatCategoriaSelection
    }).on('select2:select', function(e) {
        var data = e.params.data;
        console.log("Categoría seleccionada:", data);
        
        if (data.id === 'new') {
            $(this).val(null).trigger('change');
            console.log("Creando categoría:", data.term);
            abrirOffcanvasCategoria(data.term);
        }
    });
    
    // Si hay un valor inicial, cargarlo en Select2
    if (categoriaInicial && categoriaTextoInicial && categoriaTextoInicial !== 'Seleccione una categoría') {
        console.log('Cargando categoría inicial:', categoriaInicial, categoriaTextoInicial);
        var newOption = new Option(categoriaTextoInicial, categoriaInicial, true, true);
        $categoriaSelect.append(newOption).trigger('change');
    }
}

// Funciones para formatear resultados (clonadas de Items)
function formatCategoriaResult(categoria) {
    if (categoria.loading) {
        return categoria.text;
    }
    
    if (categoria.id === 'new') {
        return $('<div class="select2-result-categoria">' +
                 '<div class="select2-result-categoria__action"><i class="fas fa-plus-circle text-success me-1"></i> ' + 
                 categoria.text + '</div>' +
                 '</div>');
    }
    
    return $('<div class="select2-result-categoria">' +
             '<div class="select2-result-categoria__name">' + categoria.text + '</div>' +
             '</div>');
}

function formatCategoriaSelection(categoria) {
    if (!categoria.id || categoria.id === 'new') {
        return categoria.text;
    }
    
    // Añadir iconos de edición junto al nombre
    var $container = $('<div class="d-flex align-items-center justify-content-between w-100"></div>');
    var $categoriaNombre = $('<div>' + categoria.text + '</div>');
    var $actions = $('<div class="categoria-actions ms-2"></div>');
    
    var $editBtn = $('<button type="button" class="btn btn-link btn-sm p-0 me-2 edit-categoria" ' +
                    'data-id="' + categoria.id + '" data-name="' + categoria.text + '">' +
                    '<i class="fas fa-pencil-alt text-primary"></i></button>');
    
    $actions.append($editBtn);
    $container.append($categoriaNombre);
    $container.append($actions);
    
    // Ocultar el botón "x" del Select2 (Clear)
    setTimeout(function() {
        $('.select2-selection__clear').hide();
    }, 100);
    
    // Asignar manejadores de eventos inmediatamente
    $editBtn.on('click', function(e) {
        e.preventDefault();
        e.stopPropagation();
        editCategoria(categoria.id, categoria.text);
        return false;
    });
    
    return $container;
}

// Función para abrir el offcanvas de categoría (clonada de Items)
function abrirOffcanvasCategoria(nombreCategoria) {
    const offcanvasElement = document.getElementById('offcanvasCategoria');
    if (!offcanvasElement) {
        console.error("No se encontró el offcanvas de categoría");
        return;
    }
    
    // Los estilos se aplican mediante CSS, no inline
    offcanvasElement.style.width = '600px';
    const offcanvasBS = bootstrap.Offcanvas.getOrCreateInstance(offcanvasElement);
    offcanvasBS.show();
    
    const formContainer = document.getElementById('formCategoriaContainer');
    if (formContainer) {
        formContainer.innerHTML = '<div class="text-center"><div class="spinner-border text-primary" role="status"></div></div>';
        
        // Cargar el formulario de creación
        $.ajax({
            url: '/Categoria/CreatePartial',
            type: 'GET',
            success: function(response) {
                formContainer.innerHTML = response;
                
                // Inicializar Select2 para familia, impuesto y cuentas
                if ($.fn.select2) {
                    $('#formCategoriaContainer .select2-familia, #formCategoriaContainer .select2-impuesto, #formCategoriaContainer .select2-cuenta').select2({
                        theme: 'bootstrap-5',
                        width: '100%',
                        dropdownParent: $('#offcanvasCategoria')
                    });
                }
                
                // Prellenar el nombre si se proporcionó
                if (nombreCategoria) {
                    setTimeout(function() {
                        const nombreInput = formContainer.querySelector('#CategoriaNombre') || 
                                          formContainer.querySelector('[name="Nombre"]');
                        if (nombreInput) {
                            nombreInput.value = nombreCategoria;
                        }
                    }, 100);
                }
            },
            error: function(xhr, status, error) {
                console.error("Error al cargar el formulario:", error);
                formContainer.innerHTML = '<div class="alert alert-danger">Error al cargar el formulario: ' + error + '</div>';
            }
        });
    }
}

// Función para editar categoría (clonada de Items)
function editCategoria(id, nombre) {
    $('#categoriaId').select2('close');
    console.log("Editando categoría:", id, nombre);
    
    // Validar que el ID sea válido
    if (!id || id === 'new' || isNaN(parseInt(id))) {
        console.error("ID de categoría inválido:", id);
        Swal.fire({ 
            icon: 'warning', 
            title: 'Aviso', 
            text: 'No se puede editar la categoría en este momento. Intente más tarde.',
            footer: 'Sugerencia: Recargue la página si acaba de crear esta categoría.'
        });
        return;
    }
    
    // Mostrar indicador de carga
    Swal.fire({
        title: 'Cargando...',
        text: 'Obteniendo datos de la categoría',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
    
    // Retrasar un poco la carga para asegurar que el elemento esté guardado en el servidor
    setTimeout(() => {
        // Siempre cargar los datos por AJAX antes de abrir el offcanvas
        $.ajax({
            url: `/Categoria/Obtener/${id}`,
            type: 'GET',
            success: function(data) {
                Swal.close();
                const offcanvasElement = document.getElementById('offcanvasCategoria');
                if (!offcanvasElement) {
                    console.error("No se encontró el offcanvas");
                    return false;
                }
                // Los estilos se aplican mediante CSS, no inline
                offcanvasElement.style.width = '600px';
                const offcanvasBS = bootstrap.Offcanvas.getOrCreateInstance(offcanvasElement);
                offcanvasBS.show();
                const formContainer = document.getElementById('formCategoriaContainer');
                if (formContainer) {
                    formContainer.innerHTML = '<div class="text-center"><div class="spinner-border text-primary" role="status"></div></div>';
                    // Cargar el formulario de edición con los datos recibidos
                    $.ajax({
                        url: `/Categoria/EditPartial/${id}`,
                        type: 'GET',
                        success: function(response) {
                            formContainer.innerHTML = response;
                            
                            // Inicializar Select2 para familia, impuesto y cuentas
                            if ($.fn.select2) {
                                $('#formCategoriaContainer .select2-familia, #formCategoriaContainer .select2-impuesto, #formCategoriaContainer .select2-cuenta').select2({
                                    theme: 'bootstrap-5',
                                    width: '100%',
                                    dropdownParent: $('#offcanvasCategoria')
                                });
                            }
                            
                            // Prellenar campos si es necesario
                            setTimeout(function() {
                                let nombreInput = formContainer.querySelector('[name="Nombre"]') || formContainer.querySelector('#Nombre') || formContainer.querySelector('#CategoriaNombre');
                                if (nombreInput && data.nombre) {
                                    nombreInput.value = data.nombre;
                                }
                            }, 100);
                        },
                        error: function(xhr, status, error) {
                            console.error("Error al cargar el formulario de edición:", error, xhr.status);
                            let mensaje = "Error al cargar el formulario de edición.";
                            if (xhr.status === 404) {
                                mensaje += " Es posible que la categoría no exista o esté siendo procesada.";
                            }
                            formContainer.innerHTML = `<div class="alert alert-danger">${mensaje}<br><small>Por favor, recargue la página e intente nuevamente.</small></div>`;
                        }
                    });
                }
            },
            error: function(xhr, status, error) {
                Swal.close();
                console.error("Error al obtener categoría:", error, xhr.status);
                let mensaje = "No se pudo cargar la categoría para editar.";
                if (xhr.status === 404) {
                    mensaje += " Es posible que la categoría no exista o acabe de ser creada.";
                }
                Swal.fire({ 
                    icon: 'error', 
                    title: 'Error', 
                    text: mensaje,
                    footer: 'Por favor, recargue la página e intente nuevamente.'
                });
            }
        });
    }, 500); // Retraso de 500ms para asegurar que el elemento esté guardado
}


// Función para inicializar Select2 de rutas de impresión
function initRutasImpresionSelect2() {
    var $rutaSelect = $('#rutaImpresoraId');
    
    $rutaSelect.select2({
        theme: 'bootstrap-5',
        placeholder: 'Seleccione una ruta de impresión',
        allowClear: true,
        width: '100%',
        language: {
            noResults: function() {
                return "No se encontraron rutas de impresión";
            }
        }
    });
}

// Función para inicializar el componente de carga de imágenes
function initImageUploader() {
    const imageUploaderCard = document.getElementById('imageUploaderCard');
    const imagenArchivoInput = document.getElementById('imagenArchivo');
    const imagenPreview = document.getElementById('imagenPreview');
    const uploadPlaceholder = document.getElementById('uploadPlaceholder');
    const removeImageBtn = document.getElementById('removeImageBtn');
    const imagenUrlInput = document.getElementById('imagenUrl');
    
    // Si hay una URL de imagen existente (modo edición), mostrarla
    const productoId = document.getElementById('productoId');
    if (productoId && productoId.value && imagenUrlInput.value) {
        mostrarImagenExistente(imagenUrlInput.value);
    }
    
    // Click en la card para abrir selector de archivos
    if (imageUploaderCard) {
        imageUploaderCard.addEventListener('click', function(e) {
            // No activar si se hace click en el botón de quitar
            if (e.target !== removeImageBtn && !removeImageBtn.contains(e.target)) {
                imagenArchivoInput.click();
            }
        });
        
        // Drag and drop
        imageUploaderCard.addEventListener('dragover', function(e) {
            e.preventDefault();
            e.stopPropagation();
            imageUploaderCard.classList.add('dragover');
        });
        
        imageUploaderCard.addEventListener('dragleave', function(e) {
            e.preventDefault();
            e.stopPropagation();
            imageUploaderCard.classList.remove('dragover');
        });
        
        imageUploaderCard.addEventListener('drop', function(e) {
            e.preventDefault();
            e.stopPropagation();
            imageUploaderCard.classList.remove('dragover');
            
            const files = e.dataTransfer.files;
            if (files.length > 0) {
                const file = files[0];
                if (file.type.startsWith('image/')) {
                    // Simular que el archivo fue seleccionado en el input
                    const dataTransfer = new DataTransfer();
                    dataTransfer.items.add(file);
                    imagenArchivoInput.files = dataTransfer.files;
                    handleFileSelect(file);
                } else {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Archivo no válido',
                        text: 'Por favor, selecciona un archivo de imagen válido.'
                    });
                }
            }
        });
    }
    
    // Selección de archivo
    if (imagenArchivoInput) {
        imagenArchivoInput.addEventListener('change', function(e) {
            if (e.target.files && e.target.files[0]) {
                handleFileSelect(e.target.files[0]);
            }
        });
    }
    
    // Botón quitar imagen
    if (removeImageBtn) {
        removeImageBtn.addEventListener('click', function(e) {
            e.stopPropagation();
            imagenArchivoInput.value = '';
            imagenPreview.src = '#';
            imagenPreview.style.display = 'none';
            uploadPlaceholder.style.display = 'flex';
            removeImageBtn.style.display = 'none';
            imagenUrlInput.value = '';
        });
    }
    
    // Función para manejar la selección de archivo
    function handleFileSelect(file) {
        // Validar tamaño (2MB máximo)
        const maxSize = 2 * 1024 * 1024; // 2MB
        if (file.size > maxSize) {
            Swal.fire({
                icon: 'warning',
                title: 'Archivo muy grande',
                text: 'El archivo debe ser menor a 2MB.'
            });
            imagenArchivoInput.value = '';
            return;
        }
        
        if (file.type.startsWith('image/')) {
            const reader = new FileReader();
            reader.onload = function(e) {
                imagenPreview.src = e.target.result;
                imagenPreview.style.display = 'block';
                uploadPlaceholder.style.display = 'none';
                removeImageBtn.style.display = 'inline-block';
                // Limpiar URL existente ya que ahora hay un archivo nuevo
                imagenUrlInput.value = '';
            }
            reader.readAsDataURL(file);
        } else {
            Swal.fire({
                icon: 'warning',
                title: 'Archivo no válido',
                text: 'Por favor, selecciona un archivo de imagen válido (JPG, PNG, GIF).'
            });
            imagenArchivoInput.value = '';
        }
    }
    
    // Función para mostrar imagen existente (modo edición)
    function mostrarImagenExistente(url) {
        if (url) {
            imagenPreview.src = url;
            imagenPreview.style.display = 'block';
            uploadPlaceholder.style.display = 'none';
            removeImageBtn.style.display = 'inline-block';
        }
    }
}

// Función para inicializar Pickr color picker
function initColorPicker() {
    const colorValueInput = document.getElementById('colorBotonTPV_value');
    const colorPickerTrigger = document.getElementById('colorPickerTrigger');

    if (colorPickerTrigger && colorValueInput) {
        const pickrInstance = Pickr.create({
            el: colorPickerTrigger, // El div que actuará como botón
            theme: 'classic',
            useAsButton: true, // MUY IMPORTANTE: hace que 'el' actúe como el botón que muestra el picker
            default: colorValueInput.value || '#d62828', // Color inicial

            swatches: [
                '#F44336', '#E91E63', '#9C27B0', '#673AB7', '#3F51B5', 
                '#2196F3', '#03A9F4', '#00BCD4', '#009688', '#4CAF50',
                '#8BC34A', '#CDDC39', '#FFEB3B', '#FFC107', '#FF9800',
                '#FF5722', '#795548', '#9E9E9E', '#607D8B', '#000000'
            ],

            components: {
                preview: true,
                opacity: true, // La imagen de referencia muestra un slider de opacidad
                hue: true,
                interaction: {
                    hex: true,  // Para mostrar el input HEX dentro del picker
                    rgba: false, // No mostrar input RGBA
                    hsla: false, // No mostrar input HSLA
                    hsva: false, // No mostrar input HSVA
                    cmyk: false, // No mostrar input CMYK
                    input: true, // Habilitar el campo de entrada de texto para HEX/RGBA dentro del picker
                    clear: false,
                    save: true  // Botón para confirmar selección (o "Aplicar")
                }
            },
            strings: { save: 'Aplicar', hex: 'HEX' } // O 'Guardar'
        });

        // Establecer el color de fondo inicial del botón trigger
        const initialColorObj = pickrInstance.getColor();
        if (initialColorObj) {
            colorPickerTrigger.style.backgroundColor = initialColorObj.toRGBA().toString(0); // Usar RGBA para opacidad
        }

        pickrInstance.on('save', (color, instance) => {
            if (color) {
                // Si opacity es true, toRGBA().toString(0) da "rgba(r,g,b,a)"
                // Si quieres solo HEX #RRGGBB, usa color.toHEXA().toString(0)
                // La imagen parece mostrar solo HEX en el input de arriba, así que usaremos HEX.
                colorValueInput.value = color.toHEXA().toString(0); // #RRGGBB
                colorPickerTrigger.style.backgroundColor = color.toRGBA().toString(); // El botón sí puede tener opacidad
            }
            pickrInstance.hide(); // Opcional: ocultar después de guardar si no lo hace por defecto
        });

        // Sincronizar si el usuario edita el input de texto directamente
        colorValueInput.addEventListener('change', function(event) {
            const success = pickrInstance.setColor(event.target.value, true); // true para modo silencioso
            if (success) {
                colorPickerTrigger.style.backgroundColor = pickrInstance.getColor().toRGBA().toString();
            } else {
                // Opcional: manejar color inválido, ej: revertir al color anterior de pickr
                // colorValueInput.value = pickrInstance.getColor().toHEXA().toString(0);
            }
        });
        
        // Manejar cambios en tiempo real mientras se ajusta el color
        pickrInstance.on('change', (color, instance) => {
            if (color) {
                colorValueInput.value = color.toHEXA().toString(0); // Actualizar input en tiempo real
                colorPickerTrigger.style.backgroundColor = color.toRGBA().toString();
            }
        });
        
        // También escuchar el evento input para actualizaciones más inmediatas en el input
        colorValueInput.addEventListener('input', function(event) {
            const newColor = event.target.value;
            if (/^#[0-9A-F]{6}$/i.test(newColor)) {
                const success = pickrInstance.setColor(newColor, true);
                if (success) {
                    colorPickerTrigger.style.backgroundColor = pickrInstance.getColor().toRGBA().toString();
                }
            }
        });
    }
}

// Sistema de precios dinámicos
function initPricingSystem() {
    const priceRowsContainer = document.getElementById('priceRowsContainer');
    const btnAddPriceRow = document.getElementById('btnAddPriceRow');
    let priceRowIndex = 0;
    let impuestosDataGlobal = {}; // Store global para impuestos

    console.log('Iniciando sistema de precios...');

    function initializePriceRow(rowElement, index) {
        const precioBaseInput = rowElement.querySelector('.precio-base');
        const impuestoSelect = rowElement.querySelector('.select2-impuestos');
        const precioTotalInput = rowElement.querySelector('.precio-total');

        console.log(`Inicializando fila ${index}...`);

        // Destruir instancia previa si existe (importante para elementos clonados)
        if ($(impuestoSelect).data('select2')) {
            $(impuestoSelect).select2('destroy');
        }

        // Inicializar Select2 para impuestos
        $(impuestoSelect).select2({
            theme: "bootstrap-5",
            dropdownParent: document.body,
            placeholder: 'Seleccione impuestos',
            allowClear: true,
            width: '100%',
            ajax: {
                url: '/api/impuestos',
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        page: params.page || 1
                    };
                },
                processResults: function (data) {
                    if (data && data.length) {
                        // Almacenar los porcentajes en el store global
                        data.forEach(item => {
                            impuestosDataGlobal[item.id] = parseFloat(item.porcentaje);
                        });
                        return {
                            results: data.map(item => ({
                                id: item.id,
                                text: `${item.nombre} (${item.porcentaje}%)`,
                                percentage: parseFloat(item.porcentaje)
                            }))
                        };
                    }
                    return { results: [] };
                },
                cache: true
            }
        });

        // Actualizar cálculos cuando cambian los impuestos seleccionados
        $(impuestoSelect).on('select2:select select2:unselect', function(e) {
            calcularPrecioTotal();
        });

        // Funciones de cálculo
        function calcularPrecioTotal() {
            const base = parseFloat(precioBaseInput.value) || 0;
            let totalImpuestosPorcentaje = 0;
            
            // Obtener los impuestos seleccionados
            const selectedValues = $(impuestoSelect).val() || [];
            selectedValues.forEach(value => {
                const percentage = impuestosDataGlobal[value] || 0;
                totalImpuestosPorcentaje += percentage / 100;
            });
            
            const total = base * (1 + totalImpuestosPorcentaje);
            precioTotalInput.value = total.toFixed(2);
        }

        function calcularPrecioBase() {
            const total = parseFloat(precioTotalInput.value) || 0;
            let totalImpuestosPorcentaje = 0;
            
            // Obtener los impuestos seleccionados
            const selectedValues = $(impuestoSelect).val() || [];
            selectedValues.forEach(value => {
                const percentage = impuestosDataGlobal[value] || 0;
                totalImpuestosPorcentaje += percentage / 100;
            });
            
            if ((1 + totalImpuestosPorcentaje) === 0) {
                precioBaseInput.value = total.toFixed(2);
                return;
            }
            
            const base = total / (1 + totalImpuestosPorcentaje);
            precioBaseInput.value = base.toFixed(2);
        }

        // Event Listeners con prevención de bucles
        let isCalculatingBase = false;
        let isCalculatingTotal = false;

        precioBaseInput.addEventListener('input', function() {
            if (isCalculatingBase) return;
            isCalculatingTotal = true;
            calcularPrecioTotal();
            isCalculatingTotal = false;
        });

        precioTotalInput.addEventListener('input', function() {
            if (isCalculatingTotal) return;
            isCalculatingBase = true;
            calcularPrecioBase();
            isCalculatingBase = false;
        });

        // Actualizar cuando cambien los impuestos seleccionados
        $(impuestoSelect).on('change', function() {
            if (isCalculatingBase) return;
            isCalculatingTotal = true;
            calcularPrecioTotal();
            isCalculatingTotal = false;
        });

        // Botón eliminar
        const btnEliminar = rowElement.querySelector('.btn-eliminar-precio');
        if (btnEliminar) {
            btnEliminar.addEventListener('click', function() {
                rowElement.remove();
                updateDeleteButtons();
                updateInputNames();
            });
        }

        // Actualizar nombres de inputs para el binding
        updateInputNames();
    }

    function updateInputNames() {
        priceRowsContainer.querySelectorAll('.price-row').forEach((row, index) => {
            row.querySelectorAll('[data-original-name]').forEach(input => {
                const originalName = input.getAttribute('data-original-name');
                input.name = `Precios[${index}].${originalName}`;
            });
            
            // Los select2 ya tienen el nombre correcto, no necesitan actualización adicional
        });
    }

    function addPriceRow() {
        // Crear nueva fila a partir de un template limpio
        const template = document.createElement('div');
        template.innerHTML = `
            <div class="row price-row gx-3 mb-3 align-items-center" data-index="${priceRowIndex + 1}">
                <div class="col-md-3">
                    <label class="form-label">Precio base *</label>
                    <div class="input-group">
                        <span class="input-group-text">$</span>
                        <input type="number" class="form-control precio-base" step="0.01" placeholder="0.00" required 
                               name="Precios[${priceRowIndex + 1}].PrecioBase" data-original-name="PrecioBase" value="0.00">
                    </div>
                    <span class="text-danger validation-message"></span>
                </div>
                <div class="col-auto d-flex align-items-end pb-1">
                    <span class="h4 mx-1">+</span>
                </div>
                <div class="col-md-4">
                    <label class="form-label">Impuestos</label>
                    <select class="form-select select2-impuestos" multiple="multiple" style="width: 100%;" 
                            name="Precios[${priceRowIndex + 1}].ImpuestoIds" data-original-name="ImpuestoIds">
                        <!-- Las opciones se cargarán por AJAX -->
                    </select>
                    <span class="text-danger validation-message"></span>
                </div>
                <div class="col-auto d-flex align-items-end pb-1">
                    <span class="h4 mx-1">=</span>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Precio Total *</label>
                    <div class="input-group">
                        <span class="input-group-text">$</span>
                        <input type="number" class="form-control precio-total" step="0.01" placeholder="0.00" required
                               name="Precios[${priceRowIndex + 1}].PrecioTotal" data-original-name="PrecioTotal" value="0.00">
                    </div>
                    <span class="text-danger validation-message"></span>
                </div>
                <div class="col-auto d-flex align-items-center">
                    <button type="button" class="btn btn-sm btn-outline-danger btn-eliminar-precio" title="Eliminar este nivel de precio">
                        <i class="fas fa-trash-alt"></i>
                    </button>
                </div>
            </div>
        `;

        const newRow = template.firstElementChild;
        priceRowsContainer.appendChild(newRow);
        priceRowIndex++;
        initializePriceRow(newRow, priceRowIndex);
        updateDeleteButtons();
    }

    function updateDeleteButtons() {
        const rows = priceRowsContainer.querySelectorAll('.price-row');
        rows.forEach((row, index) => {
            const btnEliminar = row.querySelector('.btn-eliminar-precio');
            if (btnEliminar) {
                if (index === 0) { // Primera fila
                    btnEliminar.style.display = 'none';
                } else { // Filas subsecuentes
                    btnEliminar.style.display = rows.length > 1 ? 'inline-flex' : 'none'; // Mostrar si hay más de una fila
                }
            }
        });
    }

    // Inicializar primera fila
    const firstRow = priceRowsContainer.querySelector('.price-row');
    if (firstRow) {
        initializePriceRow(firstRow, 0);
    }

    // Botón añadir fila
    if (btnAddPriceRow) {
        btnAddPriceRow.addEventListener('click', addPriceRow);
    }

    // Inicializar el botón de eliminar de la primera fila
    updateDeleteButtons();

    // Función para recopilar datos al enviar el formulario
    window.collectPricingData = function() {
        const preciosData = [];
        priceRowsContainer.querySelectorAll('.price-row').forEach((row, index) => {
            const precioBase = row.querySelector('.precio-base').value;
            const precioTotal = row.querySelector('.precio-total').value;
            const impuestoIds = [];
            
            row.querySelectorAll('.impuesto-checkbox:checked').forEach(checkbox => {
                impuestoIds.push(parseInt(checkbox.value));
            });

            preciosData.push({
                PrecioBase: parseFloat(precioBase) || 0,
                PrecioTotal: parseFloat(precioTotal) || 0,
                ImpuestoIds: impuestoIds
            });
        });
        return preciosData;
    };
}

// Función para inicializar selectores de cuentas contables
function initCuentasContablesSelect2() {
    // Inicializar cada selector de cuentas
    $('.select2-cuenta-contable').each(function() {
        const $this = $(this);
        const cuentaInicial = $this.val();
        const cuentaTextoInicial = $this.find('option:selected').text();
        
        $this.select2({
            theme: 'bootstrap-5',
            width: '100%',
            placeholder: 'Buscar cuenta contable...',
            allowClear: true,
            minimumInputLength: 1,
            language: {
                inputTooShort: function() {
                    return "Ingrese al menos 1 caracter para buscar";
                },
                noResults: function() {
                    return "No se encontraron resultados";
                },
                searching: function() {
                    return "Buscando...";
                }
            },
            ajax: {
                url: '/Productos/BuscarCuentasContables',
                dataType: 'json',
                delay: 250,
                data: function(params) {
                    return {
                        term: params.term || ''
                    };
                },
                processResults: function(data) {
                    return data;
                },
                cache: true
            },
            templateResult: formatCuentaResult,
            templateSelection: formatCuentaSelection
        });
        
        // Si hay un valor inicial, cargarlo
        if (cuentaInicial && cuentaTextoInicial) {
            const newOption = new Option(cuentaTextoInicial, cuentaInicial, true, true);
            $this.append(newOption).trigger('change');
        }
    });
}

// Función para formatear resultados de búsqueda de cuentas
function formatCuentaResult(cuenta) {
    if (!cuenta.id) {
        return cuenta.text;
    }
    
    const $resultado = $(
        '<div class="select2-result">' +
            '<span class="badge bg-secondary me-2">' + (cuenta.codigo || '') + '</span>' +
            '<span>' + (cuenta.nombre || cuenta.text) + '</span>' +
        '</div>'
    );
    return $resultado;
}

// Función para formatear la selección de cuenta
function formatCuentaSelection(cuenta) {
    if (!cuenta.id) {
        return cuenta.text;
    }
    return cuenta.text;
}

// IMPORTANTE: Exponer las funciones globalmente para que funcionen los eventos onclick
window.editCategoria = editCategoria;
window.abrirOffcanvasCategoria = abrirOffcanvasCategoria;