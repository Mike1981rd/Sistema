# Fixed Category Inheritance in Item/Create

## Problem
When selecting a category in Item/Create, only the tax was being inherited but not the accounting accounts.

## Root Cause
1. The category controller returns different property names than expected:
   - Returns: `cuentaVentaId` (missing 's')
   - Expected: `cuentaVentasId`
   - Returns: `cuentaCompraId`
   - Expected: `cuentaComprasInventariosId`
   
2. The account names were not being loaded, only IDs were passed

## Solution Implemented

### 1. Property Name Mapping
Updated `/wwwroot/js/items/tab-info-init.js` to map the response correctly:

```javascript
const mappedResponse = {
    success: true,
    cuentaVentasId: response.cuentaVentaId, // Map from cuentaVentaId
    cuentaComprasInventariosId: response.cuentaCompraId, // Map from cuentaCompraId
    cuentaCostoVentasGastosId: response.cuentaInventarioId, // Map from cuentaInventarioId
    // ... other mappings
};
```

### 2. Dynamic Account Loading
Updated `/wwwroot/js/items/tab-contabilidad-init.js` to load account names dynamically:

```javascript
function updateAccountingSelectWithLoad(selector, value) {
    // Add loading option first
    const loadingOption = new Option('Cargando...', value, true, true);
    $select.append(loadingOption);
    
    // Then load the actual account data
    $.ajax({
        url: '/api/CuentasContables/Buscar',
        type: 'GET',
        data: { term: value },
        success: function(data) {
            // Find and set the account with proper name
        }
    });
}
```

### 3. Fixed API URL
Changed from incorrect `/api/CuentasContables/Search` to correct `/api/CuentasContables/Buscar`

## Testing

Created test pages to verify functionality:
1. `/Item/TestSelect2` - Tests basic select2 functionality
2. `/Item/TestCategoryInheritance` - Tests category inheritance

Navigate to `/Item/TestCategoryInheritance` to see the inheritance in action.

## Files Modified

1. `/wwwroot/js/items/tab-info-init.js`
   - Added property mapping for category response
   - Added console logging for debugging

2. `/wwwroot/js/items/tab-contabilidad-init.js`
   - Added `updateAccountingSelectWithLoad` function
   - Updated event listener to use dynamic loading
   - Fixed API URL to use `/api/CuentasContables/Buscar`

3. Created test files:
   - `/Views/Item/TestCategoryInheritance.cshtml`
   - Updated `/Controllers/ItemController.cs` with test action

## How It Works Now

1. User selects a category in the Information tab
2. The system fetches category data using `/Categoria/ObtenerDatos/{id}`
3. The response is mapped to match expected property names
4. Tax is updated immediately in the Information tab
5. An event is triggered to notify the Accounting tab
6. The Accounting tab loads account names dynamically using the API
7. All inherited fields are properly populated with correct names

## Next Steps

If you need to fix the property names at the source, update the `CategoriaController.ObtenerDatos` method to return:
- `cuentaVentasId` instead of `cuentaVentaId`
- `cuentaComprasInventariosId` instead of `cuentaCompraId`
- `cuentaCostoVentasGastosId` instead of `cuentaInventarioId`

This would eliminate the need for property mapping in JavaScript.