// Simple diagnostic to identify Select2 issues
$(document).ready(function() {
    // Wait for page to fully load
    $(window).on('load', function() {
        console.log("=== SELECT2 DIAGNOSTIC ===");
        
        // Check all select elements
        $('select').each(function(index) {
            var $el = $(this);
            console.log(`Select ${index}:`, {
                id: $el.attr('id'),
                class: $el.attr('class'),
                isSelect2: $el.hasClass('select2-hidden-accessible'),
                visible: $el.is(':visible'),
                value: $el.val(),
                optionCount: $el.find('option').length
            });
        });
        
        // Check for duplicate Select2 elements
        console.log("\nSelect2 containers found:", $('.select2-container').length);
        $('.select2-container').each(function(index) {
            console.log(`Container ${index}:`, {
                id: $(this).attr('id'),
                parentSelectId: $(this).siblings('select').attr('id')
            });
        });
        
        // Check for orphaned Select2 elements
        console.log("\nOrphaned Select2 elements:");
        $('.select2-container').each(function() {
            var $relatedSelect = $(this).siblings('select.select2-hidden-accessible');
            if ($relatedSelect.length === 0) {
                console.log("Orphaned container:", $(this).attr('id'));
            }
        });
    });
});