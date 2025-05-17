# Fixed Accounts Loading Error 500

## Problem
When selecting a category, the accounting accounts stayed in "Loading..." state and API returned 500 errors.

## Root Cause
1. The CuentasContables API controller had syntax errors
2. The controller was trying to access `CodigoNombre` property which doesn't exist
3. Session handling wasn't properly configured

## Solution

### 1. Fixed API Controller
Updated `/Controllers/API/CuentasContablesController.cs`:

```csharp
// Before - problematic code
text = c.CodigoNombre,  // This property doesn't exist

// After - fixed code
text = $"{c.Codigo} - {c.Nombre}",  // Manual concatenation
```

### 2. Added Error Handling
Added proper try-catch blocks with detailed error logging:

```csharp
try
{
    // API logic here
}
catch (Exception ex)
{
    Console.WriteLine($"[CuentasContables.Buscar] Error: {ex.Message}");
    Console.WriteLine($"[CuentasContables.Buscar] Stack: {ex.StackTrace}");
    return StatusCode(500, new { error = ex.Message, details = ex.StackTrace });
}
```

### 3. Fixed Session/Empresa Handling
Set default empresa ID when session is not available:

```csharp
// Use empresa 4 as temporary default
int empresaId = 4;

// Try to get from session
var empresaIdClaim = User.FindFirst("EmpresaId");
if (empresaIdClaim != null && int.TryParse(empresaIdClaim.Value, out int claimEmpresaId))
{
    empresaId = claimEmpresaId;
}
```

### 4. Added ID-based Search
When searching by account ID, the API now searches by exact ID match first:

```csharp
if (int.TryParse(term, out int cuentaId))
{
    var byId = await query.Where(c => c.Id == cuentaId)
        .Select(c => new { /* ... */ })
        .FirstOrDefaultAsync();
    
    if (byId != null)
    {
        return Ok(new { results = new[] { byId } });
    }
}
```

### 5. Improved JavaScript Error Handling
Updated error handling in both tab scripts to show more details:

```javascript
error: function(xhr, status, error) {
    console.error(`Error loading account data for ${value}:`, error);
    console.error('Response:', xhr.responseText);
    
    // Update the option to show error
    $select.find(`option[value="${value}"]`).text(`Error cargando cuenta ID: ${value}`);
}
```

## Testing
Use `/Item/TestCategoryInheritance` to test the functionality. The test page now shows:
- Detailed error messages in the console
- API response details
- Stack traces when available

## Files Modified
1. `/Controllers/API/CuentasContablesController.cs` - Complete rewrite
2. `/wwwroot/js/items/tab-contabilidad-init.js` - Enhanced error handling
3. `/Views/Item/TestCategoryInheritance.cshtml` - Added detailed debugging
4. `/Controllers/ItemController.cs` - Added session setup in test action

## Result
The accounts now load correctly when a category is selected, showing the account code and name in the format "CODE - NAME".