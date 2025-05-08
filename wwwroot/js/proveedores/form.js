/**
 * Proveedores Form JS
 * Maneja la funcionalidad del formulario de proveedores, incluyendo:
 * - Validación del formulario
 * - Carga de imágenes con vista previa
 * - Inicialización de Select2 para los campos de búsqueda
 */

$(document).ready(function() {
    // Manejo de la subida de imágenes
    setupImageUpload();
    
    // Asegurar que se mantenga como proveedor
    setupProveedorTypeCheckbox();
    
    // Validación personalizada del formulario
    setupFormValidation();
});

/**
 * Configurar la subida de imágenes con arrastrar y soltar
 */
function setupImageUpload() {
    const dropzoneArea = $('#dropzone-area');
    const fileInput = $('#imagen');
    const previewContainer = $('#preview-container');
    const fileName = $('#file-name');
    const fileSize = $('#file-size');
    const removeFileBtn = $('#remove-file');
    
    // Evento de clic en la zona de drop para abrir el selector de archivos
    dropzoneArea.on('click', function() {
        fileInput.click();
    });
    
    // Evento de cambio en el input de archivo
    fileInput.on('change', function() {
        const file = this.files[0];
        
        if (file) {
            showFilePreview(file);
        }
    });
    
    // Arrastrar y soltar
    dropzoneArea.on('dragover', function(e) {
        e.preventDefault();
        dropzoneArea.addClass('dropzone-active');
    });
    
    dropzoneArea.on('dragleave', function() {
        dropzoneArea.removeClass('dropzone-active');
    });
    
    dropzoneArea.on('drop', function(e) {
        e.preventDefault();
        dropzoneArea.removeClass('dropzone-active');
        
        const file = e.originalEvent.dataTransfer.files[0];
        
        if (file) {
            fileInput[0].files = e.originalEvent.dataTransfer.files;
            showFilePreview(file);
        }
    });
    
    // Eliminar archivo seleccionado
    removeFileBtn.on('click', function() {
        fileInput.val('');
        previewContainer.addClass('d-none');
        dropzoneArea.removeClass('d-none');
    });
    
    // Mostrar vista previa del archivo
    function showFilePreview(file) {
        // Verificar que sea una imagen
        if (!file.type.match('image.*')) {
            alert('Por favor, seleccione una imagen válida.');
            return;
        }
        
        fileName.text(file.name);
        fileSize.text(formatFileSize(file.size));
        
        dropzoneArea.addClass('d-none');
        previewContainer.removeClass('d-none');
    }
    
    // Formatear tamaño del archivo
    function formatFileSize(bytes) {
        if (bytes === 0) return '0 Bytes';
        
        const k = 1024;
        const sizes = ['Bytes', 'KB', 'MB', 'GB'];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        
        return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
    }
}

/**
 * Configurar el checkbox de tipo de proveedor (asegurar que siempre se mantenga como proveedor)
 */
function setupProveedorTypeCheckbox() {
    const esProveedorCheckbox = $('#EsProveedor');
    const esClienteCheckbox = $('#EsCliente');
    
    // Asegurar que siempre esté marcado como proveedor
    esProveedorCheckbox.prop('checked', true);
    
    // Prevenir desmarcarlo
    esProveedorCheckbox.on('change', function() {
        if (!$(this).prop('checked')) {
            // Forzar a marcado
            $(this).prop('checked', true);
        }
    });
}

/**
 * Configurar la validación del formulario
 */
function setupFormValidation() {
    $('form').on('submit', function(e) {
        const nombreRazonSocial = $('#NombreRazonSocial').val().trim();
        const tipoIdentificacionId = $('#TipoIdentificacionId').val();
        const numeroIdentificacion = $('#NumeroIdentificacion').val().trim();
        
        // Validar campos requeridos
        if (!nombreRazonSocial) {
            e.preventDefault();
            alert('El nombre o razón social es obligatorio.');
            return false;
        }
        
        if (!tipoIdentificacionId) {
            e.preventDefault();
            alert('El tipo de identificación es obligatorio.');
            return false;
        }
        
        if (!numeroIdentificacion) {
            e.preventDefault();
            alert('El número de identificación es obligatorio.');
            return false;
        }
        
        // Preparar el valor del límite de crédito para envío
        prepareDecimalFieldsForSubmission();
        
        // Asegurar que esté marcado como proveedor
        $('#EsProveedor').prop('checked', true);
        
        // Todo está bien, permitir el envío del formulario
        return true;
    });
}

/**
 * Prepara los campos decimales para su envío, asegurando que tengan el formato correcto
 */
function prepareDecimalFieldsForSubmission() {
    // Preparar el campo LimiteCredito para su envío
    const limiteCreditoInput = document.getElementById('LimiteCredito');
    if (limiteCreditoInput && limiteCreditoInput.cleave) {
        // Obtener el valor sin formato y normalizarlo para el servidor
        const rawValue = limiteCreditoInput.cleave.getRawValue();
        // Si hay un valor, asegurar que tenga el formato correcto para el servidor
        if (rawValue) {
            // Usar punto como separador decimal para el valor que se envía al servidor
            const numericValue = parseFloat(rawValue.replace(',', '.'));
            // Asignar el valor numérico directamente al campo
            limiteCreditoInput.value = numericValue;
        }
    }
}

/**
 * Inicializa los campos Select2 para búsqueda de cuentas contables
 */
function initSelect2() {
    // Inicializar Select2 para cuentas contables
    $('.select2-cuenta').select2({
        theme: 'bootstrap-5',
        placeholder: 'Buscar cuenta contable...',
        allowClear: true,
        width: '100%',
        dropdownParent: $('body'), // Asegura que el dropdown se muestre correctamente
        data: window.cuentasContablesData || [],
        matcher: function(params, data) {
            // Si no hay búsqueda, mostrar todo
            if ($.trim(params.term) === '') {
                return data;
            }
            
            // Convertir a minúsculas para búsqueda sin distinguir mayúsculas/minúsculas
            const term = params.term.toLowerCase();
            
            // Buscar en el texto completo
            if (data.text.toLowerCase().indexOf(term) > -1) {
                return data;
            }
            
            // Buscar en datos adicionales (código y nombre por separado)
            if (data.codigo && data.codigo.toLowerCase().indexOf(term) > -1) {
                return data;
            }
            
            if (data.nombre && data.nombre.toLowerCase().indexOf(term) > -1) {
                return data;
            }
            
            return null;
        }
    });
    
    // Inicializar Select2 para Vendedores con opción de creación
    $('#VendedorId').select2({
        theme: 'bootstrap-5',
        placeholder: 'Seleccione o cree un vendedor',
        allowClear: true,
        width: '100%',
        dropdownParent: $('body'),
        ajax: {
            url: window.location.origin + '/Proveedores/BuscarVendedores',
            dataType: 'json',
            delay: 250,
            data: function(params) {
                console.log("Enviando búsqueda:", params.term);
                return {
                    term: params.term || ''
                };
            },
            processResults: function(data, params) {
                console.log("Resultados recibidos:", data);
                // Añadir opción para crear nuevo vendedor si la búsqueda no coincide exactamente
                var results = data.results || [];
                
                // Usar params.term que es el término de búsqueda actual
                if (results.length == 0 && params.term && params.term.trim() !== '') {
                    console.log("Agregando opción para crear nuevo vendedor:", params.term);
                    results.push({
                        id: 'new',
                        text: 'Crear vendedor: "' + params.term + '"',
                        term: params.term
                    });
                }
                
                return {
                    results: results
                };
            },
            error: function(xhr, status, error) {
                console.error("Error en la búsqueda AJAX:", error);
                console.error("Estado:", status);
                console.error("Respuesta:", xhr.responseText);
            },
            cache: true
        },
        templateResult: formatVendedorResult,
        templateSelection: formatVendedorSelection
    }).on('select2:select', function(e) {
        var data = e.params.data;
        console.log("Elemento seleccionado:", data);
        
        // Si se selecciona la opción "Crear nuevo vendedor"
        if (data.id === 'new') {
            // Eliminar la selección actual
            $(this).val(null).trigger('change');
            
            // Mostrar modal de confirmación
            if (confirm('¿Desea crear el vendedor "' + data.term + '"?')) {
                console.log("Creando vendedor:", data.term);
                // Crear el vendedor mediante AJAX
                $.ajax({
                    url: window.location.origin + '/Proveedores/CrearVendedor',
                    method: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({ 
                        nombre: data.term 
                    }),
                    success: function(response) {
                        console.log("Respuesta del servidor:", response);
                        if (response.success) {
                            // Añadir el nuevo vendedor a las opciones
                            var newOption = new Option(response.vendedor.text, response.vendedor.id, true, true);
                            $('#VendedorId').append(newOption).trigger('change');
                            
                            // Mostrar notificación de éxito
                            alert('Vendedor creado exitosamente');
                        } else {
                            alert('Error: ' + response.message);
                        }
                    },
                    error: function(xhr, status, error) {
                        console.error("Error al crear vendedor:", error);
                        console.error("Estado:", status);
                        console.error("Respuesta:", xhr.responseText);
                        alert('Error al crear el vendedor: ' + error);
                    }
                });
            }
        }
    });
    
    // Inicializar Select2 para países con banderas
    $('.select2-flags').select2({
        theme: 'bootstrap-5',
        placeholder: "Seleccione un país",
        allowClear: true,
        width: '100%',
        dropdownParent: $('body')
    });
    
    // Aplicar formatos especiales
    initFormatters();
}

/**
 * Inicializa los formateadores para campos específicos
 */
function initFormatters() {
    // Formatear el límite de crédito como moneda usando Cleave.js
    if (document.getElementById('LimiteCredito')) {
        // Obtener el separador decimal de la configuración global
        const separadorDecimal = window.appConfig?.separadorDecimal || ',';
        
        // Nueva instancia de Cleave con la configuración correcta para separador decimal (,)
        const cleaveInstance = new Cleave('#LimiteCredito', {
            numeral: true,
            numeralThousandsGroupStyle: 'thousand',
            numeralDecimalMark: separadorDecimal,
            numeralDecimalScale: 2,
            delimiter: (separadorDecimal === ',' ? '.' : ','), // Si el decimal es coma, el delimitador debe ser punto
            numeralPositiveOnly: true // Solo permitir números positivos
        });
        
        // Guardar referencia a la instancia de Cleave en el elemento para poder acceder después
        document.getElementById('LimiteCredito').cleave = cleaveInstance;
    }
}

/**
 * Maneja la carga y preview de la imagen
 */
function initImageUpload() {
    const imageInput = document.getElementById('imagen');
    const profileImage = document.querySelector('.profile-image');
    const resetBtn = document.getElementById('reset-image');
    
    if (!imageInput || !profileImage) return;
    
    // Manejar cambio de archivo
    imageInput.addEventListener('change', function() {
        if (imageInput.files && imageInput.files[0]) {
            const file = imageInput.files[0];
            
            // Validar tipo de archivo
            const validTypes = ['image/jpeg', 'image/png', 'image/gif'];
            if (!validTypes.includes(file.type)) {
                alert('Por favor seleccione un archivo de imagen válido (JPG, PNG o GIF)');
                imageInput.value = '';
                return;
            }
            
            // Validar tamaño máximo (800KB)
            if (file.size > 800 * 1024) {
                alert('El tamaño de la imagen no debe exceder 800KB');
                imageInput.value = '';
                return;
            }
            
            const reader = new FileReader();
            reader.onload = function(e) {
                // Eliminar placeholder y cualquier imagen previa
                profileImage.innerHTML = '';
                
                // Crear elemento de imagen 
                const img = document.createElement('img');
                img.src = e.target.result;
                img.alt = 'Vista previa';
                img.style.width = '100%';
                img.style.height = '100%';
                img.style.objectFit = 'cover';
                
                // Añadir la imagen a la previsualización
                profileImage.appendChild(img);
                
                // Mostrar botón de reset
                if (resetBtn) resetBtn.style.display = 'inline-block';
            };
            
            reader.readAsDataURL(file);
        }
    });
    
    // Manejar reset de imagen
    if (resetBtn) {
        resetBtn.addEventListener('click', function() {
            // Limpiar el input de archivo
            imageInput.value = '';
            
            // Si estamos en modo edición y hay una imagen existente, restaurarla
            if (profileImage.dataset.originalImage) {
                profileImage.innerHTML = `<img src="${profileImage.dataset.originalImage}" alt="Imagen original" style="width:100%;height:100%;object-fit:cover;">`;
            } else {
                // Restaurar el placeholder
                profileImage.innerHTML = '<i class="fas fa-user profile-image-placeholder"></i>';
            }
            
            // Ocultar botón de reset si estamos en modo creación
            if (!profileImage.dataset.originalImage) {
                resetBtn.style.display = 'none';
            }
        });
    }
    
    // Guardar referencia a la imagen original (solo en modo edición)
    if (profileImage.querySelector('img')) {
        const originalImageSrc = profileImage.querySelector('img').src;
        profileImage.dataset.originalImage = originalImageSrc;
    }
}

/**
 * Inicializa el manejo de pestañas
 */
function initTabs() {
    $('.nav-tabs-underline .nav-link').on('click', function(e) {
        e.preventDefault();
        
        // Remover la clase active de todas las pestañas y contenidos
        $('.nav-tabs-underline .nav-link').removeClass('active');
        $('.form-tab-pane').removeClass('active');
        
        // Añadir la clase active a la pestaña actual y su contenido
        $(this).addClass('active');
        const targetId = $(this).attr('href');
        $(targetId).addClass('active');
    });
}

/**
 * Añade indicadores visuales para el scroll horizontal de pestañas
 */
function setupTabsScroll() {
    // Seleccionar el contenedor de pestañas
    const tabsContainer = $('.nav-tabs-underline');
    
    // Verificar si se necesita scroll
    function checkScroll() {
        if (tabsContainer.length === 0) return;
        
        const scrollWidth = tabsContainer[0].scrollWidth;
        const clientWidth = tabsContainer[0].clientWidth;
        
        // Si hay scroll disponible, agregar clase para mostrar indicador
        if (scrollWidth > clientWidth) {
            tabsContainer.addClass('has-more-tabs');
            
            // Verificar posición de scroll
            if (tabsContainer.scrollLeft() > 10) {
                tabsContainer.addClass('scrolled-right');
            } else {
                tabsContainer.removeClass('scrolled-right');
            }
            
            if (tabsContainer.scrollLeft() + clientWidth < scrollWidth - 10) {
                tabsContainer.addClass('scrolled-left');
            } else {
                tabsContainer.removeClass('scrolled-left');
            }
        } else {
            tabsContainer.removeClass('has-more-tabs scrolled-right scrolled-left');
        }
    }
    
    // Ejecutar en la carga inicial
    checkScroll();
    
    // Ejecutar cuando cambie el tamaño de la ventana
    $(window).on('resize', checkScroll);
    
    // Ejecutar cuando se haga scroll en las pestañas
    tabsContainer.on('scroll', checkScroll);
}

/**
 * Inicialización al cargar la página
 */
$(document).ready(function() {
    initTabs();
    initSelect2();
    initImageUpload();
    setupTabsScroll();
});

/**
 * Formatea la visualización de resultados de vendedores en el dropdown
 */
function formatVendedorResult(vendedor) {
    if (vendedor.loading) {
        return vendedor.text;
    }
    
    if (vendedor.id === 'new') {
        return $('<div class="select2-result-vendedor">' +
                 '<div class="select2-result-vendedor__action"><i class="fas fa-plus-circle text-success me-1"></i> ' + 
                 vendedor.text + '</div>' +
                 '</div>');
    }
    
    return $('<div class="select2-result-vendedor">' +
             '<div class="select2-result-vendedor__name">' + vendedor.text + '</div>' +
             '</div>');
}

/**
 * Formatea la visualización del vendedor seleccionado con opciones de edición
 */
function formatVendedorSelection(vendedor) {
    if (!vendedor.id || vendedor.id === 'new') {
        return vendedor.text;
    }
    
    // Añadir iconos de edición y eliminación junto al nombre del vendedor
    var $container = $('<div class="d-flex align-items-center justify-content-between w-100"></div>');
    var $vendedorName = $('<div>' + vendedor.text + '</div>');
    var $actions = $('<div class="vendedor-actions ms-2"></div>');
    
    var $editBtn = $('<button type="button" class="btn btn-link btn-sm p-0 me-2 edit-vendedor" ' +
                    'data-id="' + vendedor.id + '" data-name="' + vendedor.text + '">' +
                    '<i class="fas fa-pencil-alt text-primary"></i></button>');
    
    var $deleteBtn = $('<button type="button" class="btn btn-link btn-sm p-0 delete-vendedor" ' +
                      'data-id="' + vendedor.id + '" data-name="' + vendedor.text + '">' +
                      '<i class="fas fa-trash-alt text-danger"></i></button>');
    
    $actions.append($editBtn);
    $actions.append($deleteBtn);
    
    $container.append($vendedorName);
    $container.append($actions);
    
    // Manejar eventos de botones
    setTimeout(function() {
        $('.edit-vendedor').off('click').on('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            editVendedor($(this).data('id'), $(this).data('name'));
        });
        
        $('.delete-vendedor').off('click').on('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            deleteVendedor($(this).data('id'), $(this).data('name'));
        });
    }, 100);
    
    return $container;
}

/**
 * Edita un vendedor existente
 */
function editVendedor(id, currentName) {
    var newName = prompt('Editar nombre del vendedor:', currentName);
    
    if (newName && newName.trim() !== '' && newName !== currentName) {
        $.ajax({
            url: window.location.origin + '/Proveedores/EditarVendedor',
            method: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify({ 
                id: id,
                nombre: newName
            }),
            success: function(response) {
                if (response.success) {
                    // Actualizar opción en el select
                    var $option = $('#VendedorId option[value="' + id + '"]');
                    $option.text(response.vendedor.text);
                    
                    // Si está seleccionado, actualizar la visualización
                    if ($('#VendedorId').val() == id) {
                        $('#VendedorId').trigger('change');
                    }
                    
                    alert('Vendedor actualizado correctamente');
                } else {
                    alert('Error: ' + response.message);
                }
            },
            error: function(xhr) {
                var errorMsg = 'Error al actualizar el vendedor';
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    errorMsg = xhr.responseJSON.message;
                }
                alert(errorMsg);
            }
        });
    }
}

/**
 * Elimina un vendedor existente
 */
function deleteVendedor(id, name) {
    if (confirm('¿Está seguro de que desea eliminar al vendedor "' + name + '"?')) {
        $.ajax({
            url: window.location.origin + '/Proveedores/EliminarVendedor',
            method: 'DELETE',
            contentType: 'application/json',
            data: JSON.stringify({ id: id }),
            success: function(response) {
                if (response.success) {
                    // Quitar el vendedor de la lista
                    var $select = $('#VendedorId');
                    var currentVal = $select.val();
                    
                    // Remover la opción
                    $select.find('option[value="' + id + '"]').remove();
                    
                    // Si era el vendedor seleccionado, limpiar selección
                    if (currentVal == id) {
                        $select.val(null).trigger('change');
                    }
                    
                    alert(response.message);
                } else {
                    alert('Error: ' + response.message);
                }
            },
            error: function() {
                alert('Error al eliminar el vendedor');
            }
        });
    }
} 