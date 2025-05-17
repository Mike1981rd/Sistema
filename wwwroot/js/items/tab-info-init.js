// Force refresh that works
window.refreshCategorySelect2 = function(categoriaId, categoriaName) {
    const $categoriaSelect = $('#CategoriaId');
    if (!$categoriaSelect.length) return;
    
    // Update option
    let $option = $categoriaSelect.find(`option[value="${categoriaId}"]`);
    if (!$option.length) {
        $option = $('<option></option>').attr('value', categoriaId).text(categoriaName);
        $categoriaSelect.append($option);
    } else {
        $option.text(categoriaName);
    }
    
    // Force select2 to refresh
    $categoriaSelect.val(categoriaId);
    $categoriaSelect.trigger('change');
    
    // Wait for DOM update then manually inject edit button
    setTimeout(function() {
        const $selection = $categoriaSelect.siblings('.select2-container').find('.select2-selection__rendered');
        $selection.html(`
            <div class="d-flex align-items-center justify-content-between w-100">
                <div>${categoriaName}</div>
                <div class="categoria-actions ms-2">
                    <button type="button" class="btn btn-link btn-sm p-0 me-2 edit-categoria" 
                            data-id="${categoriaId}" data-name="${categoriaName}">
                        <i class="fas fa-pencil-alt text-primary"></i>
                    </button>
                </div>
            </div>
        `);
    }, 100);
};

// Function to initialize category select2 with all configurations
function initCategorySelect2($element) {
    if (!$element || $element.length === 0) return;
    
    $element.select2({
        theme: 'bootstrap-5',
        width: '100%',
        placeholder: 'Seleccione o cree una categoría',
        allowClear: true,
        ajax: {
            url: '/api/Categorias/Buscar',
            dataType: 'json',
            delay: 250,
            data: function(params) {
                return {
                    term: params.term || ''
                };
            },
            processResults: function(data, params) {
                var results = data.results || [];
                var searchTerm = params.term ? params.term.trim() : '';
                var exactMatch = false;
                
                if (searchTerm !== '') {
                    exactMatch = results.some(function(item) {
                        return item.text.toLowerCase() === searchTerm.toLowerCase();
                    });
                    
                    if (!exactMatch) {
                        results.push({
                            id: 'new',
                            text: 'Crear categoría: "' + searchTerm + '"',
                            term: searchTerm,
                            _isNew: true
                        });
                    }
                }
                
                return { results: results };
            },
            cache: true
        },
        templateResult: formatCategoriaResult,
        templateSelection: formatCategoriaSelection,
        escapeMarkup: function(markup) { return markup; }
    }).on('select2:select', function(e) {
        var data = e.params.data;
        
        if (data.id === 'new') {
            $(this).val(null).trigger('change');
            abrirOffcanvasCategoria(data.term);
        }
    });
}

$(document).ready(function() {
    console.log("=== INFO TAB INITIALIZATION ===");
    
    // Initialize immediately on page load
    setTimeout(function() {
        initInfoTab();
        initBarcodeGeneration();
    }, 300);
    
    // Attach edit button handler at document level for dynamic elements
    // Esto es para el botón de editar *DESPUÉS* de seleccionar una categoría/marca en el select2
    // form.js también tiene lógica similar en formatCategoriaSelection/formatMarcaSelection, 
    // pero es para el botón DENTRO del resultado del select2. Revisar si son compatibles o si uno debe prevalecer.
    // Por ahora, lo dejamos, pero si hay doble botón de editar o no funciona, este es un punto a revisar.
    /* --- SE COMENTA TEMPORALMENTE SI form.js YA MANEJA ESTO MEJOR ---
    $(document).on('click', '.edit-categoria', function(e) {
        e.preventDefault();
        e.stopPropagation();
        const id = $(this).data('id');
        const nombre = $(this).data('name');
        editCategoria(id, nombre); // Si form.js tiene editCategoria, esta llamada podría ser un problema.
    });
    */
    
    function initInfoTab() {
        // Category Select2 - Initialize using centralized function
        /* --- COMENTADO: form.js se encargará de esto ---
        var $category = $('#CategoriaId');
        if ($category.length > 0 && !$category.hasClass('select2-hidden-accessible')) {
            initCategorySelect2($category); // initCategorySelect2 está definido en este mismo archivo, pero form.js tiene su propia inicialización.
            console.log("✓ Category Select2 initialized with AJAX (tab-info-init)");
        }
        */
        
        // Brand Select2 - Initialize with AJAX
        /* --- COMENTADO: form.js se encargará de esto ---
        var $brand = $('#MarcaId');
        if ($brand.length > 0 && !$brand.hasClass('select2-hidden-accessible')) {
            $brand.select2({
                theme: 'bootstrap-5',
                width: '100%',
                placeholder: 'Seleccione o cree una marca',
                allowClear: true,
                ajax: {
                    url: '/api/Marcas/Buscar',
                    dataType: 'json',
                    delay: 250,
                    data: function(params) {
                        return {
                            term: params.term || ''
                        };
                    },
                    processResults: function(data, params) {
                        var results = data.results || [];
                        
                        // Agregar opción para crear nueva marca
                        if (results.length == 0 && params.term && params.term.trim() !== '') {
                            results.push({
                                id: 'new',
                                text: 'Crear marca: "' + params.term + '"',
                                term: params.term,
                                _isNew: true
                            });
                        }
                        
                        return { results: results };
                    },
                    cache: true
                },
                templateResult: formatMarcaResult,
                templateSelection: formatMarcaSelection
            }).on('select2:select', function(e) {
                var data = e.params.data;
                if (data.id === 'new') {
                    $(this).val(null).trigger('change');
                    // abrirOffcanvasMarca(data.term); // Esta función también se comentará más abajo
                }
            });
            console.log("✓ Brand Select2 initialized with AJAX (tab-info-init)");
        }
        */
        
        // Tax Select2 - Initialize with AJAX (Esto puede quedarse si form.js no lo maneja)
        var $tax = $('#ImpuestoId');
        if ($tax.length > 0 && !$tax.hasClass('select2-hidden-accessible')) {
            $tax.select2({
                theme: 'bootstrap-5',
                width: '100%',
                placeholder: 'Seleccione un impuesto',
                allowClear: true,
                ajax: {
                    url: '/api/Impuestos/Buscar',
                    dataType: 'json',
                    delay: 250,
                    data: function(params) {
                        return {
                            term: params.term || ''
                        };
                    },
                    processResults: function(data) {
                        return data; // Asume que la data ya viene en formato {results: [...]} o es un array
                    },
                    cache: true
                }
            });
            console.log("✓ Tax Select2 initialized with AJAX (tab-info-init)");
        }
        
        // Image upload functionality (Esto se queda, form.js no parece manejarlo)
        initImageUpload();
        
        // Category inheritance (Esto se queda, y usa eventos. Ver si es compatible con form.js)
        initCategoryInheritance();
    }
    
    // Initialize on page load if Info tab is active
    if ($('#tab-info').hasClass('active') || $('#tab-info').length > 0) {
        initInfoTab();
    }
    
    // Reinitialize when Info tab is shown
    $('a[data-bs-toggle="tab"][href="#tab-info"]').on('shown.bs.tab', function() {
        console.log("Info tab shown - initializing...");
        initInfoTab();
    });

    // Generar código de ítem automáticamente (Esto se queda, form.js tenía otra versión)
    console.log("Verificando condiciones para generar código de ítem:");
    console.log("1. Existe #Codigo?", $('#Codigo').length > 0);
    if ($('#Codigo').length > 0) {
        console.log("2. Es #Codigo readonly?", ($('#Codigo').is('[readonly]') || $('#Codigo').prop('readonly')));
    }
    console.log("3. URL incluye '/item/create'?", window.location.pathname.toLowerCase().includes('/item/create'));

    if ($('#Codigo').length && ($('#Codigo').is('[readonly]') || $('#Codigo').prop('readonly')) && window.location.pathname.toLowerCase().includes('/item/create')) {
        console.log("CONDICIONES CUMPLIDAS. Intentando generar código de ítem automáticamente...");
        $.ajax({
            url: '/Item/GenerarCodigoAutomatico',
            type: 'GET',
            success: function(response) {
                if (response.success && response.codigo) {
                    $('#Codigo').val(response.codigo);
                    console.log("Código de ítem generado y asignado:", response.codigo);
                    // Opcional: si el código de barras debe ser el mismo inicialmente
                    // if ($('#CodigoBarras').length && ($('#CodigoBarras').val() === '' || $('#CodigoBarras').val() === null)) {
                    //     $('#CodigoBarras').val(response.codigo);
                    //     generateBarcodeVisualization(response.codigo); 
                    // }
                } else {
                    console.warn("No se pudo generar el código de ítem automáticamente del backend.", response.message);
                }
            },
            error: function(jqXHR, textStatus, errorThrown) {
                console.error("Error al llamar a /Item/GenerarCodigoAutomatico:", textStatus, errorThrown);
            }
        });
    }

    // Listener para el guardado de Marca desde el offcanvas (ELIMINADO DE AQUÍ, form.js lo manejará)
    /* --- ELIMINADO ---
    $(document).on('click', '#btnGuardarMarca', function(e) {
        // ... (código que añadimos en el paso anterior)
    });
    */
});

// Barcode generation functionality
function initBarcodeGeneration() {
    // Si ya fue inicializado por otro script, no hacer nada
    if (window.itemCreateInitialized || window.barcodeInitialized) {
        console.log("Barcode generation already initialized by another script - skipping");
        return;
    }
    
    console.log("Initializing barcode generation...");
    window.barcodeInitialized = true;
    
    // Generate barcode button
    $('#generarCodigoBarras').off('click').on('click', function() {
        $.ajax({
            url: '/Item/GenerarCodigoBarras',
            type: 'POST',
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            success: function(data) {
                if (data.success) {
                    $('#CodigoBarras').val(data.codigo);
                    // Generate barcode visualization
                    if (data.codigo) {
                        generateBarcodeVisualization(data.codigo);
                    }
                } else {
                    Swal.fire('Error', 'No se pudo generar el código de barras', 'error');
                }
            },
            error: function() {
                Swal.fire('Error', 'Ocurrió un error al generar el código de barras', 'error');
            }
        });
    });
    
    // Print barcode button
    $('#imprimirCodigoBarras').off('click').on('click', function() {
        const codigoBarras = $('#CodigoBarras').val();
        if (!codigoBarras) {
            Swal.fire('Advertencia', 'Debe generar o ingresar un código de barras primero', 'warning');
            return;
        }
        printBarcode(codigoBarras);
    });
    
    // If there's already a barcode, generate visualization
    const existingBarcode = $('#CodigoBarras').val();
    if (existingBarcode) {
        generateBarcodeVisualization(existingBarcode);
    }
}

// Generate barcode visualization using JsBarcode
function generateBarcodeVisualization(codigo) {
    if (typeof JsBarcode !== 'undefined') {
        JsBarcode("#barcode", codigo, {
            format: "CODE128",
            lineColor: "#000",
            width: 2,
            height: 80,
            displayValue: true
        });
        $('#codigoBarrasPreview').show();
        $('#imprimirCodigoBarras').prop('disabled', false);
    }
}

// Print barcode
function printBarcode(codigo) {
    const printWindow = window.open('', '_blank');
    printWindow.document.write(`
        <html>
        <head>
            <title>Código de Barras</title>
            <script src="https://cdn.jsdelivr.net/npm/jsbarcode@3.11.5/dist/JsBarcode.all.min.js"></script>
        </head>
        <body>
            <svg id="barcode"></svg>
            <script>
                JsBarcode("#barcode", "${codigo}", {
                    format: "CODE128",
                    lineColor: "#000",
                    width: 2,
                    height: 80,
                    displayValue: true
                });
                window.print();
                window.close();
            </script>
        </body>
        </html>
    `);
}

// Image upload handler
let initImageUploadCallCount = 0; // Contador para depuración
const IMAGE_SESSION_STORAGE_KEY = 'itemImagePreviewBase64';

function initImageUpload() {
    initImageUploadCallCount++;
    console.log(`Inicializando cargador de imágenes v5... (Llamada #${initImageUploadCallCount})`);

    const $preview = $('#preview');
    const $imagePreviewDiv = $('#imagePreview');
    const $imageDefaultDiv = $('#imageDefault');
    const $clearImageBtn = $('#clearImageBtn');
    const $imageFileInput = $('#imageFile');

    if ($preview.length === 0 || $imagePreviewDiv.length === 0 || $imageDefaultDiv.length === 0) {
        console.error("Elementos esenciales para la previsualización de imagen no encontrados.");
        return;
    }

    let modelImageUrl = $preview.data('original-src');
    if (modelImageUrl === '#' || modelImageUrl === '') {
        modelImageUrl = null;
    }

    // Intenta recuperar la imagen local desde sessionStorage al inicio de la función
    let localImageDataBase64 = sessionStorage.getItem(IMAGE_SESSION_STORAGE_KEY);

    function displayImage(src, isModelImageSource = false) {
        // Si la imagen ya tiene esta fuente y está visible, no hacer nada para evitar bucles.
        if ($preview.attr('src') === src && $imagePreviewDiv.is(':visible')) {
            // console.log("displayImage: src es el mismo y ya está visible, evitando re-actualización.");
            // Asegurarse de que el botón clear esté visible si hay imagen
            if (src && src !== '#') $clearImageBtn.show();
            return;
        }

        $preview.attr('src', src);
        $imagePreviewDiv.show();
        $imageDefaultDiv.hide();
        $clearImageBtn.show();

        if (!isModelImageSource) {
            localImageDataBase64 = src; // Es una imagen local (DataURL)
            try {
                sessionStorage.setItem(IMAGE_SESSION_STORAGE_KEY, src);
            } catch (e) {
                console.warn("No se pudo guardar la imagen en sessionStorage:", e);
            }
        } else {
            // Si estamos mostrando la imagen del modelo, la imagen local ya no es la fuente principal.
            // No limpiamos sessionStorage aquí directamente; se limpiará al borrar explícitamente
            // o si se carga una nueva imagen local.
        }
        console.log("Mostrando imagen:", src, "Fuente es modelo:", isModelImageSource);
    }

    function displayDefault(clearOriginalModelUrl = false) {
        // console.trace("displayDefault called");
        // Si ya está en default, no hacer nada
        if ($imageDefaultDiv.is(':visible') && $preview.attr('src') === '#') {
            // console.log("displayDefault: ya está en estado default, evitando re-actualización.");
            return;
        }

        $preview.attr('src', '#');
        $imagePreviewDiv.hide();
        $imageDefaultDiv.show();
        $clearImageBtn.hide();
        localImageDataBase64 = null;
        try {
            sessionStorage.removeItem(IMAGE_SESSION_STORAGE_KEY);
        } catch (e) {
            console.warn("No se pudo remover la imagen de sessionStorage:", e);
        }

        if (clearOriginalModelUrl) {
            modelImageUrl = null;
            $preview.data('original-src', '');
        }
        console.log("Mostrando placeholder de imagen.");
    }

    if (localImageDataBase64) { // Si recuperamos algo de sessionStorage
        console.log("Restaurando imagen local desde sessionStorage al inicio de initImageUpload:", localImageDataBase64.substring(0,50) + "...");
        displayImage(localImageDataBase64, false);
    } else if (modelImageUrl) {
        console.log("Intentando mostrar imagen del modelo (original-src) al inicio:", modelImageUrl);
        const timestamp = new Date().getTime();
        const finalModelImageUrl = modelImageUrl.includes('?') ? `${modelImageUrl}&t=${timestamp}` : `${modelImageUrl}?t=${timestamp}`;
        
        // Quitar handlers previos para evitar duplicados si se llama múltiples veces
        $preview.off('.imagehandler').on('load.imagehandler', function() {
            // $(this) es el elemento img#preview
            if (this.naturalWidth > 0 && this.naturalHeight > 0) { 
                console.log("Imagen del modelo cargada y visible (desde evento load inicial):");
                // Llamar a displayImage SOLO si el src actual no es ya el finalModelImageUrl
                // o si no está correctamente visible. La propia displayImage ya tiene una guarda.
                displayImage(finalModelImageUrl, true);
            } else {
                console.warn("Evento load inicial disparado, pero imagen parece no válida. Modelo URL:", finalModelImageUrl);
                // No llamar a displayDefault aquí para evitar problemas si el placeholder dispara load.
            }
        }).on('error.imagehandler', function() {
            console.error("Error cargando imagen del modelo (desde evento error inicial):", finalModelImageUrl);
            displayDefault();
        });

        // Iniciar la carga de la imagen del modelo. El `src` ya debería estar puesto por Razor.
        // Si el src no es el finalModelImageUrl (ej. solo la URL base), lo actualizamos para el cache busting.
        if($preview.attr('src') !== finalModelImageUrl){
             $preview.attr('src', finalModelImageUrl); 
        }

        if ($preview[0] && $preview[0].complete && $preview[0].naturalWidth > 0 && $preview[0].naturalHeight > 0) {
            console.log("Imagen del modelo ya en caché y completa al inicio.");
            displayImage(finalModelImageUrl, true);
        }
    } else {
        displayDefault();
    }

    $('#browseButton').off('click').on('click', function() {
        $imageFileInput.click();
    });

    $imageFileInput.off('change').on('change', function(e) {
        const file = e.target.files[0];
        if (file) {
            if (!file.type.match('image.*')) {
                Swal.fire('Error', 'El archivo seleccionado no es una imagen.', 'error');
                return;
            }
            const reader = new FileReader();
            reader.onload = function(event) {
                displayImage(event.target.result, false); // false porque es fuente local
                console.log("Imagen cargada desde archivo local para previsualización.");
            };
            reader.onerror = function() {
                Swal.fire('Error', 'Error al leer el archivo de imagen.', 'error');
                displayDefault(); // Mostrar default si falla la lectura
            };
            reader.readAsDataURL(file);
        } else {
             // Si se cancela la selección de archivo, restaurar la imagen previa si existe
            if (localImageDataBase64) {
                displayImage(localImageDataBase64, false);
            } else if (modelImageUrl) {
                const ts = new Date().getTime();
                const finalModelUrl = modelImageUrl.includes('?') ? `${modelImageUrl}&t=${ts}` : `${modelImageUrl}?t=${ts}`;
                displayImage(finalModelUrl, true);
            } else {
                displayDefault();
            }
        }
    });

    $clearImageBtn.off('click').on('click', function() {
        displayDefault(true); // true para limpiar también modelImageUrl y data-original-src
        $imageFileInput.val('');
        // Asegurarse de que sessionStorage también se limpie aquí
        try {
            sessionStorage.removeItem(IMAGE_SESSION_STORAGE_KEY);
        } catch (e) {
            console.warn("No se pudo remover la imagen de sessionStorage al limpiar:", e);
        }
        if (!$('#DeleteImage').length) {
            $('<input>').attr({ type: 'hidden', id: 'DeleteImage', name: 'DeleteImage', value: 'true' }).appendTo('form');
        } else {
            $('#DeleteImage').val('true');
        }
        console.log("Previsualización de imagen limpiada y marcada para borrado.");
    });
    
    const tabObserver = new MutationObserver(function(mutations) {
        mutations.forEach(function(mutation) {
            if (mutation.attributeName === 'class') {
                const $target = $(mutation.target);
                if ($target.is('#tab-info') && $target.hasClass('active')) {
                    console.log("Pestaña de Info activada, restaurando vista de imagen (v5.2 - sessionStorage)...");
                    
                    // Priorizar sessionStorage para la imagen local
                    let imageFromSession = null;
                    try {
                        imageFromSession = sessionStorage.getItem(IMAGE_SESSION_STORAGE_KEY);
                    } catch (e) {
                        console.warn("No se pudo leer de sessionStorage en observer:", e);
                    }

                    if (imageFromSession) {
                        console.log("Restaurando desde sessionStorage (observer)");
                        localImageDataBase64 = imageFromSession; // Sincronizar la variable en memoria
                        displayImage(imageFromSession, false);
                    } else {
                        // Si no hay imagen en sessionStorage, intentar con la imagen del modelo.
                        // Re-leer modelImageUrl desde data-original-src por si se corrompió en memoria.
                        let currentModelUrl = $preview.data('original-src');
                        if (currentModelUrl === '#' || currentModelUrl === '') {
                            currentModelUrl = null;
                        }

                        if (currentModelUrl) {
                            console.log("Restaurando desde modelImageUrl (data-original-src):", currentModelUrl);
                            const timestamp = new Date().getTime();
                            const finalModelUrl = currentModelUrl.includes('?') ? `${currentModelUrl}&t=${timestamp}` : `${currentModelUrl}?t=${timestamp}`;
                            
                            $preview.off('.imagehandler').on('load.imagehandler', function() {
                                // Verificar si la imagen realmente se cargó y tiene dimensiones
                                if (this.naturalWidth > 0 && this.naturalHeight > 0) {
                                    console.log("Imagen del modelo restaurada y visible (desde evento load en observer).");
                                    // displayImage se llamará, y tiene su propia guarda para evitar bucles si el src ya es el correcto.
                                    displayImage(finalModelUrl, true);
                                } else {
                                     console.warn("Evento load disparado en observer, pero imagen parece no válida (dimensiones 0). Modelo URL:", finalModelUrl);
                                }
                            }).on('error.imagehandler', function() {
                                console.error("Error cargando imagen del modelo al restaurar (desde evento error en observer):", finalModelUrl);
                                displayDefault(); 
                            });

                            if ($preview.attr('src') !== finalModelUrl) {
                                console.log("Asignando nuevo src para restaurar:", finalModelUrl);
                                $preview.attr('src', finalModelUrl);
                            } else if ($preview[0] && $preview[0].complete && $preview[0].naturalWidth > 0 && $preview[0].naturalHeight > 0) {
                                console.log("Imagen del modelo (data-original-src) ya en src y completa, ajustando display.");
                                displayImage(finalModelUrl, true);
                            } else if ($preview.attr('src') === '#' || ($preview[0] && !$preview[0].complete)) {
                                 console.log("Src es placeholder o imagen no completa, re-asignando src para restaurar:", finalModelUrl);
                                 $preview.attr('src', finalModelUrl); // Reintentar si está en default o no completa
                            } else {
                                console.log("Src ya es finalModelUrl y no se cumplen otras condiciones, se asume que está cargando o ya manejado.");
                            }

                        } else {
                            console.log("No hay imagen local ni modelImageUrl (data-original-src) para restaurar. Mostrando default.");
                            displayDefault();
                        }
                    }
                }
            }
        });
    });

    const tabInfoElement = document.getElementById('tab-info');
    if (tabInfoElement) {
        tabObserver.observe(tabInfoElement, { attributes: true });
    }
    console.log("Manejador de carga de imágenes v5 inicializado.");
}

// Format functions for Select2
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
    
    return $container;
}

function formatMarcaResult(marca) {
    if (marca.loading) {
        return marca.text;
    }
    
    if (marca.id === 'new') {
        return $('<div class="select2-result-marca">' +
                 '<div class="select2-result-marca__action"><i class="fas fa-plus-circle text-success me-1"></i> ' + 
                 marca.text + '</div>' +
                 '</div>');
    }
    
    return marca.text;
}

function formatMarcaSelection(marca) {
    if (!marca.id || marca.id === 'new') {
        return marca.text;
    }
    
    var $container = $('<div class="d-flex align-items-center justify-content-between w-100"></div>');
    var $marcaNombre = $('<div>' + marca.text + '</div>');
    var $actions = $('<div class="marca-actions ms-2"></div>');
    
    var $editBtn = $('<button type="button" class="btn btn-link btn-sm p-0 me-2 edit-marca" ' +
                    'data-id="' + marca.id + '" data-name="' + marca.text + '">' +
                    '<i class="fas fa-pencil-alt text-primary"></i></button>');
    
    $actions.append($editBtn);
    $container.append($marcaNombre);
    $container.append($actions);
    
    return $container;
}

// Función para abrir el offcanvas de categoría (ELIMINADA DE AQUÍ, form.js lo manejará)
/* --- ELIMINADO ---
function abrirOffcanvasCategoria(nombreCategoria) { ... }
*/

// Función para abrir el offcanvas de marca (ELIMINADA DE AQUÍ, form.js lo manejará)
/* --- ELIMINADO ---
function abrirOffcanvasMarca(nombreMarca) { ... }
*/

// Eventos para editar (ELIMINADOS DE AQUÍ, form.js debería manejar la edición desde su formatSelection)
/* --- ELIMINADO ---
$(document).on('click', '.edit-marca', function(e) { ... });
function editCategoria(id, nombre) { ... }
function editMarca(id, nombre) { ... }
*/

// Initialize category inheritance
function initCategoryInheritance() {
    $('#CategoriaId').off('change.inheritance').on('change.inheritance', function() {
        const categoriaId = $(this).val();
        
        if (!categoriaId) return;
        
        // Skip if it's a new category
        const dataItem = $('#CategoriaId').select2('data')[0];
        if (dataItem && (dataItem._isNew || dataItem.id === 'new')) {
            return;
        }
        
        // Show loading indicator
        Swal.fire({
            title: 'Procesando...',
            text: 'Obteniendo datos de la categoría',
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });
        
        // Get category data
        $.ajax({
            url: `/Categoria/ObtenerDetalle/${categoriaId}`,
            type: 'GET',
            success: function(response) {
                if (response && response.success && response.categoria) {
                    const cat = response.categoria;
                    
                    // Update tax if present
                    if (cat.impuestoId) {
                        $('#ImpuestoId').val(cat.impuestoId).trigger('change');
                    }
                    
                    // The ObtenerDetalle method returns properties in the correct format
                    const mappedResponse = {
                        success: true,
                        cuentaVentasId: cat.cuentaVentasId,
                        cuentaComprasInventariosId: cat.cuentaComprasInventariosId,
                        cuentaCostoVentasGastosId: cat.cuentaCostoVentasGastosId,
                        cuentaDescuentosId: cat.cuentaDescuentosId,
                        cuentaDevolucionesId: cat.cuentaDevolucionesId,
                        cuentaAjustesId: cat.cuentaAjustesId,
                        cuentaCostoMateriaPrimaId: cat.cuentaCostoMateriaPrimaId,
                        impuestoId: cat.impuestoId,
                        propinaImpuestoId: cat.propinaImpuestoId
                    };
                    
                    console.log("Category data from ObtenerDetalle:", mappedResponse);
                    
                    // Trigger event for accounting tab to update
                    $(document).trigger('categoria:changed', [mappedResponse]);
                    
                    Swal.close();
                    
                    // Show success notification
                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'top-end',
                        showConfirmButton: false,
                        timer: 3000,
                        timerProgressBar: true
                    });
                    
                    Toast.fire({
                        icon: 'success',
                        title: 'Datos de categoría aplicados'
                    });
                } else {
                    Swal.fire('Error', 'No se pudieron obtener los datos de la categoría', 'error');
                }
            },
            error: function() {
                Swal.fire('Error', 'Error al obtener datos de la categoría', 'error');
            }
        });
    });
}