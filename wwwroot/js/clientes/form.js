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
        
        return true;
    });
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
    $('#PaisId').select2({
        theme: 'bootstrap-5',
        placeholder: 'Seleccione un país',
        allowClear: true,
        width: '100%',
        dropdownParent: $('body'),
        templateResult: formatCountryOption,
        templateSelection: formatCountrySelection
    });
    
    // Aplicar formatos especiales
    initFormatters();
}

/**
 * Formatea las opciones del país con banderas
 */
function formatCountryOption(country) {
    if (!country.id) return country.text;
    
    // En un entorno real, aquí obtendrías la bandera del país
    // Para este ejemplo, usaremos banderas de ejemplo
    const countryCode = country.id.toString().padStart(2, '0');
    const flagUrl = `/images/flags/${countryCode}.png`;
    
    return $(`
        <div class="country-option">
            <img src="${flagUrl}" class="country-flag" onerror="this.src='/images/flags/placeholder.png'" />
            <span class="country-name">${country.text}</span>
        </div>
    `);
}

/**
 * Formatea la opción seleccionada del país con bandera
 */
function formatCountrySelection(country) {
    if (!country.id) return country.text;
    
    const countryCode = country.id.toString().padStart(2, '0');
    const flagUrl = `/images/flags/${countryCode}.png`;
    
    return $(`
        <div class="country-option">
            <img src="${flagUrl}" class="country-flag" onerror="this.style.display='none'" />
            <span class="country-name">${country.text}</span>
        </div>
    `);
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
 * Inicializa formateadores para campos específicos
 */
function initFormatters() {
    // Formatear el límite de crédito como moneda
    $('#LimiteCredito').on('input', function() {
        formatCurrency(this);
    });
    
    // Formatear inicialmente
    formatCurrency(document.getElementById('LimiteCredito'));
}

/**
 * Formatea un valor como moneda según las preferencias de la empresa
 */
function formatCurrency(input) {
    // Quitar todo excepto números y punto/coma
    let value = input.value.replace(/[^\d.,]/g, '');
    
    // Si está vacío, no hacemos nada
    if (!value) return;
    
    // Convertir a un número (asumiendo que el separador decimal es punto)
    // En un entorno real, esto debería venir de la configuración de la empresa
    const decimalSeparator = ','; // o '.' dependiendo de la configuración
    const thousandsSeparator = decimalSeparator === ',' ? '.' : ',';
    
    // Eliminar separadores existentes para trabajar con un número limpio
    value = value.replace(new RegExp('\\' + thousandsSeparator, 'g'), '');
    value = value.replace(new RegExp('\\' + decimalSeparator, 'g'), '.');
    
    // Convertir a número y formatear
    let numValue = parseFloat(value);
    if (isNaN(numValue)) return;
    
    // Formatear con 2 decimales
    const formatted = numValue.toFixed(2);
    
    // Convertir a formato local
    const parts = formatted.split('.');
    const integerPart = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, thousandsSeparator);
    const decimalPart = parts[1];
    
    // Actualizar el valor del input
    input.value = integerPart + decimalSeparator + decimalPart;
}

/**
 * Maneja la carga y preview de la imagen
 */
function initImageUpload() {
    const imageInput = document.getElementById('imagen');
    const previewContainer = document.getElementById('preview-container');
    const dropzoneArea = document.getElementById('dropzone-area');
    const fileNameElem = document.getElementById('file-name');
    const fileSizeElem = document.getElementById('file-size');
    const removeFileBtn = document.getElementById('remove-file');
    
    if (!imageInput || !previewContainer || !dropzoneArea) return;
    
    // Manejo de arrastrar y soltar
    dropzoneArea.addEventListener('dragover', function(e) {
        e.preventDefault();
        dropzoneArea.classList.add('dragover');
    });
    
    dropzoneArea.addEventListener('dragleave', function() {
        dropzoneArea.classList.remove('dragover');
    });
    
    dropzoneArea.addEventListener('drop', function(e) {
        e.preventDefault();
        dropzoneArea.classList.remove('dragover');
        
        if (e.dataTransfer.files.length) {
            imageInput.files = e.dataTransfer.files;
            handleFileSelect();
        }
    });
    
    // Manejar clic en el área
    dropzoneArea.addEventListener('click', function() {
        imageInput.click();
    });
    
    // Manejar cambio de archivo
    imageInput.addEventListener('change', handleFileSelect);
    
    // Botón para eliminar archivo
    if (removeFileBtn) {
        removeFileBtn.addEventListener('click', function() {
            imageInput.value = '';
            previewContainer.classList.add('d-none');
            dropzoneArea.classList.remove('d-none');
            fileNameElem.textContent = '';
            fileSizeElem.textContent = '';
        });
    }
    
    function handleFileSelect() {
        if (imageInput.files && imageInput.files[0]) {
            const file = imageInput.files[0];
            
            // Validar tipo de archivo
            const validTypes = ['image/jpeg', 'image/png', 'image/gif'];
            if (!validTypes.includes(file.type)) {
                alert('Por favor seleccione un archivo de imagen válido (JPG, PNG o GIF)');
                imageInput.value = '';
                return;
            }
            
            // Validar tamaño (máximo 800KB)
            const maxSize = 800 * 1024; // 800KB en bytes
            if (file.size > maxSize) {
                alert('La imagen es demasiado grande. El tamaño máximo permitido es 800KB.');
                imageInput.value = '';
                return;
            }
            
            // Mostrar información del archivo
            fileNameElem.textContent = file.name;
            fileSizeElem.textContent = formatFileSize(file.size);
            
            // Mostrar contenedor de preview y ocultar dropzone
            previewContainer.classList.remove('d-none');
            dropzoneArea.classList.add('d-none');
            
            // En un caso real, también podríamos mostrar una previsualización de la imagen
        }
    }
    
    function formatFileSize(bytes) {
        if (bytes < 1024) return bytes + ' bytes';
        if (bytes < 1024 * 1024) return (bytes / 1024).toFixed(1) + ' KB';
        return (bytes / (1024 * 1024)).toFixed(1) + ' MB';
    }
}

/**
 * Inicialización al cargar la página
 */
$(document).ready(function() {
    initTabs();
    initSelect2();
    initImageUpload();
}); 