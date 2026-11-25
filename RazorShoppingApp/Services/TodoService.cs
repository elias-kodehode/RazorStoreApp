using Microsoft.EntityFrameworkCore;
using RazorShoppingApp.DTOs;
using RazorShoppingApp.Models;

namespace RazorShoppingApp.Services;

public class TodoService(AppDbContext db, ICurrentUserService currentUser)
{
    private readonly string _userId = currentUser.UserId ?? throw new();

    public async Task<List<TodoModel>> GetTodosAsync()
    {
        var todos = await db.Todos.Where(x => x.UserId == _userId).ToListAsync();

        return todos;
    }

    public async Task<bool> UpdateTodoAsync(UpdateTodo dto)
    {
        var todo = await db.Todos.FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (todo is null) return false;

        todo.Title = dto.Title;
        todo.Description = dto.Description;
        todo.IsCompleted = dto.IsCompleted;
        await db.SaveChangesAsync();

        return true;
    }
}
