using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorShoppingApp.DTOs;
using RazorShoppingApp.Models;
using RazorShoppingApp.Services;

namespace RazorShoppingApp.Pages;

public class TodosModel(TodoService todoService) : PageModel
{

    public List<TodoModel> Todos { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {

        var todos = await todoService.GetTodosAsync();

        if (todos is null)
            return NotFound();


        this.Todos = todos;
        return Page();
    }

    public async Task<IActionResult> OnPostUpdateAsync(UpdateTodo dto)
    {
        if (!ModelState.IsValid)
            return RedirectToPage();


        var updated = await todoService.UpdateTodoAsync(dto);

        if (!updated) return NotFound();

        return RedirectToPage();
    }
}
