// Minimal Select2 initialization for debugging
$(document).ready(function() {
    console.log("Starting minimal Select2 init...");
    
    // Try to initialize just the category select
    var $cat = $('select[name="CategoriaId"]');
    console.log("Category select found:", $cat.length);
    console.log("Category select HTML:", $cat[0]);
    
    if ($cat.length > 0) {
        try {
            $cat.select2({
                theme: 'bootstrap-5',
                width: '100%'
            });
            console.log("Category Select2 initialized successfully!");
        } catch(e) {
            console.error("Category Select2 error:", e);
        }
    }
    
    // Check the result
    setTimeout(function() {
        console.log("Category has Select2:", $cat.hasClass('select2-hidden-accessible'));
        console.log("Select2 containers on page:", $('.select2-container').length);
    }, 500);
});