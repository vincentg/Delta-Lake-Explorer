using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Delta_Lake_Explorer.Core.Contracts.Services;
using Delta_Lake_Explorer.Core.Models;
using Delta_Lake_Explorer.Helpers;

namespace Delta_Lake_Explorer.ViewModels;

public class AuthenticationViewModel : ObservableRecipient
{
    private readonly IAuthenticationService _authenticationService;
    private AzureAuthentication? _azureAuthentication;
    private bool? _isAuthenticated;
    private string? _buttonText;

    public AuthenticationViewModel(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
        _azureAuthentication = (AzureAuthentication)_authenticationService.GetAuthenticationAsync().Result;
        _isAuthenticated = _azureAuthentication.IsAuthenticated;
        _buttonText = (bool)_isAuthenticated ? "Authentication_LoggedIn".GetLocalized() : "Authentication_ClickToLogin".GetLocalized();
    }
    public bool? IsAuthenticated
    {
        get => _isAuthenticated;
        set => SetProperty(ref _isAuthenticated, value);
    }

    public string ButtonText
    {
        get => _buttonText;
        set => SetProperty(ref _buttonText, value);
    }

    public ICommand LoginCommand => new RelayCommand(async () =>
       {
           if (IsAuthenticated == true)
           {
               //IsAuthenticated = await _authenticationService.LogoutAsync();
               //ButtonText = ResourceExtensions.GetLocalized("Authentication_ClickToLogin.Text");
           }
           else
           {
               _azureAuthentication = (AzureAuthentication) await _authenticationService.AuthenticateAsync();
               IsAuthenticated = _azureAuthentication.IsAuthenticated;
               ButtonText = "Authentication_LoggedIn".GetLocalized();
           }
       });

}
