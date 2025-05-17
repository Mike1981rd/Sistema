# FIX: Issue with EmpresaId and Empty SelectLists

## Problem
The Edit Item screen had empty SelectLists for Categories, Brands, and Taxes because:
1. The `UserService.GetEmpresaId()` was hardcoded to return 1
2. The actual data in the database was associated with a different empresaId
3. The session wasn't properly setting the empresaId

## Solution Applied

### 1. Updated ItemController to use EmpresaService
- Changed all occurrences of `_userService.GetEmpresaId()` to `await _empresaService.ObtenerEmpresaActualId()`
- Made all methods that use `await` marked as `async`
- Fixed the `GenerarCodigoAutomatico` method which was missing the `async` modifier

### 2. Made PrepararViewModel Async
- Changed `PrepararViewModel` to `PrepararViewModelAsync`
- Updated all calls to use `await`
- Added comprehensive logging to debug empresaId issues

### 3. Added Session Management
- Modified `HomeController` to set empresaId in session when accessing home page
- Added `SetEmpresaSession` method in ItemController for testing

### 4. Created Debug Actions and Views
- Added `TestData` action and view to show all data with empresaId
- Added `EditTest` action and view to test SelectLists
- These help verify the fix is working correctly

## Files Modified
1. `/Controllers/ItemController.cs` - Updated to use async empresaId
2. `/Controllers/HomeController.cs` - Added session management
3. `/Views/Item/TestData.cshtml` - Debug view for testing
4. `/Views/Item/EditTest.cshtml` - Debug view for Edit testing

## How to Test
1. Access the home page first: `/` (to set session)
2. Or manually set empresaId: `/Item/SetEmpresaSession/{id}`
3. Check debug data: `/Item/TestData`
4. Test edit page: `/Item/Edit/{id}`

## Result
The SelectLists should now properly load data for the correct empresaId, and the Edit screen should work correctly.