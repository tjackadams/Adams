﻿using Blazored.FluentValidation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Nexus.Portal.Features.Clients;
using Nexus.Portal.Features.GlobalProgress;
using Nexus.WeightTracker;
using Nexus.WeightTracker.Contracts;

namespace Nexus.Portal.Components;

public partial class AddClientMetricDialog
{
    private FluentValidationValidator? _clientValidator;
    private readonly ClientMetricModel _model = new();
    private ServerValidator? _serverValidator;

    [Inject]
    public WeightTrackerClient TrackerClient { get; set; } = null!;

    [Inject]
    public IMediator Mediator { get; set; } = null!;

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public int ClientId { get; set; }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task SubmitAsync()
    {
        if (await _clientValidator!.ValidateAsync())
        {
            try
            {
                await Mediator.Send(new ProgressState.ShowProgressAction());
                await Mediator.Send(new ClientState.CreateClientMetricAction(
                    ClientId,
                    DateOnly.FromDateTime(_model.RecordedDate!.Value),
                    _model.RecordedValue!.Value));
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

    public class ClientMetricModel
    {
        public DateTime? RecordedDate { get; set; }
        public double? RecordedValue { get; set; }
    }

    public class Validator : AbstractValidator<ClientMetricModel>
    {
        public Validator()
        {
            RuleFor(c => c.RecordedDate)
                .NotEmpty();

            RuleFor(c => c.RecordedValue)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
