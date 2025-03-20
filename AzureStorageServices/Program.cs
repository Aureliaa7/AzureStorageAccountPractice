using AzureStorageServices.Options;
using AzureStorageServices.Services;
using AzureStorageServices.Services.Interfaces;
using AzureTables.Options;
using AzureTables.Services;
using AzureTables.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<IQueueService, QueueService>();

builder.Services.Configure<AzureTableStorageOptions>(builder.Configuration.GetSection(AzureTableStorageOptions.AzureTableStorage));
builder.Services.Configure<AzureContainerOptions>(builder.Configuration.GetSection(AzureContainerOptions.AzureContainerStorage));
builder.Services.Configure<AzureQueueStorageOptions>(builder.Configuration.GetSection(AzureQueueStorageOptions.AzureQueueStorage));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
