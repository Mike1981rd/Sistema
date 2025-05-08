/**
 * Clientes Form JS
 * Maneja la funcionalidad del formulario de clientes, incluyendo:
 * - Validación del formulario
 * - Carga de imágenes con vista previa
 * - Inicialización de Select2 para los campos de búsqueda
 */

$(document).ready(function() {
    // Manejo de la subida de imágenes
    setupImageUpload();
    
    // Asegurar que al menos uno de los checkboxes esté seleccionado
    setupClientTypeCheckboxes();
    
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
 * Configurar los checkboxes de tipo de cliente/proveedor
 */
function setupClientTypeCheckboxes() {
    const esClienteCheckbox = $('#EsCliente');
    const esProveedorCheckbox = $('#EsProveedor');
    
    // Al cambiar un checkbox, verificar que al menos uno esté seleccionado
    esClienteCheckbox.add(esProveedorCheckbox).on('change', function() {
        if (!esClienteCheckbox.prop('checked') && !esProveedorCheckbox.prop('checked')) {
            // Si ambos están desmarcados, marcar el que se acaba de desmarcar
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
 * Inicialización al cargar la página
 */
$(document).ready(function() {
    initTabs();
    initSelect2();
    initImageUpload();
});

/**
 * Inicializa los campos Select2 para una mejor experiencia de usuario
 */
function initSelectFields() {
    // Inicializar select2 para países con banderas
    $('.select2-country').select2({
        placeholder: "Seleccione un país",
        allowClear: true,
        theme: "bootstrap-5",
        width: '100%',
        templateResult: formatCountryOption,
        templateSelection: formatCountrySelection
    }).on('select2:open', function() {
        document.querySelector('.select2-search__field').focus();
    });
} 