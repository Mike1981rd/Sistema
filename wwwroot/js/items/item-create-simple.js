// Versión simplificada basada en gestionar-producto.js que sí funciona
$(document).ready(function() {
    // Marcar como inicializado para evitar ejecuciones duplicadas
    window.itemCreateInitialized = true;
    console.log("=== ITEM CREATE INICIALIZADO ===");
    
    // Inicializar Select2 para categorías
    var $categoriaSelect = $('#CategoriaId'); // Nota: CategoriaId con mayúscula
    
    initCategoriaSelect2();
    
    // Actualizamos solo el impuesto sin herencia de datos contables
    $categoriaSelect.on('change', function() {
        const categoriaId = $(this).val();
        
        if (!categoriaId || categoriaId === 'new') return;
        
        // Considerar emitir el evento para que otros componentes puedan reaccionar
        $(document).trigger('categoria:seleccionada', [categoriaId]);
    });
    
    // Inicializar Select2 para marca
    initMarcaSelect2();
    
    // Evento para el botón de edición en la selección (después de seleccionar)
    $(document).on('click', '.edit-categoria', function(e) {
        e.preventDefault();
        e.stopPropagation();
        var id = $(this).data('id');
        var nombre = $(this).data('name');
        editCategoria(id, nombre);
    });
    
    // Evento para el botón de edición de marca
    $(document).on('click', '.edit-marca', function(e) {
        e.preventDefault();
        e.stopPropagation();
        var id = $(this).data('id');
        var nombre = $(this).data('name');
        editMarca(id, nombre);
    });
    
    // Handlers globales para los nuevos botones de marca
    $(document).on('click', '#btnGuardarMarcaNuevo', function(e) {
        e.preventDefault();
        console.log('Botón guardar marca clickeado (handler global)');
        const $form = $(this).closest('form');
        if ($form.length) {
            console.log('Formulario encontrado:', $form);
            const offcanvasElement = $(this).closest('.offcanvas');
            const offcanvasBS = bootstrap.Offcanvas.getInstance(offcanvasElement);
            guardarMarca($form, offcanvasBS);
        } else {
            console.error('No se encontró el formulario');
        }
    });
    
    $(document).on('click', '#btnCancelarMarca', function(e) {
        e.preventDefault();
        console.log('Botón cancelar marca clickeado');
        const offcanvasElement = $(this).closest('.offcanvas');
        const offcanvasBS = bootstrap.Offcanvas.getInstance(offcanvasElement);
        if (offcanvasBS) {
            offcanvasBS.hide();
        }
    });
    
    // Manejador para guardar categoría desde el offcanvas
    $(document).on('submit', '#formCrearCategoria, #formEditarCategoria', function(e) {
        e.preventDefault();
        
        const $form = $(this);
        const formData = $form.serialize();
        const isEdit = $form.attr('id') === 'formEditarCategoria';
        
        $.ajax({
            url: $form.attr('action'),
            type: $form.attr('method') || 'POST',
            data: formData,
            success: function(response) {
                if (response.success) {
                    // Cerrar offcanvas
                    const offcanvasElement = document.getElementById('offcanvasCategoria');
                    const offcanvasBS = bootstrap.Offcanvas.getInstance(offcanvasElement);
                    offcanvasBS.hide();
                    
                    Swal.fire('Éxito', 'Categoría guardada correctamente', 'success');
                    
                    // Solución radical: reemplazar completamente el select
                    setTimeout(function() {
                        // 1. Guardar el valor actual e información para restaurarlo después
                        const categoriaId = response.categoria.id;
                        const categoriaNombre = response.categoria.nombre;
                        
                        // 2. Obtener el elemento padre del select actual
                        const $parent = $('#CategoriaId').parent();
                        const selectName = $('#CategoriaId').attr('name');
                        const selectId = $('#CategoriaId').attr('id');
                        const isRequired = $('#CategoriaId').prop('required');
                        const selectClass = $('#CategoriaId').attr('class');
                        
                        // 3. Eliminar todo el contenido relacionado con select2
                        $parent.find('.select2-container').remove();
                        $('#CategoriaId').remove();
                        
                        // 4. Crear un nuevo elemento select desde cero
                        const newSelect = $('<select></select>')
                            .attr('id', selectId)
                            .attr('name', selectName)
                            .addClass(selectClass);
                            
                        if (isRequired) {
                            newSelect.prop('required', true);
                        }
                        
                        // Agregar la opción placeholder
                        newSelect.append('<option value="">Seleccione una categoría</option>');
                        
                        // Agregar al DOM
                        $parent.append(newSelect);
                        
                        // 5. Inicializar el nuevo select2
                        initCategoriaSelect2();
                        
                        // 6. Marcar elemento con timestamp para que podamos identificarlo más tarde
                        newSelect.attr('data-regenerated', new Date().getTime());
                        
                        // 7. Establecer el valor de la categoría guardada
                        const newOption = new Option(categoriaNombre, categoriaId, true, true);
                        newSelect.append(newOption).trigger('change');
                        
                        // 8. Registrar nuevamente el evento de cambio
                        newSelect.on('change', function() {
                            const categoriaId = $(this).val();
                            
                            if (!categoriaId || categoriaId === 'new') return;
                            
                            // Simplemente emitir el evento para que otros componentes puedan reaccionar
                            $(document).trigger('categoria:seleccionada', [categoriaId]);
                        });
                        
                        // 9. Limpiar el offcanvas para evitar problemas con formularios antiguos
                        $('#offcanvasCategoria .offcanvas-body').html('<div id="formCategoriaContainer"></div>');
                        
                        // 10. Enfocar brevemente para forzar la recarga de opciones
                        setTimeout(function() {
                            newSelect.select2('open');
                            newSelect.select2('close');
                        }, 200);
                    }, 500);
                } else {
                    Swal.fire('Error', response.message || 'Error al guardar', 'error');
                }
            },
            error: function() {
                Swal.fire('Error', 'Error al guardar la categoría', 'error');
            }
        });
    });
    
    // Funciones auxiliares
    function formatCategoriaResult(categoria) {
        if (categoria.id === 'new') {
            return $('<div><i class="fas fa-plus-circle text-success me-1"></i> ' + categoria.text + '</div>');
        }
        return categoria.text;
    }
    
    function formatCategoriaSelection(categoria) {
        if (!categoria.id || categoria.id === 'new') {
            return categoria.text;
        }
        
        var $container = $('<div class="d-flex align-items-center justify-content-between w-100"></div>');
        var $text = $('<div>' + categoria.text + '</div>');
        var $editBtn = $('<button type="button" class="btn btn-link btn-sm p-0 edit-categoria" ' +
                        'data-id="' + categoria.id + '" data-name="' + categoria.text + '">' +
                        '<i class="fas fa-pencil-alt text-primary"></i></button>');
        
        $container.append($text).append($editBtn);
        
        return $container;
    }
    
    function formatMarcaResult(marca) {
        if (marca.id === 'new') {
            return $('<div><i class="fas fa-plus-circle text-success me-1"></i> ' + marca.text + '</div>');
        }
        return marca.text;
    }
    
    function formatMarcaSelection(marca) {
        if (!marca.id || marca.id === 'new') {
            return marca.text;
        }
        
        var $container = $('<div class="d-flex align-items-center justify-content-between w-100"></div>');
        var $text = $('<div>' + marca.text + '</div>');
        var $editBtn = $('<button type="button" class="btn btn-link btn-sm p-0 edit-marca" ' +
                        'data-id="' + marca.id + '" data-name="' + marca.text + '">' +
                        '<i class="fas fa-pencil-alt text-primary"></i></button>');
        
        $container.append($text).append($editBtn);
        
        return $container;
    }
    
    function initCategoriaSelect2() {
        var $categoriaSelect = $('#CategoriaId');
        if (!$categoriaSelect.length) return;
        
        // Destruir primero si ya existe
        if ($categoriaSelect.hasClass('select2-hidden-accessible')) {
            $categoriaSelect.select2('destroy');
        }
        
        // Limpiar cualquier opción previa o caché
        $categoriaSelect.empty();
        
        // Asegurarse de que no haya eventos duplicados
        $categoriaSelect.off();
        
        $categoriaSelect.select2({
            theme: 'bootstrap-5',
            placeholder: 'Seleccione o cree una categoría',
            allowClear: true,
            width: '100%',
            dropdownParent: $('body'), // Asegurar que el dropdown aparezca en el cuerpo
            ajax: {
                url: '/Categoria/Buscar',
                dataType: 'json',
                delay: 250,
                cache: false, // Desactivar caché para siempre obtener resultados frescos
                data: function(params) {
                    return { 
                        term: params.term || '',
                        timestamp: new Date().getTime() // Añadir timestamp para evitar caché
                    };
                },
                processResults: function(data, params) {
                    var results = data.results || [];
                    var searchTerm = params.term ? params.term.trim() : '';
                    
                    // Agregar opción para crear nueva categoría
                    if (searchTerm !== '' && !results.some(r => r.text.toLowerCase() === searchTerm.toLowerCase())) {
                        results.push({
                            id: 'new',
                            text: 'Crear categoría: "' + searchTerm + '"',
                            term: searchTerm
                        });
                    }
                    
                    return { results: results };
                }
            },
            templateResult: formatCategoriaResult,
            templateSelection: formatCategoriaSelection,
            escapeMarkup: function(markup) { return markup; } // Permitir HTML en los templates
        }).on('select2:select', function(e) {
            var data = e.params.data;
            
            if (data.id === 'new') {
                $(this).val(null).trigger('change');
                abrirOffcanvasCategoria(data.term);
            }
        });
        
        // Añadir un foco inicial para asegurar que se cargue correctamente
        setTimeout(function() {
            $categoriaSelect.select2('open');
            $categoriaSelect.select2('close');
        }, 100);
    }
    
    function initMarcaSelect2() {
        var $marcaSelect = $('#MarcaId');
        if (!$marcaSelect.length) return;
        
        // Destruir primero si ya existe
        if ($marcaSelect.hasClass('select2-hidden-accessible')) {
            $marcaSelect.select2('destroy');
        }
        
        // Limpiar cualquier opción previa o caché
        $marcaSelect.empty();
        
        // Asegurarse de que no haya eventos duplicados
        $marcaSelect.off();
        
        $marcaSelect.select2({
            theme: 'bootstrap-5',
            placeholder: 'Genérica',
            allowClear: true,
            width: '100%',
            dropdownParent: $('body'), // Asegurar que el dropdown aparezca en el cuerpo
            ajax: {
                url: '/Marca/Buscar',
                dataType: 'json',
                delay: 250,
                cache: false, // Desactivar caché para siempre obtener resultados frescos
                data: function(params) {
                    return { 
                        term: params.term || '',
                        timestamp: new Date().getTime() // Añadir timestamp para evitar caché
                    };
                },
                processResults: function(data, params) {
                    var results = data.results || [];
                    var searchTerm = params.term ? params.term.trim() : '';
                    
                    // Agregar opción para crear nueva marca
                    if (searchTerm !== '' && !results.some(r => r.text.toLowerCase() === searchTerm.toLowerCase())) {
                        results.push({
                            id: 'new',
                            text: 'Crear marca: "' + searchTerm + '"',
                            term: searchTerm
                        });
                    }
                    
                    return { results: results };
                }
            },
            templateResult: formatMarcaResult,
            templateSelection: formatMarcaSelection,
            escapeMarkup: function(markup) { return markup; } // Permitir HTML en los templates
        }).on('select2:select', function(e) {
            var data = e.params.data;
            
            if (data.id === 'new') {
                $(this).val(null).trigger('change');
                abrirOffcanvasMarca(data.term);
            }
        });
        
        // Añadir un foco inicial para asegurar que se cargue correctamente
        setTimeout(function() {
            $marcaSelect.select2('open');
            $marcaSelect.select2('close');
        }, 100);
    }
    
    function abrirOffcanvasCategoria(termino) {
        $('#CategoriaId').select2('close');
        
        const offcanvasElement = document.getElementById('offcanvasCategoria');
        if (!offcanvasElement) return;
        
        const offcanvasBS = new bootstrap.Offcanvas(offcanvasElement);
        offcanvasBS.show();
        
        // Cargar formulario
        const formContainer = document.getElementById('formCategoriaContainer');
        if (formContainer) {
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
                    
                    // Prellenar nombre
                    setTimeout(() => {
                        const nombreInput = formContainer.querySelector('[name="Nombre"]');
                        if (nombreInput && termino) {
                            nombreInput.value = termino;
                        }
                    }, 100);
                }
            });
        }
    }
    
    function abrirOffcanvasMarca(termino) {
        $('#MarcaId').select2('close');
        
        const offcanvasElement = document.getElementById('offcanvasMarca');
        if (!offcanvasElement) return;
        
        const offcanvasBS = new bootstrap.Offcanvas(offcanvasElement);
        offcanvasBS.show();
        
        // Cargar formulario
        const formContainer = document.getElementById('formMarcaContainer');
        if (formContainer) {
            $.ajax({
                url: '/Marca/CreatePartial',
                type: 'GET',
                success: function(response) {
                    formContainer.innerHTML = response;
                    
                    // Prellenar nombre
                    setTimeout(() => {
                        const nombreInput = formContainer.querySelector('[name="Nombre"]');
                        if (nombreInput && termino) {
                            nombreInput.value = termino;
                        }
                    }, 100);
                    
                    // Manejar el guardado de la marca por submit del form
                    const form = formContainer.querySelector('form');
                    if (form) {
                        $(form).on('submit', function(e) {
                            e.preventDefault();
                            guardarMarca($(this), offcanvasBS);
                        });
                    }
                }
            });
        }
    }
    
    function guardarMarca($form, offcanvas) {
        console.log('Iniciando guardarMarca');
        console.log('URL:', $form.attr('action'));
        console.log('Method:', $form.attr('method'));
        console.log('Data:', $form.serialize());
        
        $.ajax({
            url: $form.attr('action'),
            type: $form.attr('method') || 'POST',
            data: $form.serialize(),
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            success: function(response) {
                console.log('Respuesta exitosa:', response);
                if (response.success) {
                    offcanvas.hide();
                    Swal.fire('Éxito', 'Marca guardada correctamente', 'success');
                    
                    // Solución radical: reemplazar completamente el select
                    setTimeout(function() {
                        // 1. Guardar el valor actual e información para restaurarlo después
                        const marcaId = response.marca.id;
                        const marcaNombre = response.marca.nombre;
                        
                        // 2. Obtener el elemento padre del select actual
                        const $parent = $('#MarcaId').parent();
                        const selectName = $('#MarcaId').attr('name');
                        const selectId = $('#MarcaId').attr('id');
                        const selectClass = $('#MarcaId').attr('class');
                        
                        // 3. Eliminar todo el contenido relacionado con select2
                        $parent.find('.select2-container').remove();
                        $('#MarcaId').remove();
                        
                        // 4. Crear un nuevo elemento select desde cero
                        const newSelect = $('<select></select>')
                            .attr('id', selectId)
                            .attr('name', selectName)
                            .addClass(selectClass);
                        
                        // Agregar la opción placeholder
                        newSelect.append('<option value="">Genérica</option>');
                        
                        // Agregar al DOM
                        $parent.append(newSelect);
                        
                        // 5. Inicializar el nuevo select2
                        initMarcaSelect2();
                        
                        // 6. Marcar elemento con timestamp para que podamos identificarlo más tarde
                        newSelect.attr('data-regenerated', new Date().getTime());
                        
                        // 7. Establecer el valor de la marca guardada
                        const newOption = new Option(marcaNombre, marcaId, true, true);
                        newSelect.append(newOption).trigger('change');
                        
                        // 8. Limpiar el offcanvas para evitar problemas con formularios antiguos
                        $('#offcanvasMarca .offcanvas-body').html('<div id="formMarcaContainer"></div>');
                        
                        // 9. Enfocar brevemente para forzar la recarga de opciones
                        setTimeout(function() {
                            newSelect.select2('open');
                            newSelect.select2('close');
                        }, 200);
                    }, 500);
                } else {
                    Swal.fire('Error', response.message || 'Error al guardar', 'error');
                }
            },
            error: function(xhr, status, error) {
                console.error('Error en la petición:', status, error);
                console.error('Respuesta del servidor:', xhr.responseText);
                Swal.fire('Error', 'Error al guardar la marca: ' + error, 'error');
            }
        });
    }
    
    function editCategoria(id, nombre) {
        $('#CategoriaId').select2('close');
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
    }
    
    function editMarca(id, nombre) {
        $('#MarcaId').select2('close');
        console.log("Editando marca:", id, nombre);

        // Validar que el ID sea válido
        if (!id || id === 'new' || isNaN(parseInt(id))) {
            console.error("ID de marca inválido:", id);
            Swal.fire({ 
                icon: 'warning', 
                title: 'Aviso', 
                text: 'No se puede editar la marca en este momento. Intente más tarde.',
                footer: 'Sugerencia: Recargue la página si acaba de crear esta marca.'
            });
            return;
        }

        // Mostrar indicador de carga
        Swal.fire({
            title: 'Cargando...',
            text: 'Obteniendo datos de la marca',
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });
        
        // Siempre cargar los datos por AJAX antes de abrir el offcanvas
        $.ajax({
            url: `/Marca/Obtener/${id}`,
            type: 'GET',
            success: function(data) {
                Swal.close();
                const offcanvasElement = document.getElementById('offcanvasMarca');
                if (!offcanvasElement) {
                    console.error("No se encontró el offcanvas");
                    return false;
                }
                
                const offcanvasBS = bootstrap.Offcanvas.getOrCreateInstance(offcanvasElement);
                offcanvasBS.show();
                
                const formContainer = document.getElementById('formMarcaContainer');
                if (formContainer) {
                    formContainer.innerHTML = '<div class="text-center"><div class="spinner-border text-primary" role="status"></div></div>';
                    
                    // Cargar el formulario de edición con los datos recibidos
                    $.ajax({
                        url: `/Marca/EditPartial/${id}`,
                        type: 'GET',
                        success: function(response) {
                            formContainer.innerHTML = response;
                            
                            // Prellenar campos si es necesario
                            setTimeout(function() {
                                let nombreInput = formContainer.querySelector('[name="Nombre"]') || formContainer.querySelector('#Nombre');
                                if (nombreInput && data.nombre) {
                                    nombreInput.value = data.nombre;
                                }
                                
                                // Manejar el guardado de la marca
                                const form = formContainer.querySelector('form');
                                if (form) {
                                    $(form).on('submit', function(e) {
                                        e.preventDefault();
                                        guardarMarca($(this), offcanvasBS);
                                    });
                                }
                            }, 100);
                        },
                        error: function(xhr, status, error) {
                            console.error("Error al cargar el formulario de edición:", error, xhr.status);
                            let mensaje = "Error al cargar el formulario de edición.";
                            if (xhr.status === 404) {
                                mensaje += " Es posible que la marca no exista o esté siendo procesada.";
                            }
                            formContainer.innerHTML = `<div class="alert alert-danger">${mensaje}<br><small>Por favor, recargue la página e intente nuevamente.</small></div>`;
                        }
                    });
                }
            },
            error: function(xhr, status, error) {
                Swal.close();
                console.error("Error al obtener marca:", error, xhr.status);
                let mensaje = "No se pudo cargar la marca para editar.";
                if (xhr.status === 404) {
                    mensaje += " Es posible que la marca no exista o acabe de ser creada.";
                }
                Swal.fire({ 
                    icon: 'error', 
                    title: 'Error', 
                    text: mensaje,
                    footer: 'Por favor, recargue la página e intente nuevamente.'
                });
            }
        });
    }
    
    function actualizarSelect(selector, valor) {
        const $select = $(selector);
        if ($select.length && valor) {
            // Si el select tiene ajax, precargar la opción
            if ($select.hasClass('select2')) {
                // Crear opción temporal
                const newOption = new Option('Cargando...', valor, true, true);
                $select.append(newOption).trigger('change');
                
                // Buscar el nombre real
                // TODO: Implementar búsqueda del nombre real según el tipo de campo
            } else {
                $select.val(valor).trigger('change');
            }
        }
    }
    
    // ==== INICIALIZACIÓN DE CÓDIGO DE BARRAS ====
    // Inicializar generador de código de barras
    function initBarcode() {
        // 1. Asignar evento al botón de generar código de barras
        $('#generarCodigoBarras').off('click').on('click', function() {
            // Usar directamente POST con token antifalsificación
            var token = $('input[name="__RequestVerificationToken"]').val();
            
            $.ajax({
                url: '/Item/GenerarCodigoBarras',
                type: 'POST',
                headers: {
                    'RequestVerificationToken': token
                },
                success: function(response) {
                    if (response.success) {
                        // Asignar código al campo
                        $('#CodigoBarras').val(response.codigo || response.codigoBarras);
                        
                        // Generar visualización del código de barras
                        if (typeof JsBarcode !== 'undefined') {
                            JsBarcode("#barcode", response.codigo || response.codigoBarras, {
                                format: "CODE128",
                                displayValue: true,
                                fontSize: 14,
                                height: 100
                            });
                            
                            // Mostrar contenedor y habilitar botón de impresión
                            $('#codigoBarrasPreview').show();
                            $('#imprimirCodigoBarras').prop('disabled', false);
                        } else {
                            console.error('JsBarcode no está disponible');
                        }
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message || 'Error al generar el código de barras'
                        });
                    }
                },
                error: function(xhr, status, error) {
                    console.error('Error generando código de barras:', error, xhr.responseText);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'No se pudo generar el código de barras'
                    });
                }
            });
        });
        
        // 2. Asignar evento al botón de impresión
        $('#imprimirCodigoBarras').off('click').on('click', function() {
            const codigo = $('#CodigoBarras').val();
            if (!codigo) {
                Swal.fire('Advertencia', 'Debe generar un código de barras primero', 'warning');
                return;
            }
            
            const nombre = $('#Nombre').val() || 'Producto';
            
            // Crear un formulario y enviarlo para generar el PDF
            const form = document.createElement('form');
            form.method = 'POST';
            form.action = '/Item/ImprimirCodigoBarras';
            form.target = '_blank';
            
            // Agregar campos ocultos con la información
            const addHiddenField = (name, value) => {
                const input = document.createElement('input');
                input.type = 'hidden';
                input.name = name;
                input.value = value;
                form.appendChild(input);
            };
            
            addHiddenField('codigoBarras', codigo);
            addHiddenField('nombre', nombre);
            addHiddenField('formato', 'Etiqueta');
            addHiddenField('cantidad', '1');
            
            // Enviar el formulario
            document.body.appendChild(form);
            form.submit();
            document.body.removeChild(form);
        });
        
        // 3. Verificar si ya hay un código de barras y mostrar la visualización
        const existingCode = $('#CodigoBarras').val();
        if (existingCode && typeof JsBarcode !== 'undefined') {
            try {
                JsBarcode("#barcode", existingCode, {
                    format: "CODE128",
                    displayValue: true,
                    fontSize: 14,
                    height: 100
                });
                $('#codigoBarrasPreview').show();
                $('#imprimirCodigoBarras').prop('disabled', false);
            } catch (e) {
                console.error('Error mostrando código de barras existente:', e);
            }
        }
    }
    
    // Inicializar código de barras al cargar la página
    initBarcode();
});