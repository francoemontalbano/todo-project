using Microsoft.EntityFrameworkCore;
using Todo.Api.Data;
using Todo.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

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

app.MapTasks(); //Endpoints

app.Run();
