using Microsoft.AspNetCore.Authentication.Cookies;
using Warehouse.Manager.Services;
using Warehouse.Manager.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

var appSettingsSection = builder.Configuration.GetSection("AppConfig");
var appSettings = appSettingsSection.Get<AppConfig>();
builder.Services.Configure<AppConfig>(appSettingsSection);

builder.Services.AddSingleton<ILoggingSingletonService, LoggingSingletonService>();
builder.Services.AddScoped<IDbDataScopedService, DbDataScopedService>();
builder.Services.AddTransient<IExceptionHandlerService, ExceptionHandlerService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                  options.LoginPath = "/login";
                });
builder.Services.AddAuthorization();

builder.Services.AddMvc(options =>
{
  options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
    _ => "Null on Required property found.");
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
 