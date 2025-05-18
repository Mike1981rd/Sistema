// Handler para el offcanvas de proveedores
(function() {
    'use strict';
    
    console.log('offcanvas-proveedor-handler.js cargado');
    
    // Función global para debug
    window.debugOffcanvasProveedor = function() {
        console.log('=== DEBUG OFFCANVAS PROVEEDOR ===');
        const offcanvas = document.getElementById('offcanvasProveedor');
        if (offcanvas) {
            console.log('Offcanvas encontrado:', offcanvas);
            console.log('Ancho actual:', window.getComputedStyle(offcanvas).width);
            console.log('Estilos computados:', window.getComputedStyle(offcanvas));
            
            // Verificar contenido
            const container = document.getElementById('formProveedorContainer');
            if (container) {
                console.log('Container encontrado:', container);
                console.log('HTML interno:', container.innerHTML.substring(0, 200) + '...');
                
                // Buscar botones
                const botones = container.querySelectorAll('button');
                console.log('Botones encontrados:', botones.length);
                botones.forEach((btn, index) => {
                    console.log(`Botón ${index}:`, btn.id || btn.className, btn.textContent);
                    console.log('  onclick:', btn.onclick);
                    console.log('  eventos:', btn.hasAttribute('onclick'));
                });
            }
        } else {
            console.log('Offcanvas no encontrado');
        }
    };
    
    // Función para manejar guardado de proveedor
    window.handleGuardarProveedor = function(formId) {
        console.log('handleGuardarProveedor llamado con formulario:', formId);
        
        const form = document.getElementById(formId);
        if (!form) {
            console.error('Formulario no encontrado:', formId);
            return;
        }
        
        // Obtener datos del formulario
        const formData = new FormData(form);
        
        // Log de datos
        console.log('Datos del formulario:');
        for (let [key, value] of formData.entries()) {
            console.log(`  ${key}: ${value}`);
        }
        
        // Llamar a CreateJson
        const data = {
            NombreRazonSocial: formData.get('NombreRazonSocial'),
            TipoIdentificacionId: formData.get('TipoIdentificacionId'),
            NumeroIdentificacion: formData.get('NumeroIdentificacion'),
            Telefono: formData.get('Telefono'),
            Email: formData.get('Email'),
            Direccion: formData.get('Direccion'),
            PlazoPagoId: formData.get('PlazoPagoId'),
            TipoNcfId: formData.get('TipoNcfId'),
            CuentaPorPagarId: formData.get('CuentaPorPagarId'),
            CuentaPorCobrarId: formData.get('CuentaPorCobrarId'),
            EsProveedor: true
        };
        
        // Hacer petición AJAX
        fetch('/Proveedores/CreateJson', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: JSON.stringify(data)
        })
        .then(response => response.json())
        .then(result => {
            console.log('Respuesta del servidor:', result);
            
            if (result.success) {
                // Éxito
                Swal.fire({
                    icon: 'success',
                    title: 'Éxito',
                    text: result.message || 'Proveedor creado correctamente',
                    timer: 2000,
                    showConfirmButton: false
                });
                
                // Cerrar offcanvas
                const offcanvasEl = document.getElementById('offcanvasProveedor');
                const offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
                if (offcanvas) {
                    offcanvas.hide();
                }
                
                // Actualizar select si existe
                const select2 = $('#ProveedorId');
                if (select2.length > 0) {
                    const newOption = new Option(result.nombre, result.id, true, true);
                    select2.append(newOption).trigger('change');
                    
                    // Habilitar botón agregar si existe
                    const btnAgregar = document.getElementById('btnAgregarProveedor');
                    if (btnAgregar) {
                        btnAgregar.disabled = false;
                    }
                }
            } else {
                // Error
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: result.message || 'No se pudo crear el proveedor'
                });
            }
        })
        .catch(error => {
            console.error('Error:', error);
            Swal.fire({
                icon: 'error',
                title: 'Error de conexión',
                text: 'No se pudo conectar con el servidor'
            });
        });
    };
    
    // Inicializar cuando el DOM esté listo
    document.addEventListener('DOMContentLoaded', function() {
        console.log('DOM listo en offcanvas-proveedor-handler');
        
        // Agregar CSS para forzar ancho
        const style = document.createElement('style');
        style.textContent = `
            #offcanvasProveedor {
                width: 850px !important;
                max-width: 90vw !important;
            }
            
            @media (max-width: 900px) {
                #offcanvasProveedor {
                    width: 100vw !important;
                    max-width: 100vw !important;
                }
            }
        `;
        document.head.appendChild(style);
        
        // Observar cambios en el offcanvas para agregar eventos cuando se cargue contenido
        const offcanvas = document.getElementById('offcanvasProveedor');
        if (offcanvas) {
            const observer = new MutationObserver(function(mutations) {
                mutations.forEach(function(mutation) {
                    if (mutation.type === 'childList') {
                        console.log('Contenido del offcanvas cambió');
                        
                        // Buscar botones nuevos
                        const btns = offcanvas.querySelectorAll('button');
                        btns.forEach(btn => {
                            if (!btn.hasAttribute('data-handler-attached')) {
                                console.log('Agregando handler a botón:', btn.id || btn.className);
                                
                                if (btn.id === 'btnNuevoGuardarProveedor' || btn.textContent.includes('Guardar (Nuevo)')) {
                                    btn.addEventListener('click', function() {
                                        console.log('Click detectado en botón nuevo');
                                        const form = btn.closest('form');
                                        if (form) {
                                            handleGuardarProveedor(form.id);
                                        }
                                    });
                                    btn.setAttribute('data-handler-attached', 'true');
                                }
                            }
                        });
                    }
                });
            });
            
            // Configurar observer
            observer.observe(offcanvas, { childList: true, subtree: true });
        }
    });
})();