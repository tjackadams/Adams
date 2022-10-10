using StronglyTypedIds;

namespace Nexus.Todo.Api.Domain
{
    [StronglyTypedId(backingType: StronglyTypedIdBackingType.Int, converters: StronglyTypedIdConverter.TypeConverter | StronglyTypedIdConverter.SystemTextJson | StronglyTypedIdConverter.EfCoreValueConverter)]
    public partial struct TodoId
    {
    };

    public class Todo
    {
        public TodoId TodoId { get; set; }

        public DateTimeOffset CreatedTime { get; set; }
    }
}
