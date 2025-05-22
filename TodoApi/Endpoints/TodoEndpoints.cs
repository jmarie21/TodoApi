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

            group.MapPost("/", TodoHandlers.CreateTodo);
            group.MapGet("/", TodoHandlers.GetAllTodos);
            group.MapGet("/{id}", TodoHandlers.GetTodoById);
            group.MapPut("/{id}", TodoHandlers.UpdateTodo);
            group.MapDelete("/{id}", TodoHandlers.DeleteTodo);

            return group;
        }

        public static class TodoHandlers
        {
            public static async Task<IResult> CreateTodo(CreateTodoDto createTodo, TodoContext dbContext)
            {
                var todo = new Todo
                {
                    Title = createTodo.Title,
                    Description = createTodo.Description,
                    IsComplete = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                dbContext.Todos.Add(todo);
                await dbContext.SaveChangesAsync();

                return Results.Created($"/todo/{todo.Id}", todo);
            }

            public static async Task<IResult> GetAllTodos(TodoContext dbContext)
            {
                var todo = await dbContext.Todos.ToListAsync();

                return TypedResults.Ok(todo);
            }

            public static async Task<Results<Ok<Todo>, NotFound>> GetTodoById(int id, TodoContext dbContext)
            {
                var todo = await dbContext.Todos.FindAsync(id);

                if (todo is null)
                    return TypedResults.NotFound();

                return TypedResults.Ok(todo);
            }

            public static async Task<Results<NoContent, NotFound>> UpdateTodo(int id, UpdateTodoDto updateTodo, TodoContext dbContext)
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
            }

            public static async Task<Results<NoContent, NotFound>> DeleteTodo(int id, TodoContext dbContext)
            {
                var todo = await dbContext.Todos.FindAsync(id);

                if (todo is null)
                    return TypedResults.NotFound();

                dbContext.Todos.Remove(todo);
                await dbContext.SaveChangesAsync();

                return TypedResults.NoContent();
            }
        }
    }
}
