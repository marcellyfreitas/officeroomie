using WebApi.Database;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddCustomControllers();
builder.Services.AddRazorPages();
builder.Services.AddMvc();
builder.Services.AddSingleton<IApplicationDbContext, ApplicationDbContext>();
builder.Services.AddCustomServices();
builder.Services.AddCustomSwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger();
}

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapCustomControllerRoutes();
app.UseHttpsRedirection();
app.MapControllers();
app.MapRazorPages();

await app.SeedDatabase();


app.Run();
