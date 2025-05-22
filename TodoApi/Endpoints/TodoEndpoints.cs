using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Endpoints
{
    public static class TodoEndpoints
    {
        public static RouteGroupBuilder MapTodoEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/todo");

            group.MapPost("/", async Task<IResult> (CreateTodoDto createTodo, TodoContext dbContext) =>
            {
                var todo = new Todo
                {
                    Title = createTodo.Title,
                    Description = createTodo.Description,
                    IsComplete = createTodo.IsComplete,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                dbContext.Todos.Add(todo);
                await dbContext.SaveChangesAsync();

                return Results.Created($"/todo/{todo.Id}", todo);
            });

            group.MapGet("/", async Task<IResult> (TodoContext dbContext) =>
            {
                var todo = await dbContext.Todos.ToListAsync();

                return TypedResults.Ok(todo);
            });

            group.MapGet("/{id}", async Task<Results<Ok<Todo>, NotFound>> (int id, TodoContext dbContext) =>
            {
                var todo = await dbContext.Todos.FindAsync(id);

                if (todo is null)
                    return TypedResults.NotFound();

                return TypedResults.Ok(todo);
            });

            group.MapPut("/{id}", async Task<Results<NoContent, NotFound>> (int id, UpdateTodoDto updateTodo, TodoContext dbContext) =>
            {
                var todo = await dbContext.Todos.FindAsync(id);

                if (todo is null)
                    return TypedResults.NotFound();

                todo.Title = updateTodo.Title;
                todo.Description = updateTodo.Description;
                todo.IsComplete = updateTodo.IsComplete;
                todo.UpdatedAt = DateTime.UtcNow;

                await dbContext.SaveChangesAsync();

                return TypedResults.NoContent();
            });

            group.MapDelete("/{id}", async Task<Results<NoContent, NotFound>> (int id, TodoContext dbContext) =>
            {
                var todo = await dbContext.Todos.FindAsync(id);

                if (todo is null)
                    return TypedResults.NotFound();

                dbContext.Todos.Remove(todo);
                await dbContext.SaveChangesAsync();

                return TypedResults.NoContent();
            });


            return group;
        }
    }
}
