using SistemaContable.Data;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Services;
using Microsoft.Extensions.Logging;

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
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IImpuestoService, ImpuestoService>();
builder.Services.AddScoped<IPlazoPagoService, PlazoPagoService>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
