namespace Todo.Api.Dtos;

public record TaskCreateRequest(string Title, DateTime? DueDate);
public record TaskUpdateRequest(string Title, DateTime? DueDate);
public record TaskListResponse(IEnumerable<TaskResponse> Items, int Total, int Page, int PageSize);
public record TaskResponse(int Id, string Title, bool IsDone, DateTime? DueDate, DateTime CreatedAt);
