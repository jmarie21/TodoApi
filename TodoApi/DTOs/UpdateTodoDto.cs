namespace TodoApi.DTOs
{
    public record class UpdateTodoDto(
        string Title,
        string Description,
        bool IsComplete,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}
