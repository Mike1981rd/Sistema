// Automatic code generation for Item
/* --- COMENTADO PARA EVITAR CONFLICTO CON tab-info-init.js ---
$(document).ready(function() {
    console.log("=== CODE GENERATOR INITIALIZED ===");
    
    // Generate code when page loads if field is empty
    setTimeout(function() {
        generateItemCode();
    }, 500);
    
    // Function to generate automatic item code
    function generateItemCode() {
        const $codigoField = $('#Codigo');
        
        // Only generate if field is empty
        if ($codigoField.length > 0 && !$codigoField.val()) {
            $.ajax({
                url: '/Item/GenerarCodigo', // URL INCORRECTA
                type: 'GET',
                success: function(response) {
                    if (response.success) {
                        $codigoField.val(response.codigo);
                        console.log("Item code generated:", response.codigo);
                    } else {
                        console.error("Error generating code:", response.message);
                    }
                },
                error: function(xhr, status, error) {
                    console.error("Error calling code generation:", error);
                }
            });
        }
    }
    
    // Also allow manual code regeneration if needed
    window.regenerateItemCode = function() {
        $('#Codigo').val(''); // Clear current value
        generateItemCode();
    };
});
--- FIN DE COMENTARIO --- */