using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Nexus.Portal.Components;

public class ServerValidator : ComponentBase
{
    [CascadingParameter]
    public EditContext? EditContext { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (EditContext is null)
        {
            throw new InvalidOperationException($"{nameof(ServerValidator)} requires a cascading " +
                                                $"parameter of type {nameof(EditContext)}. For example, you can use {nameof(ServerValidator)} " +
                                                "inside an EditForm.");
        }
    }

    public Task ValidateAsync(HttpValidationProblemDetails problem, object model)
    {
        var messages = new ValidationMessageStore(EditContext!);

        if (problem.Errors.Any())
        {
            messages.Clear();

            foreach (var error in problem.Errors)
            {
                var fieldIdentifier = new FieldIdentifier(model, error.Key);
                messages.Add(fieldIdentifier, error.Value);
            }

            EditContext!.NotifyValidationStateChanged();
        }

        return Task.CompletedTask;
    }
}
