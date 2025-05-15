// Test directo del endpoint de categorías
console.log("=== Test del Endpoint de Categorías ===");

// Test 1: Llamada directa con jQuery
$.ajax({
    url: '/Categoria/Buscar',
    type: 'GET',
    data: { term: '' },
    success: function(data) {
        console.log("✅ Éxito con jQuery AJAX:", data);
    },
    error: function(xhr, status, error) {
        console.error("❌ Error con jQuery AJAX:", error);
        console.error("Estado HTTP:", xhr.status);
        console.error("Respuesta:", xhr.responseText);
    }
});

// Test 2: Llamada con fetch
fetch('/Categoria/Buscar?term=')
    .then(response => {
        console.log("Estado HTTP con fetch:", response.status);
        return response.json();
    })
    .then(data => {
        console.log("✅ Éxito con fetch:", data);
    })
    .catch(error => {
        console.error("❌ Error con fetch:", error);
    });

// Test 3: Verificar si el Select2 está configurado correctamente
setTimeout(function() {
    const config = $('#categoriaId').data('select2');
    if (config) {
        console.log("Configuración del Select2:", {
            ajax: config.options.options.ajax,
            url: config.options.options.ajax.url
        });
    }
}, 1000);