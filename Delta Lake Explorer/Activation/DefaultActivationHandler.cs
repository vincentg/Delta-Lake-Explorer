using Delta_Lake_Explorer.Contracts.Services;
using Delta_Lake_Explorer.ViewModels;

using Microsoft.UI.Xaml;

namespace Delta_Lake_Explorer.Activation;

public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        // None of the ActivationHandlers has handled the activation.
        return _navigationService.Frame?.Content == null;
    }

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        _navigationService.NavigateTo(typeof(AuthenticationViewModel).FullName!, args.Arguments);

        await Task.CompletedTask;
    }
}
