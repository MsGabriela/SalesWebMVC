using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMVC.Data;
using SalesWebMVC.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SalesWebMVCContext");
builder.Services.AddTransient<SeedingService>();
builder.Services.AddDbContext<SalesWebMVCContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<SellerService>();

builder.Services.AddScoped<DepartmentService>();
//var connectionString = builder.Services.AddDbContext<SalesWebMVCContext>(options => options.
//UseSqlServer(builder.Configuration.
//GetConnectionString("SalesWebMVCContext") ,
//builder => builder.MigrationsAssembly("SalesWebMVC")));


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    SeedData(app);
}

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<SeedingService>();
        service.Seed();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

    app.UseHsts();
}

//app.MapGet("/", (Func<string>)(() => "Hello World!"));

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
