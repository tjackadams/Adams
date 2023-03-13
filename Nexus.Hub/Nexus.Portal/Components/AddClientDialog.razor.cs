using Blazored.FluentValidation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Nexus.Portal.Features.Clients;
using Nexus.Portal.Features.GlobalProgress;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Components;

public partial class AddClientDialog
{
    private readonly ClientModel _model = new();
    private FluentValidationValidator? _clientValidator;
    private ServerValidator? _serverValidator;

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    [Inject]
    public Validator ClientValidator { get; set; } = null!;

    [Inject]
    public IMediator Mediator { get; set; } = null!;

    private async Task SubmitAsync()
    {
        if (await _clientValidator!.ValidateAsync())
        {
            try
            {
                await Mediator.Send(new ProgressState.ShowProgressAction());
                await Mediator.Send(new ClientState.CreateClientAction(_model.Name!));
                MudDialog.Close(DialogResult.Ok(true));
            }
            catch (SwaggerException<HttpValidationProblemDetails> ex)
            {
                await _serverValidator!.ValidateAsync(ex.Result, _model);
            }
            finally
            {
                await Mediator.Send(new ProgressState.HideProgressAction());
            }
        }
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    public class ClientModel
    {
        public string? Name { get; set; }
    }

    public class Validator : AbstractValidator<ClientModel>
    {
        public Validator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(20);
        }
    }
}
