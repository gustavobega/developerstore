using DeveloperStore.Infrastructure;
using DeveloperStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using AutoMapper;
using DeveloperStore.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") 
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

var dbFolder = Path.Combine(builder.Environment.ContentRootPath, "../DeveloperStore.Infrastructure/Data");
if (!Directory.Exists(dbFolder))
{
    Directory.CreateDirectory(dbFolder);
}

builder.Services.AddDbContext<DeveloperStoreDbContext>(options =>
    options.UseSqlite($"Data Source={Path.Combine(dbFolder, "developerstore.db")}"));

builder.Services.AddApplicationServices(); 

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DeveloperStore API", Version = "v1" });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseCors("AllowAngularApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeveloperStore API V1"));
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DeveloperStoreDbContext>();
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
