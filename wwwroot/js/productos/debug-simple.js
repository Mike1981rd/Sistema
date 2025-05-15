// debug-simple.js - Código mínimo de debug
console.log("=== Debug Simple - Productos ===");

$(document).ready(function() {
    console.log("jQuery está listo");
    console.log("¿Select2 existe?", typeof $.fn.select2);
    console.log("¿Elemento categoriaId existe?", $('#categoriaId').length);
    
    // Verificar después de un momento si Select2 se inicializó
    setTimeout(function() {
        console.log("¿Select2 se inicializó?", $('#categoriaId').hasClass('select2-hidden-accessible'));
    }, 500);
});