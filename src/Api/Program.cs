using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.Projects;
using Infrastructure.Repositories;
using Application.Tasks;
using Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// EF Core DbContext configuration (SQLite by default; InMemory if USE_INMEMORY=true)
var useInMemory = builder.Configuration.GetValue("USE_INMEMORY", false);
if (useInMemory)
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("AppDb"));
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                           ?? "Data Source=app.db";
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(connectionString));
}

// DI registrations
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskItemService, TaskItemService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API v1");
    });
}

app.UseHttpsRedirection();

// CORS
app.UseCors("AllowClient");

app.UseAuthorization();

app.MapControllers();

// Seed data (development)
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    ctx.Database.EnsureCreated();
    if (!ctx.Projects.Any())
    {
        var demo = new Project
        {
            Name = "Örnek Proje",
            Description = "Demo verisi",
            Tasks = new List<TaskItem>
            {
                new TaskItem { Title = "İlk görev" },
                new TaskItem { Title = "İkinci görev", IsCompleted = true }
            }
        };
        ctx.Projects.Add(demo);
        ctx.SaveChanges();
    }
}

app.Run();
