using AttestationSystem.DB;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AttestationContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache(); // Для сессий
builder.Services.AddSession(options => 
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
    await next.Invoke();
});
app.UseDeveloperExceptionPage();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
#pragma warning disable ASP0014
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
#pragma warning restore ASP0014

app.Run();

