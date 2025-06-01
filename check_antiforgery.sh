#!/bin/bash

# Script to check for missing AntiForgeryToken in toggle forms

echo "Checking for forms with formToggleEstado that may be missing AntiForgeryToken..."
echo "="*80

# Files with formToggleEstado
files=(
    "/mnt/c/Users/hp/Documents/Visual Studio 2022/Projects/AuroraContabilidad/SistemaContable/Views/Usuarios/Index.cshtml"
    "/mnt/c/Users/hp/Documents/Visual Studio 2022/Projects/AuroraContabilidad/SistemaContable/Views/Retenciones/Index.cshtml"
    "/mnt/c/Users/hp/Documents/Visual Studio 2022/Projects/AuroraContabilidad/SistemaContable/Views/Familia/Index.cshtml"
    "/mnt/c/Users/hp/Documents/Visual Studio 2022/Projects/AuroraContabilidad/SistemaContable/Views/Impresora/Index.cshtml"
    "/mnt/c/Users/hp/Documents/Visual Studio 2022/Projects/AuroraContabilidad/SistemaContable/Views/RutaImpresora/Index.cshtml"
    "/mnt/c/Users/hp/Documents/Visual Studio 2022/Projects/AuroraContabilidad/SistemaContable/Views/Almacen/Index.cshtml"
)

for file in "${files[@]}"; do
    if [ -f "$file" ]; then
        echo "Checking: $(basename $(dirname "$file"))/$(basename "$file")"
        
        # Check if file has formToggleEstado
        if grep -q "formToggleEstado" "$file"; then
            # Check if the same block has AntiForgeryToken
            if grep -A 10 "formToggleEstado" "$file" | grep -q "AntiForgeryToken"; then
                echo "  ✅ HAS AntiForgeryToken"
            else
                echo "  ❌ MISSING AntiForgeryToken"
                echo "     Found form around line:" $(grep -n "formToggleEstado" "$file" | cut -d: -f1)
            fi
        else
            echo "  ⚠️  No formToggleEstado found"
        fi
        echo
    else
        echo "File not found: $file"
    fi
done

echo "Check complete."