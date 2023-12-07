using AzOpenAIChatDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddScoped<IFileUploadService, CloudFileUploadService>();
builder.Services.AddScoped<IAzCognitiveSearchService, AzCognitiveSearchService>();
builder.Services.AddScoped<IAzOpenAIService, AzOpenAIService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
