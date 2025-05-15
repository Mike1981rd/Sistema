// Script para manejar el comportamiento del menú
document.addEventListener('DOMContentLoaded', function() {
    // Prevenir que se ejecute múltiples veces
    if (window.menuInitialized) return;
    window.menuInitialized = true;
    
    // Evitar parpadeos durante la inicialización
    const body = document.body;
    body.classList.add('menu-initializing');
    
    // Congelar brevemente para evitar saltos
    const originalOverflow = body.style.overflow;
    body.style.overflow = 'hidden';
    
    // Marcar el elemento activo basado en la URL actual
    const currentPath = window.location.pathname.toLowerCase();
    let activeItem = null;
    let activeParent = null;
    
    // Expandir submenú de configuración inmediatamente para páginas de esa sección
    const isInConfigSection = currentPath.includes('/plazopago') || 
                              currentPath.includes('/retenciones') || 
                              currentPath.includes('/comprobantes') || 
                              currentPath.includes('/comprobantes-fiscales') || 
                              currentPath.includes('/impuestos') || 
                              currentPath.includes('/empresas/configurar') ||
                              currentPath.includes('/configuracion/') ||
                              currentPath.includes('/usuarios');
    
    if (isInConfigSection) {
        const configMenu = document.querySelector('.aurora-nav-item[data-title="Configuración"]');
        const configSubmenu = configMenu?.nextElementSibling;
        
        if (configMenu && configSubmenu) {
            configMenu.classList.add('expanded');
            configSubmenu.classList.add('show');
            
            // Marcar el elemento específico como activo
            if (currentPath.includes('/plazopago')) {
                const targetItem = configSubmenu.querySelector('a[href*="/PlazoPago"]');
                if (targetItem) targetItem.classList.add('active');
            } else if (currentPath.includes('/retenciones')) {
                const targetItem = configSubmenu.querySelector('a[href*="/Retenciones"]');
                if (targetItem) targetItem.classList.add('active');
            } else if (currentPath.includes('/comprobantes') || currentPath.includes('/comprobantes-fiscales')) {
                const targetItem = configSubmenu.querySelector('a[href*="/comprobantes"]');
                if (targetItem) targetItem.classList.add('active');
            }
        }
    }
    
    // Expandir submenú de Punto de Venta para páginas de esa sección
    const isInPOSSection = currentPath.includes('/productos');
    
    if (isInPOSSection) {
        const posMenu = document.querySelector('.aurora-nav-item[data-title="Punto de Venta"]');
        const posSubmenu = posMenu?.nextElementSibling;
        
        if (posMenu && posSubmenu) {
            posMenu.classList.add('expanded');
            posSubmenu.classList.add('show');
            
            // Marcar el elemento específico como activo
            if (currentPath.includes('/productos')) {
                const targetItem = posSubmenu.querySelector('a[href="/Productos"]');
                if (targetItem) targetItem.classList.add('active');
            }
        }
    }
    
    // Configurar activación por URL
    function markActiveItems() {
        document.querySelectorAll('.aurora-submenu-item').forEach(item => {
            const href = item.getAttribute('href')?.toLowerCase();
            
            if (href && (
                currentPath === href || 
                currentPath.startsWith(href + '/') || 
                (href !== '/' && currentPath.includes(href))
            )) {
                activeItem = item;
                activeParent = item.closest('.aurora-submenu')?.previousElementSibling;
                
                // Marcar como activo
                item.classList.add('active');
                
                // Expandir el padre si existe
                if (activeParent) {
                    activeParent.classList.add('expanded');
                    const submenu = activeParent.nextElementSibling;
                    if (submenu && submenu.classList.contains('aurora-submenu')) {
                        submenu.classList.add('show');
                    }
                }
            }
        });
    }
    
    // CORRECCIÓN: Volver a aplicar eventos de clic en elementos del menú
    function setupMenuEvents() {
        document.querySelectorAll('.aurora-nav-item.has-submenu').forEach(item => {
            // Eliminar manejadores anteriores para evitar duplicados
            item.removeEventListener('click', handleSubmenuToggle);
            // Añadir manejador de clic
            item.addEventListener('click', handleSubmenuToggle);
        });
    }
    
    // Manejador de clic para alternar submenús
    function handleSubmenuToggle(e) {
        e.preventDefault();
        e.stopPropagation();
        
        // Alternar clase expanded
        this.classList.toggle('expanded');
        
        // Alternar visibilidad del submenú
        const submenu = this.nextElementSibling;
        if (submenu && submenu.classList.contains('aurora-submenu')) {
            submenu.classList.toggle('show');
        }
    }
    
    // CORREGIDO: Configurar colapsador del sidebar completo
    // Este evento debe configurarse directamente, no a través de una función intermedia
    const sidebarToggler = document.getElementById('sidebarCollapseToggler');
    if (sidebarToggler) {
        sidebarToggler.addEventListener('click', function() {
            const wrapper = document.querySelector('.aurora-wrapper');
            wrapper.classList.toggle('sidebar-collapsed');
            
            // Cambiar el ícono del toggler según el estado
            const icon = this.querySelector('i');
            if (wrapper.classList.contains('sidebar-collapsed')) {
                icon.classList.remove('fa-angle-left');
                icon.classList.add('fa-angle-right');
            } else {
                icon.classList.remove('fa-angle-right');
                icon.classList.add('fa-angle-left');
            }
        });
    }
    
    // Inicializar menú
    markActiveItems();
    setupMenuEvents();
    
    // Restaurar comportamiento normal después de inicialización
    setTimeout(function() {
        // Restaurar scroll
        body.style.overflow = originalOverflow;
        
        // Permitir transiciones
        body.classList.remove('menu-initializing');
        
        // Estabilizar la página
        document.documentElement.scrollTop = document.documentElement.scrollTop;
    }, 100);
}); 