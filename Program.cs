using SistemaContable.Data;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Services;
using Microsoft.Extensions.Logging;
using SistemaContable.Repositories;
using SistemaContable.Repositories.Interfaces;
using SistemaContable.Services.Interfaces;
using OfficeOpenXml;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar sesión
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure database context
var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
dataSourceBuilder.EnableDynamicJson();
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(dataSource)
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IImpuestoService, ImpuestoService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPlazoPagoService, PlazoPagoService>();

// Registro de repositorios para Cuenta Contable y Contacto
builder.Services.AddScoped<ICuentaContableRepository, CuentaContableRepository>();
builder.Services.AddScoped<IContactoRepository, ContactoRepository>();

// Registro de repositorios y servicios para el módulo de Entradas de Diario
builder.Services.AddScoped<IEntradaDiarioRepository, EntradaDiarioRepository>();
builder.Services.AddScoped<ITipoEntradaDiarioRepository, TipoEntradaDiarioRepository>();
builder.Services.AddScoped<INumeracionEntradaDiarioRepository, NumeracionEntradaDiarioRepository>();
builder.Services.AddScoped<IEntradaDiarioService, EntradaDiarioService>();

// Configurar EPPlus para la licencia no comercial
ExcelPackage.License.SetNonCommercialPersonal("SistemaContable");

// Configura logging para EntityFramework en entorno de desarrollo
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddLogging(builder =>
    {
        builder.AddConsole();
        builder.AddDebug();
        builder.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Information);
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Usar sesión
app.UseSession();

app.MapControllerRoute(
    name: "catalogoCuentas",
    pattern: "contabilidad/catalogo",
    defaults: new { controller = "Catalogo", action = "Index" });

app.MapControllerRoute(
    name: "empresas",
    pattern: "empresas/{action=Index}/{id?}",
    defaults: new { controller = "Empresas" });

app.MapControllerRoute(
    name: "bancos",
    pattern: "bancos/cuentas",
    defaults: new { controller = "Banco", action = "Index" });

app.MapControllerRoute(
    name: "bancosTransacciones",
    pattern: "bancos/transacciones",
    defaults: new { controller = "Banco", action = "TransaccionesBancarias" });

app.MapControllerRoute(
    name: "bancosConciliacion",
    pattern: "bancos/conciliacion", 
    defaults: new { controller = "Banco", action = "Conciliacion" });

app.MapControllerRoute(
    name: "plazosPago",
    pattern: "PlazoPago/{action=Index}/{id?}",
    defaults: new { controller = "PlazoPago" });

app.MapControllerRoute(
    name: "retenciones",
    pattern: "Retenciones/{action=Index}/{id?}",
    defaults: new { controller = "Retenciones" });

app.MapControllerRoute(
    name: "ventasClientes",
    pattern: "ventas/clientes/{action=Index}/{id?}",
    defaults: new { controller = "Clientes" });

app.MapControllerRoute(
    name: "comprasProveedores",
    pattern: "compras/proveedores/{action=Index}/{id?}",
    defaults: new { controller = "Proveedores" });

// Rutas para el módulo de Entradas de Diario
app.MapControllerRoute(
    name: "entradaDiario",
    pattern: "contabilidad/entradas-diario/{action=Index}/{id?}",
    defaults: new { controller = "EntradaDiario" });

app.MapControllerRoute(
    name: "tipoEntradaDiario",
    pattern: "contabilidad/tipos-entrada-diario/{action=Index}/{id?}",
    defaults: new { controller = "TipoEntradaDiario" });

app.MapControllerRoute(
    name: "numeracionEntradaDiario",
    pattern: "contabilidad/numeraciones-entrada-diario/{action=Index}/{id?}",
    defaults: new { controller = "NumeracionEntradaDiario" });

app.MapControllerRoute(
    name: "categoria",
    pattern: "Categoria/{action=Index}/{id?}",
    defaults: new { controller = "Categoria" });

// Rutas para módulos principales
app.MapControllerRoute(
    name: "impuestos",
    pattern: "Impuestos/{action=Index}/{id?}",
    defaults: new { controller = "Impuestos" });

app.MapControllerRoute(
    name: "usuarios",
    pattern: "Usuarios/{action=Index}/{id?}",
    defaults: new { controller = "Usuarios" });

app.MapControllerRoute(
    name: "roles",
    pattern: "Roles/{action=Index}/{id?}",
    defaults: new { controller = "Roles" });

app.MapControllerRoute(
    name: "comprobantes",
    pattern: "configuracion/comprobantes-fiscales/{action=Index}/{id?}",
    defaults: new { controller = "Comprobantes" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
