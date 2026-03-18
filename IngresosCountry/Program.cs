using IngresosCountry.Data;
using IngresosCountry.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Database connection
builder.Services.AddSingleton<DatabaseConnection>();

// Services (Business Logic Layer)
builder.Services.AddScoped<ISocioService, SocioService>();
builder.Services.AddScoped<IInvitadoService, InvitadoService>();
builder.Services.AddScoped<IVisitanteService, VisitanteService>();
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IAccessLogService, AccessLogService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ICatalogService, CatalogService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();