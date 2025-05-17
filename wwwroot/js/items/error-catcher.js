// Error catching for debugging
window.addEventListener('error', function(e) {
    console.error('JavaScript Error:', e.message);
    console.error('File:', e.filename);
    console.error('Line:', e.lineno);
    console.error('Column:', e.colno);
    console.error('Stack:', e.error.stack);
});

// Also catch unhandled promise rejections
window.addEventListener('unhandledrejection', function(e) {
    console.error('Unhandled Promise Rejection:', e.reason);
});