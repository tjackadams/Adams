using System.Diagnostics.CodeAnalysis;
using StronglyTypedIds;

namespace Nexus.Todo.Api.Domain;

[StronglyTypedId(StronglyTypedIdBackingType.Int,
    StronglyTypedIdConverter.TypeConverter | StronglyTypedIdConverter.SystemTextJson |
    StronglyTypedIdConverter.EfCoreValueConverter)]
public partial struct TodoId
{
    public static bool TryParse(string? value, IFormatProvider? _, out TodoId result)
    {
        if (!string.IsNullOrEmpty(value) && int.TryParse(value, out var todoId))
        {
            result = new TodoId(todoId);
            return true;
        }

        result = new TodoId();
        return false;
    }
}



public class Todo
{
    public const int MaximumTitleLength = 2000;

    private readonly List<TodoTask> _tasks = new List<TodoTask>();
    private Todo()
    {
        Tasks = _tasks.AsReadOnly();
    }

    public static Todo Create(string title)
    {
        return new Todo { Title = title };
    }

    public TodoId TodoId { get; private set; }

    public string Title { get; private set; } = null!;
    public DateTimeOffset CreatedTime { get; private set; }

    public IReadOnlyCollection<TodoTask> Tasks { get; }

    public void AddTask(string title)
    {
        _tasks.Add(TodoTask.Create(title));
    }
}

[StronglyTypedId(StronglyTypedIdBackingType.Int,
    StronglyTypedIdConverter.TypeConverter | StronglyTypedIdConverter.SystemTextJson |
    StronglyTypedIdConverter.EfCoreValueConverter)]
public partial struct TodoTaskId
{
}

public class TodoTask
{
    private TodoTask()
    {

    }

    public static TodoTask Create(string title)
    {
        return new TodoTask { Title = title };
    }

    public TodoTaskId TodoTaskId { get; set; }

    public string Title { get; set; } = null!;


}
