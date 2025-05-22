namespace TodoApi.DTOs
{
    public record class CreateTodoDto (
        string Title,
        string Description,
        bool IsComplete,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}
