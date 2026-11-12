using ClinicaVet.Data;
using ClinicaVet.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

// Cria o construtor da aplicação e carrega as configurações do appsettings/User Secrets.
var builder = WebApplication.CreateBuilder(args);

// Habilita controllers MVC e descoberta das views Razor.
builder.Services.AddControllersWithViews();

// Permite testar com SQLite local ou usar SQL Server na entrega.
var useSqlite = string.Equals(builder.Configuration["DatabaseProvider"], "Sqlite", StringComparison.OrdinalIgnoreCase);
builder.Services.AddDbContext<ClinicaContext>(options =>
{
    if (useSqlite)
        options.UseSqlite(builder.Configuration.GetConnectionString("LocalConnection"));
    else
        options.UseSqlServer(builder.Configuration.GetConnectionString("ClinicaConnection"));
});

// Registra proteção de dados e o serviço responsável por cifrar CPFs.
builder.Services.AddDataProtection();
builder.Services.AddScoped<ICpfCripto, CpfCripto>();

// Configura autenticação por cookie com expiração e renovação deslizante.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

// Habilita o mecanismo [Authorize] nos controllers.
builder.Services.AddAuthorization();

// Constrói a aplicação com os serviços registrados acima.
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    // Em produção, mostra uma página de erro genérica e ativa HSTS.
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Configura os middlewares na ordem necessária para uma aplicação MVC.
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Cria/aplica o banco e insere dados de demonstração na primeira execução.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ClinicaContext>();
    if (db.Database.IsRelational())
    {
        // SQLite usa EnsureCreated para iniciar rapidamente o banco local.
        if (useSqlite)
            db.Database.EnsureCreated();
        // SQL Server usa migrations para atualizar a estrutura oficialmente.
        else
            db.Database.Migrate();
        SeedData.Inicializar(db, scope.ServiceProvider.GetRequiredService<ICpfCripto>());
    }
}

// Define a rota padrão: /Controller/Action/Id.
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicia o servidor web e aguarda requisições.
app.Run();
