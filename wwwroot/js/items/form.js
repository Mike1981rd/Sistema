$(document).ready(function() {
    // Función temporal para evitar errores
    window.initImageUpload = function() {};

    // Datos estáticos para pruebas
    const categorias = [
        {id: 1, text: "Electrónicos"},
        {id: 2, text: "Comestibles"},
        {id: 3, text: "Ropa"}
    ];

    const marcas = [
        {id: 1, text: "Samsung"},
        {id: 2, text: "Apple"},
        {id: 3, text: "Nike"}
    ];

    const impuestos = [
        {id: 1, text: "ITBIS 18%"},
        {id: 2, text: "ITBIS 0%"},
        {id: 3, text: "Exento"}
    ];

    // Función para encontrar un offcanvas por contenido o ID
    function encontrarOffcanvas(tipo) {
        console.log(`Buscando offcanvas de ${tipo}...`);
        
        // Buscar todos los offcanvas en la página
        const offcanvases = document.querySelectorAll('.offcanvas');
        console.log(`Encontrados ${offcanvases.length} elementos offcanvas en la página`);
        
        // Recorrer todos los offcanvas para encontrar el correcto
        let offcanvasEncontrado = null;
        
        for (const canvas of offcanvases) {
            // Verificar si es el offcanvas correcto por su contenido
            const contenido = canvas.innerHTML.toLowerCase();
            const tipoLower = tipo.toLowerCase();
            
            if (contenido.includes(tipoLower) || 
                contenido.includes(tipoLower.replace('a', 'á')) ||
                contenido.includes('form' + tipoLower) ||
                contenido.includes('form' + tipoLower.replace('a', 'á'))) {
                
                console.log(`Encontrado un offcanvas que parece ser de ${tipo}:`, canvas);
                offcanvasEncontrado = canvas;
                break;
            }
        }
        
        // Si no encontramos nada por contenido, intentar por IDs comunes
        if (!offcanvasEncontrado) {
            const posiblesIds = [
                `offcanvas${tipo}`,
                `offcanvas${tipo}Form`,
                `${tipo.toLowerCase()}Offcanvas`
            ];
            
            for (const id of posiblesIds) {
                const canvas = document.getElementById(id);
                if (canvas) {
                    console.log(`Encontrado offcanvas con ID: ${id}`);
                    offcanvasEncontrado = canvas;
                    break;
                }
            }
        }
        
        return offcanvasEncontrado;
    }

    // Función para mostrar botón de edición en elementos seleccionados
    function formatSelectionWithEdit(data, container) {
        if (!data.id || data.id === 'new') {
            return data.text;
        }
        
        // Crear contenedor con botón de edición
        return $(`
            <div class="d-flex justify-content-between align-items-center w-100">
                <div>${data.text}</div>
                <button type="button" class="btn btn-link btn-sm p-0 edit-item" data-id="${data.id}" data-type="${$(container).attr('id')}">
                    <i class="fas fa-pencil-alt text-primary"></i>
                </button>
            </div>
        `);
    }

    // Formateo para los resultados en el dropdown de categorías
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

    // Formateo para la selección de categorías con botones de edición
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

    // Formateo para los resultados en el dropdown de marcas
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
        
        return $('<div class="select2-result-marca">' +
                 '<div class="select2-result-marca__name">' + marca.text + '</div>' +
                 '</div>');
    }

    // Formateo para la selección de marcas con botones de edición
    function formatMarcaSelection(marca) {
        if (!marca.id || marca.id === 'new') {
            return marca.text;
        }
        
        // Añadir iconos de edición junto al nombre
        var $container = $('<div class="d-flex align-items-center justify-content-between w-100"></div>');
        var $marcaNombre = $('<div>' + marca.text + '</div>');
        var $actions = $('<div class="marca-actions ms-2"></div>');
        
        var $editBtn = $('<button type="button" class="btn btn-link btn-sm p-0 me-2 edit-marca" ' +
                        'data-id="' + marca.id + '" data-name="' + marca.text + '">' +
                        '<i class="fas fa-pencil-alt text-primary"></i></button>');
        
        $actions.append($editBtn);
        $container.append($marcaNombre);
        $container.append($actions);
        
        // Ocultar el botón "x" del Select2 (Clear)
        setTimeout(function() {
            $('.select2-selection__clear').hide();
        }, 100);
        
        // Asignar manejadores de eventos inmediatamente
        $editBtn.on('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            editMarca(marca.id, marca.text);
            return false;
        });
        
        return $container;
    }

    // Función para abrir y preparar el offcanvas de categoría
    function abrirOffcanvasCategoria(termino) {
        // Fix visual: cerrar Select2 antes de abrir el offcanvas
        $('#CategoriaId').select2('close');
        console.log("Abriendo offcanvas de categoría:", termino);
        
        // 1. Obtener el offcanvas existente
        const offcanvasElement = document.getElementById('offcanvasCategoria');
        if (!offcanvasElement) {
            console.error("No se encontró el offcanvas con ID 'offcanvasCategoria'");
            return false;
        }
        
        // Aplicar estilos al header
        const header = offcanvasElement.querySelector('.offcanvas-header');
        if (header) {
            header.style.backgroundColor = '#3944BC';
            header.style.color = 'white';
        }
        
        // Hacer el offcanvas más ancho
        offcanvasElement.style.width = '600px';
        
        // 2. Abrir el offcanvas
        const offcanvasBS = new bootstrap.Offcanvas(offcanvasElement);
        offcanvasBS.show();
        
        // 3. Cargar el contenido del formulario
        const formContainer = document.getElementById('formCategoriaContainer');
        if (formContainer) {
            // Mostrar indicador de carga
            formContainer.innerHTML = '<div class="text-center"><div class="spinner-border text-primary" role="status"><span class="visually-hidden">Cargando...</span></div></div>';
            
            // Cargar el formulario vía AJAX - usando la ruta correcta
            $.ajax({
                url: '/Categoria/CreatePartial',
                type: 'GET',
                success: function(response) {
                    // Insertar el formulario en el contenedor
                    formContainer.innerHTML = response;
                    
                    // Inicializar selects si es necesario
                    if ($.fn.select2) {
                        $('#formCategoriaContainer .select2-familia, #formCategoriaContainer .select2-impuesto, #formCategoriaContainer .select2-cuenta').select2({
                            theme: 'bootstrap-5',
                            width: '100%',
                            dropdownParent: $('#offcanvasCategoria')
                        });
                    }
                    
                    // 4. Prellenar el campo de nombre
                    setTimeout(function() {
                        // Buscar el campo por nombre primero
                        let nombreInput = formContainer.querySelector('[name="Nombre"]');
                        
                        // Si no lo encontramos así, buscar por posibles IDs
                        if (!nombreInput) {
                            nombreInput = formContainer.querySelector('#Nombre') || 
                                         formContainer.querySelector('#CategoriaNombre');
                        }
                        
                        // Si lo encontramos, prellenarlo
                        if (nombreInput && termino) {
                            nombreInput.value = termino;
                            console.log("Campo nombre prellenado con:", termino);
                        } else {
                            console.warn("No se pudo encontrar el campo de nombre");
                        }
                    }, 100);
                },
                error: function(xhr, status, error) {
                    console.error("Error al cargar el formulario:", error);
                    formContainer.innerHTML = `<div class="alert alert-danger">Error al cargar el formulario: ${error}</div>`;
                }
            });
        } else {
            console.error("No se encontró el contenedor para el formulario");
            return false;
        }
        
        return true;
    }

    // Función para editar categoría
    function editCategoria(id, nombre) {
        $('#CategoriaId').select2('close');
        console.log("Editando categoría:", id, nombre);
        
        const offcanvasElement = document.getElementById('offcanvasCategoria');
        if (!offcanvasElement) {
            console.error("No se encontró el offcanvas");
            return false;
        }
        
        // Aplicar estilos al header
        const header = offcanvasElement.querySelector('.offcanvas-header');
        if (header) {
            header.style.backgroundColor = '#3944BC';
            header.style.color = 'white';
        }
        
        // Hacer el offcanvas más ancho
        offcanvasElement.style.width = '600px';
        
        const offcanvasBS = new bootstrap.Offcanvas(offcanvasElement);
        offcanvasBS.show();
        
        const formContainer = document.getElementById('formCategoriaContainer');
        if (formContainer) {
            formContainer.innerHTML = '<div class="text-center"><div class="spinner-border text-primary" role="status"></div></div>';
            
            // Usar la URL correcta para edición
            $.ajax({
                url: `/Categoria/EditPartial/${id}`,
                type: 'GET',
                success: function(response) {
                    formContainer.innerHTML = response;
                    
                    // Inicializar select2 si es necesario
                    if ($.fn.select2) {
                        $('#formCategoriaContainer .select2-familia, #formCategoriaContainer .select2-impuesto, #formCategoriaContainer .select2-cuenta').select2({
                            theme: 'bootstrap-5',
                            width: '100%',
                            dropdownParent: $('#offcanvasCategoria')
                        });
                    }
                },
                error: function(xhr, status, error) {
                    console.error("Error al cargar el formulario de edición:", error);
                    formContainer.innerHTML = `<div class="alert alert-danger">Error al cargar el formulario: ${error}</div>`;
                }
            });
        }
        
        return true;
    }

    // Función para abrir y preparar el offcanvas de marca
    function abrirOffcanvasMarca(termino) {
        $('#MarcaId').select2('close');
        console.log("Abriendo offcanvas de marca:", termino);
        
        // 1. Obtener el offcanvas existente
        const offcanvasElement = document.getElementById('offcanvasMarca');
        if (!offcanvasElement) {
            console.error("No se encontró el offcanvas con ID 'offcanvasMarca'");
            return false;
        }
        
        // Aplicar estilos al header
        const header = offcanvasElement.querySelector('.offcanvas-header');
        if (header) {
            header.style.backgroundColor = '#3944BC';
            header.style.color = 'white';
        }
        
        // Hacer el offcanvas más ancho
        offcanvasElement.style.width = '600px';
        
        // 2. Abrir el offcanvas
        const offcanvasBS = new bootstrap.Offcanvas(offcanvasElement);
        offcanvasBS.show();
        
        // 3. Cargar el contenido del formulario
        const formContainer = document.getElementById('formMarcaContainer');
        if (formContainer) {
            // Mostrar indicador de carga
            formContainer.innerHTML = '<div class="text-center"><div class="spinner-border text-primary" role="status"><span class="visually-hidden">Cargando...</span></div></div>';
            
            // Cargar el formulario vía AJAX
            $.ajax({
                url: '/Marca/CreatePartial',
                type: 'GET',
                success: function(response) {
                    // Insertar el formulario en el contenedor
                    formContainer.innerHTML = response;
                    
                    // Prellenar el campo de nombre
                    setTimeout(function() {
                        let nombreInput = formContainer.querySelector('[name="Nombre"]') || 
                                        formContainer.querySelector('#Nombre') || 
                                        formContainer.querySelector('#MarcaNombre');
                        
                        if (nombreInput && termino) {
                            nombreInput.value = termino;
                            console.log("Campo nombre prellenado con:", termino);
                        } else {
                            console.warn("No se pudo encontrar el campo de nombre");
                        }
                    }, 100);
                },
                error: function(xhr, status, error) {
                    console.error("Error al cargar el formulario:", error);
                    formContainer.innerHTML = `<div class="alert alert-danger">Error al cargar el formulario: ${error}</div>`;
                }
            });
        } else {
            console.error("No se encontró el contenedor para el formulario");
            return false;
        }
        
        return true;
    }

    // Función para editar marca
    function editMarca(id, nombre) {
        $('#MarcaId').select2('close');
        console.log("Editando marca:", id, nombre);
        
        const offcanvasElement = document.getElementById('offcanvasMarca');
        if (!offcanvasElement) {
            console.error("No se encontró el offcanvas");
            return false;
        }
        
        // Aplicar estilos al header
        const header = offcanvasElement.querySelector('.offcanvas-header');
        if (header) {
            header.style.backgroundColor = '#3944BC';
            header.style.color = 'white';
        }
        
        // Hacer el offcanvas más ancho
        offcanvasElement.style.width = '600px';
        
        const offcanvasBS = new bootstrap.Offcanvas(offcanvasElement);
        offcanvasBS.show();
        
        const formContainer = document.getElementById('formMarcaContainer');
        if (formContainer) {
            formContainer.innerHTML = '<div class="text-center"><div class="spinner-border text-primary" role="status"></div></div>';
            
            // Usar la URL correcta para edición
            $.ajax({
                url: `/Marca/EditPartial/${id}`,
                type: 'GET',
                success: function(response) {
                    formContainer.innerHTML = response;
                },
                error: function(xhr, status, error) {
                    console.error("Error al cargar el formulario de edición:", error);
                    formContainer.innerHTML = `<div class="alert alert-danger">Error al cargar el formulario: ${error}</div>`;
                }
            });
        }
        
        return true;
    }

    // Inicializar Select2 para Categoría
    $('#CategoriaId').select2({
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
                        term: params.term
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

    // Inicializar Select2 para Marca
    $('#MarcaId').select2({
        theme: 'bootstrap-5',
        placeholder: 'Seleccione o cree una marca',
        allowClear: true,
        width: '100%',
        dropdownParent: $('body'),
        ajax: {
            url: window.location.origin + '/Marca/Buscar',
            dataType: 'json',
            delay: 250,
            data: function(params) {
                console.log("Enviando búsqueda marca:", params.term);
                return {
                    term: params.term || ''
                };
            },
            processResults: function(data, params) {
                console.log("Resultados marcas recibidos:", data);
                var results = data.results || [];
                
                if (results.length == 0 && params.term && params.term.trim() !== '') {
                    console.log("Agregando opción para crear nueva marca:", params.term);
                    results.push({
                        id: 'new',
                        text: 'Crear marca: "' + params.term + '"',
                        term: params.term
                    });
                }
                
                return {
                    results: results
                };
            },
            error: function(xhr, status, error) {
                console.error("Error en la búsqueda de marcas:", error);
                console.error("Estado:", status);
                console.error("Respuesta:", xhr.responseText);
            },
            cache: true
        },
        templateResult: formatMarcaResult,
        templateSelection: formatMarcaSelection
    }).on('select2:select', function(e) {
        var data = e.params.data;
        console.log("Marca seleccionada:", data);
        
        if (data.id === 'new') {
            $(this).val(null).trigger('change');
            console.log("Creando marca:", data.term);
            abrirOffcanvasMarca(data.term);
        }
    });

    // Inicializar Select2 para Impuesto
    $('#ImpuestoId').select2({
        theme: 'bootstrap-5',
        placeholder: 'Seleccione un impuesto',
        allowClear: true,
        width: '100%',
        ajax: {
            url: '/Impuestos/Buscar',
            dataType: 'json',
            delay: 250,
            data: function(params) {
                return {
                    term: params.term
                };
            },
            processResults: function(data) {
                return {
                    results: data.results
                };
            },
            cache: true
        },
        minimumInputLength: 0,
        language: {
            noResults: function() {
                return "No se encontraron resultados";
            },
            searching: function() {
                return "Buscando...";
            }
        },
        templateResult: function(data) {
            if (data.loading) return data.text;
            return data.text;
        },
        templateSelection: function(data) {
            return data.text;
        }
    });

    // Manejar clics en botones de edición
    $(document).on('click', '.edit-item', function(e) {
        e.preventDefault();
        e.stopPropagation();
        
        const id = $(this).data('id');
        const type = $(this).data('type');
        
        if (type === 'CategoriaId') {
            // Abrir offcanvas de edición de categoría
            // Primero cargar datos de la categoría
            $.ajax({
                url: `/Categoria/Obtener/${id}`,
                type: 'GET',
                success: function(data) {
                    // Llenar el formulario con los datos
                    const offcanvas = new bootstrap.Offcanvas(document.getElementById('offcanvasCategoriaForm'));
                    
                    // Llenar campos del formulario
                    setTimeout(() => {
                        const form = document.querySelector('#offcanvasCategoriaForm form');
                        if (form) {
                            form.reset();
                            // Establecer ID para edición
                            const idInput = form.querySelector('#Id');
                            if (idInput) idInput.value = id;
                            
                            // Llenar otros campos si están disponibles
                            for (const key in data) {
                                const input = form.querySelector(`#${key}`);
                                if (input) input.value = data[key];
                            }
                        }
                    }, 200);
                    
                    offcanvas.show();
                }
            });
        } else if (type === 'MarcaId') {
            // Similar para marcas
            $.ajax({
                url: `/Marca/Obtener/${id}`,
                type: 'GET',
                success: function(data) {
                    const offcanvas = new bootstrap.Offcanvas(document.getElementById('offcanvasMarcaForm'));
                    
                    setTimeout(() => {
                        const form = document.querySelector('#offcanvasMarcaForm form');
                        if (form) {
                            form.reset();
                            const idInput = form.querySelector('#Id');
                            if (idInput) idInput.value = id;
                            
                            for (const key in data) {
                                const input = form.querySelector(`#${key}`);
                                if (input) input.value = data[key];
                            }
                        }
                    }, 200);
                    
                    offcanvas.show();
                }
            });
        }
    });

    // Manejar el envío del formulario
    $('#itemForm').on('submit', function(e) {
        e.preventDefault();
        
        // Validar campos requeridos
        if (!validateForm()) {
            return false;
        }

        // Enviar formulario
        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: $(this).serialize(),
            success: function(response) {
                if (response.success) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Éxito',
                        text: 'El item se ha guardado correctamente',
                        showConfirmButton: false,
                        timer: 1500
                    }).then(function() {
                        window.location.href = '/Item';
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: response.message || 'Ha ocurrido un error al guardar el item'
                    });
                }
            },
            error: function() {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Ha ocurrido un error al procesar la solicitud'
                });
            }
        });
    });

    // Función para validar el formulario
    function validateForm() {
        let isValid = true;
        
        // Limpiar mensajes de error previos
        $('.is-invalid').removeClass('is-invalid');
        $('.invalid-feedback').remove();

        // Validar campos requeridos
        $('#itemForm [required]').each(function() {
            if (!$(this).val()) {
                $(this).addClass('is-invalid');
                $(this).after('<div class="invalid-feedback">Este campo es requerido</div>');
                isValid = false;
            }
        });

        return isValid;
    }

    // Manejar el botón de generar código de barras
    $('#btnGenerarCodigoBarras').on('click', function() {
        $.ajax({
            url: '/Item/GenerarCodigoBarras',
            type: 'POST',
            success: function(response) {
                if (response.success) {
                    $('#CodigoBarras').val(response.codigoBarras);
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: response.message || 'Error al generar el código de barras'
                    });
                }
            },
            error: function() {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al procesar la solicitud'
                });
            }
        });
    });

    // Manejar el botón de imprimir código de barras
    $('#btnImprimirCodigoBarras').on('click', function() {
        const codigoBarras = $('#CodigoBarras').val();
        if (!codigoBarras) {
            Swal.fire({
                icon: 'warning',
                title: 'Advertencia',
                text: 'Primero debe generar un código de barras'
            });
            return;
        }

        // Abrir ventana de impresión
        window.open(`/Item/ImprimirCodigoBarras?codigo=${codigoBarras}`, '_blank');
    });

    // Delegar el guardado de categoría por AJAX
    $(document).on('click', '#btnGuardarCategoria', function(e) {
        e.preventDefault();
        var $form = $('#formCategoriaContainer form');
        if ($form.length === 0) return;
        var formData = $form.serialize();
        $.ajax({
            url: $form.attr('action'),
            type: $form.attr('method') || 'POST',
            data: formData,
            success: function(response) {
                if (response.success) {
                    // Agregar al select y seleccionar
                    var newOption = new Option(response.nombre, response.id, true, true);
                    $('#CategoriaId').append(newOption).trigger('change');
                    // Cerrar offcanvas
                    $('#offcanvasCategoria').offcanvas('hide');
                    Swal.fire({ icon: 'success', title: 'Éxito', text: 'Categoría guardada correctamente', timer: 1500, showConfirmButton: false });
                } else {
                    Swal.fire({ icon: 'error', title: 'Error', text: response.message || 'No se pudo guardar la categoría' });
                }
            },
            error: function() {
                Swal.fire({ icon: 'error', title: 'Error', text: 'Ocurrió un error al guardar la categoría' });
            }
        });
    });

    // Delegar el guardado de marca por AJAX
    $(document).on('click', '#btnGuardarMarca', function(e) {
        e.preventDefault();
        var $form = $('#formMarcaContainer form');
        if ($form.length === 0) return;
        var formData = $form.serialize();
        $.ajax({
            url: $form.attr('action'),
            type: $form.attr('method') || 'POST',
            data: formData,
            success: function(response) {
                if (response.success) {
                    var newOption = new Option(response.nombre, response.id, true, true);
                    $('#MarcaId').append(newOption).trigger('change');
                    $('#offcanvasMarca').offcanvas('hide');
                    Swal.fire({ icon: 'success', title: 'Éxito', text: 'Marca guardada correctamente', timer: 1500, showConfirmButton: false });
                } else {
                    Swal.fire({ icon: 'error', title: 'Error', text: response.message || 'No se pudo guardar la marca' });
                }
            },
            error: function() {
                Swal.fire({ icon: 'error', title: 'Error', text: 'Ocurrió un error al guardar la marca' });
            }
        });
    });
}); 