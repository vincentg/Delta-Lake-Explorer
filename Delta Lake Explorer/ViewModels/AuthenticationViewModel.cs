using System.Collections.ObjectModel;
using System.Windows.Input;
using Azure.ResourceManager.Resources;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Delta_Lake_Explorer.Core.Contracts.Services;
using Delta_Lake_Explorer.Core.Contracts.Services.Azure;
using Delta_Lake_Explorer.Core.Models.Azure;
using Delta_Lake_Explorer.Core.Services.Azure;
using Delta_Lake_Explorer.Helpers;

namespace Delta_Lake_Explorer.ViewModels;

public class AuthenticationViewModel : ObservableRecipient
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IARMService _armService;
    private AzureAuthentication? _azureAuthentication;
    private bool? _isAuthenticated;
    private string? _buttonText;
    private string? _labelText;
    private ObservableCollection<SubscriptionData> _subscriptionList;

    public AuthenticationViewModel(IAuthenticationService authenticationService, IARMService armService)
    {
        _authenticationService = authenticationService;
        _armService = armService;
        _azureAuthentication = (AzureAuthentication)_authenticationService.GetAuthenticationAsync().Result;
        _isAuthenticated = _azureAuthentication.IsAuthenticated;
        _buttonText = (bool)_isAuthenticated ? "Authentication_LoggedIn".GetLocalized() : "Authentication_ClickToLogin".GetLocalized();
        _labelText = generateLabelText();
        _subscriptionList = new ObservableCollection<SubscriptionData>(armService.GetSubscriptionsAsync().Result.Select(i => i.Data));
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
    public ObservableCollection<SubscriptionData> SubscriptionList
    {
    
           get => _subscriptionList;
           set => SetProperty(ref _subscriptionList, value);
       }

    public string LabelText
    {

        get => _labelText;
        set => SetProperty(ref _labelText, value);
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
               LabelText = generateLabelText();
               SubscriptionList = new ObservableCollection<SubscriptionData>(
                   (await _armService.GetSubscriptionsAsync()).Select(i => i.Data));
           }
       });

    private string generateLabelText()
    {
        if (_isAuthenticated == true)
        {
            return "Authentication_Username".GetLocalized() + $": {_azureAuthentication.UserName}\r\n" +
                   "Authentication_ClientId".GetLocalized() + $": {_azureAuthentication.ClientId}\r\n" +
                   "Authentication_TenantId".GetLocalized() + $": {_azureAuthentication.TenantId}\r\n\r\n" +
                   "Authentication_SubscriptionSelect".GetLocalized() + "\r\n";
        }
        else
        {

            return "Authentication_NotLoggedIn".GetLocalized();
        }
    }
}
