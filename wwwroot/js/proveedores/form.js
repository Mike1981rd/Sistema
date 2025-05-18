/**
 * Proveedores Form JS
 * Maneja la funcionalidad del formulario de proveedores, incluyendo:
 * - Validación del formulario
 * - Carga de imágenes con vista previa
 * - Inicialización de Select2 para los campos de búsqueda
 */

$(document).ready(function() {
    // Inicializar Select2
    initializeSelect2();
    
    // Manejo de la subida de imágenes
    setupImageUpload();
    
    // Asegurar que se mantenga como proveedor
    setupProveedorTypeCheckbox();
    
    // Validación personalizada del formulario
    setupFormValidation();
});

/**
 * Inicializar Select2 en los campos de selección
 */
function initializeSelect2() {
    // Inicializar Select2 para tipo de identificación
    $('#TipoIdentificacionId').select2({
        theme: 'bootstrap-5',
        placeholder: '-- Seleccione --',
        allowClear: true,
        width: '100%'
    });
    
    // Inicializar Select2 para otros campos
    $('#PlazoPagoId, #TipoNcfId, #ListaPrecioId, #MunicipioId').select2({
        theme: 'bootstrap-5',
        placeholder: '-- Seleccione --',
        allowClear: true,
        width: '100%'
    });
    
    // Inicializar Select2 para vendedor con búsqueda AJAX
    $('#VendedorId').select2({
        theme: 'bootstrap-5',
        placeholder: 'Buscar vendedor...',
        allowClear: true,
        width: '100%',
        ajax: {
            url: '/Proveedores/BuscarVendedores',
            dataType: 'json',
            delay: 250,
            data: function(params) {
                return {
                    term: params.term || ''
                };
            },
            processResults: function(data) {
                return {
                    results: data.results || []
                };
            },
            cache: true
        }
    });
    
    // Inicializar Select2 para cuentas contables (usando la clase)
    $('.select2-cuenta').select2({
        theme: 'bootstrap-5',
        placeholder: 'Buscar cuenta contable...',
        allowClear: true,
        width: '100%',
        ajax: {
            url: '/Proveedores/BuscarCuentasContables',
            dataType: 'json',
            delay: 250,
            data: function(params) {
                return {
                    term: params.term || ''
                };
            },
            processResults: function(data) {
                return {
                    results: data.results || []
                };
            },
            cache: true
        },
        minimumInputLength: 0  // Cambiar a 0 para permitir búsquedas sin restricción
    });
    
    // Select2 con banderas para país
    $('.select2-flags').select2({
        theme: 'bootstrap-5',
        placeholder: 'Seleccione un país',
        allowClear: true,
        width: '100%',
        templateResult: formatCountryOption,
        templateSelection: formatCountryOption
    });
}

/**
 * Formatear opciones de país con banderas
 */
function formatCountryOption(state) {
    if (!state.id) return state.text;
    var $state = $('<span><img class="flag-icon" src="https://flagcdn.com/w20/' + 
        $(state.element).data('flag') + '.png" /> ' + state.text + '</span>');
    return $state;
}

/**
 * Configurar la subida de imágenes
 */
function setupImageUpload() {
    const fileInput = $('#imagen');
    const profileImage = $('.profile-image');
    const resetButton = $('#reset-image');
    
    // Evento de cambio en el input de archivo
    fileInput.on('change', function() {
        const file = this.files[0];
        
        if (file) {
            showImagePreview(file);
        }
    });
    
    // Resetear imagen
    resetButton.on('click', function() {
        fileInput.val('');
        profileImage.html('<i class="fas fa-user profile-image-placeholder"></i>');
    });
    
    // Mostrar vista previa de la imagen
    function showImagePreview(file) {
        // Verificar que sea una imagen
        if (!file.type.match('image.*')) {
            if (typeof Swal !== 'undefined') {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Por favor, seleccione una imagen válida (JPG, PNG, GIF)'
                });
            } else {
                alert('Por favor, seleccione una imagen válida (JPG, PNG, GIF)');
            }
            fileInput.val('');
            return;
        }
        
        // Verificar tamaño (máximo 800KB)
        const maxSize = 800 * 1024; // 800KB in bytes
        if (file.size > maxSize) {
            if (typeof Swal !== 'undefined') {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'La imagen es demasiado grande. El tamaño máximo es 800KB'
                });
            } else {
                alert('La imagen es demasiado grande. El tamaño máximo es 800KB');
            }
            fileInput.val('');
            return;
        }
        
        // Crear URL temporal y mostrar preview
        const reader = new FileReader();
        reader.onload = function(e) {
            profileImage.html(`<img src="${e.target.result}" alt="Vista previa" style="width: 100%; height: 100%; object-fit: cover; border-radius: 50%;">`);
        };
        reader.readAsDataURL(file);
    }
}

/**
 * Configurar el checkbox de tipo de proveedor (asegurar que siempre se mantenga como proveedor)
 */
function setupProveedorTypeCheckbox() {
    // En el contexto de proveedores, siempre debe estar marcado EsProveedor
    const esProveedorInput = $('input[name="EsProveedor"]');
    if (esProveedorInput.length > 0) {
        esProveedorInput.val('true');
        esProveedorInput.prop('checked', true);
    }
}

/**
 * Configurar validación personalizada del formulario
 */
function setupFormValidation() {
    // Agregar validación personalizada si es necesaria
    const form = $('form');
    if (form.length > 0) {
        // Intercept form submission
        form.on('submit', function(e) {
            e.preventDefault();
            
            // Debug: Log all form fields
            console.log('=== VALIDACIÓN DE FORMULARIO ===');
            $(this).find('input, select, textarea').each(function() {
                console.log(`Campo: ${this.name}, Valor: ${this.value}, Required: ${this.hasAttribute('required')}`);
            });
            
            // Validar solo campos requeridos específicos
            var requiredValid = true;
            var missingFields = [];
            
            $(this).find('[required]').each(function() {
                console.log(`Validando campo requerido: ${this.name}`);
                if (!this.value && this.name !== 'imagen') {
                    requiredValid = false;
                    missingFields.push(this.name);
                    $(this).addClass('is-invalid');
                } else {
                    $(this).removeClass('is-invalid');
                }
            });
            
            if (!requiredValid) {
                console.log('Campos faltantes:', missingFields);
                if (typeof Swal !== 'undefined') {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Campos requeridos',
                        text: 'Por favor complete los siguientes campos: ' + missingFields.join(', ')
                    });
                } else {
                    alert('Por favor complete los campos requeridos: ' + missingFields.join(', '));
                }
                $(this).addClass('was-validated');
                return;
            }
            
            // Show loading state
            var submitBtn = $(this).find('button[type="submit"]');
            var originalText = submitBtn.html();
            submitBtn.prop('disabled', true).html('<i class="fas fa-spinner fa-spin"></i> Guardando...');
            
            // Get form data
            var formData = new FormData(this);
            
            // Log form data for debugging
            console.log('Enviando formulario de proveedor...');
            
            // Check and remove empty file input if present
            var imageInput = $(this).find('input[name="imagen"]')[0];
            if (imageInput && (!imageInput.files || imageInput.files.length === 0)) {
                formData.delete('imagen');
                console.log('No hay imagen seleccionada, removiendo campo imagen');
            }
            
            for (var pair of formData.entries()) {
                console.log(pair[0] + ': ' + pair[1]);
                
                // Special check for image field
                if (pair[0] === 'imagen' && pair[1] instanceof File) {
                    console.log('Imagen info:', {
                        name: pair[1].name,
                        size: pair[1].size,
                        type: pair[1].type
                    });
                    
                    // If file is empty, remove it
                    if (pair[1].size === 0) {
                        formData.delete('imagen');
                        console.log('Archivo de imagen vacío, removiendo');
                    }
                }
            }
            
            // Se asegura de que EsProveedor siempre sea true
            formData.set('EsProveedor', 'true');
            
            // Log final form data
            console.log('=== DATOS A ENVIAR ===');
            for (var pair of formData.entries()) {
                console.log(`${pair[0]}: ${pair[1]}`);
            }
            
            // Determinar la URL según si hay imagen o no
            var hasImage = false;
            for (var pair of formData.entries()) {
                if (pair[0] === 'imagen' && pair[1] instanceof File && pair[1].size > 0) {
                    hasImage = true;
                    break;
                }
            }
            
            var actionUrl = hasImage ? '/Proveedores/Create' : '/Proveedores/CreateWithoutImage';
            console.log('URL de envío:', actionUrl, 'Tiene imagen:', hasImage);
            
            $.ajax({
                url: actionUrl,
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                },
                beforeSend: function(xhr) {
                    console.log('Enviando petición AJAX a:', actionUrl);
                },
                success: function(response) {
                    console.log('Respuesta del servidor:', response);
                    if (response.success) {
                        // Show success message
                        if (typeof Swal !== 'undefined') {
                            Swal.fire({
                                icon: 'success',
                                title: 'Éxito',
                                text: response.message || 'Proveedor creado exitosamente',
                                timer: 1500,
                                showConfirmButton: false
                            }).then(() => {
                                window.location.href = '/Proveedores';
                            });
                        } else {
                            alert(response.message || 'Proveedor creado exitosamente');
                            window.location.href = '/Proveedores';
                        }
                    } else {
                        // Show error message
                        var errorMsg = response.message || 'Error al crear el proveedor';
                        if (response.errors) {
                            errorMsg += '\n' + Object.values(response.errors).flat().join('\n');
                        }
                        
                        if (typeof Swal !== 'undefined') {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                html: errorMsg.replace(/\n/g, '<br>'),
                                confirmButtonText: 'OK'
                            });
                        } else {
                            alert(errorMsg);
                        }
                        
                        // Re-enable submit button
                        submitBtn.prop('disabled', false).html(originalText);
                    }
                },
                error: function(xhr, status, error) {
                    console.error('Error AJAX:', {
                        status: xhr.status,
                        statusText: xhr.statusText,
                        responseText: xhr.responseText,
                        error: error
                    });
                    
                    var errorMsg = 'Error ' + xhr.status + ': ';
                    try {
                        var response = JSON.parse(xhr.responseText);
                        errorMsg += response.message || error;
                        if (response.details) {
                            errorMsg += '\nDetalles: ' + response.details;
                        }
                    } catch (e) {
                        errorMsg += error;
                    }
                    
                    if (typeof Swal !== 'undefined') {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            html: errorMsg.replace(/\n/g, '<br>'),
                            confirmButtonText: 'OK'
                        });
                    } else {
                        alert(errorMsg);
                    }
                    
                    // Re-enable submit button
                    submitBtn.prop('disabled', false).html(originalText);
                }
            });
        });
    }
}