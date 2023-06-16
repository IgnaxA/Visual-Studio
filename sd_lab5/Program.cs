using sd_lab5.Service;
using sd_lab5.Service.Plugins;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddSingleton<UploadImageProcessedService>();
builder.Services.AddSingleton<ImageGrayPlugin>();
builder.Services.AddSingleton<ImageMatrixImproveResolution>();
builder.Services.AddSingleton<ImageImproveBrightness>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Plugin}/{action=ShowPlugins}");

app.Run();
