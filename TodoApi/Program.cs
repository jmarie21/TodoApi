using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TodoContext")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

app.MapTodoEndpoints();
app.MapUserEndPoints();

app.Run();
