using Microsoft.EntityFrameworkCore;
using Todo.Api.Data;
using Todo.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// CORS para Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Puerto por defecto de Angular
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext (SQL Server)
var cs = builder.Configuration.GetConnectionString("TodoDb")
         ?? throw new InvalidOperationException("Missing connection string 'TodoDb'");
builder.Services.AddDbContext<TodoDbContext>(opt => opt.UseSqlServer(cs));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilitar CORS
app.UseCors("AllowAngular");

app.MapTasks(); //Endpoints

app.Run();
