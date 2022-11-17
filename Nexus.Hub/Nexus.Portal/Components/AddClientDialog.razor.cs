using Blazored.FluentValidation;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Nexus.WeightTracker;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Components;

public partial class AddClientDialog
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    [Inject]
    public WeightTrackerClient TrackerClient { get; set; } = null!;

    [Inject]
    public Validator ClientValidator { get; set; } = null!;

    private async Task SubmitAsync()
    {
        if (await _clientValidator!.ValidateAsync())
        {
            try
            {
                await TrackerClient.CreateClientAsync(_model.Name, CancellationToken.None);
                MudDialog.Close(DialogResult.Ok(true));
            }
            catch (SwaggerException<HttpValidationProblemDetails> ex)
            {
                await _serverValidator!.ValidateAsync(ex.Result, _model);
            }
        }
    }
    private void Cancel() => MudDialog.Cancel();

    private readonly ClientModel _model = new ClientModel();
    private FluentValidationValidator? _clientValidator;
    private ServerValidator? _serverValidator;

    public class ClientModel
    {
        public string? Name { get; set; }
    }

    public class Validator : AbstractValidator<ClientModel>
    {
        public Validator()
        {
            RuleFor(c => c.Name)
                .NotEmpty();
            //.MaximumLength(20);
        }
    }
}

