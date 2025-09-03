using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Data;
using Todo.Api.Dtos;
using Todo.Api.Models;

namespace Todo.Api.Endpoints;

public static class TasksEndpoints
{
    public static RouteGroupBuilder MapTasks(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/tasks").WithTags("Tasks");

        // Crear
        group.MapPost("/", async Task<Results<Created<TaskResponse>, ValidationProblem>> (
            TaskCreateRequest req, TodoDbContext db) =>
        {
            var errors = ValidateTitle(req.Title);
            if (errors is not null) return TypedResults.ValidationProblem(errors);

            var entity = new TaskItem { Title = req.Title.Trim(), DueDate = req.DueDate };
            db.Tasks.Add(entity);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/tasks/{entity.Id}", ToResponse(entity));
        });

        // Obtener con ID
        group.MapGet("/{id:int}", async Task<Results<Ok<TaskResponse>, NotFound>> (int id, TodoDbContext db) =>
        {
            var e = await db.Tasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return e is null ? TypedResults.NotFound() : TypedResults.Ok(ToResponse(e));
        });

        // Listar con filtros y paginación
        group.MapGet("/", async Task<Ok<TaskListResponse>> (
            TodoDbContext db, string? status, DateTime? dueBefore,
            int page = 1, int pageSize = 20, string sort = "createdAt", string order = "desc") =>
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

            IQueryable<TaskItem> query = db.Tasks.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (status.Equals("pending", StringComparison.OrdinalIgnoreCase)) query = query.Where(t => !t.IsDone);
                else if (status.Equals("done", StringComparison.OrdinalIgnoreCase)) query = query.Where(t => t.IsDone);
            }

            if (dueBefore is not null)
                query = query.Where(t => t.DueDate != null && t.DueDate < dueBefore);

            bool asc = order.Equals("asc", StringComparison.OrdinalIgnoreCase);
            query = (sort.ToLowerInvariant()) switch
            {
                "duedate" => asc ? query.OrderBy(t => t.DueDate) : query.OrderByDescending(t => t.DueDate),
                "createdat" => asc ? query.OrderBy(t => t.CreatedAt) : query.OrderByDescending(t => t.CreatedAt),
                _ => asc ? query.OrderBy(t => t.CreatedAt) : query.OrderByDescending(t => t.CreatedAt)
            };

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
                                   .Select(t => ToResponse(t)).ToListAsync();

            return TypedResults.Ok(new TaskListResponse(items, total, page, pageSize));
        });

        // Actualizar
        group.MapPut("/{id:int}", async Task<Results<NoContent, NotFound, ValidationProblem>> (
            int id, TaskUpdateRequest req, TodoDbContext db) =>
        {
            var errors = ValidateTitle(req.Title);
            if (errors is not null) return TypedResults.ValidationProblem(errors);

            var e = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (e is null) return TypedResults.NotFound();

            e.Title = req.Title.Trim();
            e.DueDate = req.DueDate;
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        });

        // Tarea realizada
        group.MapPatch("/{id:int}/complete", async Task<Results<NoContent, NotFound>> (int id, TodoDbContext db) =>
        {
            var e = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (e is null) return TypedResults.NotFound();

            if (!e.IsDone) { e.IsDone = true; await db.SaveChangesAsync(); }
            return TypedResults.NoContent();
        });

        // Eliminar
        group.MapDelete("/{id:int}", async Task<Results<NoContent, NotFound>> (int id, TodoDbContext db) =>
        {
            var e = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (e is null) return TypedResults.NotFound();

            db.Tasks.Remove(e);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        });

        return group;
    }

    private static TaskResponse ToResponse(TaskItem e)
        => new(e.Id, e.Title, e.IsDone, e.DueDate, e.CreatedAt);

    private static Dictionary<string, string[]>? ValidateTitle(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return new() { ["title"] = new[] { "Titulo es requerido." } };
        if (title.Trim().Length > 120)
            return new() { ["title"] = new[] { "Máximo 120 caracteres." } };
        return null;
    }
}
