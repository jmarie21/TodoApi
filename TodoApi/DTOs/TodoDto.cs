namespace TodoApi.DTOs
{
    public record class TodoDto(
       string Title,
       string Description,
       bool IsComplete,
       DateTime CreatedAt,
       DateTime UpdatedAt
    );
}
