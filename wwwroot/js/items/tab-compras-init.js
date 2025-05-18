// Inicialización específica para la pestaña de Compras
$(document).ready(function() {
    console.log("=== TAB COMPRAS INITIALIZATION V2 ===");
    
    function initComprasTab() {
        console.log("Inicializando pestaña de compras V2...");
        initContenedores();
        initProveedoresGenerales(); // Cambiado para diferenciar
    }
    
    $('a[data-bs-toggle="tab"][href="#tab-compras"]').on('shown.bs.tab', function() {
        console.log("Pestaña de compras activada V2, inicializando...");
        setTimeout(initComprasTab, 150); // Ligero aumento del timeout
    });
    
    if ($('#tab-compras').is(':visible') || $('#tab-compras').hasClass('active')) {
        console.log("Pestaña de compras ya activa/visible al cargar V2, inicializando...");
        setTimeout(initComprasTab, 350); // Ligero aumento del timeout
    }
    
    function initContenedores() {
        console.log("TC-V2: Inicializando selectores de CONTENEDORES en tabla conversiones...");
        
        // Si no hay filas y existe el body (para la tabla de conversiones)
        // Comentado para ceder el control a contenedores.js
        /*
        if ($('#contenedores-body').length > 0 && $('#contenedores-body tr').length === 0 && typeof agregarFilaContenedor === 'function') {
            console.log("TC-V2: No hay contenedores en tabla conversiones, agregando fila inicial...");
            agregarFilaContenedor(); // Esta función debe estar definida globalmente o dentro de este scope
        }
        */
        // Asegurarse de que la función global de carga de contenedores.js se llame si es necesario
        if (typeof window.cargarContenedoresExistentes === 'function' && $('#contenedores-body tr').length === 0) {
            console.log("TC-V2: Llamando a window.cargarContenedoresExistentes() para inicializar desde tab-compras-init.js");
            window.cargarContenedoresExistentes([]); // Llama con un array vacío para que agregue la primera fila
        }
         else {
            // Inicializar cualquier select2 de contenedor que ya exista y no esté inicializado (esto puede ser útil si las filas se renderizan desde el servidor)
            $('#tablaContenedores .select2-contenedor').each(function() {
                const $select = $(this);
                if ($select.hasClass('select2-hidden-accessible')) {
                    console.log("TC-V2: Select de contenedor en tabla ya inicializado, saltando.", $select.attr('name'));
                    return;
                }
                console.log("TC-V2: Inicializando select2 para CONTENEDOR existente en tabla conversiones (desde tab-compras-init):", $select.attr('name'));
                // Usamos la inicialización de contenedores.js si está disponible, sino la genérica
                if (typeof window.inicializarSelectContenedorFila === 'function') { // Suponiendo que exponemos una función así desde contenedores.js
                    window.inicializarSelectContenedorFila($select);
                } else {
                    initializeSingleSelect2($select, '/Contenedor/Buscar', 'Seleccione un contenedor', function() { actualizarEtiquetaContenedor($select); });
                }
            });
        }

        // Evento para agregar nueva fila de contenedor (si el botón es parte del parcial)
        // Comentado para ceder el control a contenedores.js, que ya tiene un listener para este botón.
        /*
        $('#btnAgregarContenedor').off('click.comprasInit').on('click.comprasInit', function() {
            console.log("TC-V2: Agregando nuevo contenedor desde tab-compras-init...");
            if (typeof agregarFilaContenedor === 'function') {
                agregarFilaContenedor();
            }
        });
        */
    }
    
    // Función auxiliar para inicializar un Select2 genérico
    function initializeSingleSelect2($element, url, placeholder, onSelectCallback = null) {
        if (!$element || $element.length === 0) return;
        if ($element.hasClass('select2-hidden-accessible')) return; // Ya inicializado

        console.log(`TC-V2: Preparing to init Select2 for [${$element.attr('id') || $element.attr('name')}] with URL: ${url}`);

        $element.select2({
            theme: 'bootstrap-5',
            width: '100%',
            placeholder: placeholder,
            allowClear: true,
            ajax: {
                url: url, // Esta es la URL que Select2 usará
                dataType: 'json',
                delay: 250,
                data: function(params) {
                    // Log para depurar la URL en el momento de la petición
                    console.log(`TC-V2: AJAX data function called for [${$element.attr('id') || $element.attr('name')}]. URL from closure: ${url}. Term: ${params.term}`);
                    // Asegurarse de que la URL usada por select2 internamente también sea correcta
                    if (this && this.ajaxOptions && typeof this.ajaxOptions.url !== 'undefined' && this.ajaxOptions.url !== url) {
                        console.warn(`TC-V2: URL mismatch! Initial: ${url}, Current internal: ${this.ajaxOptions.url}`);
                    }
                    return { term: params.term || '' };
                },
                processResults: function(data) { 
                    console.log(`TC-V2: Datos AJAX para ${url}:`, data);
                    return { results: data.results || [] }; 
                },
                cache: true
            }
        }).on('select2:select', function(e) {
            if (onSelectCallback) {
                onSelectCallback(e);
            }
        });
        console.log("TC-V2: Select2 inicializado para:", $element.attr('name') || $element.attr('id'));
    }

    // Esta función ahora solo se encarga de los selectores de proveedor y contenedor de compra GLOBALES
    // si el script del parcial _ComprasPartial.cshtml NO los maneja.
    function initProveedoresGenerales() {
        console.log("TC-V2: Inicializando selectores de PROVEEDORES y CONTENEDORES DE COMPRA generales...");

        // Para el selector principal de Proveedor en _ComprasPartial.cshtml
        const $mainProveedorSelect = $('#ProveedorId.select2-proveedor');
        if ($mainProveedorSelect.length > 0 && !$mainProveedorSelect.hasClass('select2-hidden-accessible')) {
            console.log("TC-V2: Inicializando selector principal de Proveedor #ProveedorId");
            initializeProveedorSelect2($mainProveedorSelect);
        } else {
            console.log("TC-V2: Selector principal #ProveedorId ya inicializado o no encontrado con clase .select2-proveedor.");
        }

        // Para los selectores de contenedor DENTRO de las filas de proveedores (si se añaden por fuera del script del parcial)
        // La clase esperada aquí es .select2-contenedor-compra
        $('#tablaProveedores .select2-contenedor-compra').each(function() {
            const $select = $(this);
            if ($select.hasClass('select2-hidden-accessible')) {
                 console.log("TC-V2: Select de contenedor-compra en tabla proveedores ya inicializado, saltando.", $select.attr('name'));
                return;
            }
            console.log("TC-V2: Inicializando select2 para CONTENEDOR DE COMPRA en tabla proveedores:", $select.attr('name'));
            // initializeSingleSelect2($select, '/Contenedor/Buscar', 'Seleccione contenedor de compra'); // Ya no usamos AJAX global
            initializeContenedorDeCompraSelectConDatosLocales($select);
        });
    }

    function initializeContenedorDeCompraSelectConDatosLocales($element) {
        console.log(`TAB-COMPRAS-INIT: Intentando inicializar ContenedorDeCompra LOCALMENTE para el elemento:`, $element);
        if (!$element || $element.length === 0) return;
        if ($element.hasClass('select2-hidden-accessible')) return; // Ya inicializado

        const placeholder = 'Seleccione contenedor de compra';
        console.log(`TC-V2: Preparing to init Select2 for [${$element.attr('id') || $element.attr('name')}] with LOCAL data from #contenedores-body.`);

        let opcionesContenedor = [];
        $('#contenedores-body tr').each(function(index) {
            const $filaContenedor = $(this);
            const nombreContenedor = $filaContenedor.find('.contenedor-select option:selected').text();
            const etiquetaContenedor = $filaContenedor.find('.etiqueta-input').val(); // Usamos la etiqueta que ya tiene el formato deseado
            const idContenedor = $filaContenedor.find('.contenedor-select').val(); // ID real del contenedor si ya está seleccionado

            if (idContenedor && nombreContenedor) {
                opcionesContenedor.push({
                    id: etiquetaContenedor, // Usamos la etiqueta como ID y texto para simplicidad aquí, o podríamos usar el ID real si fuera necesario diferenciar.
                    text: etiquetaContenedor 
                    // Podríamos necesitar el ID real del contenedor para guardarlo: id: idContenedor, text: etiquetaContenedor
                });
            }
        });

        if (opcionesContenedor.length === 0) {
            console.warn("TC-V2: No se encontraron contenedores en #contenedores-body para poblar el select de contenedor de compra.");
            // Podríamos añadir una opción por defecto o deshabilitar el select
        }

        $element.select2({
            theme: 'bootstrap-5',
            width: '100%',
            placeholder: placeholder,
            allowClear: true,
            data: opcionesContenedor, // Poblar con datos locales
            language: {
                noResults: function () {
                    return "No hay contenedores configurados para este ítem.";
                }
            }
        });
        console.log("TC-V2: Select2 (contenedor de compra) inicializado con datos locales para:", $element.attr('name') || $element.attr('id'));
    }

    // Función específica para inicializar el Select2 de proveedores con opción de crear nuevo
    function initializeProveedorSelect2($element) {
        if (!$element || $element.length === 0) return;
        if ($element.hasClass('select2-hidden-accessible')) return; // Ya inicializado

        console.log("TC-V2: Inicializando Select2 de proveedor con opción crear nuevo");

        $element.select2({
            theme: 'bootstrap-5',
            width: '100%',
            placeholder: 'Seleccione o cree un proveedor',
            allowClear: true,
            ajax: {
                url: '/api/Proveedores/Buscar',
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
                                text: 'Crear proveedor: "' + searchTerm + '"',
                                term: searchTerm,
                                _isNew: true
                            });
                        }
                    }
                    
                    return { results: results };
                },
                cache: true
            },
            templateResult: formatProveedorResult,
            templateSelection: formatProveedorSelection,
            escapeMarkup: function(markup) { return markup; }
        }).on('select2:select', function(e) {
            var data = e.params.data;
            
            if (data.id === 'new') {
                $(this).val(null).trigger('change');
                abrirOffcanvasProveedor(data.term);
            }
        });
    }

    // Formatear resultados de proveedor en el dropdown
    function formatProveedorResult(proveedor) {
        if (proveedor.loading) {
            return proveedor.text;
        }
        
        if (proveedor.id === 'new') {
            return $('<div class="select2-result-proveedor">' +
                     '<div class="select2-result-proveedor__action"><i class="fas fa-plus-circle text-success me-1"></i> ' + 
                     proveedor.text + '</div>' +
                     '</div>');
        }
        
        return $('<div class="select2-result-proveedor">' +
                 '<div class="select2-result-proveedor__name">' + proveedor.text + '</div>' +
                 '</div>');
    }

    // Formatear selección de proveedor
    function formatProveedorSelection(proveedor) {
        if (!proveedor.id || proveedor.id === 'new') {
            return proveedor.text;
        }
        
        // Añadir iconos de edición junto al nombre
        var $container = $('<div class="d-flex align-items-center justify-content-between w-100"></div>');
        var $proveedorNombre = $('<div>' + proveedor.text + '</div>');
        var $actions = $('<div class="proveedor-actions ms-2"></div>');
        
        var $editBtn = $('<button type="button" class="btn btn-link btn-sm p-0 me-2 edit-proveedor" ' +
                        'data-id="' + proveedor.id + '" data-name="' + proveedor.text + '">' +
                        '<i class="fas fa-pencil-alt text-primary"></i></button>');
        
        $actions.append($editBtn);
        $container.append($proveedorNombre);
        $container.append($actions);
        
        return $container;
    }

    // Función para abrir el offcanvas de proveedor
    function abrirOffcanvasProveedor(nombreProveedor) {
        const offcanvasElement = document.getElementById('offcanvasProveedor');
        if (!offcanvasElement) {
            console.error("No se encontró el offcanvas de proveedor");
            return;
        }
        
        // Mostrar el offcanvas
        const offcanvas = new bootstrap.Offcanvas(offcanvasElement);
        offcanvas.show();
        
        // Cargar el formulario en el contenedor
        const formContainer = document.getElementById('formProveedorContainer');
        if (formContainer) {
            formContainer.innerHTML = '<div class="text-center"><div class="spinner-border text-primary" role="status"></div></div>';
            
            // Cargar el formulario de creación de proveedor
            $.ajax({
                url: '/Proveedores/CreatePartial',
                type: 'GET',
                success: function(response) {
                    console.log('Formulario cargado correctamente');
                    formContainer.innerHTML = response;
                    
                    // Ejecutar scripts embebidos en el HTML cargado
                    const scripts = formContainer.querySelectorAll('script');
                    scripts.forEach(script => {
                        console.log('Ejecutando script embebido...');
                        if (script.src) {
                            // Crear nuevo script con src
                            const newScript = document.createElement('script');
                            newScript.src = script.src;
                            document.body.appendChild(newScript);
                        } else {
                            // Ejecutar script inline
                            try {
                                console.log('Script content:', script.innerHTML.substring(0, 200));
                                const scriptContent = script.innerHTML;
                                const newScript = document.createElement('script');
                                newScript.textContent = scriptContent;
                                document.body.appendChild(newScript);
                            } catch (e) {
                                console.error('Error ejecutando script:', e);
                            }
                        }
                    });
                    
                    // Prellenar el campo de nombre si se proporciona
                    if (nombreProveedor) {
                        setTimeout(function() {
                            // Buscar el campo correcto para el nombre
                            let nombreInput = formContainer.querySelector('#ProveedorNombreRazonSocial');
                            if (!nombreInput) {
                                nombreInput = formContainer.querySelector('#NombreRazonSocial');
                            }
                            if (nombreInput) {
                                nombreInput.value = nombreProveedor;
                                console.log('Nombre prellenado:', nombreProveedor);
                            } else {
                                console.error('No se encontró el campo de nombre');
                            }
                        }, 100);
                    }
                    
                    // Re-inicializar scripts del formulario cargado dinámicamente
                    if (typeof $.fn.select2 !== 'undefined') {
                        $('#formProveedorContainer .select2-pais').select2({
                            theme: 'bootstrap-5',
                            width: '100%',
                            placeholder: 'Seleccione un país',
                            allowClear: true,
                            dropdownParent: $('#offcanvasProveedor')
                        });
                    }
                    
                    // Debug: Verificar qué botones están disponibles
                    console.log('Botones disponibles:', formContainer.querySelectorAll('button'));
                    
                    // Agregar listener al botón que sabemos que existe en el formulario de Item
                    const btnNuevo = formContainer.querySelector('#btnNuevoGuardarProveedor');
                    if (btnNuevo) {
                        console.log('Agregando listener al botón nuevo...');
                        btnNuevo.addEventListener('click', function() {
                            console.log('Click en botón nuevo detectado');
                            if (typeof guardarProveedorNuevo === 'function') {
                                guardarProveedorNuevo();
                            } else {
                                console.error('guardarProveedorNuevo no está definida');
                            }
                        });
                    }
                    
                    // También verificar si hay un botón submit estándar
                    const btnSubmit = formContainer.querySelector('button[type="submit"]');
                    if (btnSubmit) {
                        console.log('Botón submit encontrado:', btnSubmit);
                    }
                },
                error: function(xhr, status, error) {
                    console.error("Error al cargar el formulario:", error);
                    formContainer.innerHTML = `<div class="alert alert-danger">Error al cargar el formulario: ${error}</div>`;
                }
            });
        }
    }

    // Handler para editar proveedor cuando se hace clic en el botón de edición
    $(document).on('click', '.edit-proveedor', function(e) {
        e.preventDefault();
        e.stopPropagation();
        const id = $(this).data('id');
        const nombre = $(this).data('name');
        editProveedor(id, nombre);
    });

    // Función para editar proveedor
    function editProveedor(id, nombre) {
        $('#ProveedorId').select2('close');
        console.log("Editando proveedor:", id, nombre);

        // Mostrar indicador de carga
        Swal.fire({
            title: 'Cargando...',
            text: 'Obteniendo datos del proveedor',
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });
        
        // Cargar datos del proveedor
        $.ajax({
            url: `/Proveedores/Details/${id}`,
            type: 'GET',
            success: function(data) {
                Swal.close();
                
                const offcanvasElement = document.getElementById('offcanvasProveedor');
                if (!offcanvasElement) {
                    console.error("No se encontró el offcanvas de proveedor");
                    return;
                }
                
                // Mostrar el offcanvas
                const offcanvas = new bootstrap.Offcanvas(offcanvasElement);
                offcanvas.show();
                
                // Cargar el formulario de edición
                const formContainer = document.getElementById('formProveedorContainer');
                if (formContainer) {
                    formContainer.innerHTML = '<div class="text-center"><div class="spinner-border text-primary" role="status"></div></div>';
                    
                    $.ajax({
                        url: `/Proveedores/EditPartial/${id}`,
                        type: 'GET',
                        success: function(response) {
                            console.log('Formulario de edición cargado correctamente');
                            formContainer.innerHTML = response;
                            
                            // Ejecutar scripts embebidos en el HTML cargado
                            const scripts = formContainer.querySelectorAll('script');
                            scripts.forEach(script => {
                                console.log('Ejecutando script embebido en edit...');
                                if (script.src) {
                                    // Crear nuevo script con src
                                    const newScript = document.createElement('script');
                                    newScript.src = script.src;
                                    document.body.appendChild(newScript);
                                } else {
                                    // Ejecutar script inline
                                    try {
                                        console.log('Script content:', script.innerHTML.substring(0, 200));
                                        const scriptContent = script.innerHTML;
                                        const newScript = document.createElement('script');
                                        newScript.textContent = scriptContent;
                                        document.body.appendChild(newScript);
                                    } catch (e) {
                                        console.error('Error ejecutando script:', e);
                                    }
                                }
                            });
                            
                            // Re-inicializar Select2 si está disponible
                            if (typeof $.fn.select2 !== 'undefined') {
                                $('#EditTipoIdentificacionId').select2({
                                    theme: 'bootstrap-5',
                                    width: '100%',
                                    dropdownParent: $('#offcanvasProveedor')
                                });
                            }
                            
                            // Debug: Verificar qué botones están disponibles
                            console.log('Botones disponibles en edit:', formContainer.querySelectorAll('button'));
                        },
                        error: function(xhr, status, error) {
                            console.error("Error al cargar el formulario de edición:", error);
                            formContainer.innerHTML = `<div class="alert alert-danger">Error al cargar el formulario de edición</div>`;
                        }
                    });
                }
            },
            error: function() {
                Swal.fire('Error', 'Error al obtener datos del proveedor', 'error');
            }
        });
    }

    // Función para actualizar la etiqueta del contenedor
    function actualizarEtiquetaContenedor($select) {
        const selectedData = $select.select2('data')[0];
        if (!selectedData) return;
        
        const $row = $select.closest('tr');
        const $etiqueta = $row.find('.etiqueta-contenedor');
        
        if (selectedData.etiqueta) {
            $etiqueta.val(selectedData.etiqueta);
        } else {
            $etiqueta.val(selectedData.text);
        }
        
        console.log("Etiqueta de contenedor actualizada:", $etiqueta.val());
    }
    
    // Función para agregar una fila de contenedor
    /* --- COMENTADO ---   
    function agregarFilaContenedor() {
        const $tabla = $('#contenedores-body');
// ... toda la función agregarFilaContenedor comentada ...
        $('#sinContenedores').hide();
    }
    */
    
    // Función para eliminar una fila de contenedor
    // Comentada si contenedores.js maneja esto
    /* --- COMENTADO --- 
    function eliminarFilaContenedor($button) {
        const $row = $button.closest('tr');
// ... toda la función eliminarFilaContenedor comentada ...
        }
    }
    */
}); 