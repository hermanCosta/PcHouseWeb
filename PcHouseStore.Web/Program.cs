using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PcHouseStore.Web.Data;
using PcHouseStore.Web.Services;
using PcHouseStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

// Add HTTP client and services
builder.Services.AddHttpClient<ApiService>(client =>
{
    var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7061";
    client.BaseAddress = new Uri(apiBaseUrl + "/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<SaleService>();
builder.Services.AddScoped<ServiceOrderService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<CompanyContext>();

// Add Entity Framework
builder.Services.AddDbContext<PcHouseStoreDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        new MySqlServerVersion(new Version(8, 0, 21))));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Seed test data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PcHouseStoreDbContext>();
    context.Database.EnsureCreated();
    
    // Add test company if none exists
    if (!context.Companies.Any())
    {
        var testCompany = new PcHouseStore.Domain.Models.Company
        {
            Name = "Test Company",
            Address = "123 Main Street, City, State 12345",
            ContactOne = "555-0123",
            Email = "test@company.com",
            Password = "password123" // In production, this should be hashed
        };
        
        context.Companies.Add(testCompany);
        context.SaveChanges();
    }
}

app.Run();
