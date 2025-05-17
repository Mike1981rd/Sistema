# Fixed Compiler Error: EstablecerEmpresaActualAsync

## Problem
Compilation error: `'IUserService' does not contain a definition for 'EstablecerEmpresaActualAsync'`

## Solution
Removed the non-existent method call and simply set the session value directly.

## Changes

### Before
```csharp
public async Task<IActionResult> TestCategoryInheritance()
{
    HttpContext.Session.SetInt32("EmpresaActualId", 4);
    await _userService.EstablecerEmpresaActualAsync(4); // This method doesn't exist
    return View();
}
```

### After
```csharp
public IActionResult TestCategoryInheritance()
{
    HttpContext.Session.SetInt32("EmpresaActualId", 4);
    return View();
}
```

## Files Modified
1. `/Controllers/ItemController.cs`
   - `TestCategoryInheritance` - Removed async and non-existent method call
   - `TestSelect2` - Added session setup
   - `Create` - Added session setup to ensure empresa 4 is set

## Summary
The application now sets the empresa ID directly in the session without trying to call non-existent methods. All test pages and the Create page now ensure empresa 4 is set in the session for consistent behavior.