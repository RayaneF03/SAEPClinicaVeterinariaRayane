using ClinicaVet.Data;
using ClinicaVet.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ClinicaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ClinicaConnection")));
builder.Services.AddDataProtection();
builder.Services.AddScoped<ICpfCripto, CpfCripto>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ClinicaContext>();
    if (db.Database.IsRelational())
    {
        db.Database.Migrate();
        SeedData.Inicializar(db, scope.ServiceProvider.GetRequiredService<ICpfCripto>());
    }
}

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
